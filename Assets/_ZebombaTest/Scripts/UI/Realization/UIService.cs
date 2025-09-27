using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIService : IUIService
        {
            private readonly IInstantiator _instantiator;
            private readonly IUIRoot _uiRoot;
            private readonly CameraView _cameraView;

            private readonly Dictionary<Type, UIWindow> _viewStorage = new Dictionary<Type, UIWindow>();
            private Dictionary<Type, GameObject> _initWindows = new();
        
            public UIService(
                IInstantiator instantiator,
                IUIRoot uiRoot,
                CameraView cameraView)
            {
                _instantiator = instantiator;
                _uiRoot = uiRoot;
                _cameraView = cameraView;
            }
            
            public T Show<T>() where T : UIWindow
            {
                var type = typeof(T);
                if (!_initWindows.ContainsKey(type)) return null;
                var view = _initWindows[type];
                    
                var component = view.GetComponent<T>();
                component.Show();
                return component;
            }
            
            public void Hide<T>() where T : UIWindow
            {
                var type = typeof(T);
                if (!_initWindows.ContainsKey(type)) return;
                
                var view = _initWindows[type];
                view.GetComponent<T>().Hide();
            }
            
            public T Get<T>() where T : UIWindow
            {
                var type = typeof(T);
                return !_initWindows.ContainsKey(type) ? null : _initWindows[type].GetComponent<T>();
            }
            
            public void LoadWindows()
            {
                var windows = Resources.LoadAll("UIWindows", typeof(UIWindow));
                foreach (var t in windows)
                {
                    _viewStorage.Add(t.GetType(), (UIWindow)t);
                }
            }

            public void InitWindows()
            {
                foreach (var uiWindow in _viewStorage.Where(uiWindow => !_initWindows.ContainsKey(uiWindow.Key)))
                {
                    uiWindow.Value.Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    uiWindow.Value.Canvas.worldCamera = _cameraView.Camera;
                    var view = _instantiator.InstantiatePrefab(_viewStorage[uiWindow.Key], _uiRoot.Container);
                    _initWindows.Add(uiWindow.Key, view);
                }
            }
        }
    }
}


