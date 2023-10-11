using System;

namespace Core.Mediation.Impl
{
    public sealed class MediatorBindingInfo : IMediatorBindingInfo
    {
        public Type ViewType { get; }
        public Type MediatorType { get; }

        public MediatorBindingInfo(Type viewType, Type mediatorType)
        {
            ViewType = viewType;
            MediatorType = mediatorType;
        }
    }
}