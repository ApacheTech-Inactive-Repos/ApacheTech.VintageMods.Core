using ApacheTech.VintageMods.Core.Abstractions.Enum;

// ReSharper disable StringLiteralTypo
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global


namespace ApacheTech.VintageMods.Core.GameContent.AssetEnum
{
    public sealed class TraderType : StringEnum<TraderType>
    {
        public static readonly string Artisan = Create("artisan");
        public static readonly string BuildingSupplies = Create("buildmaterials");
        public static readonly string Clothing = Create("clothing");
        public static readonly string Commodities = Create("commodities");
        public static readonly string Foods = Create("foods");
        public static readonly string Furniture = Create("furniture");
        public static readonly string Luxuries = Create("luxuries");
        public static readonly string SurvivalGoods = Create("survivalgoods");
        public static readonly string TreasureHunter = Create("treasurehunter");
    }
}