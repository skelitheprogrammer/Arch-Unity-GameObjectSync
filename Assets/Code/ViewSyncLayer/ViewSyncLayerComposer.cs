using System;
using Arch.Core;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using UnityEngine;

namespace Code.ViewSyncLayer
{
    public static class ViewSyncLayerComposer
    {
        public static void Setup(Action<Type, ISystem[], World> addSystems, World world, EntityInstanceStorage<GameObject> instanceStorage)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchSyncView), new ISystem[]
            {
                new ViewPositionSyncSystem(instanceStorage),
                new ViewRotationSyncSystem(instanceStorage),
                new ViewStateSync(instanceStorage)
            }, world);
        }
    }
}