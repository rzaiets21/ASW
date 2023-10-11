using System;
using Core.Engine;
using Core.Modules;

namespace Core.Extensions
{
    public abstract class Extension
    {
        protected IContext Context { get; private set; }
        
        internal void SetContext(IContext context)
        {
            Context = context;
        }
        
        public virtual void Initialize() { }
        public virtual void Setup() { }
        public virtual void Dispose()    { }
        
        protected Extension AddModule<T>() where T : Module, new()
        {
            Context.AddModule<T>();
            return this;
        }
        
        protected T GetDependentExtension<T>() where T : Extension
        {
            if (Context.TryGetExtension<T>(out var extension))
                return extension;
            throw new NullReferenceException();
        }
    }
}