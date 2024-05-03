using Arch.Core;
using Arch.Core.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Arch.Arch.View
{
    public class ContextBehaviour : MonoBehaviour
    {
        private World _world;

        private GameObjectViewHandlerSystem _viewHandler;
        public AssetReference Reference;

        private void Awake()
        {
            _world = World.Create();
            GameObjectViewFactory viewFactory = new();
            _viewHandler = new GameObjectViewHandlerSystem(viewFactory);
        }

        private async void Start()
        {
            var result = await Reference.LoadAssetAsync<GameObject>().Task;
            _world.Create().Add(new ViewInitializer<GameObject>()
            {
                Resource = result
            });
        }

        private void Update()
        {
            _viewHandler.Execute(_world);
        }
    }
}