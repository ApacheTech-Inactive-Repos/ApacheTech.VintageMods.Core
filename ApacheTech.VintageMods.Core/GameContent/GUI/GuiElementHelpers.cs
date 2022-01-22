using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.GameContent.GUI
{
    public static class GuiElementHelpers
    {
        public static GuiComposer AddStaticImage(this GuiComposer composer, ElementBounds bounds, AssetLocation imageAsset)
        {
            if (!composer.Composed)
            {
                composer.AddStaticElement(new GuiElementImage(composer.Api, bounds, imageAsset));
            }
            return composer;
        }

        public static GuiComposer AddHoverTextForCellList<TCellEntry>(this GuiComposer composer, string cellListName,IList<TCellEntry> cellEntries, ElementBounds clippedBounds)
            where TCellEntry : SavegameCellEntry
        {
            var cellListElement = composer.GetCellList<TCellEntry>(cellListName);
            cellListElement.BeforeCalcBounds();
            for (var i = 0; i < cellEntries.Count; i++)
            {
                var hoverText = new StringBuilder(cellEntries[i].Title);
                hoverText.AppendLine(cellEntries[i].HoverText);

                var hoverTextBounds = cellListElement.elementCells[i].Bounds.ForkChild();

                cellListElement.elementCells[i].Bounds.ChildBounds.Add(hoverTextBounds);
                hoverTextBounds.fixedWidth -= 56.0;
                hoverTextBounds.fixedY = -3.0;
                hoverTextBounds.fixedX -= 6.0;
                hoverTextBounds.fixedHeight -= 2.0;
                composer.AddHoverText(hoverText.ToString(), CairoFont.WhiteDetailText(), 320, hoverTextBounds, $"{cellListName}-hover-{i}");
                composer.GetHoverText($"{cellListName}-hover-{i}").InsideClipBounds = clippedBounds;
            }

            return composer;
        }
    }
}