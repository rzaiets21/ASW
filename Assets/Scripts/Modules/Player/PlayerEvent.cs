using Core.Events;

namespace Modules.Player
{
    public sealed class PlayerEvent
    {
        public static Event<uint> BalanceUpdated = new();
    }
}