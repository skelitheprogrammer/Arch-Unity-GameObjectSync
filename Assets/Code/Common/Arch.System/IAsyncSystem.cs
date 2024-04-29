using System.Threading.Tasks;
using Arch.Core;
using Cysharp.Threading.Tasks;

namespace Code.System
{
    public interface IAsyncSystem : ISystem
    {
        new UniTask Execute(World world);
    }
}