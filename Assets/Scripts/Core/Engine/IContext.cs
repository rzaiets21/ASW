using System;
using Core.Extensions;
using Core.Modules;

namespace Core.Engine
{
    public interface IContext
    {
        event Action<Module> OnModuleInitialized;
        
        event Action<IContext> OnStarting;
        event Action<IContext> OnStarted;
        event Action<IContext> OnLoaded;

        IContext AddModule<T>() where T : Module, new();
        IContext AddExtension<T>() where T : Extension, new();
        
        bool TryGetExtension<T>(out T extension) where T : Extension;

        void Start();
    }
}
