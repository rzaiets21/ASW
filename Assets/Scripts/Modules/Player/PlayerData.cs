using System;
using Newtonsoft.Json;

namespace Modules.Player
{
    [Serializable]
    public sealed class PlayerData
    {
        [JsonProperty("name")] public string Name { get; private set; }
        [JsonProperty("balance")] public uint Balance { get; private set; }
        
        [JsonIgnore] public bool IsDirty { get; private set; }

        public static PlayerData Default()
        {
            var playerData = new PlayerData();
            playerData.ChangeName("PlayerName");
            playerData.UpdateBalance(0);
            return playerData;
        }
        
        public void ChangeName(string name)
        {
            Name = name;
            IsDirty = true;
        }

        public void UpdateBalance(int balance)
        {
            if (balance < 0)
                balance = 0;
            Balance = (uint)balance;
            IsDirty = true;
        }

        public void AddBalance(int value)
        {
            if(value < 0)
                return;
            Balance += (uint)value;
        }

        public void ResetDirty()
        {
            IsDirty = false;
        }
    }
}