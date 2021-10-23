using System.IO;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts
{
    /// <summary>
    ///     Represents a binary (ProtoBuf) file on the filesystem.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IBinaryModFile : IModFile
    {
        /// <summary>
        ///     Parses the file into a primitive byte array.
        /// </summary>
        /// <returns>An array of type <see cref="byte"/>, populated with data from this file.</returns>
        public byte[] ParseAsByteArray();

        /// <summary>
        ///     Parses the file into a memory stream.
        /// </summary>
        /// <returns>An instance of type <see cref="MemoryStream"/>, populated with data from this file.</returns>
        public MemoryStream ParseAsMemoryStream();
    }
}