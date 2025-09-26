namespace ZebombaTest.Scripts
{
    namespace UI
    {
        public interface IUIService
        {
            T Get<T>() where T : UIWindow;
        }
    }
    
}