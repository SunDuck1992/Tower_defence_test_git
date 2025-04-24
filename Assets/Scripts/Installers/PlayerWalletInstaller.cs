using Zenject;
using PlayerSpace;

namespace Installers
{
    public class PlayerWalletInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var playerWallet = new PlayerWallet();

            Container.Bind<PlayerWallet>().FromInstance(playerWallet).AsSingle();
        }
    }
}