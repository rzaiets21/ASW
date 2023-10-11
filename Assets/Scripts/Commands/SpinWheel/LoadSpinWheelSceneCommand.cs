using System.Collections;
using Core.Attributes;
using Core.Commands;
using Core.Coroutines;
using UnityEngine.SceneManagement;

namespace Commands.SpinWheel
{
    public sealed class LoadSpinWheelSceneCommand : Command
    {
        private const string SceneName = "SpinWheel";
        
        [Inject] private ICoroutineProvider CoroutineProvider { get; set; }
        
        protected override void Execute()
        {
            Retain();

            CoroutineProvider.StartCoroutine(LoadSpinWheelSceneCoroutine());
        }

        private IEnumerator LoadSpinWheelSceneCoroutine()
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            if (operation == null)
                yield break;

            while (!operation.isDone)
                yield return null;

            OnSceneLoaded();
        }

        private void OnSceneLoaded()
        {
            Release();
        }
    }
}