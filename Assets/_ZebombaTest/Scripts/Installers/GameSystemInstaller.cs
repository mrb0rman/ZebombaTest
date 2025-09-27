using ZebombaTest.Scripts.GameSystem;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace Installers
    {
        public class GameSystemInstaller : Installer<GameSystemInstaller>
        {
            public override void InstallBindings()
            {
                Container
                    .BindMemoryPool<BallView, BallView.Pool>()
                    .WithInitialSize(9)
                    .FromComponentInNewPrefabResource(ResourcesSourceConst.BallViewSource)
                    .UnderTransformGroup("BallGroup");

                Container
                    .BindMemoryPool<ParticleEffectView, ParticleEffectView.Pool>()
                    .WithInitialSize(9)
                    .FromComponentInNewPrefabResource(ResourcesSourceConst.ParticleViewSource)
                    .UnderTransformGroup("ParticleGroup");
                
                Container
                    .Bind<GameController>()
                    .AsSingle();
            }
        }
    }
}

