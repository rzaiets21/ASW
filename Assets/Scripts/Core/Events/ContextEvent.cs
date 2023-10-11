namespace Core.Events
{
    public sealed class ContextEvent
    {
        public static readonly Event Started = new();
        public static readonly Event Loaded = new();
    }
}
