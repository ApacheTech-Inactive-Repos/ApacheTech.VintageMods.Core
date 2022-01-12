using ProtoBuf;

namespace ApacheTech.VintageMods.Core.Services.Network.Packets
{
    [ProtoContract]
    public class WorldNamePacket
    {
        [ProtoMember(1)]
        public string Name { get; set; }
    }
}