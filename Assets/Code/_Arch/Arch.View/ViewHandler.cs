namespace Code._Arch.Arch.View
{
    public interface IViewHandler<T>
    {
        T Resource { get; }

        public T GetInstance();
        void RemoveInstance(T instance);
    }
}