using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.Buffer;
using Arch.Core;
using Code._Arch.Arch.EntityHandling;
using Code._Arch.Arch.Infrastructure;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code._Arch.Arch.View.Unity;
using Code.AppLayer;
using Code.CubeLayer;
using Code.UtilityLayer.DataSources;
using Code.ViewSyncLayer;
using PlayerLoopExtender;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Code
{
    public readonly struct SystemGroup
    {
        public readonly World World;
        public readonly ISystem[] Systems;

        public SystemGroup(World world, ISystem[] systems)
        {
            World = world;
            Systems = systems;
        }
    }

    public sealed class MainCompositionRoot : ICompositionRoot
    {
        private Dictionary<Type, SystemGroup[]> _map = new();
        private World _world;
        private CommandBuffer _buffer;

        private IDisposable[] _subscribers;

        private bool _initialized;

        public async void Init<T>(T contextHolder)
        {
            if (_initialized)
            {
                return;
            }

            await Compose(contextHolder as MainContext);
        }

        private async Task Compose(MainContext contextHolder)
        {
            _world = World.Create();
            _buffer = new(256);
            CubeSpawnData cubeSpawnData = contextHolder.SpawnDataSo.SpawnData;

            IResourceStorage resourceStorage = new ResourceStorage();
            int cubeResourceId = resourceStorage.Register(contextHolder.SpawnDataSo.SpawnData.Prefab);

            IViewFactory<GameObject> viewFactory = new GameObjectFactory(resourceStorage);
            IViewHandler<GameObject> cubeViewHandler = new CubePooledViewHandler(viewFactory, cubeResourceId);
            EntityInstanceHolder<GameObject> entityInstanceHolder = new EntityInstanceHolder<GameObject>();

            CubeEntityFactory cubeEntityFactory = new(cubeViewHandler, entityInstanceHolder);

            ConfigureSystems();
            AttachPlayerLoop();

            _subscribers = new IDisposable[]
            {
                _world,
                _buffer,
            };

            void ConfigureSystems()
            {
                Dictionary<Type, List<SystemGroup>> map = new()
                {
                    {typeof(PlayerLoopArchHelper.ArchInitialization), new List<SystemGroup>()},
                    {typeof(PlayerLoopArchHelper.ArchUpdateSimulation), new List<SystemGroup>()},
                    {typeof(PlayerLoopArchHelper.ArchSyncView), new List<SystemGroup>()},
                    {typeof(PlayerLoopArchHelper.ArchPostSimulation), new List<SystemGroup>()}
                };

                CubeLayerComposer.Compose(AddSystemGroup, _world, cubeSpawnData, cubeEntityFactory, cubeResourceId);
                ViewSyncLayerComposer.Setup(AddSystemGroup, _world, entityInstanceHolder);
                AppLayerComposer.Setup(AddSystemGroup, _world, _buffer);

                _map = map.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());

                void AddSystemGroup(Type type, ISystem[] systems, World world) => map[type].Add(new SystemGroup(world, systems));
            }
        }

        private void AttachPlayerLoop()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

            copyLoop.FindSystem(typeof(PlayerLoopArchHelper.ArchInitialization)).updateDelegate = () =>
            {
                if (_initialized)
                {
                    return;
                }

                lock (this)
                {
                    ExecuteSystems<PlayerLoopArchHelper.ArchInitialization>();
                    _initialized = true;
                }
            };
            copyLoop.FindSystem(typeof(PlayerLoopArchHelper.ArchUpdateSimulation)).updateDelegate = ExecuteSystems<PlayerLoopArchHelper.ArchUpdateSimulation>;
            copyLoop.FindSystem(typeof(PlayerLoopArchHelper.ArchSyncView)).updateDelegate = ExecuteSystems<PlayerLoopArchHelper.ArchSyncView>;
            copyLoop.FindSystem(typeof(PlayerLoopArchHelper.ArchPostSimulation)).updateDelegate = ExecuteSystems<PlayerLoopArchHelper.ArchPostSimulation>;

            void ExecuteSystems<T>() where T : struct
            {
                foreach (SystemGroup systemGroup in _map[typeof(T)])
                {
                    foreach (ISystem systemGroupSystem in systemGroup.Systems)
                    {
                        systemGroupSystem.Execute(systemGroup.World);
                    }
                }
            }

            PlayerLoop.SetPlayerLoop(copyLoop);
        }

        void IDisposable.Dispose()
        {
            foreach (IDisposable subscriber in _subscribers)
            {
                subscriber.Dispose();
            }

            _map.Clear();
        }
    }
}