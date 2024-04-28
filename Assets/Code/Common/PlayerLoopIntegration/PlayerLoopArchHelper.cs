using System;
using PlayerLoopExtender;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace src.Tests.Morpeh
{
    internal static class PlayerLoopArchHelper
    {
        internal readonly struct ArchInitialization
        {
            public static PlayerLoopSystem Create() => new()
            {
                type = typeof(ArchInitialization)
            };
        }

        internal readonly struct ArchUpdateSimulation
        {
            public static PlayerLoopSystem Create() => new()
            {
                type = typeof(ArchUpdateSimulation)
            };
        }

        internal readonly struct ArchSyncView
        {
            public static PlayerLoopSystem Create() => new()
            {
                type = typeof(ArchSyncView)
            };
        }

        private static readonly Type[] MORPEH_SYSTEM_NAMES =
        {
            typeof(ArchInitialization),
            typeof(ArchUpdateSimulation),
            typeof(ArchSyncView),
        };

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitInEditor()
        {
            EditorApplication.playModeStateChanged -= EditorApplicationOnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;

            Init();

            static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange obj)
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
        }

        private static void ResetSystems()
        {
            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

            foreach (Type type in MORPEH_SYSTEM_NAMES)
            {
                copyLoop.FindSystem(type).updateDelegate = null;
            }

            PlayerLoop.SetPlayerLoop(copyLoop);
        }
    }
}