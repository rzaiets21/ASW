using Core.Screens;

namespace Core.Modules.UI
{
    public abstract class UIControl
    {
        public readonly string Name;
        public readonly UIConfig UIConfig;

        protected UIControl(string name, UIConfig uiConfig)
        {
            Name = name;
            UIConfig = uiConfig;
        }
    }
}