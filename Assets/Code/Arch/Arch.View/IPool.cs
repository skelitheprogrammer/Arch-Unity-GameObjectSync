namespace Code.Arch.Arch.View
{
    public interface IPool<T>
    {
        T Rent();
        void Return(T view);
    }
}