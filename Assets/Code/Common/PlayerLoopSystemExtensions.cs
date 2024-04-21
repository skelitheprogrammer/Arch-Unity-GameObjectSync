using System;
using UnityEngine.LowLevel;

namespace PlayerLoopExtender
{
    public static partial class PlayerLoopSystemExtensions
    {
        public enum InsertType
        {
            BEFORE,
            AFTER,
        }

        public static void InsertSystem(this ref PlayerLoopSystem root, in PlayerLoopSystem toInsert, Type insertTarget, InsertType insertType)
        {
            if (root.subSystemList == null)
            {
                return;
            }

            int indexOfTarget = -1;
            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                if (root.subSystemList[i].type == insertTarget)
                {
                    indexOfTarget = i;
                    break;
                }

                InsertSystem(ref root.subSystemList[i], toInsert, insertTarget, insertType);
            }

            if (indexOfTarget != -1)
            {
                InsertSystemInternal(ref root, toInsert, indexOfTarget, insertType);
            }
        }

        private static void InsertSystemInternal(ref PlayerLoopSystem currentLoopRecursive, in PlayerLoopSystem toInsert, int indexOfTarget, InsertType insertType)
        {
            PlayerLoopSystem[] newSubSystems = new PlayerLoopSystem[currentLoopRecursive.subSystemList.Length + 1];

            int insertIndex = insertType == InsertType.BEFORE
                ? indexOfTarget
                : indexOfTarget + 1;

            if (insertIndex > 0)
            {
                Array.Copy(currentLoopRecursive.subSystemList, newSubSystems, insertIndex);
            }

            newSubSystems[insertIndex] = toInsert;

            if (insertIndex < currentLoopRecursive.subSystemList.Length)
            {
                Array.Copy(currentLoopRecursive.subSystemList, insertIndex, newSubSystems, insertIndex + 1, currentLoopRecursive.subSystemList.Length - insertIndex);
            }

            currentLoopRecursive.subSystemList = newSubSystems;
        }
    }
}