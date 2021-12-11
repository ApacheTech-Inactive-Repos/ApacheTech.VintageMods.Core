using ApacheTech.VintageMods.Core.Abstractions.Enum;

namespace ApacheTech.VintageMods.Core.GameContent.AssetEnum
{
    public class WaypointIcon : StringEnum<WaypointIcon>
    {
        public static WaypointIcon Spiral { get; } = Create("spiral");
    }
}