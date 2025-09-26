using Zenject;

namespace ZebombaTest.Scripts
{
    namespace Installers
    {
        public class ApplicationInstaller : MonoInstaller
        {
            public override void InstallBindings()
            {
                SystemInstaller.Install(Container);
            
            
            }
        }
    }
}

