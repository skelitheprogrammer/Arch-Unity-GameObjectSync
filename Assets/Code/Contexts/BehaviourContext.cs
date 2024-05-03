using Arch.Core;
using Arch.Core.Extensions;
using UnityEngine;

namespace Code
{
    public class BehaviourContext : MonoBehaviour
    {
        private World _world;

        public float _period;
        public float _timer;

        private QueryDescription _description = new QueryDescription().WithAll<TestComponent>();
        private QueryDescription _changedReactor = new QueryDescription().WithAll<TestComponent>().WithAll<Changed<TestComponent>>();
        private QueryDescription _changedSync = new QueryDescription().WithAll<Changed<TestComponent>>();

        private void Start()
        {
            _world = World.Create();
            Entity entity = _world.Create();
            entity.Add(new TestComponent());
            entity.Add(new Changed<TestComponent>(entity.Reference()));
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            _world.InlineQuery<ChangedSync<TestComponent>>(_changedSync);

            if (_timer >= _period)
            {
                _world.InlineQuery<TestComponentIncreaser>(_description);
                _timer = 0;
            }

            _world.InlineQuery<TestComponentChangedReactor>(_changedReactor);
        }
    }

    public readonly struct TestComponentIncreaser : IForEach
    {
        public void Update(Entity entity)
        {
            entity.Get<TestComponent>().Value++;
        }
    }

    public readonly struct TestComponentChangedReactor : IForEach
    {
        public void Update(Entity entity)
        {
            Changed<TestComponent> changed = entity.Get<Changed<TestComponent>>();

            if (changed.IsChanged)
            {
                Debug.Log($"changed from {changed.OldValue.Value} to {changed.CurrentValue.Value}");
            }
        }
    }

    public struct TestComponent
    {
        public int Value;
    }
}