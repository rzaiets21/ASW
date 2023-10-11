using Core.Commands.Binding;
using Core.Commands.Binding.Impl;
using Core.Engine;
using Core.Events;
using Core.Events.Impl;
using Core.Injection;
using Core.Injection.Impl;
using Core.Mediation;
using Core.Mediation.Impl;
using Core.Modules;

namespace Core.Extensions
{
    public sealed class CoreExtension : Extension
    {
        public IInjectionBinder InjectionBinder { get; }
        public IMediatorBinder MediatorBinder { get; }
        public IEventMap EventMap { get; }
        public ICommandBinder CommandBinder { get; }
        
        public CoreExtension()
        {
            CommandBinder = new CommandBinder();
            InjectionBinder = new InjectionBinder();
            EventMap = new EventMap((CommandBinder)CommandBinder);
            MediatorBinder = new MediatorBinder(InjectionBinder);
        }
        
        public override void Initialize()
        {
            Context.OnModuleInitialized += OnModuleInitialized;
            Context.OnStarted += OnContextStarted;
            Context.OnLoaded += OnContextLoaded;
            Context.OnStarting += OnContextStarting;
            
            InjectionBinder.Bind(EventMap);
            InjectionBinder.Bind(InjectionBinder);
            InjectionBinder.Bind(CommandBinder).ConstructOnStart(true);
            InjectionBinder.Bind(MediatorBinder);
        }

        private void OnContextStarting(IContext context)
        {
            InjectionBinder.ForEachBinding(binding =>
            {
                if (binding.ToConstruct)
                {
                    InjectionBinder.GetInstance(binding);
                }
            });
        }
        
        private void OnContextStarted(IContext context)
        {
            EventMap.Dispatch(ContextEvent.Started);
        }
        
        private void OnContextLoaded(IContext context)
        {
            EventMap.Dispatch(ContextEvent.Loaded);
        }

        public override void Dispose()
        {
            Context.OnModuleInitialized -= OnModuleInitialized;
            Context.OnStarted -= OnContextStarted;
            Context.OnLoaded -= OnContextLoaded;
            Context.OnStarting -= OnContextStarting;
        }
        
        private void OnModuleInitialized(Module module)
        {
            InjectionBinder.Construct(module);
        }
    }
}