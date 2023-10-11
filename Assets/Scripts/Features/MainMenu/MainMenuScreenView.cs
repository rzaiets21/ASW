using System;
using Core.Attributes;
using Core.View.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenu
{
    [Mediator(typeof(MainMenuScreenMediator))]
    public sealed class MainMenuScreenView : UnityView
    {
        [SerializeField] private Button startButton;

        public event Action OnStartClick;

        protected override void OnEnabled()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        protected override void OnDisabled()
        {
            startButton.onClick.RemoveListener(OnStartButtonClick);
        }

        private void OnStartButtonClick()
        {
            OnStartClick?.Invoke();
        }
    }
}
