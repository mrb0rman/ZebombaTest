using ZebombaTest.Scripts.UI;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace Installers
    {
        public class UIInstaller : Installer<UIInstaller>
        {
            public override void InstallBindings()
            {
                Container
                    .Bind<IUIRoot>()
                    .FromComponentInNewPrefabResource(ResourcesSourceConst.UIRootSource)
                    .AsSingle();
                
                Container
                    .Bind<IUIService>()
                    .To<UIService>()
                    .AsSingle();
            }
        }
    }
}