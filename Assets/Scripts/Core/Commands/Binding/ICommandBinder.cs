using System.Collections.Generic;
using Core.Commands.Binding.Impl;
using Core.Events;

namespace Core.Commands.Binding
{
    public interface ICommandBinder
    {
        CommandBinding Bind(Event @event);

        IList<CommandBinding> GetBindings(Event @event);
    }
}