#if DEBUG
#else

[assembly: Vintagestory.API.Common.ModInfo(
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
