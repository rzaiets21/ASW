using Core.Commands;
using UnityEngine;

namespace Commands.App
{
    public sealed class AppInitializeCommand : Command
    {
        protected override void Execute()
        {
            Application.targetFrameRate = 60;
        }
    }
}