using System;
using Microsoft.Extensions.Configuration;

namespace ApacheTech.VintageMods.Core.Services.Configuration.Contracts
{
    public interface IWorldConfiguration : IConfigurationRoot, IDisposable
    {
    }
}