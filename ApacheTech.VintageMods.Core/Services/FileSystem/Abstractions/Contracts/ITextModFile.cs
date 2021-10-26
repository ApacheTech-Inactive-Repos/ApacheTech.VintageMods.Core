using JetBrains.Annotations;
using System.Threading.Tasks;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts
{
    /// <summary>
    ///     Represents a Text file on the filesystem.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ITextModFile : IModFileBase
    {
        /// <summary>
        ///     Opens the file, reads all lines of text, and then closes the file.
        /// </summary>
        /// <returns>A <see cref="string" />, containing all lines of text within the file.</returns>
        public string ReadAllText();

        /// <summary>
        ///     Asynchronously opens the file, reads all lines of text, and then closes the file.
        /// </summary>
        /// <returns>A <see cref="string" />, containing all lines of text within the file.</returns>
        public Task<string> ReadAllTextAsync();
    }
}