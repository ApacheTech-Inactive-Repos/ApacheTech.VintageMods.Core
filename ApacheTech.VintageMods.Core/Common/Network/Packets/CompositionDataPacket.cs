using System.Collections.Generic;
using JetBrains.Annotations;
using ProtoBuf;

namespace ApacheTech.VintageMods.Core.Common.Network.Packets
{
    /// <summary>
    ///     Represents a network packet used for MEF composition.
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record CompositionDataPacket(string Contract, IEnumerable<byte> Data);
}