using System;
using System.Collections.Generic;
using Core.Events;

namespace Core.Commands.Binding.Impl
{
    public sealed class CommandBinding : ICommandBinding
    {
        internal EventBase Event { get; }
        internal CommandBinder CommandBinder { get; }
        internal List<Type> Commands { get; }
        internal Dictionary<int, CommandParams> Params { get; set; }
        internal int CommandsExecuted { get; set; }
        internal EventBase CompleteEvent { get; set; }
        internal bool IsSequence { get; set; }
        
        public bool IsExecuting { get; private set; }

        public CommandBinding(Event @event, CommandBinder commandBinder)
        {
            Event = @event;
            CommandBinder = commandBinder;

            Commands = new List<Type>();
            Params = new Dictionary<int, CommandParams>();
        }
        
        public ICommandBinding To<T>() where T : Command, new()
        {
            AddCommand<T>();
            return this;
        }

        public ICommandBinding To<T, TP0>(TP0 param01) where T : Command<TP0>, new()
        {
            AddCommand<T, TP0>(param01);
            return this;
        }

        public ICommandBinding OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding InSequence()
        {
            IsSequence = true;
            return this;
        }

        private void AddCommand<T>() where T : Command
        {
            Commands.Add(typeof(T));
        }
        
        private void AddCommand<T, T1>(T1 param01) where T : CommandBase
        {
            Commands.Add(typeof(T));

            var param = new CommandParams<T1>
            {
                Param01 = param01
            };

            Params.Add(Commands.Count - 1, param);
        }
        
        internal void StartExecution()
        {
            IsExecuting = true;
        }
        
        internal void FinishExecution()
        {
            CommandsExecuted = 0;
            IsExecuting = false;
        }
        
        internal void RegisterCommandExecute()
        {
            CommandsExecuted++;
        }
        
        internal bool CheckAllExecuted()
        {
            return Commands.Count == CommandsExecuted;
        }
    }
}