using System.Collections.Generic;
using ApacheTech.VintageMods.Core.Services.Configuration.Contracts;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace ApacheTech.VintageMods.Core.Services.Configuration
{
    [UsedImplicitly]
    internal class WorldConfigurationRoot : ConfigurationRoot, IWorldConfiguration
    {
        public WorldConfigurationRoot(IList<IConfigurationProvider> providers) : base(providers)
        {
        }
    }
}