using System;
using Core.Engine;
using UnityEngine;

namespace Core.ContextView
{
    public sealed class ContextViewComponent : MonoBehaviour, IContextView
    {
        public IContext Context { get; private set; }
        public object ViewRaw => this;
        
        public void SetContext(IContext context)
        {
            Context = context;
        }
        
        public T As<T>() where T : class
        {
            if (typeof(T) == typeof(MonoBehaviour))
                return this as T;
            if (typeof(T) == typeof(GameObject))
                return gameObject as T;
            if (typeof(T) == typeof(Transform))
                return transform as T;
            
            throw new InvalidCastException();
        }
    }
}