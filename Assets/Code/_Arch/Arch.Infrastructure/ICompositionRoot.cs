using System;
using System.Threading.Tasks;

namespace Code._Arch.Arch.Infrastructure
{
    public interface ICompositionRoot : IDisposable
    {
        Task Create<T>(T contextHolder);
        void Init();
    }
}