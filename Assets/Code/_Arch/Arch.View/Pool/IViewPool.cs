namespace Code._Arch.Arch.View
{
    public interface IViewPool<T>
    {
        T Rent();
        void Recycle(T view);
    }
}