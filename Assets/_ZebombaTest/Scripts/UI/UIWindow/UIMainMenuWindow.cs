using System;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIMainMenuWindow : UIWindow
        {
            public Action StartGameEvent;
            
            [SerializeField] private UIButton startButton;
            
            private void Start()
            {
                startButton.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
                startButton.behaviours.GetBehaviour(UIBehaviour.Name.PointerClick).Event.AddListener(StartGameHandler);
            }
            
            private void StartGameHandler()
            {
                StartGameEvent?.Invoke();
                
                _uiService.Hide<UIMainMenuWindow>();
                _uiService.Show<UIEndGameWindow>();
            }
        }
    }
}

