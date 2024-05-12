using System;
using Arch.Core;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;
using Code.CubeLayer.Systems;
using Code.UtilityLayer.DataSources;

namespace Code.CubeLayer
{
    public static class CubeLayerComposer
    {
        public static void Compose(Action<Type, ISystem[], World> addSystems, World world, CubeSpawnData spawnData, CubeEntityFactory factory, int resourceId)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchInitialization), new ISystem[]
            {
                new CubeStartupSystem(spawnData, factory, resourceId)
            }, world);

            addSystems(typeof(PlayerLoopArchHelper.ArchUpdateSimulation), new ISystem[]
            {
                new CubeFaceDirectionSystem(),
                new CubeSineWaveDirectionUpdateSystem(),
                new CubeMoveSystem(),
            }, world);
        }
    }
}