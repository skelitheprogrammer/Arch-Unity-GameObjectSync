namespace Code._Arch.Arch.View
{
    /// <summary>
    /// Describes how to Get and Remove particular View.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewHandler<T>
    {
        T Get();
        void Remove(T view);
    }
}