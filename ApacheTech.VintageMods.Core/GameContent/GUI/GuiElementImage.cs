using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.GameContent.GUI
{
    public class GuiElementImage : GuiElementTextBase
    {
        private readonly AssetLocation _imageAsset;

        public GuiElementImage(ICoreClientAPI capi, ElementBounds bounds, AssetLocation imageAsset) 
            : base(capi, "", null, bounds) => _imageAsset = imageAsset;

        public override void ComposeElements(Context context, ImageSurface surface)
        {
            context.Save();

            using var imageSurface = getImageSurfaceFromAsset(api, _imageAsset);

            var pattern = getPattern(api, _imageAsset);
            pattern.Filter = Filter.Best;

            context.Scale(1, 1);

            context.SetSource(pattern);
            context.Rectangle(Bounds.drawX, Bounds.drawY, Bounds.OuterWidth, Bounds.OuterHeight);
            context.SetSourceSurface(imageSurface, (int)Bounds.drawX, (int)Bounds.drawY);
            context.FillPreserve();

            context.Restore();
        }
    }
}