
namespace Game.Common
{
    public abstract class SingletonObserverBase : ISingltonMember
    {
        protected SingletonObserverBase()
        {
            SingletonObserverManager.Instance.Register(this);
        }

        public abstract void OnDestroy();
    }
}