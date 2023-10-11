using Core.Attributes;
using Core.HUD;
using Core.HUD.Impl;
using Core.Injection;
using Core.Modules.UI.Layers;
using Core.Modules.UI.Layers.Impl;
using Core.Screens;
using Core.Screens.Impl;

namespace Core.Modules.UI
{
    public sealed class UIModule : Module
    {
        [Inject] private IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IScreensController, ScreensController>();
            InjectionBinder.Bind<IHUDController, HUDController>();
            InjectionBinder.Bind<IUILayersController, UILayersController>();
        }
    }
}
