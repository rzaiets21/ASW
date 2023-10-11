using Core.Attributes;
using Core.Mediation.Impl;
using Modules.Player;

namespace Features.SpinWheel
{
    public sealed class SpinWheelMediator : Mediator
    {
        [Inject] private SpinWheelScreenView View { get; set; }
        [Inject] private IPlayerController PlayerController { get; set; }
        
        public override void OnEnable()
        {
            View.InitSpinWheel();

            View.OnSpinStarted += OnSpinStarted;
            View.OnSpinStopped += OnSpinStopped;
        }

        private void OnSpinStarted()
        {
            View.SetSpinButtonInteractable(false);
        }

        private void OnSpinStopped(int winningValue)
        {
            View.SetSpinButtonInteractable(true);
            PlayerController.AddBalance(winningValue);
        }
    }
}