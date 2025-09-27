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
                UIInstaller.Install(Container);
                
                Container
                    .Bind<CameraView>()
                    .FromComponentInNewPrefabResource(ResourcesSourceConst.CameraSource)
                    .AsSingle();
                
                GameSystemInstaller.Install(Container);
                
                Container
                    .Bind<ApplicationLaunch>()
                    .AsSingle()
                    .NonLazy();
            }
        }
    }
}

