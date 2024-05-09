using Arch.Buffer;
using Arch.Core;
using Code._Arch.Arch.System;

namespace Code.AppLayer
{
    public class ExecuteCommandBufferSystem : ISystem
    {
        private readonly CommandBuffer _buffer;

        public ExecuteCommandBufferSystem(CommandBuffer buffer)
        {
            _buffer = buffer;
        }

        public void Execute(World world)
        {
            _buffer.Playback(world);
        }
    }
}