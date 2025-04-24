using System;
using UnityEngine;
using YG;
using Unit;
using Common;
using Installers;
using Pool;
using PlayerSpace;

namespace EnemySpace
{
    public class EnemyManager
    {
        private readonly EnemyPool _enemyPool;
        private readonly UISettings _uiSettings;
        private readonly TargetController _targetController;
        private readonly PlayerWallet _playerWallet;
        private readonly EnemyImprover _enemyImprover;

        public EnemyManager(EnemyPool enemyPool, UISettings uISettings, TargetController targetController,
                            PlayerWallet playerWallet, EnemyImprover enemyImprover)
        {
            _enemyPool = enemyPool;
            _uiSettings = uISettings;
            _targetController = targetController;
            _playerWallet = playerWallet;
            _enemyImprover = enemyImprover;

            _uiSettings.SlowEnemyButton.EnableBonus.AddListener(ActivateSlowEnemy);
            _uiSettings.SlowEnemyButton.DisableBonus.AddListener(DeActivateSlowEnemy);
        }

        ~EnemyManager()
        {
            _uiSettings.SlowEnemyButton.EnableBonus.RemoveAllListeners();
            _uiSettings.SlowEnemyButton.DisableBonus.RemoveAllListeners();
        }

        public event Action EnemyDied;

        public void Create(Vector3 point)
        {
            Enemy enemy = _enemyPool.Spawn() as Enemy;
            enemy.transform.position = point;
            enemy.TargetController = _targetController;
            enemy.ImproveCharacteristic(_enemyImprover.Health, _enemyImprover.Damage);
            enemy.Enable();
            enemy.DiedCompleted.AddListener(Destroy);
            enemy.DiedStarted.AddListener(CleanTarget);
        }

        private void ActivateSlowEnemy(int cost)
        {
            if (_playerWallet.TrySpendGem(cost))
            {
                for (int i = 0; i < _targetController.Enemies.Count; i++)
                {
                    var enemy = _targetController.Enemies[i] as Enemy;
                    enemy.ChangeSpeedModifyier(enemy.SlowSpeed);
                    enemy.Animator.SetFloat("Speed", enemy.SlowSpeed);
                    enemy.SwitchFreezePartical(true);
                }
            }
        }

        private void DeActivateSlowEnemy(int cost)
        {
            for (int i = 0; i < _targetController.Enemies.Count; i++)
            {
                var enemy = _targetController.Enemies[i] as Enemy;
                enemy.ChangeSpeedModifyier(enemy.FullSpeed);
                enemy.Animator.SetFloat("Speed", enemy.FullSpeed);
                enemy.SwitchFreezePartical(false);
            }
        }

        private void Destroy(GameUnit gameUnit)
        {
            var enemy = gameUnit as Enemy;
            gameUnit.DiedCompleted.RemoveAllListeners();
            gameUnit.DiedStarted.RemoveAllListeners();
            EnemyDied?.Invoke();
        }

        private void CleanTarget(GameUnit gameUnit)
        {
            var enemy = gameUnit as Enemy;
            _targetController.RemoveTarget(gameUnit);
            _playerWallet.AddGold(enemy.Award * YandexGame.savesData.goldScaleLevel);
        }
    }
}