using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts
{
    /// <summary>
    ///     Represents a JSON file on the filesystem.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ITextModFile : IModFileBase
    {
        /// <summary>
        ///     Parses the file into a primitive string.
        /// </summary>
        /// <returns>An instance of type <see cref="string"/>, populated with data from this file.</returns>
        public string ReadAllText();
    }
}