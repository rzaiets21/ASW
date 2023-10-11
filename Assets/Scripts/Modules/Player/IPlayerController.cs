namespace Modules.Player
{
    public interface IPlayerController
    {
        void ChangeName(string name);

        void UpdateBalance(int balance);
        void AddBalance(int value);

        PlayerData GetPlayerData();
    }
}