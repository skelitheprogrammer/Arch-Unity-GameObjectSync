using Code._Arch.Arch.Infrastructure.Unity;
using Code.CubeLayer;
using Code.UtilityLayer;
using UnityEngine.AddressableAssets;

namespace Code
{
    public class MainContext : UnityContext<MainCompositionRoot>
    {
        public AssetReferenceT<CubeSpawnDataSo> SpawnData;
    }
}