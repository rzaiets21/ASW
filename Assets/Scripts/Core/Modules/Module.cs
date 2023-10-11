using Core.Engine;

namespace Core.Modules
{
    public abstract class Module
    {
        private IContext _context;

        internal void SetContext(IContext context)
        {
            _context = context;
        }
        
        protected void AddModule<T>() where T : Module, new()
        {
            _context.AddModule<T>();
        }
    }
}
