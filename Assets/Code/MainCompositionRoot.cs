using System;
using System.Collections.Generic;
using System.Linq;
using Arch.Buffer;
using Arch.Core;
using Code._Arch.Arch.Infrastructure;
using Code._Arch.Arch.PlayerLoopIntegration;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.AppLayer;
using Code.CubeLayer;
using Code.UtilityLayer;
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

        public void Create<T>(T contextHolder)
        {
            _world = World.Create();
            _buffer = new(256);
        }

        public void Init<T>(T contextHolder)
        {
            if (_initialized)
            {
                return;
            }

            Compose(contextHolder as MainContext);
        }

        private void Compose(MainContext contextHolder)
        {
            ResourceStorage<GameObject> resourceStorage = new ResourceStorage<GameObject>();
            IViewHandler<GameObject> cubeViewHandler = new CubeViewHandler(contextHolder.DataConfigSo.DefaultConfig.Prefab, 16);
            int cubeResourceId = resourceStorage.Add(cubeViewHandler);
            EntityInstanceStorage<GameObject> gameObjectInstanceStorage = new EntityInstanceStorage<GameObject>(resourceStorage);

            CubeEntityFactory cubeEntityFactory = new(cubeResourceId, gameObjectInstanceStorage);

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

                CubeLayerComposer.Compose(AddSystemGroup, _world, contextHolder.DataConfigSo.DefaultConfig, cubeEntityFactory);
                ViewSyncLayerComposer.Setup(AddSystemGroup, _world, gameObjectInstanceStorage);
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