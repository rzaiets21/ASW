using Commands.Screens;
using Core.Attributes;
using Core.Commands.Binding;
using Core.Screens;
using Core.Modules;

namespace Modules.App
{
    public sealed class AppModule : Module
    {
        [Inject] private ICommandBinder CommandBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            CommandBinder.Bind(AppEvent.Start)
                .To<ShowScreenCommand, Screen>(Screens.MainMenu);
        }
    }
}