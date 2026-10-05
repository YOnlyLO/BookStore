namespace BookStore.GUI.Forms;

partial class EditorFormBase
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
        errorBanner = new BookStore.GUI.Controls.AlertBanner();
        bodyPanel = new Panel();
        footerPanel = new FlowLayoutPanel();
        saveButton = new BookStore.GUI.Controls.ThemedButton();
        cancelButton = new BookStore.GUI.Controls.ThemedButton();
        rootLayout.SuspendLayout();
        footerPanel.SuspendLayout();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.AutoSize = true;
        rootLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle());
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(errorBanner, 0, 1);
        rootLayout.Controls.Add(bodyPanel, 0, 2);
        rootLayout.Controls.Add(footerPanel, 0, 3);
        rootLayout.Location = new Point(0, 0);
        rootLayout.Margin = new Padding(0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(24, 20, 24, 20);
        rootLayout.RowCount = 4;
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.TabIndex = 0;
        //
        // titleLabel
        //
        titleLabel.Margin = new Padding(0, 0, 0, 16);
        titleLabel.Name = "titleLabel";
        titleLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Запись";
        //
        // errorBanner
        //
        errorBanner.Dock = DockStyle.Fill;
        errorBanner.Margin = new Padding(0, 0, 0, 16);
        errorBanner.Name = "errorBanner";
        errorBanner.TabIndex = 1;
        errorBanner.Visible = false;
        //
        // bodyPanel
        //
        bodyPanel.AutoSize = true;
        bodyPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bodyPanel.Margin = new Padding(0);
        bodyPanel.Name = "bodyPanel";
        bodyPanel.TabIndex = 2;
        //
        // footerPanel
        //
        footerPanel.AutoSize = true;
        footerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        footerPanel.Controls.Add(saveButton);
        footerPanel.Controls.Add(cancelButton);
        footerPanel.Dock = DockStyle.Fill;
        footerPanel.FlowDirection = FlowDirection.RightToLeft;
        footerPanel.Margin = new Padding(0, 20, 0, 0);
        footerPanel.Name = "footerPanel";
        footerPanel.TabIndex = 3;
        footerPanel.WrapContents = false;
        //
        // saveButton
        //
        saveButton.Glyph = "\uE8FB";
        saveButton.Margin = new Padding(8, 0, 0, 0);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(140, 36);
        saveButton.TabIndex = 0;
        saveButton.Text = "Сохранить";
        saveButton.Variant = BookStore.GUI.Controls.ButtonVariant.Primary;
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
        // EditorFormBase
        //
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        CancelButton = cancelButton;
        ClientSize = new Size(468, 200);
        Controls.Add(rootLayout);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EditorFormBase";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Запись";
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        footerPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel rootLayout;
    private BookStore.GUI.Controls.ThemedLabel titleLabel;
    private BookStore.GUI.Controls.AlertBanner errorBanner;
    protected Panel bodyPanel;
    private FlowLayoutPanel footerPanel;
    private BookStore.GUI.Controls.ThemedButton saveButton;
    private BookStore.GUI.Controls.ThemedButton cancelButton;
}
