using Arch.Core;
using Arch.Core.Extensions;
using Code.System;

namespace Code.EngineSync
{
    public class LinkGameObjectToEntitySystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>();
        private GetView _getView;

        public LinkGameObjectToEntitySystem(GameObjectResourceManager manager)
        {
            _getView = new GetView(manager);
        }

        public void Execute(World world)
        {
            world.InlineQuery(in _description, ref _getView);
        }

        private readonly struct GetView : IForEach
        {
            private readonly GameObjectResourceManager _manager;

            public GetView(GameObjectResourceManager manager)
            {
                _manager = manager;
            }

            public void Update(Entity entity)
            {
                ref ViewReference viewReference = ref entity.Get<ViewReference>();
                bool isLoaded = viewReference.IsLoaded;

                if (!isLoaded)
                {
                    _manager.Get(viewReference.Key, entity.Reference());
                    viewReference.IsLoaded = true;
                }
            }
        }
    }
}