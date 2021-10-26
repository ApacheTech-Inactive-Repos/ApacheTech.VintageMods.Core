using ProtoBuf;

namespace ApacheTech.VintageMods.Core.Services.Network.Packets
{
    [ProtoContract]
    [ProtoInclude(03, typeof(SetPropertyPacket<bool>))]
    [ProtoInclude(04, typeof(SetPropertyPacket<byte>))]
    [ProtoInclude(05, typeof(SetPropertyPacket<sbyte>))]
    [ProtoInclude(06, typeof(SetPropertyPacket<char>))]
    [ProtoInclude(07, typeof(SetPropertyPacket<decimal>))]
    [ProtoInclude(08, typeof(SetPropertyPacket<double>))]
    [ProtoInclude(09, typeof(SetPropertyPacket<float>))]
    [ProtoInclude(10, typeof(SetPropertyPacket<int>))]
    [ProtoInclude(11, typeof(SetPropertyPacket<uint>))]
    [ProtoInclude(14, typeof(SetPropertyPacket<long>))]
    [ProtoInclude(15, typeof(SetPropertyPacket<ulong>))]
    [ProtoInclude(16, typeof(SetPropertyPacket<short>))]
    [ProtoInclude(17, typeof(SetPropertyPacket<ushort>))]
    [ProtoInclude(18, typeof(SetPropertyPacket<string>))]
    public abstract class SetPropertyPacket
    {
        public object Value
        {
            get => UntypedValue;
            set => UntypedValue = value;
        }

        [ProtoMember(2)]
        public string PropertyName { get; set; }

        protected abstract object UntypedValue { get; set; }

        public static SetPropertyPacket<T> Create<T>(T value) => new() { Value = value };
    }

    [ProtoContract]
    public class SetPropertyPacket<T> : SetPropertyPacket
    {
        [ProtoMember(1)]
        public new T Value { get; set; }

        protected override object UntypedValue { get => Value; set => Value = (T)value; }
        public override string ToString() => Value?.ToString() ?? "";
    }
}