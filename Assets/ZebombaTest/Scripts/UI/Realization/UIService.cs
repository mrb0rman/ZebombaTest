using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public class UIService : IUIService
        {
            private readonly IUIRoot _uiRoot;
        
            private Dictionary<Type, GameObject> _initWindows = new();
        
            public UIService(IUIRoot uiRoot)
            {
                _uiRoot = uiRoot;
                Init();
            }
        
            public T Get<T>() where T : UIWindow
            {
                var type = typeof(T);
                return !_initWindows.ContainsKey(type) ? null : _initWindows[type].GetComponent<T>();
            }
        
            private void Init()
            {
                foreach (var window in _uiRoot.Container.Windows)
                {
                    _initWindows.Add(window.GetType(), window.gameObject);
                }
            }
        }
    }
}


