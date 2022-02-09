using System;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace ApacheTech.VintageMods.Core.GameContent.GUI.Elements
{
    /// <summary>
    /// A title bar for your GUI.  
    /// </summary>
    public class GuiElementTitleBar : GuiElementTextBase
    {
        private readonly Action _onClose;

        private LoadedTexture _closeIconHoverTexture;
        private Rectangled _closeIconRect;
        
        private const int UnscaledCloseIconSize = 15;
        
        public GuiElementTitleBar(ICoreClientAPI capi, string text, Action onClose = null, CairoFont font = null, ElementBounds bounds = null) : base(capi, text, font, bounds)
        {
            _closeIconHoverTexture = new LoadedTexture(capi);
            _onClose = onClose;

            Bounds = bounds ?? ElementStdBounds.TitleBar();
            Font = font ?? CairoFont.WhiteSmallText();
        }


        public override void ComposeTextElements(Context ctx, ImageSurface surface)
        {
            RoundRectangle(ctx, Bounds.bgDrawX, Bounds.bgDrawY, Bounds.OuterWidth, Bounds.OuterHeight, GuiStyle.DialogBGRadius);

            var gradient = new LinearGradient(0, 0, Bounds.InnerWidth, 0);
            gradient.AddColorStop(0, new Color(GuiStyle.DialogDefaultBgColor[0] * 1.4, GuiStyle.DialogDefaultBgColor[1] * 1.4, GuiStyle.DialogDefaultBgColor[2] * 1.4, 1));
            gradient.AddColorStop(0.5, new Color(GuiStyle.DialogDefaultBgColor[0] * 1.1, GuiStyle.DialogDefaultBgColor[1] * 1.1, GuiStyle.DialogDefaultBgColor[2] * 1.1, 1));
            gradient.AddColorStop(1, new Color(GuiStyle.DialogDefaultBgColor[0] * 1.4, GuiStyle.DialogDefaultBgColor[1] * 1.4, GuiStyle.DialogDefaultBgColor[2] * 1.4, 1));
            ctx.SetSource(gradient);
            ctx.FillPreserve();
            gradient.Dispose();

            Bounds.CalcWorldBounds();

            var radius = GuiStyle.DialogBGRadius;
            ctx.NewPath();
            ctx.MoveTo(Bounds.drawX, Bounds.drawY + Bounds.InnerHeight);
            ctx.LineTo(Bounds.drawX, Bounds.drawY + radius);
            ctx.Arc(Bounds.drawX + radius, Bounds.drawY + radius, radius, 180 * GameMath.DEG2RAD, 270 * GameMath.DEG2RAD);
            ctx.Arc(Bounds.drawX + Bounds.OuterWidth - radius, Bounds.drawY + radius, radius, -90 * GameMath.DEG2RAD, 0 * GameMath.DEG2RAD);
            ctx.LineTo(Bounds.drawX + Bounds.OuterWidth, Bounds.drawY + Bounds.InnerHeight);

            ctx.SetSourceRGBA(GuiStyle.TitleBarColor);
            ctx.FillPreserve();

            ctx.SetSourceRGBA(new[] { 45 / 255.0, 35 / 255.0, 33 / 255.0, 1 });
            ctx.LineWidth = 6;
            ctx.Stroke();
            
            Font.SetupContext(ctx);
            DrawTextLineAt(ctx, text, scaled(GuiStyle.ElementToDialogPadding), (Bounds.InnerHeight - Font.GetFontExtents().Height) / 2 + scaled(2));

            var crossSize = scaled(UnscaledCloseIconSize);
            var crossX = Bounds.drawX + Bounds.OuterWidth - crossSize - scaled(12);
            var iconY = Bounds.drawY + scaled(7);
            var crossWidth = scaled(2);
            var menuSize = scaled(UnscaledCloseIconSize + 2);

            _closeIconRect = new Rectangled(Bounds.OuterWidth - crossSize - scaled(12), scaled(5), menuSize, menuSize);

            ctx.Operator = Operator.Over;
            ctx.SetSourceRGBA(0, 0, 0, 0.3);
            api.Gui.Icons.DrawCross(ctx, crossX + 2, iconY + 2, crossWidth, crossSize);
            ctx.Operator = Operator.Source;
            ctx.SetSourceRGBA(GuiStyle.DialogDefaultTextColor);
            api.Gui.Icons.DrawCross(ctx, crossX, iconY, crossWidth, crossSize);
            ctx.Operator = Operator.Over;

            ComposeHoverIcons();
        }

        private void ComposeHoverIcons()
        {
            var crossSize = scaled(UnscaledCloseIconSize);
            const int crossWidth = 2;

            var surface = new ImageSurface(Format.Argb32, (int)crossSize + 4, (int)crossSize + 4);
            var ctx = genContext(surface);

            ctx.Operator = Operator.Source;
            ctx.SetSourceRGBA(0.8, 0, 0, 1);
            api.Gui.Icons.DrawCross(ctx, 1.5, 1.5, crossWidth, crossSize);
            ctx.SetSourceRGBA(0.8, 0, 0, 0.6);
            api.Gui.Icons.DrawCross(ctx, 2, 2, crossWidth, crossSize);

            generateTexture(surface, ref _closeIconHoverTexture);

            surface.Dispose();
            ctx.Dispose();
        }

        public override void RenderInteractiveElements(float deltaTime)
        {
            var mouseX = api.Input.MouseX;
            var mouseY = api.Input.MouseY;

            if (_closeIconRect.PointInside(mouseX - Bounds.absX, mouseY - Bounds.absY))
            {
                api.Render.Render2DTexturePremultipliedAlpha(_closeIconHoverTexture.TextureId, 
                    Bounds.absX + _closeIconRect.X - 3, Bounds.absY + _closeIconRect.Y - 1, _closeIconRect.Width + 4, _closeIconRect.Height + 4, 200);
            }
        }
        
        public override void OnMouseUpOnElement(ICoreClientAPI capi, MouseEvent args)
        {
            var mouseX = capi.Input.MouseX;
            var mouseY = capi.Input.MouseY;
            if (!_closeIconRect.PointInside(mouseX - Bounds.absX, mouseY - Bounds.absY)) return;
            args.Handled = true;
            _onClose?.Invoke();
        }
        
        public override void Dispose()
        {
            _closeIconHoverTexture?.Dispose();
            base.Dispose();
        }
    }
}