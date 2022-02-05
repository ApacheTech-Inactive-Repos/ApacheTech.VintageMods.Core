using System.Linq;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class GuiComposerExtensions
    {
        /// <summary>
        /// The default bounds for a menu button.
        /// </summary>
        /// <returns>The default bounds for a menu button.</returns>

        public static ElementBounds DefaultButtonBounds()
        {
            return ElementBounds.Fixed(0.0, 0.0, 0.0, 40.0).WithFixedPadding(0.0, 3.0);
        }

        /// <summary>
        /// The default bounds for a menu button.
        /// </summary>
        /// <returns>The default bounds for a menu button.</returns>

        public static ElementBounds DefaultButtonBounds(this GuiCompositeSettings _) => DefaultButtonBounds();

        public static void RegisterGuiDialogueHotKey<TDialogue>(
            this IInputAPI api, 
            string displayText, 
            GlKeys hotKey, 
            bool altPressed = false, 
            bool ctrlPressed = false, 
            bool shiftPressed = false)
            where TDialogue : GuiDialog
        {
            var dialogue = ModServices.IOC.Resolve<TDialogue>();
            api.RegisterHotKey(dialogue.ToggleKeyCombinationCode, displayText, hotKey, HotkeyType.GUIOrOtherControls, altPressed, ctrlPressed, shiftPressed);
            api.SetHotKeyHandler(dialogue.ToggleKeyCombinationCode, _ => ToggleGui(dialogue));
        }

        public static bool ToggleGui<TDialogue>(this TDialogue dialogue) where TDialogue : GuiDialog
        {
            return ApiEx.Client.OpenedGuis.Contains(dialogue.ToggleKeyCombinationCode)
                ? dialogue.TryClose() 
                : dialogue.TryOpen();
        }

        public static void RegisterTransientGuiDialogueHotKey<TDialogue>(
            this IInputAPI api,
            string displayText,
            GlKeys hotKey,
            bool altPressed = false,
            bool ctrlPressed = false,
            bool shiftPressed = false)
            where TDialogue : GuiDialog
        {
            var dialogue = ModServices.IOC.Resolve<TDialogue>();
            api.RegisterHotKey(dialogue.ToggleKeyCombinationCode, displayText, hotKey, HotkeyType.GUIOrOtherControls, altPressed, ctrlPressed, shiftPressed);
            api.SetHotKeyHandler(dialogue.ToggleKeyCombinationCode, _ => ModServices.IOC.Resolve<TDialogue>().TryOpen());
        }
    }
}