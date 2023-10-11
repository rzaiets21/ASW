using Core.Modules.UI;

namespace Core.Screens
{
    public sealed class Screen : UIControl
    {
        public readonly UIBehaviour UIBehaviour;

        public Screen(string name, UIBehaviour uiBehaviour, UIConfig uiConfig) : base(name, uiConfig)
        {
            UIBehaviour = uiBehaviour;
        }
    }
}
