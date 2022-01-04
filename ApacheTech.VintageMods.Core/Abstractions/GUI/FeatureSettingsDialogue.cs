using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.GUI
{
    /// <summary>
    ///     Acts as a base class for dialogue boxes that 
    /// </summary>
    /// <typeparam name="TFeatureSettings">The strongly-typed settings for the feature under use.</typeparam>
    /// <seealso cref="GenericDialogue" />
    public abstract class FeatureSettingsDialogue<TFeatureSettings> : GenericDialogue where TFeatureSettings : class, new()
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
        protected FeatureSettingsDialogue(ICoreClientAPI capi, TFeatureSettings settings, string featureName) : base(capi)
        {
            Settings = settings;
            FeatureName = featureName;
            Title = LangEx.FeatureString(FeatureName, "Dialogue.Title");
        }
    }
}