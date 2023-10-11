using Core.Attributes;
using Core.View.Impl;
using TMPro;
using UnityEngine;

namespace Features.InGameHUD
{
    [Mediator(typeof(InGameHUDMediator))]
    public sealed class InGameHUDView : UnityView
    {
        [SerializeField] private TextMeshProUGUI playerBalance;

        public void SetPlayerBalanceText(string playerBalanceText)
        {
            playerBalance.text = playerBalanceText;
        }
    }
}
