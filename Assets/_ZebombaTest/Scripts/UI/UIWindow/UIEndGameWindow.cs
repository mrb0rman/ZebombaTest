using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIEndGameWindow : UIWindow
        {
            public Action BackMenuEvent;
            public Action RestartGameEvent;
            
            [SerializeField] private UIButton backButton;
            [SerializeField] private UIButton restartButton;

            private void Start()
            {
                backButton.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
                restartButton.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
                backButton.behaviours.GetBehaviour(UIBehaviour.Name.PointerClick).Event.AddListener(BackMenuHandler);
                restartButton.behaviours.GetBehaviour(UIBehaviour.Name.PointerClick).Event.AddListener(RestartGameHandler);
            }
            
            private void BackMenuHandler()
            {
                BackMenuEvent?.Invoke();
                
                _uiService.Hide<UIEndGameWindow>();
                _uiService.Show<UIMainMenuWindow>();
            }

            private void RestartGameHandler()
            {
                RestartGameEvent?.Invoke();
                
                _uiService.Hide<UIEndGameWindow>();
                _uiService.Show<UIGameWindow>();
            }
        }
    }
}