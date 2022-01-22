using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.GUI
{
    /// <summary>
    ///     Acts as a base class for basic, automatically sized dialogue boxes.
    /// </summary>
    /// <seealso cref="GuiDialog" />
    public abstract class GenericDialogue : GuiDialog
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="GenericDialogue"/> class.
        /// </summary>
        /// <param name="capi">The client API.</param>
        protected GenericDialogue(ICoreClientAPI capi) : base(capi)
        {
            ToggleKeyCombinationCode = GetType().Name;
        }

        /// <summary>
        ///     The key combination string that toggles this GUI object.
        /// </summary>
        /// <value>The toggle key combination code.</value>
        public override string ToggleKeyCombinationCode { get; }

        /// <summary>
        ///     Attempts to open this dialogue.
        /// </summary>
        /// <returns>
        ///     Returns <see langword="true"/> if the dialogue window was opened correctly; otherwise, returns <see langword="false"/>
        /// </returns>
        public override bool TryOpen()
        {
            var openWindows = ApiEx.Client.OpenedGuis;
            foreach (var gui in openWindows)
            {
                if (gui is not GuiDialog window) continue;
                if (window.ToggleKeyCombinationCode is null) continue;
                if (!window.ToggleKeyCombinationCode.Equals(ToggleKeyCombinationCode)) continue;
                window.Focus();
                return false;
            }
            var success = base.TryOpen();
            Compose();
            if (success) RefreshValues();
            return opened;
        }

        /// <summary>
        ///     Composes the GUI components for this instance.
        /// </summary>
        protected virtual void Compose()
        {
            var composer = ComposeHeader()
                    .BeginChildElements(DialogueBounds);

            ComposeBody(composer);

            SingleComposer = composer
                .EndChildElements()
                .Compose();
        }

        /// <summary>
        ///     Sets the title of the dialogue box.
        /// </summary>
        /// <value>The raw, pre-localised, string literal to use for the title of the dialogue box.</value>
        public string Title { private get; set; }

        /// <summary>
        ///     Sets the alignment of the form on the screen, when set to Fixed mode.
        /// </summary>
        /// <value>The <see cref="EnumDialogArea"/> alignment to set the window as.</value>
        protected EnumDialogArea Alignment { private get; set; } = EnumDialogArea.RightBottom;

        protected ElementBounds DialogueBounds { get; private set; }

        protected bool ShowTitleBar { get; set; } = true;

        /// <summary>
        ///     Refreshes the displayed values on the form.
        /// </summary>
        protected virtual void RefreshValues()
        {
            // HACK: Until .NET enables optional abstract methods, this anti-pattern of empty virtuals is as good as it gets.
        }

        /// <summary>
        ///     Composes the header for the GUI.
        /// </summary>
        /// <param name="composer">The composer.</param>
        protected abstract void ComposeBody(GuiComposer composer);

        private GuiComposer ComposeHeader()
        {
            var dialogueBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(Alignment)
                .WithFixedAlignmentOffset(-GuiStyle.DialogToScreenPadding, -GuiStyle.DialogToScreenPadding);

            DialogueBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            DialogueBounds.BothSizing = ElementSizing.FitToChildren;

            var composer = capi.Gui
                .CreateCompo(ToggleKeyCombinationCode, dialogueBounds)
                .AddShadedDialogBG(DialogueBounds);

            if (ShowTitleBar)
            {
                composer.AddDialogTitleBar(Title, () => TryClose());
            }

            return composer;
        }
    }
}