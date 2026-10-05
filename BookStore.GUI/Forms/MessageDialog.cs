using BookStore.GUI.Controls;
using BookStore.GUI.Theme;

namespace BookStore.GUI.Forms;

/// <summary>
/// Модальное сообщение в тёмной теме — замена MessageBox, который всегда светлый.
/// </summary>
public partial class MessageDialog : Form
{
    public MessageDialog()
    {
        InitializeComponent();
        DarkTheme.Apply(this);
    }

    /// <summary>Сообщение с одной кнопкой «Понятно».</summary>
    public static void ShowAlert(IWin32Window? owner, string title, string message)
    {
        using var dialog = Create(title, message, "Понятно", ButtonVariant.Primary);

        dialog.cancelButton.Visible = false;
        dialog.CancelButton = dialog.confirmButton;
        dialog.ShowDialog(owner);
    }

    /// <summary>Вопрос с подтверждением разрушающего действия. true — пользователь подтвердил.</summary>
    public static bool Confirm(IWin32Window? owner, string title, string message, string confirmText)
    {
        using var dialog = Create(title, message, confirmText, ButtonVariant.Danger);

        // По умолчанию фокус на «Отмена», чтобы случайный Enter ничего не удалил.
        dialog.ActiveControl = dialog.cancelButton;

        return dialog.ShowDialog(owner) == DialogResult.OK;
    }

    private static MessageDialog Create(string title, string message, string confirmText, ButtonVariant variant)
    {
        var dialog = new MessageDialog
        {
            Text = title
        };

        dialog.titleLabel.Text = title;
        dialog.messageLabel.Text = message;
        dialog.confirmButton.Text = confirmText;
        dialog.confirmButton.Variant = variant;
        dialog.confirmButton.Width = Math.Max(dialog.confirmButton.Width, dialog.confirmButton.GetPreferredSize(Size.Empty).Width);

        return dialog;
    }
}
