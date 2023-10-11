using Core.Events;

namespace Core.Commands.Binding
{
    public interface ICommandBinding
    {
        ICommandBinding To<T>() where T : Command, new();
        ICommandBinding To<T, TP0>(TP0 param01) where T : Command<TP0>, new();
        
        ICommandBinding OnComplete(Event @event);
        
        ICommandBinding InSequence();
    }
}