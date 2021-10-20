using ApacheTech.VintageMods.Core.Services.FileSystem;
using Microsoft.Extensions.Configuration;

namespace ApacheTech.VintageMods.Core.Services.ModConfiguration
{
    internal class ModConfigurationService : IModConfigurationService
    {
        private IConfiguration Config { get; set; }

        public ModConfigurationService()
        {
        }
    }
}