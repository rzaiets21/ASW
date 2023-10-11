namespace Core.Modules.UI
{
    public sealed class UIConfig
    {
        public readonly string PrefabName;
        public readonly int Layer;

        public UIConfig(string prefabName, int layer)
        {
            PrefabName = prefabName;
            Layer = layer;
        }
    }
}
