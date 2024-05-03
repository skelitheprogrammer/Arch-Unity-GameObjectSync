using Arch.Core;
using Arch.Core.Extensions;

namespace Code
{
    public struct Changed<T>
    {
        private readonly EntityReference _reference;

        public T OldValue { get; internal set; }

        public T CurrentValue => _reference.Entity.Get<T>();

        public Changed(EntityReference reference) : this()
        {
            _reference = reference;
        }

        public bool IsChanged => !CurrentValue.Equals(OldValue);
    }

    public readonly struct Added<T>
    {
        private readonly EntityReference _reference;

        public bool IsAdded => _reference.Entity.Has<T>();
    }

    public struct Removed<T>
    {
        private readonly EntityReference _reference;

        private bool _wasAdded;

        public bool IsRemoved => _wasAdded && !_reference.Entity.Has<T>();
    }
}