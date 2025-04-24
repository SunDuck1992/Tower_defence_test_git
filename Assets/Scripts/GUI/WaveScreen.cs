using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;
using Zenject;
using EnemySpace;
using Installers;

namespace UI
{
    public class WaveScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countWavetext;
        [SerializeField] private TextMeshProUGUI _countEnemiesProgressText;
        [SerializeField] private Slider _progressWaveBar;
        [SerializeField] private GameObject _backGroundMusic;

        private EnemyManager _enemyManager;
        private Spawner _spawner;
        private SceneSettings _sceneSettings;

        public UnityEvent WaveComplete;
        public event Action BattleEnded;
        public event Action BattlStarted;

        public bool IsBattle { get; private set; }
        public int WaveCount { get; private set; }

        [Inject]
        public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings)
        {
            _enemyManager = enemyManager;
            _spawner = sceneSettings.Spawner;
            _sceneSettings = sceneSettings;
        }

        private void Start()
        {
            OnShowBuildAreas();

            _enemyManager.EnemyDied += OnUpdateProgressBar;
            BattleEnded += OnShowBuildAreas;
            BattleEnded += OnDisableMusic;
            BattleEnded += OnSaveLeaderData;
            BattleEnded += OnSaveWaweInfo;
            BattleEnded += OnSaveEnemyLevelUpgrade;
        }

        private void OnDestroy()
        {
            _enemyManager.EnemyDied -= OnUpdateProgressBar;
            BattleEnded -= OnShowBuildAreas;
            BattleEnded -= OnDisableMusic;
            BattleEnded -= OnSaveLeaderData;
            BattleEnded -= OnSaveWaweInfo;
            BattleEnded -= OnSaveEnemyLevelUpgrade;
        }

        public void StartBattle()
        {
            _spawner.SpawnOnClick();
            _countEnemiesProgressText.text = $"{0} / {_spawner.MaxCountEnemies}";
            _countWavetext.text = _spawner.WaveCount.ToString();
            WaveCount = _spawner.WaveCount;
            _progressWaveBar.maxValue = _spawner.MaxCountEnemies;
            _progressWaveBar.value = 0;
            IsBattle = true;
            BattlStarted?.Invoke();

            for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
            {
                _sceneSettings.BuildPoints[i].gameObject.SetActive(false);
            }
        }

        private void OnUpdateProgressBar()
        {
            _progressWaveBar.value++;
            _countEnemiesProgressText.text = $"{_progressWaveBar.value} / {_spawner.MaxCountEnemies}";

            if (_progressWaveBar.value >= _spawner.MaxCountEnemies)
            {
                WaveComplete.Invoke();
                BattleEnded?.Invoke();
                IsBattle = false;
            }
        }

        private void OnShowBuildAreas()
        {
            if (_progressWaveBar.value >= _spawner.MaxCountEnemies)
            {
                for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
                {
                    if (_sceneSettings.BuildPoints[i].WaveLevel <= _spawner.WaveCount)
                    {
                        _sceneSettings.BuildPoints[i].gameObject.SetActive(true);
                    }
                }
            }
        }

        private void OnSaveLeaderData()
        {
            YandexGame.savesData.leaderScore += 1;
        }

        private void OnSaveWaweInfo()
        {
            YandexGame.savesData.waveCount = _spawner.WaveCount;
            YandexGame.savesData.enemyCount = _spawner.CountEnemies;
        }

        private void OnDisableMusic()
        {
            _backGroundMusic.SetActive(false);
        }

        private void OnSaveEnemyLevelUpgrade()
        {
            if (YandexGame.savesData.upgradeEnemyLevel == -1)
            {
                YandexGame.savesData.upgradeEnemyLevel = 1;
            }
            else
            {
                YandexGame.savesData.upgradeEnemyLevel++;
            }
        }
    }
}