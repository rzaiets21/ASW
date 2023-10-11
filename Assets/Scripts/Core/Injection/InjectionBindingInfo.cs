using System;

namespace Core.Injection
{
    public sealed class InjectionBindingInfo
    {
        public readonly Type Type;
        public object Value { get; private set; }
        public bool ToConstruct { get; private set; }
        public bool IsConstructed { get; private set; }
        
        public InjectionBindingInfo(Type type)
        {
            Type = type;
            Value = type;
            ToConstruct = true;
        }

        public void SetValue(object value)
        {
            Value = value;
        }

        public void ConstructOnStart(bool state)
        {
            ToConstruct = state;
        }

        public void MarkAsConstruct()
        {
            IsConstructed = true;
        }
    }
}
