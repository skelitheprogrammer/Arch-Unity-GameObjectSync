using System;
using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class ViewHandler<T> : IViewHandler<T>
    {
        private readonly Func<T> _getter;
        private readonly Action<T> _remover;

        public ViewHandler(Func<T> getter, Action<T> remover)
        {
            _getter = getter;
            _remover = remover;
        }

        public T Get()
        {
            return _getter();
        }

        public void Remove(T view)
        {
            _remover(view);
        }
    }

    public class CubePooledViewHandler : IViewHandler<GameObject>
    {
        private readonly IViewPool<GameObject> _pool;

        public CubePooledViewHandler(IViewFactory<GameObject> factory, int resourceId)
        {
            _pool = new ViewPool<GameObject>(factory.Create, OnGet, OnReturn, null, resourceId);
        }

        GameObject IViewHandler<GameObject>.Get() => _pool.Rent();
        void IViewHandler<GameObject>.Remove(GameObject view) => _pool.Recycle(view);

        private static void OnGet(GameObject instance)
        {
            instance.SetActive(true);
        }

        private static void OnReturn(GameObject instance)
        {
            instance.SetActive(false);
        }
    }
}