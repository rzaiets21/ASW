using Commands.App;
using Core.Attributes;
using Core.Commands.Binding;
using Core.Events;
using Core.Modules;
using Features.SpinWheel;
using Modules.App;
using Modules.Player;

namespace Modules
{
    public sealed class EngineModule : Module
    {
        [Inject] private ICommandBinder CommandBinder { get; set; }
        
        [PostConstruct]
        private void PostConstruct()
        {
            AddModule<PlayerModule>();
            AddModule<SpinWheelModule>();
            AddModule<AppModule>();

            CommandBinder.Bind(ContextEvent.Loaded)
                .To<AppInitializeCommand>()
                .OnComplete(AppEvent.Start);
        }
    }
}