using System.Collections.Generic;
using ApacheTech.VintageMods.Core.Services.Configuration.Contracts;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using SmartAssembly.Attributes;

namespace ApacheTech.VintageMods.Core.Services.Configuration
{
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class WorldConfigurationRoot : ConfigurationRoot, IWorldConfiguration
    {
        public WorldConfigurationRoot(IList<IConfigurationProvider> providers) : base(providers)
        {
        }
    }
}