using Core.Attributes;
using Core.Commands;
using Core.Screens;

namespace Commands.Screens
{
    public sealed class HideScreenCommand : Command<Screen>
    {
        [Inject] private IScreensController ScreensController { get; set; }
        
        protected override void Execute(Screen screen)
        {
            ScreensController.Hide(screen);
        }
    }
}