using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;
using Code.System;
using UnityEngine;

namespace Code.Arch.Arch.View
{
    public struct ViewComponent<T>
    {
        public T View;
    }

    public interface IResourceLoader<TKey, TValue>
    {
        Task<TValue> Load(TKey key);
        void Unload(TValue resource);
    }

    public interface IViewFactory<T>
    {
        T Create(in Entity entity, in T resource);
    }

    public class GameObjectViewFactory : IViewFactory<GameObject>
    {
        public GameObject Create(in Entity entity, in GameObject prefab)
        {
            GameObject instance = Object.Instantiate(prefab);

            GameObjectEntityReferenceHolder referenceHolder = instance.AddComponent<GameObjectEntityReferenceHolder>();

            referenceHolder.Init(entity.Reference());

            return instance;
        }
    }

    public struct ViewInitializer<T>
    {
        public T Resource;
    }
    
    public class GameObjectViewHandlerSystem : ISystem
    {
        public GameObjectViewHandlerSystem(GameObjectViewFactory factory)
        {
            _foreach = new SpawnView(factory);
        }

        private SpawnView _foreach;

        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewInitializer<GameObject>>();

        public void Execute(World world)
        {
            world.InlineQuery(_description, ref _foreach);
        }

        private readonly struct SpawnView : IForEach
        {
            private readonly GameObjectViewFactory _factory;

            public SpawnView(GameObjectViewFactory factory)
            {
                _factory = factory;
            }

            public void Update(Entity entity)
            {
                ViewInitializer<GameObject> viewInitializer = entity.Get<ViewInitializer<GameObject>>();
                Entity newEntity = entity.GetWorld().Create();
                GameObject instance = _factory.Create(newEntity, viewInitializer.Resource);

                newEntity.Add(new ViewComponent<GameObject>
                {
                    View = instance,
                });

                entity.Destroy();
            }
        }
    }
}