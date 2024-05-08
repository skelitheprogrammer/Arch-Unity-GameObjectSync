using Arch.Core;
using Arch.Core.Extensions;

namespace Code
{
    public struct Changed<T>
    {
        private readonly EntityReference _reference;

        public T OldValue { get; internal set; }

        public T CurrentValue => _reference.Entity.Get<T>();
        public bool IsChanged => !CurrentValue.Equals(OldValue);

        public Changed(EntityReference reference) : this()
        {
            _reference = reference;
        }
    }
}