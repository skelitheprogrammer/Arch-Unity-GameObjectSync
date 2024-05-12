using Code._Arch.Arch.Infrastructure.Unity;
using Code.UtilityLayer.DataSources;

namespace Code
{
    public class MainContext : UnityContext<MainCompositionRoot>
    {
        public CubeSpawnDataSo SpawnDataSo;
    }
}