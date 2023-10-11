using Core.Engine;

namespace Core.ContextView
{
    public interface IContextView
    {
        IContext Context { get; }
        object ViewRaw { get; }

        T As<T>() where T : class;
    }
}