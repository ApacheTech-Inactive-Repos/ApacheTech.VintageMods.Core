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
        protected GenericDialogue(ICoreClientAPI capi) : base(capi) { }

        /// <summary>
        ///     Attempts to open this dialogue.
        /// </summary>
        /// <returns>
        ///     Returns <see langword="true"/> if the dialogue window was opened correctly; otherwise, returns <see langword="false"/>
        /// </returns>
        public override bool TryOpen()
        {
            var composer = ComposeHeader();
            ComposeBody(composer);
            SingleComposer = composer.EndChildElements().Compose();
            var success = base.TryOpen();
            if (success) RefreshValues();
            return opened;
        }

        /// <summary>
        ///     Sets the title of the dialogue box.
        /// </summary>
        /// <value>The raw, pre-localised, string literal to use for the title of the dialogue box.</value>
        protected string Title { private get; set; }

        /// <summary>
        ///     Sets the alignment of the form on the screen, when set to Fixed mode.
        /// </summary>
        /// <value>The <see cref="EnumDialogArea"/> alignment to set the window as.</value>
        protected EnumDialogArea Alignment { private get; set; } = EnumDialogArea.RightBottom;

        /// <summary>
        ///     Refreshes the displayed values on the form.
        /// </summary>
        protected virtual void RefreshValues()
        {
            // HACK: Until .NET enables optional abstract methods, this anti-pattern of empty virtuals is as good as it gets.
        }

        /// <summary>
        ///     Composes the 
        /// </summary>
        /// <param name="composer">The composer.</param>
        protected abstract void ComposeBody(GuiComposer composer);

        private GuiComposer ComposeHeader()
        {
            var dialogueBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(Alignment)
                .WithFixedAlignmentOffset(-GuiStyle.DialogToScreenPadding, -GuiStyle.DialogToScreenPadding);

            var bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            bgBounds.BothSizing = ElementSizing.FitToChildren;

            return capi.Gui.CreateCompo(ToggleKeyCombinationCode, dialogueBounds)
                .AddShadedDialogBG(bgBounds)
                .AddDialogTitleBar(Title, () => TryClose())
                .BeginChildElements(bgBounds);
        }
    }
}