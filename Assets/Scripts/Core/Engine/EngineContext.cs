using System;
using System.Collections.Generic;
using Core.Extensions;
using Core.Modules;

namespace Core.Engine
{
    public sealed class EngineContext : IContext
    {
        private readonly List<Type> _extensions;
        private readonly List<Type> _modules;
        
        private readonly Dictionary<Type, Module> _moduleInstances;
        private readonly Dictionary<Type, Extension> _extensionInstances;

        public event Action<Module> OnModuleInitialized;
        public event Action<IContext> OnStarting;
        public event Action<IContext> OnStarted;
        public event Action<IContext> OnLoaded;

        public EngineContext()
        {
            _extensions = new List<Type>();
            _modules = new List<Type>();
            _moduleInstances = new Dictionary<Type, Module>();
            _extensionInstances = new Dictionary<Type, Extension>();
        }

        public IContext AddModule<T>() where T : Module, new()
        {
            _modules.Add(typeof(T));
            return this;
        }

        public IContext AddExtension<T>() where T : Extension, new()
        {
            _extensions.Add(typeof(T));
            return this;
        }
        
        public bool TryGetExtension<T>(out T extension) where T : Extension
        {
            if (_extensionInstances.TryGetValue(typeof(T), out var extensionInstance))
            {
                extension = (T)extensionInstance;
                return true;
            }

            extension = default;
            return false;
        }

        public void Start()
        {
            var startModulesCount = _modules.Count;
            
            InitializeExtensions();
            InitializeModules(startModulesCount);
            OnStarting?.Invoke(this);
            
            OnStarted?.Invoke(this);
            EngineCore.OnContextStartedHandler(this);
            
            OnLoaded?.Invoke(this);
        }

        private void InitializeModules(int extensionModulesStartIndex)
        {
            for (var i = extensionModulesStartIndex; i < _modules.Count; i++)
                InitializeModule(_modules[i]);

            var lastModuleIndex = _modules.Count;
            
            for (var i = 0; i < extensionModulesStartIndex; i++)
                InitializeModule(_modules[i]);

            if (_modules.Count > lastModuleIndex)
            {
                for (var i = lastModuleIndex; i < _modules.Count; i++)
                    InitializeModule(_modules[i]);    
            }
        }

        private void InitializeExtensions()
        {
            foreach (var extensionType in _extensions)
            {
                if (!_extensionInstances.TryGetValue(extensionType, out var extension))
                {
                    extension = (Extension)Activator.CreateInstance(extensionType);
                    _extensionInstances.Add(extensionType, extension);
                }

                extension.SetContext(this);
            }

            foreach (var extension in _extensionInstances.Values)
                extension.Initialize();

            foreach (var extension in _extensionInstances.Values)
                extension.Setup();
        }

        private void InitializeModule(Type moduleType)
        {
            var module = (Module)Activator.CreateInstance(moduleType);
            module.SetContext(this);
            
            OnModuleInitialized?.Invoke(module);
            _moduleInstances.Add(moduleType, module);
        }
    }
}
