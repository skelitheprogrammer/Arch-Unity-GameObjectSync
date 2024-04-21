﻿using System;
using System.Collections.Generic;
using Arch.Core;
using Code.EngineSync;
using Code.MoveEntity;
using Code.Spawning;
using Code.System;
using Code.TestSystems;
using PlayerLoopExtender;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Code
{
    public delegate bool Predicate();

    public sealed class MockGame : IDisposable
    {
        private readonly struct ArchInitialization
        {
        }

        private readonly struct ArchUpdateSimulation
        {
        }

        private readonly struct ArchSyncView
        {
        }

        private readonly World _world;

        private bool _initialized;
        private bool _isRunning;

        public MockGame()
        {
            _world = World.Create();
        }

        public void Start()
        {
            if (!_initialized)
            {
                Configure();
            }

            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void Configure()
        {
            GameObjectResourceManager manager = new(Object.Instantiate, go => go.SetActive(true), go => { go.SetActive(false); });

            StartupSystem startupSystem = new(manager);

            //this system can be cached and used somewhere else, without creating duplicates
            Dictionary<Predicate, ISystem[]> conditionalSystems = new()
            {
                {() => Input.GetMouseButtonDown(0), new ISystem[] {new RemoveCubeSystem()}},
                {() => Input.GetMouseButtonDown(1), new ISystem[] {new AddCubeSystem()}}
            };

            ISystem[] simulationSystems =
            {
                new SpawnCubesSystem(),
                new UpdatePositionSystem(),
            };

            ISystem[] syncSystems =
            {
                new LinkGameObjectToEntitySystem(manager),
                new SyncGameObjectState(manager),
                new SyncPositionsSystem(manager),
                new SyncRotationSystem(manager),
            };

            AttachToPlayerLoop();

            void AttachToPlayerLoop()
            {
                PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

                object locker = new();
                PlayerLoopSystem initializationPlayerLoopSystem = new()
                {
                    type = typeof(ArchInitialization),
                    updateDelegate = () =>
                    {
                        if (_initialized) return;

                        lock (locker)
                        {
                            startupSystem.Execute(_world);
                            _initialized = true;
                        }
                    }
                };

                PlayerLoopSystem simulationPlayerLoopSystem = new()
                {
                    type = typeof(ArchUpdateSimulation),
                    updateDelegate = () =>
                    {
                        if (!_isRunning)
                        {
                            return;
                        }

                        foreach (Predicate condition in conditionalSystems.Keys)
                        {
                            if (condition())
                            {
                                IterateSystems(conditionalSystems[condition], _world);
                            }
                        }

                        IterateSystems(simulationSystems, _world);
                    }
                };

                PlayerLoopSystem syncPlayerLoopSystem = new()
                {
                    type = typeof(ArchSyncView),
                    updateDelegate = () =>
                    {
                        if (!_isRunning)
                        {
                            return;
                        }

                        IterateSystems(syncSystems, _world);
                    }
                };

                Type scriptRun = typeof(Update.ScriptRunBehaviourUpdate);
                copyLoop.InsertSystem(initializationPlayerLoopSystem, typeof(Initialization.SynchronizeInputs), PlayerLoopSystemExtensions.InsertType.BEFORE);
                copyLoop.InsertSystem(simulationPlayerLoopSystem, scriptRun, PlayerLoopSystemExtensions.InsertType.BEFORE);
                copyLoop.InsertSystem(syncPlayerLoopSystem, scriptRun, PlayerLoopSystemExtensions.InsertType.AFTER);

                PlayerLoop.SetPlayerLoop(copyLoop);
            }
        }

        private static async void IterateSystems(IEnumerable<ISystem> systems, World world)
        {
            foreach (ISystem system in systems)
            {
                if (system is IAsyncSystem asyncSystem)
                {
                    await asyncSystem.Execute(world);
                }

                system.Execute(world);
            }
        }

        void IDisposable.Dispose()
        {
            Stop();

            _world.Dispose();

            PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
        }
    }
}