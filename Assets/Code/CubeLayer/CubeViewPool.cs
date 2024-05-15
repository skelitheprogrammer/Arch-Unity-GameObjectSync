using System;
using Code._Arch.Arch.View;
using UnityEngine;

namespace Code.CubeLayer
{
    public class CubeViewPool : ViewPool<GameObject>
    {
        public CubeViewPool(Func<GameObject, GameObject> factory, GameObject resource, int allocationSize = 16) : base(factory, resource, allocationSize)
        {
        }
        protected override void OnRent(GameObject view)
        {
            view.SetActive(true);
        }

        protected override void OnRecycle(GameObject view)
        {
            view.SetActive(false);
        }


    }
}