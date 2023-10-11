using Core.ContextView;
using Core.Coroutines;
using Core.Coroutines.Impl;
using Core.Modules.UI;
using UnityEngine;

namespace Core.Extensions
{
    public sealed class AppExtension : Extension
    {
        public const string RootGameObjectName = "[AppExtension]";
        
        private GameObject _contextViewObject;
        
        public override void Initialize()
        {
            AddModule<UIModule>();
        }

        public override void Setup()
        {
            _contextViewObject = new GameObject();
            var viewAgent = _contextViewObject.AddComponent<ContextViewComponent>();
            viewAgent.SetContext(Context);

            Object.DontDestroyOnLoad(_contextViewObject);

            var injectionBinder = GetDependentExtension<CoreExtension>().InjectionBinder;
            injectionBinder.Bind<IContextView>(viewAgent).ConstructOnStart(false);
            injectionBinder.Bind<ICoroutineProvider, CoroutineProvider>();

            _contextViewObject.name = RootGameObjectName;
        }
    }
}