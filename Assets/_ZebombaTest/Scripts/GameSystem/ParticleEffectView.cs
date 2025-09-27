using UnityEngine;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace GameSystem
    {
        public class ParticleEffectView : MonoBehaviour, IPoolable<IMemoryPool>
        {
            public ParticleSystem ParticleSystem => particleSystem;
            [SerializeField] private ParticleSystem particleSystem;
            public void OnDespawned()
            { }

            public void OnSpawned(IMemoryPool p1)
            { }
            
            public class Pool : MonoMemoryPool<ParticleEffectView>
            {
                protected override void Reinitialize(ParticleEffectView item)
                {
                    item.OnSpawned(this);
                }

                protected override void OnDespawned(ParticleEffectView item)
                {
                    base.OnDespawned(item);
                    item.OnDespawned();
                }
            }
        }
    }
}

