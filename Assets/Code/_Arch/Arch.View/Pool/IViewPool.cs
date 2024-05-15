namespace Code._Arch.Arch.View.Pool
{
    public interface IViewPool<T>
    {
        T Rent();
        void Recycle(T view);
    }
}