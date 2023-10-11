using Core.Attributes;
using Core.Commands;
using Core.HUD;

namespace Commands.HUD
{
    public sealed class ShowHUDCommand : Command<Core.HUD.HUD>
    {
        [Inject] private IHUDController HUDController { get; set; }
        
        protected override void Execute(Core.HUD.HUD hud)
        {
            HUDController.Show(hud);
        }
    }
}