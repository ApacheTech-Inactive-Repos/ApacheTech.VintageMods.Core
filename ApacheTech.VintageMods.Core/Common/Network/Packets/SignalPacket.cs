using JetBrains.Annotations;
using ProtoBuf;

namespace ApacheTech.VintageMods.Core.Common.Network.Packets
{
    /// <summary>
    ///     Represents a generic signalling packet, used to raise an event for a change of state.
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record SignalPacket;
}