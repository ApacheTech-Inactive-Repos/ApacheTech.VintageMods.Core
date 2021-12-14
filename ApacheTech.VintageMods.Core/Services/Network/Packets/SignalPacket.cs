using ProtoBuf;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.Network.Packets
{
    /// <summary>
    ///     Represents a generic signalling packet, used to raise an event for a change of state.
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public readonly ref struct SignalPacket { }
}