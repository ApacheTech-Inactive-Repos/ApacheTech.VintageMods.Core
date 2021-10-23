using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
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
        /// <returns>Return an <see cref="object"/> representation of the file, on disk.</returns>
        object GetRegisteredFile(string fileName);

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IJsonModFile"/> representation of the file, on disk.</returns>
        IJsonModFile GetJsonFile(string fileName);

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IBinaryModFile"/> representation of the file, on disk.</returns>
        IBinaryModFile GetBinaryFile(string fileName);

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="ITextModFile"/> representation of the file, on disk.</returns>
        ITextModFile GetTextFile(string fileName);

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="TFileType"/> representation of the file, on disk.</returns>
        TFileType GetRegisteredFile<TFileType>(string fileName) where TFileType : IModFileBase;

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