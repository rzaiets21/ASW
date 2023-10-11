using Core.Modules.UI;
using Core.Modules.UI.Layers;

namespace Core.HUD
{
    public static class HUDs
    {
        public static HUD GameHud = new ("Game HUD", new UIConfig("GameHUD", RootLayer.CanvasHUD));
    }
}