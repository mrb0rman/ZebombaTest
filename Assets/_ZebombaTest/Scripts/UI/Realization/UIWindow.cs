using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public abstract class UIWindow : MonoBehaviour, IUIWindow
        {
            public Canvas Canvas => canvas;
            [SerializeField] protected UIContainer uiContainer;
            [SerializeField] protected Canvas canvas;
            
            protected IUIService _uiService;
    
            [Inject]
            private void Inject(
                IUIService uiService)
            {
                _uiService = uiService;
            }
            
            private void Awake()
            {
                uiContainer.OnShowCallback.Event.AddListener(Show);
                uiContainer.OnHiddenCallback.Event.AddListener(Hide);
            }

            public virtual void Show()
            {
                uiContainer.Show();
            }

            public virtual void Hide()
            {
                uiContainer.Hide();
            }
        }
    }
}