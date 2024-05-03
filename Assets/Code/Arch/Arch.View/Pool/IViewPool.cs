namespace Code.Arch.Arch.View
{
    public interface IViewPool<T>
    {
        void PreAllocate(int size);
        PooledView<T> Allocate();
        void Return(T view);
    }
}