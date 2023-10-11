using System;
using Core.ContextView;
using Core.Engine;
using Core.Extensions;
using Core.Mediation;
using UnityEngine;

namespace Core.View.Impl
{
    public abstract class UnityView : MonoBehaviour, IUnityView
    {
        public IMediator Mediator { get; private set; }
        public bool Initialized { get; private set; }

        private IContext _context;
        private IMediatorBinder _mediationBinder;
        
        public void SetMediator(IMediator mediator)
        {
            Mediator = mediator;
        }
        
        protected virtual void Awake()
        {
            if (TryFindContext(out var context))
                ProcessContextIntegration(context);
            else
                EngineCore.OnContextStarted += ProcessContextIntegration;
        }

        private void OnEnable()
        {
            OnEnabled();
            
            if(!Initialized)
                return;
            
            Mediator?.OnEnable();
        }

        private void OnDisable()
        {
            OnDisabled();
         
            if(!Initialized)
                return;
            
            Mediator?.OnDisable();
        }

        private void OnDestroy()
        {
            DisposeContextIntegration();
        }

        protected virtual void OnEnabled() {}
        protected virtual void OnDisabled() {}
        
        private void ProcessContextIntegration(IContext context)
        {
            EngineCore.OnContextStarted -= ProcessContextIntegration;
            _context = context;

            if (!_context.TryGetExtension(out CoreExtension coreExtension))
            {
                Destroy(gameObject);
                return;
            }
            
            _mediationBinder = coreExtension.MediatorBinder;
            _mediationBinder.OnViewAdd(this);

            Initialized = true;
        }

        private void DisposeContextIntegration()
        {
            if (_mediationBinder != null)
            {
                _mediationBinder?.OnViewRemove(this);
                _mediationBinder = null;
            }

            _context = null;

            Initialized = false;
        }

        private static bool TryFindContext(out IContext context)
        {
            var contextGameObject = GameObject.Find(AppExtension.RootGameObjectName);
            
            if (contextGameObject != null)
            {
                context = contextGameObject.GetComponent<ContextViewComponent>().Context;
                return true;
            }
            
            context = null;
            return false;
        }
    }
}