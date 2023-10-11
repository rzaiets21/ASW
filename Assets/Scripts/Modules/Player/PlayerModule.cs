using Core.Attributes;
using Core.Injection;
using Core.Modules;
using Modules.Player.Impl;

namespace Modules.Player
{
    public sealed class PlayerModule : Module
    {
        [Inject] private IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IPlayerController, PlayerController>();
        }
    }
}