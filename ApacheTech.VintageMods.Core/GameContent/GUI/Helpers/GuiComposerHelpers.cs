using System;
using ApacheTech.VintageMods.Core.GameContent.GUI.Elements;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.GameContent.GUI.Helpers
{
    public static class GuiComposerHelpers
    {
        /// <summary>
        ///     Adds a dialogue title bar to the GUI, with no "Movable" menu bar.  
        /// </summary>
        /// <param name="composer"></param>
        /// <param name="text">The text of the title bar.</param>
        /// <param name="onClose">The event fired when the title bar is closed.</param>
        /// <param name="font">The font of the title bar.</param>
        /// <param name="bounds">The bounds of the title bar.</param>
        public static GuiComposer AddTitleBarWithNoMenu(this GuiComposer composer, string text, Action onClose = null, CairoFont font = null, ElementBounds bounds = null)
        {
            if (!composer.Composed)
            {
                composer.AddInteractiveElement(new GuiElementTitleBar(composer.Api, text, onClose, font, bounds));
            }

            return composer;
        }
    }
}