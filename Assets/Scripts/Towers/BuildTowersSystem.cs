using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using YG;
using Unit;
using Common;
using UI;
using Installers;
using Pool;
using PlayerSpace;

namespace TowerSpace
{
    public class BuildTowersSystem
    {
        private BuildArea _currentBuildArea;
        private TargetController _targetController;
        private BulletPool _bulletPool;
        private RocketPool _rocketPool;
        private UISettings _uiSettings;
        private PlayerWallet _playerWallet;
        private WaveScreen _waveScreen;
        private Tower _towerBuild;
        private List<Tower> _allTowers = new();
        private Dictionary<BuildArea, Tower> _towerAreaLocations = new();

        public BuildTowersSystem(SceneSettings sceneSettings, TowerSettings towerSettings, TargetController targetController,
                                 BulletPool bulletPool, UISettings uiSettings, PlayerWallet playerWallet, WaveScreen waveScreen,
                                 RocketPool rocketPool)
        {
            for (int i = 0; i < sceneSettings.BuildPoints.Count; i++)
            {
                sceneSettings.BuildPoints[i].BuildTowersSystem = this;
            }

            TowerSettings = towerSettings;
            _targetController = targetController;
            _bulletPool = bulletPool;
            _rocketPool = rocketPool;
            _uiSettings = uiSettings;
            _playerWallet = playerWallet;
            _waveScreen = waveScreen;

            _uiSettings.RepairTowersButton.EnableBonus.AddListener(RepairTowers);
            _uiSettings.RepairTowersButton.DisableBonus.AddListener(RepairTowers);

            _waveScreen.BattleEnded += OnResetTowerHealth;
            _waveScreen.BattleEnded += _playerWallet.OnSaveWallet;
            _waveScreen.BattleEnded += YandexGame.SaveProgress;
            _waveScreen.BattleEnded += OnUpdateTowersSaveInfo;

            _rocketPool = rocketPool;
            _targetController.AddTarget(sceneSettings.Base, true);
            _allTowers.Add(sceneSettings.Base);
        }

        ~BuildTowersSystem()
        {
            _waveScreen.BattleEnded -= OnResetTowerHealth;
            _waveScreen.BattleEnded -= _playerWallet.OnSaveWallet;
            _waveScreen.BattleEnded -= YandexGame.SaveProgress;
            _waveScreen.BattleEnded -= OnUpdateTowersSaveInfo;
        }

        public event Action<BuildArea> InteractedBuildArea;
        public event Action DeInteractedBuildArea;

        public BuildArea CurrentBuildArea => _currentBuildArea;
        public Dictionary<BuildArea, Tower> TowerAreaLocations => _towerAreaLocations;
        public TowerSettings TowerSettings { get; }

        public void OnInteractBuildArea(BuildArea buildArea)
        {
            _currentBuildArea = buildArea;
            InteractedBuildArea?.Invoke(buildArea);
        }

        public void OnDeInteractBuildArea()
        {
            DeInteractedBuildArea?.Invoke();
            _currentBuildArea = null;
        }

        public void BuildTower(Tower prefab)
        {
            if (_currentBuildArea == null)
            {
                return;
            }

            if (_currentBuildArea.OnBuild) return;

            NavMesh.SamplePosition(_currentBuildArea.BuildPoint.position, out var hit, 1, NavMesh.AllAreas);
            Tower tower = MonoBehaviour.Instantiate(prefab, hit.position, Quaternion.identity);

            tower.TargetController = _targetController;
            tower.BulletPool = _bulletPool;
            tower.RocketPool = _rocketPool;
            tower.Enable();

            tower.BuildArea = _currentBuildArea;
            _currentBuildArea.OnBuild = true;
            _targetController.AddTarget(tower, true);
            tower.DiedCompleted.AddListener(Destroy);
            _towerBuild = tower;

            _towerAreaLocations.Add(_currentBuildArea, tower);
            _allTowers.Add(tower);
        }

        public Tower GetBuildTower()
        {
            return _towerBuild;
        }

        public void SetCurrentbuildArea(BuildArea buildArea)
        {
            _currentBuildArea = buildArea;
        }

        private void RepairTowers(int cost)
        {
            if (_playerWallet.TrySpendGem(cost))
            {
                for (int i = 0; i < _allTowers.Count; i++)
                {
                    if (_allTowers[i].gameObject.activeSelf)
                    {
                        var tower = _allTowers[i];

                        tower.ResetHealth();
                        tower.EnableHealParticle();
                    }
                }
            }
        }

        private void Destroy(GameUnit gameUnit)
        {
            _uiSettings.RepairTowersButton.EnableBonus.RemoveListener(RepairTowers);
            _uiSettings.RepairTowersButton.DisableBonus.RemoveListener(RepairTowers);
            //gameUnit.DiedCompleted.RemoveAllListeners();

            if (gameUnit is Tower tower)
            {
                BuildArea buildArea = tower.BuildArea;

                if (buildArea != null)
                {
                    for (int i = 0; i < YandexGame.savesData.buildedAreas.Count; i++)
                    {
                        if (_waveScreen.IsBattle)
                        {
                            if (YandexGame.savesData.buildedAreas[i].name == buildArea.name)
                            {
                                bool flag = false;

                                for (int j = 0; j < YandexGame.savesData.destroyedTowers.Count; j++)
                                {
                                    if (YandexGame.savesData.destroyedTowers[j].name == buildArea.name)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }

                                if (!flag)
                                {
                                    YandexGame.savesData.destroyedTowers.Add(new BuildedAreaInfo(YandexGame.savesData.buildedAreas[i].name,
                                                                                                 YandexGame.savesData.buildedAreas[i].value,
                                                                                                 false));
                                }

                                YandexGame.SaveProgress();
                                break;
                            }
                        }
                    }

                    buildArea.OnBuild = false;
                    _towerAreaLocations.Remove(buildArea);
                }
            }

            _targetController.RemoveTarget(gameUnit);
            gameUnit.gameObject.SetActive(false);
        }

        private void OnUpdateTowersSaveInfo()
        {
            for (int i = 0; i < YandexGame.savesData.buildedAreas.Count; i++)
            {
                for (int j = 0; j < YandexGame.savesData.destroyedTowers.Count; j++)
                {
                    if (YandexGame.savesData.buildedAreas[i].name == YandexGame.savesData.destroyedTowers[j].name)
                    {
                        YandexGame.savesData.buildedAreas[i].isBuilded = YandexGame.savesData.destroyedTowers[j].isBuilded;
                    }
                }
            }

            YandexGame.savesData.destroyedTowers.Clear();
            YandexGame.SaveProgress();
        }

        private void OnResetTowerHealth()
        {
            for (int i = 0; i < _targetController.Towers.Count; i++)
            {
                var tower = _targetController.Towers[i];
                tower.ResetHealth();
            }
        }
    }
}