using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIRoot : MonoBehaviour, IUIRoot
        {
            public Camera Camera { get; set; }
            
            public Transform Container => container;
            [SerializeField] private Transform container;
        }
    }
}