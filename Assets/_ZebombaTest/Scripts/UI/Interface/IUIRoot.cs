using UnityEngine;

namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public interface IUIRoot
        {
            Transform Container { get; }
            Camera Camera { get; set; }
        }
    }
    
}