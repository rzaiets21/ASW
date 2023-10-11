using System;
using System.Collections.Generic;
using Core.Attributes;
using Core.Injection;
using Core.View;
using UnityEngine;

namespace Core.Mediation.Impl
{
    public sealed class MediatorBinder : IMediatorBinder
    {
        private readonly Dictionary<IUnityView, IMediator> _mediators;

        private readonly IInjectionBinder _injectionBinder;
        
        public MediatorBinder(IInjectionBinder injectionBinder)
        {
            _injectionBinder = injectionBinder;

            _mediators = new Dictionary<IUnityView, IMediator>();
        }
        
        public void OnViewAdd(IUnityView view)
        {
            if (_mediators.ContainsKey(view))
                return;

            var type = view.GetType();

            _injectionBinder.Construct(view);

            var bindingInfo = GetMediationBindingInfo(type);
            
            if (TryCreateMediator(bindingInfo, out var mediator))
            {
                view.SetMediator(mediator);
                InjectViewAndDependencies(bindingInfo, view, mediator);
            }

            _mediators.Add(view, mediator);
        }

        public void OnViewRemove(IUnityView view)
        {
            if (!_mediators.TryGetValue(view, out var mediator))
                return;

            if (mediator != null)
                _injectionBinder.Destroy(mediator);

            _injectionBinder.Destroy(view);

            _mediators.Remove(view);
        }

        private MediatorBindingInfo GetMediationBindingInfo(Type viewType)
        {
            var injections = viewType.GetCustomAttributes(typeof(MediatorAttribute), true);
            if (injections.Length == 0)
            {
                return null;
            }

            var attribute = (MediatorAttribute)injections[0];

            return new MediatorBindingInfo(viewType, attribute.MediatorType);
        }
        
        private bool TryCreateMediator(IMediatorBindingInfo binding, out IMediator mediator)
        {
            if (binding != null && binding.MediatorType != null)
            {
                mediator = (IMediator)Activator.CreateInstance(binding.MediatorType);
                return true;
            }

            mediator = null;
            return false;
        }
        
        private void InjectViewAndDependencies(MediatorBindingInfo binding, IUnityView view, IMediator mediator)
        {
            var injectionType = binding.ViewType;
            
            _injectionBinder.Bind(injectionType, view);
            _injectionBinder.Construct(mediator);
            _injectionBinder.Unbind(injectionType);
        }
    }
}