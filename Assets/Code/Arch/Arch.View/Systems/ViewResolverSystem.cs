using System;
using Arch.Core;
using Code.System;

namespace Code.Arch.Arch.View.Systems
{
    public class ViewResolverSystem<T> : ISystem
    {
        private Func<T, T> _viewGetter;

        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewInitializer<T>>();

        public void Execute(World world)
        {
        }
    }

    public struct ViewInitializer<T>
    {
        public EntityReference Reference;
        public T Resource;
    }
}