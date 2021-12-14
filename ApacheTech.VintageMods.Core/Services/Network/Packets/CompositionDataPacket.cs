using System.Collections.Generic;
using ProtoBuf;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.Network.Packets
{
    /// <summary>
    ///     Represents a network packet used for MEF composition.
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public record CompositionDataPacket(string Contract, IEnumerable<byte> Data);
}