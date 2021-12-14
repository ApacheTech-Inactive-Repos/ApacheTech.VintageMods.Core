using System.IO;
using System.Threading.Tasks;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.FileSystem.FileAdaptors
{
    /// <summary>
    ///     Represents a Text file, used by the mod. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ModFile" />
    public sealed class TextModFile : ModFileBase, ITextModFile
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonModFile"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TextModFile(string filePath) : base(filePath)
        {
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonModFile"/> class.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        public TextModFile(FileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        ///     Gets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        public override FileType FileType => FileType.Text;

        /// <summary>
        ///     Opens the file, reads all lines of text, and then closes the file.
        /// </summary>
        /// <returns>A <see cref="string" />, containing all lines of text within the file.</returns>
        public string ReadAllText()
        {
            return File.ReadAllText(ModFileInfo.FullName);
        }

        /// <summary>
        ///     Asynchronously opens the file, reads all lines of text, and then closes the file.
        /// </summary>
        /// <returns>A <see cref="string" />, containing all lines of text within the file.</returns>
        public Task<string> ReadAllTextAsync()
        {
            return ModFileInfo.OpenText().ReadToEndAsync();
        }
    }
}