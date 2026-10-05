namespace BookStore.GUI.Forms;

partial class MessageDialog
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
        rootLayout = new TableLayoutPanel();
        titleLabel = new BookStore.GUI.Controls.ThemedLabel();
        messageLabel = new BookStore.GUI.Controls.ThemedLabel();
        buttonsPanel = new FlowLayoutPanel();
        confirmButton = new BookStore.GUI.Controls.ThemedButton();
        cancelButton = new BookStore.GUI.Controls.ThemedButton();
        rootLayout.SuspendLayout();
        buttonsPanel.SuspendLayout();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.AutoSize = true;
        rootLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle());
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(messageLabel, 0, 1);
        rootLayout.Controls.Add(buttonsPanel, 0, 2);
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(24, 20, 24, 20);
        rootLayout.RowCount = 3;
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.TabIndex = 0;
        //
        // titleLabel
        //
        titleLabel.Margin = new Padding(0, 0, 0, 10);
        titleLabel.Name = "titleLabel";
        titleLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Заголовок";
        //
        // messageLabel
        //
        messageLabel.Margin = new Padding(0, 0, 0, 24);
        messageLabel.MaximumSize = new Size(440, 0);
        messageLabel.MinimumSize = new Size(380, 0);
        messageLabel.Name = "messageLabel";
        messageLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        messageLabel.TabIndex = 1;
        messageLabel.Text = "Текст сообщения";
        //
        // buttonsPanel
        //
        buttonsPanel.AutoSize = true;
        buttonsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttonsPanel.Controls.Add(confirmButton);
        buttonsPanel.Controls.Add(cancelButton);
        buttonsPanel.Dock = DockStyle.Fill;
        buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonsPanel.Margin = new Padding(0);
        buttonsPanel.Name = "buttonsPanel";
        buttonsPanel.TabIndex = 2;
        buttonsPanel.WrapContents = false;
        //
        // confirmButton
        //
        confirmButton.DialogResult = DialogResult.OK;
        confirmButton.Margin = new Padding(8, 0, 0, 0);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new Size(130, 36);
        confirmButton.TabIndex = 0;
        confirmButton.Text = "ОК";
        confirmButton.Variant = BookStore.GUI.Controls.ButtonVariant.Primary;
        //
        // cancelButton
        //
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Margin = new Padding(0);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(110, 36);
        cancelButton.TabIndex = 1;
        cancelButton.Text = "Отмена";
        //
        // MessageDialog
        //
        AcceptButton = confirmButton;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        CancelButton = cancelButton;
        ClientSize = new Size(428, 160);
        Controls.Add(rootLayout);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MessageDialog";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "BookStore";
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        buttonsPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel rootLayout;
    private BookStore.GUI.Controls.ThemedLabel titleLabel;
    private BookStore.GUI.Controls.ThemedLabel messageLabel;
    private FlowLayoutPanel buttonsPanel;
    private BookStore.GUI.Controls.ThemedButton confirmButton;
    private BookStore.GUI.Controls.ThemedButton cancelButton;
}
