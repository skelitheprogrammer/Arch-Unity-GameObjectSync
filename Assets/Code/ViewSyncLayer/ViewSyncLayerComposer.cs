using System;
using Arch.Core;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;

namespace Code.ViewSyncLayer
{
    public static class ViewSyncLayerComposer
    {
        public static void Setup(Action<Type, ISystem[], World> addSystems, World world)
        {
            addSystems(typeof(PlayerLoopArchHelper.ArchSyncView), new ISystem[]
            {
                new ViewPositionSyncSystem(),
                new ViewRotationSyncSystem()
            }, world);
        }
    }
}