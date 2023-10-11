using System.Collections;
using UnityEngine;

namespace Core.Coroutines
{
    public interface ICoroutineProvider
    {
        Coroutine StartCoroutine(IEnumerator enumerator);

        void StopCoroutine(Coroutine coroutine);
    }
}