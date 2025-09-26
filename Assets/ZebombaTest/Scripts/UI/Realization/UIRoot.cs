using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIRoot : MonoBehaviour, IUIRoot
        {
            public LayerContainer Container => container;
        
            [SerializeField] private LayerContainer container;
        }
    }
    
}

