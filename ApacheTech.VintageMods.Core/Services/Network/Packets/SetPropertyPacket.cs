using ProtoBuf;

namespace ApacheTech.VintageMods.Core.Services.Network.Packets
{
    [ProtoContract]
    public class SetPropertyPacket<T> : SetPropertyPacketBase
    {
        [ProtoMember(1)]
        public new T Value { get; set; }

        protected override object UntypedValue { get => Value; set => Value = (T)value; }
        public override string ToString() => Value?.ToString() ?? "";
    }
}