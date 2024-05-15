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
        public static void Compose(Action<Type, ISystem[], World> addSystems, World world, CubeDataConfig dataConfig, CubeEntityFactory factory)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchInitialization), new ISystem[]
            {
                new CubeStartupSystem(dataConfig, factory)
            }, world);

            addSystems(typeof(PlayerLoopArchHelper.ArchUpdateSimulation), new ISystem[]
            {
                new CubeFaceDirectionSystem(),
                new CubeMoveSystem(),
                new CalculateDistanceTraveledSystem(),
                new DestroyOnDistanceTraveled()
            }, world);
        }
    }
}