using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.ReflectionInfo
{
    public interface IReflectorInfo
    {
        public List<PropertyInfo> Injects { get; }
        
        public List<MethodInfo> PostConstructs { get; }
        public List<MethodInfo> PreDestroys { get; }

        void Build(Type type);
    }
}
