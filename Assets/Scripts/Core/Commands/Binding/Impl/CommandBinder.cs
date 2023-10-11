using System;
using System.Collections;
using System.Collections.Generic;
using Core.Attributes;
using Core.Events;
using Core.Injection;
using Event = Core.Events.Event;

namespace Core.Commands.Binding.Impl
{
    public sealed class CommandBinder : ICommandBinder 
    {
        [Inject] private IEventMap EventMap { get; set; }
        [Inject] private IInjectionBinder InjectionBinder { get; set; }
        
        private readonly Dictionary<EventBase, IList> _bindings;
        private readonly CommandParams NoParams = new();
        
        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, IList>();
        }
        
        public CommandBinding Bind(Event @event)
        {
            var binding = new CommandBinding(@event, this);
            if (_bindings.TryGetValue(@event, out var bindings))
                ((List<CommandBinding>)bindings).Add(binding);
            else
                _bindings[@event] = new List<CommandBinding> { binding };
            return binding;
        }
        
        public void ProcessEvent(Event @event)
        {
            var bindings = GetBindings(@event);
            
            if (bindings == null)
                return;

            foreach (var binding in bindings)
            {
                if (binding.IsExecuting)
                    break;

                binding.StartExecution();
                ProcessBindingCommand(binding, 0, NoParams);
            }
        }
        
        private void ProcessBindingCommand(CommandBinding binding, int index, CommandParams param)
        {
            if (binding.CheckAllExecuted())
            {
                FinishBindingExecution(binding, param);
                return;
            }

            if (index >= binding.Commands.Count)
                return;

            var command = GetCommand(binding.Commands[index]);
            command.Setup(index, binding, param);

            if (binding.Params != null && binding.Params.TryGetValue(command.Index, out var commandParam))
                command.InternalExecute(param, commandParam);
            else
                command.InternalExecute(param, null);
            
            command.PostExecute();
            binding.RegisterCommandExecute();

            if (!command.IsRetained)
            {
                OnCommandFinished(command);
                return;
            }

            if (!binding.IsSequence)
                ProcessBindingCommand(binding, index + 1, param);
        }
        
        public void OnCommandFinish(CommandBase command)
        {
            if (command.IsExecuted)
                OnCommandFinished(command);
        }
        
        private void OnCommandFinished(CommandBase command)
        {
            var index = command.Index;
            var binding = command.Binding;
            var param = command.Params;

            ReturnCommand(command);

            if (binding.IsSequence)
            {
                ProcessBindingCommand(binding, index + 1, param);
                return;
            }

            if (binding.CheckAllExecuted())
            {
                FinishBindingExecution(binding, param);
            }
        }
        
        private void FinishBindingExecution(CommandBinding binding, CommandParams param)
        {
            binding.FinishExecution();

            if (binding.CompleteEvent != null)
                param.DispatchParams(EventMap, binding.CompleteEvent);
        }
        
        public IList<CommandBinding> GetBindings(Event @event)
        {
            _bindings.TryGetValue(@event, out var bindings);
            return (List<CommandBinding>)bindings;
        }
        
        private CommandBase GetCommand(Type commandType)
        {
            var command = (CommandBase)Activator.CreateInstance(commandType);
            command.SetCommandBinder(this);
            InjectionBinder.Construct(command);
            
            return command;
        }
        
        private void ReturnCommand(CommandBase command)
        {
            command.Reset();

            InjectionBinder.Destroy(command);
        }
    }
}