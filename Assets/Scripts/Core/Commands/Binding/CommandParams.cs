using Core.Events;

namespace Core.Commands.Binding
{
    internal class CommandParams
    {
        internal virtual void DispatchParams(IEventMap eventMap, EventBase @event)
        {
            if(@event is Event event0)
                eventMap.Dispatch(event0);
        }
    }
    
    internal class CommandParams<T1> : CommandParams
    {
        internal T1 Param01 { get; set; }
    }
}