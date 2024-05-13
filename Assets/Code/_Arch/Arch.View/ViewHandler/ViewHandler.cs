using System;

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
}