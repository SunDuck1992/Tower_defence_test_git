using UnityEngine;
using Zenject;
using EnemySpace;
using PlayerSpace;

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerShooter _playerShooter;
        [SerializeField] private Enemy _enemies;
        [SerializeField] private PlayerMovement _playerMovement;

        public override void InstallBindings()
        {
            Container.Bind<Enemy>().FromInstance(_enemies).AsSingle();
            Container.Bind<EnemyManager>().AsSingle();
            Container.Bind<EnemyImprover>().AsSingle();
            Container.Bind<PlayerShooter>().FromInstance(_playerShooter).AsSingle();
            Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle();
            Container.Bind<PlayerUpgradeSystem>().AsSingle();
        }
    }
}