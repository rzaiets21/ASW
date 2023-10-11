using Commands.HUD;
using Commands.Screens;
using Commands.SpinWheel;
using Core.Attributes;
using Core.Commands.Binding;
using Core.HUD;
using Core.Injection;
using Core.Modules;
using Core.Screens;
using Features.SpinWheel.Impl;

namespace Features.SpinWheel
{
    public sealed class SpinWheelModule : Module
    {
        [Inject] private IInjectionBinder InjectionBinder { get; set; }
        [Inject] private ICommandBinder CommandBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<ISpinWheelController, SpinWheelController>();

            CommandBinder.Bind(SpinWheelEvent.Enter)
                .To<HideScreenCommand, Screen>(Screens.MainMenu)
                .To<LoadSpinWheelSceneCommand>()
                .To<ShowHUDCommand, HUD>(HUDs.GameHud)
                .InSequence();
        }
    }
}