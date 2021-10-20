using ApacheTech.VintageMods.Core.Services.FileSystem;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheTech.VintageMods.Core.Common.Singletons
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

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IModFile" /> representation of the file, on disk.</returns>
        public static IModFile GetRegisteredFile(string fileName)
        {
            return Service.GetRegisteredFile(fileName);
        }
    }
}