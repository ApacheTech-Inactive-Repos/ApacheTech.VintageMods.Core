using System;

namespace ApacheTech.VintageMods.Core.Abstractions
{
    public interface IScopedOperation : IDisposable
    {
        void OnFinally();
    }
}