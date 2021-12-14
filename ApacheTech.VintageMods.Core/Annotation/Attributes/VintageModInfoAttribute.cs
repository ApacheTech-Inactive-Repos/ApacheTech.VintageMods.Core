using System;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Annotation.Attributes
{
    /// <summary>
    ///     Contains the metadata shared between VintageMods mods.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Assembly)]
    [DoNotPruneType, DoNotPrune]
    [DoNotObfuscateType, DoNotObfuscate, DoNotObfuscateControlFlow]
    public sealed class VintageModInfoAttribute : Attribute, IVintageModInfo
    {
        /// <summary>
        ///     Gets or sets the mod identifier.
        /// </summary>
        /// <value>The mod identifier.</value>
        public string ModId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the mod.
        /// </summary>
        /// <value>The name of the mod.</value>
        public string ModName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the root directory.
        /// </summary>
        /// <value>The name of the root directory.</value>
        public string RootDirectoryName { get; set; }

        /// <summary>
        ///     Gets or sets the latest version.
        /// </summary>
        /// <value>The latest version.</value>
        public string Version { get; set; }

        /// <summary>
        ///     Gets or sets the network version.
        /// </summary>
        /// <value>The network version.</value>
        public string NetworkVersion { get; set; }

        /// <summary>
        ///     Gets or sets the app side that the mod will run on.
        /// </summary>
        /// <value>The app side to run the mod on.</value>
        public EnumAppSide Side { get; set; }
    }
}