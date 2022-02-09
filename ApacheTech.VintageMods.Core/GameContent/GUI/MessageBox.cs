using System;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.GameContent.GUI
{
    /// <summary>
    ///     A generic message box that can be used for basic confirmation within the game.
    /// </summary>
    /// <seealso cref="GenericDialogue" />
    public class MessageBox : GenericDialogue
    {
        private readonly ButtonLayout _buttons;
        private readonly string _message;

        private Action OnOkAction { get; set; }
        private Action OnCancelAction { get; set; }

        /// <summary>
        ///     Displays a message box that has a message, title bar caption, and button; and that runs option callback options for positive and negative responses.
        /// </summary>
        /// <param name="title">A <see cref="string"/> that specifies the title bar caption to display.</param>
        /// <param name="message">A <see cref="string"/> that specifies the text to display.</param>
        /// <param name="buttons">A <see cref="ButtonLayout"/> value that specifies which button or buttons to display.</param>
        /// <param name="onOkButtonPressed">The <see cref="Action"/> to be invoked if the user selects the confirm option.</param>
        /// <param name="onCancelButtonPressed">The <see cref="Action"/> to be invoked if the user selects the cancel option.</param>
        public static void Show(string title, string message, ButtonLayout buttons = ButtonLayout.Ok, Action onOkButtonPressed = null, Action onCancelButtonPressed = null)
        {
            var messageBox = new MessageBox(title, message, buttons)
            {
                OnCancelAction = onCancelButtonPressed,
                OnOkAction = onOkButtonPressed,
                ModalTransparency = 0.6f
            };
            messageBox.TryOpen();
        }

        private MessageBox(string title, string message, ButtonLayout buttons) : base(ApiEx.Client)
        {
            var defaultTitle = (_buttons = buttons) switch
            {
                ButtonLayout.Ok => Lang.Get("vmods:confirmation-information-title"),
                ButtonLayout.OkCancel => Lang.Get("vmods:confirmation-confirm-title"),
                _ => throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null)
            };

            Title = title.IfNullOrWhitespace(defaultTitle);
            _message = message;
            Alignment = EnumDialogArea.CenterMiddle;
        }

        protected override void ComposeBody(GuiComposer composer)
        {
            const int width = 320;
            const int height = 130;
            DialogueBounds.WithFixedSize(width, height);

            var messageFont = CairoFont.WhiteDetailText().WithOrientation(EnumTextOrientation.Center);
            var messageBounds = ElementBounds.FixedSize(EnumDialogArea.CenterMiddle, width, height).WithFixedOffset(0,30);
            
            composer.AddStaticText(_message, messageFont, EnumTextOrientation.Center, messageBounds);

            var confirmButtonText = Lang.Get("vmods:confirmation-ok");
            var cancelButtonText = Lang.Get("vmods:confirmation-cancel");

            var controlRowBoundsLeftFixed = ElementBounds.FixedSize(150, 30).WithAlignment(EnumDialogArea.LeftBottom);
            var controlRowBoundsRightFixed = ElementBounds.FixedSize(150, 30).WithAlignment(EnumDialogArea.RightBottom);
            composer
                .AddSmallButton(confirmButtonText, OnOkButtonPressed, controlRowBoundsRightFixed, EnumButtonStyle.Normal, EnumTextOrientation.Center, "btnOk")
                .AddIf(_buttons == ButtonLayout.OkCancel)
                .AddSmallButton(cancelButtonText, OnCancelButtonPressed, controlRowBoundsLeftFixed, EnumButtonStyle.Normal, EnumTextOrientation.Center, "btnCancel")
                .EndIf();
        }

        private bool OnCancelButtonPressed()
        {
            OnCancelAction?.Invoke();
            return TryClose();
        }

        private bool OnOkButtonPressed()
        {
            OnOkAction?.Invoke();
            return TryClose();
        }
    }
}