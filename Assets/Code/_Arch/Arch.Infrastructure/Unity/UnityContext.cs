using System.Collections;
using UnityEngine;

namespace Code._Arch.Arch.Infrastructure.Unity
{
    public abstract class UnityContext : MonoBehaviour
    {
    }

    public class UnityContext<T> : UnityContext where T : class, ICompositionRoot, new()
    {
        private T _root;

        private void Awake()
        {
            _root = new T();

            _root.Create(this);
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(WaitForFrameworkInitialization());
            }
        }

        private void OnDestroy()
        {
            _root.Dispose();
        }

        private IEnumerator WaitForFrameworkInitialization()
        {
            yield return new WaitForEndOfFrame();
            _root.Init();
        }
    }
}