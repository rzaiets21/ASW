using Core.Mediation;
using UnityEngine;

namespace TestScreen
{
    public class TestScreenMediator : IMediator
    {
        public void OnEnable()
        {
            Debug.LogError("OnEnable");
        }

        public void OnDisable()
        {
            Debug.LogError("OnDisable");
        }
    }
}