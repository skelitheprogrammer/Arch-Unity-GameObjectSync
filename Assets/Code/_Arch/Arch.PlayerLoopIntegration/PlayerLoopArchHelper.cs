using System;
using PlayerLoopExtender;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Code._Arch.Arch.PlayerLoopIntegration
{
    internal static class PlayerLoopArchHelper
    {
        public readonly struct ArchInitialization
        {
            internal static PlayerLoopSystem Create(PlayerLoopSystem.UpdateFunction updateFunction = null) => new()
            {
                type = typeof(ArchInitialization),
                updateDelegate = updateFunction
            };
        }

        public readonly struct ArchUpdateSimulation
        {
            internal static PlayerLoopSystem Create(PlayerLoopSystem.UpdateFunction updateFunction = null) => new()
            {
                type = typeof(ArchUpdateSimulation),
                updateDelegate = updateFunction
            };
        }

        public readonly struct ArchSyncView
        {
            internal static PlayerLoopSystem Create(PlayerLoopSystem.UpdateFunction updateFunction = null) => new()
            {
                type = typeof(ArchSyncView),
                updateDelegate = updateFunction
            };
        }

        public readonly struct ArchPostSimulation
        {
            internal static PlayerLoopSystem Create(PlayerLoopSystem.UpdateFunction updateFunction = null) => new()
            {
                type = typeof(ArchPostSimulation),
                updateDelegate = updateFunction
            };
        }

        private static readonly Type[] ARCH_SYSTEM_GROUP_NAMES =
        {
            typeof(ArchInitialization),
            typeof(ArchUpdateSimulation),
            typeof(ArchSyncView),
            typeof(ArchPostSimulation)
        };

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitInEditor()
        {
            EditorApplication.playModeStateChanged -= ResetOnPlayModeExit;
            EditorApplication.playModeStateChanged += ResetOnPlayModeExit;

            Init();

            static void ResetOnPlayModeExit(PlayModeStateChange obj)
            {
                if (obj == PlayModeStateChange.ExitingPlayMode)
                {
                    ResetSystems();
                }
            }
        }

#endif

#if UNITY_2020_1_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#else
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
        private static void Init()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

            Initialize(ref copyLoop);

            PlayerLoop.SetPlayerLoop(copyLoop);
        }

        private static void Initialize(ref PlayerLoopSystem copyLoop)
        {
            if (!copyLoop.TryGetSystem(typeof(ArchInitialization), out _))
            {
                copyLoop.InsertSystem(ArchInitialization.Create(), typeof(Initialization), PlayerLoopSystemExtensions.InsertType.BEFORE);
            }

            if (!copyLoop.TryGetSystem(typeof(ArchUpdateSimulation), out _))
            {
                copyLoop.InsertSystem(ArchUpdateSimulation.Create(), typeof(Update.ScriptRunBehaviourUpdate), PlayerLoopSystemExtensions.InsertType.BEFORE);
            }

            if (!copyLoop.TryGetSystem(typeof(ArchSyncView), out _))
            {
                copyLoop.InsertSystem(ArchSyncView.Create(), typeof(Update.ScriptRunBehaviourUpdate), PlayerLoopSystemExtensions.InsertType.AFTER);
            }

            if (!copyLoop.TryGetSystem(typeof(ArchPostSimulation), out _))
            {
                copyLoop.InsertSystem(ArchPostSimulation.Create(), typeof(Update), PlayerLoopSystemExtensions.InsertType.AFTER);
            }
        }

        private static void ResetSystems()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

            foreach (Type type in ARCH_SYSTEM_GROUP_NAMES)
            {
                copyLoop.FindSystem(type).updateDelegate = null;
            }

            PlayerLoop.SetPlayerLoop(copyLoop);
        }
    }
}