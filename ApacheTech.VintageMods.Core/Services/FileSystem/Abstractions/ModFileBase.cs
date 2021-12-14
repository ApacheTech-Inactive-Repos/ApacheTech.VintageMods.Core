using System.IO;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions
{
    /// <summary>
    ///     A wrapper of a <see cref="FileInfo" /> for a specific file on on the filesystem. This class cannot be inherited.
    /// </summary>
    public abstract class ModFileBase : IModFileBase
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        protected ModFileBase(string filePath)
        {
            ModFileInfo = new FileInfo(filePath);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo" /> class.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        protected ModFileBase(FileInfo fileInfo)
        {
            ModFileInfo = fileInfo;
        }

        /// <summary>
        ///     Gets the type of the file, be it JSON, or Binary.
        /// </summary>
        /// <value>The type of the file.</value>
        public abstract FileType FileType { get; }

        public FileInfo AsFileInfo()
        {
            return ModFileInfo;
        }

        public string Path => ModFileInfo.FullName;

        protected FileInfo ModFileInfo { get; }
    }
}