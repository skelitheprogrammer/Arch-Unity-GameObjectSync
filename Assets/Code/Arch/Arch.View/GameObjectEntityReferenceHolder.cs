using Arch.Core;
using UnityEngine;

namespace Code.Arch.Arch.View
{
    public class GameObjectEntityReferenceHolder : MonoBehaviour
    {
        private EntityReference _reference;
        public bool IsAlive => _reference.IsAlive();
        public int ID => IsAlive ? _reference.Entity.Id : -1;

        public void Init(in EntityReference reference)
        {
            _reference = reference;
        }

        public void OnEnable()
        {
            if (!IsAlive)
            {
                return;
            }
        }

        public void OnDisable()
        {
            if (!IsAlive)
            {
                return;
            }
        }

        private void OnDestroy()
        {
            if (!IsAlive)
            {
                return;
            }

            _reference.Entity.Destroy();
        }
    }
}