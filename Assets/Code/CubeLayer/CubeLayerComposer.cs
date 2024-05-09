using System;
using Arch.Buffer;
using Arch.Core;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.CubeLayer.Systems;
using Code.UtilityLayer;
using UnityEngine;

namespace Code.CubeLayer
{
    public static class CubeLayerComposer
    {
        public static void Setup(Action<Type, ISystem[], World> addSystems, World simulationWorld, CommandBuffer buffer, CubeSpawnData spawnData, IViewHandler<GameObject> viewHandler)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchInitialization), new ISystem[]
            {
                new CubeStartupSystem(spawnData)
            }, simulationWorld);

            addSystems(typeof(PlayerLoopArchHelper.ArchUpdateSimulation), new ISystem[]
            {
                new CubeSpawnSystem(buffer, viewHandler),
                new CubeMoveSystem(),
                new CubeFaceDirectionSystem()
            }, simulationWorld);
        }
    }
}