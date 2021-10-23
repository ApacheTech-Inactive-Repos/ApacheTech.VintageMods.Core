using ApacheTech.VintageMods.Core.Services.FileSystem;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Global access to the FileSystem Service.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class FileSystemEx
    {
        private static readonly IFileSystemService Service;

        static FileSystemEx()
        {
            Service = IOC.Provider.GetService<IFileSystemService>();
        }
    }
}