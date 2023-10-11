using System;

namespace Core.ReflectionInfo
{
    public interface IReflector<out T> where T : class, IReflectorInfo, new()
    {
        T Get(Type type);
    }
}
