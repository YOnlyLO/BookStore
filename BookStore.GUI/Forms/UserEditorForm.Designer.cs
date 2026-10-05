namespace BookStore.GUI.Forms;

partial class UserEditorForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        fieldsLayout = new TableLayoutPanel();
        lastNameCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        lastNameTextBox = new TextBox();
        firstNameCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        firstNameTextBox = new TextBox();
        emailCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        emailTextBox = new TextBox();
        passwordCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        passwordTextBox = new TextBox();
        passwordHintLabel = new BookStore.GUI.Controls.ThemedLabel();
        bodyPanel.SuspendLayout();
        fieldsLayout.SuspendLayout();
        SuspendLayout();
        //
        // bodyPanel
        //
        bodyPanel.Controls.Add(fieldsLayout);
        //
        // fieldsLayout
        //
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 1;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420F));
        fieldsLayout.Controls.Add(lastNameCaptionLabel, 0, 0);
        fieldsLayout.Controls.Add(lastNameTextBox, 0, 1);
        fieldsLayout.Controls.Add(firstNameCaptionLabel, 0, 2);
        fieldsLayout.Controls.Add(firstNameTextBox, 0, 3);
        fieldsLayout.Controls.Add(emailCaptionLabel, 0, 4);
        fieldsLayout.Controls.Add(emailTextBox, 0, 5);
        fieldsLayout.Controls.Add(passwordCaptionLabel, 0, 6);
        fieldsLayout.Controls.Add(passwordTextBox, 0, 7);
        fieldsLayout.Controls.Add(passwordHintLabel, 0, 8);
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 9;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.TabIndex = 0;
        //
        // lastNameCaptionLabel
        //
        lastNameCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        lastNameCaptionLabel.Name = "lastNameCaptionLabel";
        lastNameCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        lastNameCaptionLabel.TabIndex = 0;
        lastNameCaptionLabel.Text = "Фамилия";
        //
        // lastNameTextBox
        //
        lastNameTextBox.BorderStyle = BorderStyle.FixedSingle;
        lastNameTextBox.Dock = DockStyle.Fill;
        lastNameTextBox.Margin = new Padding(0, 0, 0, 14);
        lastNameTextBox.MaxLength = 100;
        lastNameTextBox.Name = "lastNameTextBox";
        lastNameTextBox.TabIndex = 1;
        //
        // firstNameCaptionLabel
        //
        firstNameCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        firstNameCaptionLabel.Name = "firstNameCaptionLabel";
        firstNameCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        firstNameCaptionLabel.TabIndex = 2;
        firstNameCaptionLabel.Text = "Имя";
        //
        // firstNameTextBox
        //
        firstNameTextBox.BorderStyle = BorderStyle.FixedSingle;
        firstNameTextBox.Dock = DockStyle.Fill;
        firstNameTextBox.Margin = new Padding(0, 0, 0, 14);
        firstNameTextBox.MaxLength = 100;
        firstNameTextBox.Name = "firstNameTextBox";
        firstNameTextBox.TabIndex = 3;
        //
        // emailCaptionLabel
        //
        emailCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        emailCaptionLabel.Name = "emailCaptionLabel";
        emailCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        emailCaptionLabel.TabIndex = 4;
        emailCaptionLabel.Text = "Email";
        //
        // emailTextBox
        //
        emailTextBox.BorderStyle = BorderStyle.FixedSingle;
        emailTextBox.Dock = DockStyle.Fill;
        emailTextBox.Margin = new Padding(0, 0, 0, 14);
        emailTextBox.MaxLength = 320;
        emailTextBox.Name = "emailTextBox";
        emailTextBox.TabIndex = 5;
        //
        // passwordCaptionLabel
        //
        passwordCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        passwordCaptionLabel.Name = "passwordCaptionLabel";
        passwordCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        passwordCaptionLabel.TabIndex = 6;
        passwordCaptionLabel.Text = "Пароль";
        //
        // passwordTextBox
        //
        passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
        passwordTextBox.Dock = DockStyle.Fill;
        passwordTextBox.Margin = new Padding(0, 0, 0, 6);
        passwordTextBox.Name = "passwordTextBox";
        passwordTextBox.TabIndex = 7;
        passwordTextBox.UseSystemPasswordChar = true;
        //
        // passwordHintLabel
        //
        passwordHintLabel.Margin = new Padding(0);
        passwordHintLabel.Name = "passwordHintLabel";
        passwordHintLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        passwordHintLabel.TabIndex = 8;
        passwordHintLabel.Text = "На сервер отправляется SHA-256-хеш пароля, а не сам пароль.";
        //
        // UserEditorForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        ClientSize = new Size(468, 440);
        Name = "UserEditorForm";
        Text = "Пользователь";
        bodyPanel.ResumeLayout(false);
        bodyPanel.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel fieldsLayout;
    private BookStore.GUI.Controls.ThemedLabel lastNameCaptionLabel;
    private TextBox lastNameTextBox;
    private BookStore.GUI.Controls.ThemedLabel firstNameCaptionLabel;
    private TextBox firstNameTextBox;
    private BookStore.GUI.Controls.ThemedLabel emailCaptionLabel;
    private TextBox emailTextBox;
    private BookStore.GUI.Controls.ThemedLabel passwordCaptionLabel;
    private TextBox passwordTextBox;
    private BookStore.GUI.Controls.ThemedLabel passwordHintLabel;
}
