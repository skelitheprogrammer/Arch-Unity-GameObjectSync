using System;
using Arch.Buffer;
using Arch.Core;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;

namespace Code.AppLayer
{
    public static class AppLayerComposer
    {
        public static void Setup(Action<Type, ISystem[], World> addSystems, World simulationWorld, CommandBuffer buffer)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchPostSimulation), new ISystem[]
            {
                new ExecuteCommandBufferSystem(buffer),
            }, simulationWorld);
        }
    }
}