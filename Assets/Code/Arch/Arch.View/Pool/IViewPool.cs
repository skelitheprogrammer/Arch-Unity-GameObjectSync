namespace Code.Arch.Arch.View
{
    public interface IViewPool<T>
    {
        void PreWarm(int size);
        PooledView<T> Rent();
        void Return(T view);
    }
}