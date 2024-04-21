using System;
using UnityEngine;

namespace Code
{
    public static class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Main()
        {
            MockGame mockGame = new();
            IDisposable disposable = mockGame;

            Application.quitting -= disposable.Dispose;
            Application.quitting += disposable.Dispose;

            mockGame.Start();
        }
    }
}