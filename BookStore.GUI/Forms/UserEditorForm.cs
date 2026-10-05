using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class UserEditorForm : EditorFormBase, IUserEditorView
{
    public UserEditorForm()
    {
        InitializeComponent();
    }

    string IUserEditorView.FirstName
    {
        get => firstNameTextBox.Text;
        set => firstNameTextBox.Text = value;
    }

    string IUserEditorView.LastName
    {
        get => lastNameTextBox.Text;
        set => lastNameTextBox.Text = value;
    }

    string IUserEditorView.Email
    {
        get => emailTextBox.Text;
        set => emailTextBox.Text = value;
    }

    string IUserEditorView.Password => passwordTextBox.Text;

    bool IUserEditorView.IsPasswordVisible
    {
        set
        {
            passwordCaptionLabel.Visible = value;
            passwordTextBox.Visible = value;
            passwordHintLabel.Visible = value;

            if (!value)
                emailTextBox.Margin = Padding.Empty;
        }
    }

    protected override Control? GetFieldControl(string field) => field switch
    {
        nameof(IUserEditorView.FirstName) => firstNameTextBox,
        nameof(IUserEditorView.LastName) => lastNameTextBox,
        nameof(IUserEditorView.Email) => emailTextBox,
        nameof(IUserEditorView.Password) => passwordTextBox,
        _ => null
    };
}
