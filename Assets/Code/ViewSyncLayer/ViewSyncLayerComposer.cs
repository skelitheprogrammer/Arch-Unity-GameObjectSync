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
        public static void Setup(Action<Type, ISystem[], World> addSystems, World world, EntityInstanceHolder<GameObject> holder)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchSyncView), new ISystem[]
            {
                new ViewPositionSyncSystem(holder),
                new ViewRotationSyncSystem(holder)
            }, world);
        }
    }
}