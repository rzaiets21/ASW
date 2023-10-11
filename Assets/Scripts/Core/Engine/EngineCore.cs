using System;
using Core.Extensions;

namespace Core.Engine
{
    public static class EngineCore
    {
        public static event Action<IContext> OnContextStarting;
        public static event Action<IContext> OnContextStarted;

        private static IContext _context;

        public static IContext Context()
        {
            _context = new EngineContext();
            _context.AddExtension<CoreExtension>();

            return _context;
        }

        internal static void OnContextStartingHandler(IContext context) { OnContextStarting?.Invoke(context); }
        internal static void OnContextStartedHandler(IContext context)  { OnContextStarted?.Invoke(context); }
    }
}