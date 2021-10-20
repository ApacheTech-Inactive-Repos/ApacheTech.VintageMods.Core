using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.FileSystem
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IFileSystemService
    {
        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IModFile"/> representation of the file, on disk.</returns>
        IModFile GetRegisteredFile(string fileName);

        /// <summary>
        ///     Registers a file with the FileSystem Service. This will copy a default implementation of the file from:
        ///      • An embedded resource.
        ///      • The mod's unpack directory.
        ///      • The mod's assets folder.
        ///
        ///     If no default implementation can be found, a new file is created, at the correct location.
        /// </summary>
        /// <param name="fileName">The name of the file, including file extension.</param>
        /// <param name="scope">The scope of the file, be it global, or per-world.</param>
        public void RegisterFile(string fileName, FileScope scope);
    }
}