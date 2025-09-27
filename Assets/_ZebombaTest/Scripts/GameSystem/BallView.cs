using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ZebombaTest.Scripts
{
    namespace GameSystem
    {
        public class BallView : MonoBehaviour, IPoolable<IMemoryPool>
        {
            public int Cost { get; private set; }
            public SpriteRenderer SpriteRenderer => spriteRenderer;
            public LineRenderer LineRenderer => lineRenderer;
            
            [SerializeField] private SpriteRenderer spriteRenderer;
            [SerializeField] private LineRenderer lineRenderer;
            private Color[] _colorList =
            {
                Color.green,
                Color.red,
                Color.blue,
            };
        
            public void OnDespawned()
            {
            
            }

            public void OnSpawned(IMemoryPool pool)
            {
                var colorNumber = Random.Range(0, _colorList.Length); 
                spriteRenderer.color = _colorList[colorNumber];
                Cost = (colorNumber + 1) * 3;
            }

            public class Pool : MonoMemoryPool<BallView>
            {
                protected override void Reinitialize(BallView item)
                {
                    item.OnSpawned(this);
                }

                protected override void OnDespawned(BallView item)
                {
                    base.OnDespawned(item);
                    item.OnDespawned();
                }
            }
        }
    }
}