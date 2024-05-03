using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Code.Arch.Arch.View
{
    public class AddressablesLoader : IResourceLoader
    {
        public async Task<T> Load<T>(object key) => await Addressables.LoadAssetAsync<T>(key).Task;

        public void Unload<T>(T value) => Addressables.Release(value);
    }
}