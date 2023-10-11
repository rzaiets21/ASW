using System.Collections;
using Core.Attributes;
using Core.ContextView;
using UnityEngine;

namespace Core.Coroutines.Impl
{
    public sealed class CoroutineProvider : ICoroutineProvider
    {
        [Inject] private IContextView ContextView { get; set; }
        
        private MonoBehaviour _agent;

        [PostConstruct]
        public void PostConstruct()
        {
            _agent = ContextView.As<MonoBehaviour>();
        }
        
        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return _agent.StartCoroutine(enumerator);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _agent.StopCoroutine(coroutine);
        }
    }
}