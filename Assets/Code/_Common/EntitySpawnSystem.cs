using Arch.Buffer;
using Arch.Core;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;

namespace Code._Common
{
    public abstract class EntitySpawnSystem<TView> : ISystem
    {
        protected CommandBuffer Buffer;
        protected IViewHandler<TView> ViewHandler;

        protected EntitySpawnSystem(CommandBuffer buffer, IViewHandler<TView> viewHandler)
        {
            Buffer = buffer;
            ViewHandler = viewHandler;
        }

        public abstract void Execute(World world);
    }
}