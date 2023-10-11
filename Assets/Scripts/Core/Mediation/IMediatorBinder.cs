using Core.View;

namespace Core.Mediation
{
    public interface IMediatorBinder
    {
        void OnViewAdd(IUnityView view);
        void OnViewRemove(IUnityView view);
    }
}