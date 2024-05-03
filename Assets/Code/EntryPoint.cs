using System;
using UnityEngine;

namespace Code
{
    public static class EntryPoint
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Main()
        {
            CubeSphereRandomFlyContext cubeSphereRandomFlyContext = new();
            IDisposable disposable = cubeSphereRandomFlyContext;

            Application.quitting -= disposable.Dispose;
            Application.quitting += disposable.Dispose;

            cubeSphereRandomFlyContext.Start();
        }
    }
}