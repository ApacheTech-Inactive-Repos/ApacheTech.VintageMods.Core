using System;
using Microsoft.Extensions.Options;

namespace ApacheTech.VintageMods.Core.Services.Configuration.WritableOptions
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}