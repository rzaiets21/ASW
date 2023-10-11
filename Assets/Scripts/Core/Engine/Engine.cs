using Core.Attributes;
using Core.Extensions;
using Core.Modules.UI.Layers;
using Core.View.Impl;
using Modules;
using UnityEngine;

namespace Core.Engine
{
    public sealed class Engine : UnityView
    {
        [Inject] private IUILayersController UILayersController { get; set; }
     
        [Header("App Layers"), SerializeField] private GameObject canvasScreens;
        [SerializeField]                       private GameObject canvasScreensExpand;
        [SerializeField]                       private GameObject canvasHUD;

        protected override void Awake()
        {
            base.Awake();
            EngineCore.Context()
                .AddExtension<AppExtension>()
                .AddModule<EngineModule>()
                .Start();
        }

        [PostConstruct]
        private void PostConstruct()
        {
            UILayersController.RegisterLayer(RootLayer.CanvasScreens, canvasScreens);
            UILayersController.RegisterLayer(RootLayer.CanvasScreensExpand, canvasScreensExpand);
            UILayersController.RegisterLayer(RootLayer.CanvasHUD, canvasHUD);
        }
    }
}
