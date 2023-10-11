using Core.Attributes;
using Core.Events;

namespace Features.SpinWheel.Impl
{
    public sealed class SpinWheelController : ISpinWheelController
    {
        [Inject] private IEventMap EventMap { get; set; }

        public void EnterScene()
        {
            EventMap.Dispatch(SpinWheelEvent.Enter);
        }
    }
}