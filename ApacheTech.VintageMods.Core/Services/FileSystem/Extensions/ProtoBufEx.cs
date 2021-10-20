using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using ProtoBuf;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ProtoBufEx
    {
        /// <summary>
        ///     Serialises the specified record.
        /// </summary>
        public static IEnumerable<byte> Serialise<T>(T record) where T : class
        {
            if (null == record) return null;
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, record);
            return stream.ToArray();
        }

        /// <summary>
        ///     Deserialises the specified data.
        /// </summary>
        public static T Deserialise<T>(IEnumerable<byte> data) where T : class
        {
            if (null == data) return null;
            using var stream = new MemoryStream(data.ToArray());
            return Serializer.Deserialize<T>(stream);
        }
    }
}