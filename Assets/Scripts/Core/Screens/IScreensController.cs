namespace Core.Screens
{
    public interface IScreensController
    {
        Screen CurrentScreen { get; }

        void Show(Screen screen);
        void Hide(Screen screen);
    }
}
