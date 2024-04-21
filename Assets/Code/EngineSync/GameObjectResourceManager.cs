using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using UnityEngine;

namespace Code.EngineSync
{
    public class GameObjectResourceManager
    {
        public readonly struct PooledObject
        {
            public readonly string Key;
            public readonly EntityReference Reference;
            public readonly GameObject GameObject;

            public PooledObject(string key, EntityReference reference, GameObject gameObject)
            {
                Key = key;
                Reference = reference;
                GameObject = gameObject;
            }
        }

        private readonly Func<GameObject, GameObject> _factory;

        private readonly Action<GameObject> _onGet;
        private readonly Action<GameObject> _onReturn;

        private readonly Dictionary<string, GameObject> _prefabs;

        private readonly Dictionary<string, Queue<GameObject>> _pools;
        private readonly List<PooledObject> _pooled;

        public IReadOnlyList<PooledObject> Pooled => _pooled;

        public GameObjectResourceManager(Func<GameObject, GameObject> factory, Action<GameObject> onGet = null, Action<GameObject> onReturn = null)
        {
            _factory = factory;
            _onGet = onGet;
            _onReturn = onReturn;
            _prefabs = new();
            _pooled = new();
            _pools = new();
        }


        public void Allocate(string key, int size, bool startActive = false)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject gameObject = _factory(_prefabs[key]);
                gameObject.SetActive(startActive);
                _pools[key].Enqueue(gameObject);
            }
        }

        public void RegisterPrefab(string key, GameObject gameObject)
        {
            _prefabs.Add(key, gameObject);
            _pools.Add(key, new Queue<GameObject>());
        }

        public GameObject Get(string key, in Entity entity)
        {
            if (!_pools[key].TryDequeue(out GameObject result))
            {
                result = _factory(_prefabs[key]);
            }

            _pooled.Add(new PooledObject(key, entity.Reference(), result));
            _onGet?.Invoke(result);
            return result;
        }

        public void Return(in PooledObject pooledObject)
        {
            GameObject pooledObjectGameObject = pooledObject.GameObject;
            _pooled.Remove(pooledObject);
            _pools[pooledObject.Key].Enqueue(pooledObjectGameObject);
            _onReturn?.Invoke(pooledObjectGameObject);
        }

        public PooledObject this[Entity entity] => _pooled.Find(x => x.Reference.IsAlive() && x.Reference.Entity.Id == entity.Id);
    }
}