using Core.Events;

namespace Core.Screens
{
    public sealed class ScreenEvent
    {
        public static readonly Event<Screen> Shown = new();
        public static readonly Event<Screen> Hidden = new();
    }
}