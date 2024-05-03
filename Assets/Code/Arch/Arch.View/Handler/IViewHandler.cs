namespace Code.Arch.Arch.View
{
    public interface IViewHandler<T>
    {
        void SetActiveState(in T view, bool state);
        void Destroy(T view);
        T Create(T resource);
    }
}