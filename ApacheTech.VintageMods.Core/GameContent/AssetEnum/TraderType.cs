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
        public static string Artisan { get; } = Create("artisan");
        public static string BuildingSupplies { get; } = Create("buildmaterials");
        public static string Clothing { get; } = Create("clothing");
        public static string Commodities { get; } = Create("commodities");
        public static string Foods { get; } = Create("foods");
        public static string Furniture { get; } = Create("furniture");
        public static string Luxuries { get; } = Create("luxuries");
        public static string SurvivalGoods { get; } = Create("survivalgoods");
        public static string TreasureHunter { get; } = Create("treasurehunter");
    }
}