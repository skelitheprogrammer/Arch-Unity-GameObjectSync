using Arch.Core;
using Arch.Core.Extensions;

namespace Code
{
    public readonly struct ChangedSync<T> : IForEach
    {
        public void Update(Entity entity)
        {
            ref Changed<T> changed = ref entity.Get<Changed<T>>();

            if (changed.IsChanged)
            {
                changed.OldValue = changed.CurrentValue;
            }
        }
    }
}