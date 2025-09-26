using ZebombaTest.Scripts.Bootstrap;
using ZebombaTest.Scripts.Command;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace Installers
    {
        public class SystemInstaller : Installer<SystemInstaller>
        {
            public override void InstallBindings()
            {
                Container
                    .Bind<IBootstrap>()
                    .To<Bootstrap.Bootstrap>()
                    .AsSingle();
            
                Container
                    .Bind<CommandStorage>()
                    .AsSingle()
                    .NonLazy();
            }
        }
    }
}

