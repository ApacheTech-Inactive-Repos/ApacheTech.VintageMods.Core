using System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.GUI
{
    /// <summary>
    ///     Acts as a base class for dialogue boxes that 
    /// </summary>
    /// <typeparam name="TFeatureSettings">The strongly-typed settings for the feature under use.</typeparam>
    /// <seealso cref="GenericDialogue" />
    public abstract class FeatureSettingsDialogue<TFeatureSettings> : GenericDialogue
        where TFeatureSettings : class, new()
    {
        /// <summary>
        ///     The strongly-typed settings for the feature under use.
        /// </summary>
        /// <value>The strongly-typed settings for the feature under use.</value>
        protected TFeatureSettings Settings { get; }

        /// <summary>
        ///     Gets the name of the feature.
        /// </summary>
        /// <value>The name of the feature.</value>
        protected string FeatureName { get; }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="FeatureSettingsDialogue{TFeatureSettings}"/> class.
        /// </summary>
        /// <param name="capi">The capi.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="featureName">Name of the feature.</param>
        protected FeatureSettingsDialogue(ICoreClientAPI capi, TFeatureSettings settings, string featureName = null) :
            base(capi)
        {
            Settings = settings;
            FeatureName = featureName ?? typeof(TFeatureSettings).Name.Replace("Settings", "");
            Title = LangEntry("Title");
        }

        /// <summary>
        ///     Gets an entry from the language files, for the feature this instance is representing.
        /// </summary>
        /// <param name="code">The entry to return.</param>
        /// <returns>A localised <see cref="string"/>, for the specified language file code.</returns>
        protected string LangEntry(string code)
        {
            return LangEx.FeatureString($"{FeatureName}.Dialogue", code);
        }

        protected void SaveFeatureChanges(FileScope scope = FileScope.World)
        {
            switch (scope)
            {
                case FileScope.Global:
                    ModSettings.Global.Save(Settings);
                    break;
                case FileScope.World:
                    ModSettings.World.Save(Settings);
                    break;
                case FileScope.Local:
                    ModSettings.Local.Save(Settings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }
        }
    }
}