using System.Reflection;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.GUI
{
    /// <summary>
    ///     Acts as a base class for dialogue boxes that
    /// </summary>
    /// <typeparam name="TFeatureSettings">The strongly-typed settings for the feature under use.</typeparam>
    /// <seealso cref="GenericDialogue" />
    public abstract class AutomaticFeatureSettingsDialogue<TFeatureSettings> : FeatureSettingsDialogue<TFeatureSettings> where TFeatureSettings : class, new()
    {
        protected AutomaticFeatureSettingsDialogue(ICoreClientAPI capi, TFeatureSettings settings, string featureName)
            : base(capi, settings, featureName)
        {
        }

        protected override void ComposeBody(GuiComposer composer)
        {
            var leftColumnBounds = ElementBounds.Fixed(0, GuiStyle.TitleBarHeight + 1.0, 250, 20);
            var rightColumnBounds = ElementBounds.Fixed(260, GuiStyle.TitleBarHeight, 20, 20);
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AddSettingSwitch(composer, propertyInfo.Name, ref leftColumnBounds, ref rightColumnBounds);
            }
        }

        private void AddSettingSwitch(GuiComposer composer, string propertyName, ref ElementBounds textBounds, ref ElementBounds sliderBounds)
        {
            const int switchSize = 20;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}"), font.Clone().WithOrientation(EnumTextOrientation.Right), textBounds);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}.HoverText"), font, 260, textBounds);
            composer.AddSwitch(state => { Settings.SetProperty(propertyName, state); RefreshValues(); },
                sliderBounds.FlatCopy().WithFixedWidth(switchSize), $"btn{propertyName}");
            textBounds = textBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
            sliderBounds = sliderBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        protected override void RefreshValues()
        {
            if (!IsOpened()) return;
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                SingleComposer.GetSwitch($"btn{propertyInfo.Name}").SetValue((bool)propertyInfo.GetValue(Settings));
            }
        }
    }
}