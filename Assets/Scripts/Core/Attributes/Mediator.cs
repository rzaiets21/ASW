using System;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MediatorAttribute : Attribute
    {
        public readonly Type MediatorType;

        public MediatorAttribute(Type mediatorType)
        {
            MediatorType = mediatorType;
        }
    }
}