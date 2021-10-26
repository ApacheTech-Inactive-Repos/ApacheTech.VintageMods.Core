#if DEBUG
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

[assembly: InternalsVisibleTo("ApacheTech.VintageMods.Core.Tests")]
[assembly: InternalsVisibleTo("ApacheTech.VintageMods.WaypointExtensions.Tests")]
[assembly: InternalsVisibleTo("ApacheTech.VintageMods.ChaosMod.Tests")]
[assembly: InternalsVisibleTo("ApacheTech.VintageMods.AccessibilityTweaks.Tests")]
[assembly: InternalsVisibleTo("ApacheTech.VintageMods.CinematicCamStudio.Tests")]

[assembly: InternalsVisibleTo("ApacheTech.VintageMods.Sandbox")]
[assembly: InternalsVisibleTo("ApacheTech.VintageMods.Sandbox.Tests")]

[assembly: InternalsVisibleTo("ApacheTech.VintageMods.Prototype")]
[assembly: InternalsVisibleTo("ApacheTech.VintageMods.Prototype.Tests")]

[assembly: ModInfo(
    "[VintageMods] - Core",
    "vmods",
    Description = "Contains core functionality for mods within the VintageMods suite.",
    Side = "Universal",
    Version = "2.0.0",
    RequiredOnClient = false,
    RequiredOnServer = false,
    NetworkVersion = "1.0.0",
    Website = "https://apachetech.co.uk",
    Contributors = new[] { "ApacheTech Solutions", "Novocain1" },
    Authors = new[] { "ApacheTech Solutions" })]
#endif
