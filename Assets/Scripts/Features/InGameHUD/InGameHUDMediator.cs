using System.Linq;
using Core.Attributes;
using Core.Events;
using Core.Mediation.Impl;
using Modules.Player;
using UnityEngine;

namespace Features.InGameHUD
{
    public sealed class InGameHUDMediator : Mediator
    {
        private const char B = 'B';
        private const char M = 'M';
        private const char K = 'K';
        
        [Inject] private InGameHUDView View { get; set; }
        [Inject] private IPlayerController PlayerController { get; set; }
        [Inject] private IEventMap EventMap { get; set; }

        public override void OnEnable()
        {
            UpdateHUDBalance(PlayerController.GetPlayerData().Balance);
            
            EventMap.Map(PlayerEvent.BalanceUpdated, OnPlayerBalanceUpdated);
        }

        public override void OnDisable()
        {
            EventMap.UnMap(PlayerEvent.BalanceUpdated, OnPlayerBalanceUpdated);
        }

        private void OnPlayerBalanceUpdated(uint balance)
        {
            UpdateHUDBalance(balance);
        }

        private void UpdateHUDBalance(uint balance)
        {
            View.SetPlayerBalanceText(FormatPlayerBalance(balance));
        }

        private string FormatPlayerBalance(uint balance)
        {
            var textBalance = balance.ToString();
            var formattedBalance = string.Empty;

            if (textBalance.Length >= 10)
            {
                var chars = textBalance.Take(textBalance.Length - 9);
                foreach (var c in chars)
                {
                    formattedBalance += c;
                }
                formattedBalance += "." + textBalance[^9] + B;
            }
            else if (textBalance.Length >= 7)
            {
                var chars = textBalance.Take(textBalance.Length - 6);
                foreach (var c in chars)
                {
                    formattedBalance += c;
                }
                formattedBalance += "." + textBalance[^6] + M;
            }
            else if (textBalance.Length >= 4)
            {
                var chars = textBalance.Take(textBalance.Length - 3);
                foreach (var c in chars)
                {
                    formattedBalance += c;
                }

                formattedBalance += K;
            }
            else
            {
                formattedBalance = textBalance;
            }

            return formattedBalance;
        }
    }
}