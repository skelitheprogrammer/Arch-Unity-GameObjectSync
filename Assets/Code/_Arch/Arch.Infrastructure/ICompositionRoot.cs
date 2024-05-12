using System;

namespace Code._Arch.Arch.Infrastructure
{
    public interface ICompositionRoot : IDisposable
    {
        void Init<T>(T contextHolder);
    }
}