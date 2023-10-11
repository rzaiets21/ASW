namespace Core.Events
{
    public abstract class EventBase
    {
        
    }
    
    public sealed class Event : EventBase
    {
    
    }

    public sealed class Event<T1> : EventBase
    {
        
    }

    public sealed class Event<T1, T2> : EventBase
    {
        
    }
}
