using Arch.Core;

namespace Code._Arch
{
    public interface IEntityHandler
    {
        Entity Create(in World world);
        void Destroy(in Entity entity);
    }
}