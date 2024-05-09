using System;

namespace Code._Arch.Arch.Infrastructure
{
    public interface ICompositionRoot : IDisposable
    {
        void Create<T>(T contextHolder);
        void Init();
    }
}