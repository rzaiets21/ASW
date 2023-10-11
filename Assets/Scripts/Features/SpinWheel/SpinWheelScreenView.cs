using System;
using Core.Attributes;
using Core.View.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SpinWheel
{
    [Mediator(typeof(SpinWheelMediator))]
    public sealed class SpinWheelScreenView : UnityView
    {
        [SerializeField] private SpinWheelComponent spinWheelComponent;
        [SerializeField] private Button spinButton;

        public event Action OnSpinStarted;
        public event Action<int> OnSpinStopped;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            spinButton.onClick.AddListener(OnSpinButtonClick);

            spinWheelComponent.OnSpinStarted += OnStartSpin;
            spinWheelComponent.OnSpinStopped += OnStartStop;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            spinButton.onClick.RemoveListener(OnSpinButtonClick);
            
            spinWheelComponent.OnSpinStarted -= OnStartSpin;
            spinWheelComponent.OnSpinStopped -= OnStartStop;
        }

        public void InitSpinWheel()
        {
            spinWheelComponent.Init();
        }

        public void SetSpinButtonInteractable(bool state)
        {
            spinButton.interactable = state;
        }
        
        private void OnSpinButtonClick()
        {
            spinWheelComponent.Spin();
        }

        private void OnStartSpin()
        {
            OnSpinStarted?.Invoke();
        }

        private void OnStartStop(int winningValue)
        {
            OnSpinStopped?.Invoke(winningValue);
        }
    }
}
