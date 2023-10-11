using System;
using System.Collections.Generic;
using Core.ReflectionInfo;
using Core.ReflectionInfo.Impl;
using UnityEngine;

namespace Core.Injection.Impl
{
    public sealed class InjectionBinder : IInjectionBinder
    {
        private readonly Dictionary<Type, InjectionBindingInfo> _bindings;

        private IReflector<ReflectorInfo> _reflector;

        public InjectionBinder()
        {
            _bindings = new Dictionary<Type, InjectionBindingInfo>();

            _reflector = new Reflector<ReflectorInfo>();
        }
        
        public InjectionBindingInfo Bind<T, V>() where V : T, new()
        {
            var bindingType = typeof(T);
            var bindingInfo = new InjectionBindingInfo(bindingType);
            bindingInfo.SetValue(typeof(V));
            
            _bindings.Add(bindingType, bindingInfo);
            
            return bindingInfo;
        }

        public InjectionBindingInfo Bind<T>(T value)
        {
            var bindingType = typeof(T);
            var bindingInfo = new InjectionBindingInfo(bindingType);
            bindingInfo.SetValue(value);
            bindingInfo.ConstructOnStart(false);
            _bindings.Add(bindingType, bindingInfo);
            
            return bindingInfo;
        }

        public InjectionBindingInfo Bind(Type bindingType, object value)
        {
            var bindingInfo = new InjectionBindingInfo(bindingType);
            bindingInfo.SetValue(value);
            _bindings.Add(bindingType, bindingInfo);

            return bindingInfo;
        }

        public void Unbind<T>()
        {
            Unbind(typeof(T));
        }

        public void Unbind(Type type)
        {
            if (!_bindings.TryGetValue(type, out var bindingInfo))
                return;
            _bindings.Remove(type);
            DestroyBinding(bindingInfo);
        }

        public void ForEachBinding(Action<InjectionBindingInfo> handler)
        {
            foreach (var binding in _bindings.Values)
                handler.Invoke(binding);
        }

        public object GetInstance(InjectionBindingInfo bindingInfo)
        {
            if (bindingInfo.ToConstruct && !bindingInfo.IsConstructed)
            {
                var value = bindingInfo.Value is Type injectionType
                    ? Activator.CreateInstance(injectionType)
                    : bindingInfo.Value;

                ConstructImpl(value);
                bindingInfo.SetValue(value);
                bindingInfo.MarkAsConstruct();
            }

            return bindingInfo.Value;
        }

        public void Construct<T>(T instance)
        {
            ConstructImpl(instance);
        }

        public T Destroy<T>(T instance)
        {
            DestroyImpl(instance);
            return instance;
        }
        
        private void DestroyImpl(object instance)
        {
            if (instance == null)
                return;

            var type = instance.GetType();
            var info = _reflector.Get(type);

            PreDestroy(instance, info);

            UnInject(instance, info);
        }
        
        private void ConstructImpl(object instance)
        {
            var type = instance.GetType();
            var info = _reflector.Get(type);
            Inject(instance, info);
            PostConstruct(instance, info);
        }

        private void DestroyBinding(InjectionBindingInfo bindingInfo)
        {
            var instance = bindingInfo.Value;

            if (instance == null)
                throw new NullReferenceException();

            var type = instance.GetType();
            var info = _reflector.Get(type);

            PreDestroy(instance, info);

            UnInject(instance, info);
        }
        
        private void Inject(object instance, IReflectorInfo info)
        {
            foreach (var injection in info.Injects)
            {
                var binding = GetBinding(injection.PropertyType);
                var value = GetInstance(binding);
                injection.SetValue(instance, value);
            }
        }

        private void UnInject(object instance, IReflectorInfo info)
        {
            foreach (var injection in info.Injects)
            {
                var type = injection.PropertyType;
                
                if (!type.IsValueType)
                    injection.SetValue(instance, null, null);
            }
        }
        
        private void PostConstruct(object instance, IReflectorInfo info)
        {
            if (info.PostConstructs.Count == 0)
                return;
            
            foreach (var method in info.PostConstructs)
                method.Invoke(instance, null);
        }

        private void PreDestroy(object instance, IReflectorInfo info)
        {
            if (info.PreDestroys.Count == 0)
                return;
            
            foreach (var method in info.PreDestroys)
                method.Invoke(instance, null);
        }
        
        private InjectionBindingInfo GetBinding(Type type)
        {
            _bindings.TryGetValue(type, out var bindingInfo);
            return bindingInfo;
        }
    }
}
