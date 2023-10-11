using Core.Attributes;
using Core.Mediation.Impl;
using Features.SpinWheel;

namespace Features.MainMenu
{
    public sealed class MainMenuScreenMediator : Mediator
    {
        [Inject] private MainMenuScreenView View { get; set; }
        [Inject] private ISpinWheelController SpinWheelController { get; set; }

        public override void OnEnable()
        {
            View.OnStartClick += OnStartClicked;
        }
        
        public override void OnDisable()
        {
            View.OnStartClick -= OnStartClicked;
        }

        private void OnStartClicked()
        {
            SpinWheelController.EnterScene();
        }
    }
}