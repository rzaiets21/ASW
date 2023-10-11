using Core.Commands.Binding;
using Core.Commands.Binding.Impl;

namespace Core.Commands
{
    public abstract class CommandBase
    {
        internal int Index { get; private set; } = -1;
        internal CommandBinding Binding { get; private set; }
        internal CommandParams Params { get; private set; }

        private CommandBinder _commandBinder;
        
        internal bool IsRetained { get; private set; }
        internal bool IsExecuted { get; private set; }

        internal void SetCommandBinder(CommandBinder commandBinder)
        {
            _commandBinder = commandBinder;
        }
        
        internal abstract void InternalExecute(CommandParams param, CommandParams paramAdditional);
        
        internal void PostExecute()
        {
            IsExecuted = true;
        }
        
        internal void Setup(int index, CommandBinding binding, CommandParams @params)
        {
            Index = index;
            Binding = binding;
            Params = @params;

            IsExecuted = false;
        }

        internal void Retain()
        {
            IsRetained = true;
        }

        internal void Release()
        {
            IsRetained = false;

            _commandBinder.OnCommandFinish(this);
        }
        
        internal virtual void Reset()
        {
            Index = -1;
            IsExecuted = false;
        }
    }
}