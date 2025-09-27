using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using ZebombaTest.Scripts.GameSystem;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIEndGameWindow : UIWindow
        {
            public Action BackMenuEvent;
            public Action RestartGameEvent;

            [SerializeField] private TMP_Text _tmpText;
            [SerializeField] private UIButton backButton;
            [SerializeField] private UIButton restartButton;

            private GameController _gameController;
            [Inject]
            private void Init(GameController gameController)
            {
                _gameController = gameController;
            }

            public void SetScore(int score)
            {
                _tmpText.text = score.ToString();
            }
            
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
                
                _gameController.Init();
            }
        }
    }
}