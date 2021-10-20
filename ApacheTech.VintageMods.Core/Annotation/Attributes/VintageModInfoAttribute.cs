using System;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Annotation.Attributes
{
    /// <summary>
    ///     
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Assembly)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
        public string LatestVersion { get; set; }
    }
}