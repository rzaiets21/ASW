using System;

namespace Core.Injection
{
    public interface IInjectionBinder
    {
        InjectionBindingInfo Bind<T, V>() where V : T, new();
        InjectionBindingInfo Bind<T>(T value);
        InjectionBindingInfo Bind(Type type, object value);

        void Unbind<T>();
        void Unbind(Type type);
        
        T Destroy<T>(T instance);
        
        void ForEachBinding(Action<InjectionBindingInfo> handler);
        
        object GetInstance(InjectionBindingInfo bindingInfo);

        void Construct<T>(T instance);
    }
}
