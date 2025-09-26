using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class LayerContainer : MonoBehaviour
        {
            public UIWindow[] Windows => windows;
        
            [SerializeField] private UIWindow[] windows;

            private void Start()
            {
                foreach (var window in windows)
                {
                    window.gameObject.SetActive(true);
                }
            }
        }
    }
}