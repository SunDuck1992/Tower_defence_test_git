using UnityEngine;
using Zenject;
using UI;

namespace Installers
{
    public class GUIInstaller : MonoInstaller
    {
        [SerializeField] private UpgradeScreen _upgradeScreen;
        [SerializeField] private WaveScreen _waveScreen;
        [SerializeField] private BuildScreen _buildScreen;
        [SerializeField] private ButtonHandler _buttonHandler;

        public override void InstallBindings()
        {
            Container.Bind<UpgradeScreen>().FromInstance(_upgradeScreen).AsSingle();
            Container.Bind<WaveScreen>().FromInstance(_waveScreen).AsSingle();
            Container.Bind<BuildScreen>().FromInstance(_buildScreen).AsSingle();
            Container.Bind<ButtonHandler>().FromInstance(_buttonHandler).AsSingle();
        }
    }
}