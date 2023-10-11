using System;
using System.Collections.Generic;

namespace Core.ReflectionInfo.Impl
{
    public sealed class Reflector<T> : IReflector<T> where T : class, IReflectorInfo, new()
    {
        private readonly Dictionary<Type, T> _infos;

        public Reflector()
        {
            _infos = new Dictionary<Type, T>();
        }

        public T Get(Type type)
        {
            if (_infos.TryGetValue(type, out var info))
                return info;

            info = new T();
            info.Build(type);
            
            _infos.Add(type, info);
            return info;
        }
    }
}
