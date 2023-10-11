using Core.Mediation;

namespace Core.View
{
    public interface IUnityView
    {
        IMediator Mediator { get; }
        bool Initialized { get; }

        void SetMediator(IMediator mediator);
        
    }
}