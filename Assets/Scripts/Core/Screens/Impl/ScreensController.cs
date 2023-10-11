using Core.Attributes;
using Core.Events;

namespace Core.Screens.Impl
{
    public sealed class ScreensController : UIController<Screen>, IScreensController
    {
        [Inject] private IEventMap EventMap { get; set; }
        
        public Screen CurrentScreen { get; private set; }

        public void Show(Screen screen)
        {
            ShowImpl(screen);
        }

        public void Hide(Screen screen)
        {
            HideImpl(screen);
        }

        private void ShowImpl(Screen screen)
        {
            if (screen == CurrentScreen)
                return;

            if (CurrentScreen != null)
            {
                HideImpl(CurrentScreen);
                CurrentScreen = null;
            }
            
            var instance = GetScreenInstance(screen);
            if (instance == null)
                return;

            CurrentScreen = screen;
            EventMap.Dispatch(ScreenEvent.Shown, screen);
        }
        
        private void HideImpl(Screen screen)
        {
            if(screen != CurrentScreen)
                return;
            
            if (!Deactivate(screen))
                return;

            EventMap.Dispatch(ScreenEvent.Hidden, screen);
        }
    }
}