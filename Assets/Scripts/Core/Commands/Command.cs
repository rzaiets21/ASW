using System;
using Core.Commands.Binding;

namespace Core.Commands
{
    public abstract class Command : CommandBase
    {
        internal override void InternalExecute(CommandParams param, CommandParams paramAdditional)
        {
            Execute();
        }

        protected abstract void Execute();
    }

    public abstract class Command<T1> : CommandBase
    {
        private T1 _param01;
        
        internal override void InternalExecute(CommandParams param, CommandParams paramAdditional)
        {
            if (param is CommandParams<T1> @params)
                _param01 = @params.Param01;
            else if (paramAdditional is CommandParams<T1> paramsOverride)
                _param01 = paramsOverride.Param01;
            else
                throw new NullReferenceException();
            
            Execute(_param01);
        }
        
        protected abstract void Execute(T1 param01);

        internal override void Reset()
        {
            base.Reset();

            _param01 = default;
        }
    }
}