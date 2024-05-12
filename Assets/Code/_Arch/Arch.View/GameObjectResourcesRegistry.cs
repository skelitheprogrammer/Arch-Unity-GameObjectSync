using System.Collections.Generic;
using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class GameObjectResourcesRegistry
    {
        private readonly List<GameObject> _prefabs;

        public GameObjectResourcesRegistry()
        {
            _prefabs = new();
        }

        public int RegisterResource(GameObject prefab)
        {
            if (_prefabs.Contains(prefab))
            {
                return _prefabs.IndexOf(prefab);
            }

            _prefabs.Add(prefab);

            return _prefabs.Count - 1;
        }


        public GameObject Get(int id)
        {
            return _prefabs[id];
        }
    }
}