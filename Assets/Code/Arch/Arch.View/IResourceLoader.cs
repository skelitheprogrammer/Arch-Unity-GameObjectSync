using System.Threading.Tasks;

namespace Code.Arch.Arch.View
{
    public interface IResourceLoader
    {
        Task<T> Load<T>(object key);
        void Unload<T>(T value);
    }
}