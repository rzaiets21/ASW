using Core.Modules.UI;
using Core.Modules.UI.Layers;

namespace Core.Screens
{
    public static class Screens
    {
        public static Screen MainMenu = new("Main Menu", UIBehaviour.DeactivateOnClose, new UIConfig("MainMenu", RootLayer.CanvasScreens));
    }
}
