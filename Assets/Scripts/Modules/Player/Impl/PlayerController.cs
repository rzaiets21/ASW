using System;
using Core.Attributes;
using Core.Events;
using Newtonsoft.Json;
using UnityEngine;

namespace Modules.Player.Impl
{
    public sealed class PlayerController : IPlayerController
    {
        [Inject] private IEventMap EventMap { get; set; }
        
        private const string PlayerPrefsName = "PlayerData";

        private PlayerData _playerData;

        [PostConstruct]
        private void PostConstruct()
        {
            LoadPlayerData();
        }

        public void ChangeName(string name)
        {
            _playerData.ChangeName(name);
        }

        public void UpdateBalance(int balance)
        {
            _playerData.UpdateBalance(balance);
            EventMap.Dispatch(PlayerEvent.BalanceUpdated, _playerData.Balance);
            TrySavePlayerData();
        }

        public void AddBalance(int value)
        {
            _playerData.AddBalance(value);
            
            EventMap.Dispatch(PlayerEvent.BalanceUpdated, _playerData.Balance);
            TrySavePlayerData();
        }

        public PlayerData GetPlayerData()
        {
            return _playerData;
        }

        private void TrySavePlayerData()
        {
            if (_playerData == null)
                throw new NullReferenceException();

            if (!_playerData.IsDirty)
                return;
            
            SavePlayerData();
        }

        private void SavePlayerData()
        {
            var json = JsonConvert.SerializeObject(_playerData);
            PlayerPrefs.SetString(PlayerPrefsName, json);
            Debug.LogError("Save");
            _playerData.ResetDirty();
        }
        
        private void LoadPlayerData()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsName))
            {
                _playerData = PlayerData.Default();
                SavePlayerData();
                return;
            }

            var json = PlayerPrefs.GetString(PlayerPrefsName);
            _playerData = JsonConvert.DeserializeObject<PlayerData>(json);
        }
    }
}