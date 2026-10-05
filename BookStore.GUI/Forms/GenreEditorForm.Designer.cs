namespace BookStore.GUI.Forms;

partial class GenreEditorForm
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
        nameCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        nameTextBox = new TextBox();
        nameHintLabel = new BookStore.GUI.Controls.ThemedLabel();
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
        fieldsLayout.Controls.Add(nameCaptionLabel, 0, 0);
        fieldsLayout.Controls.Add(nameTextBox, 0, 1);
        fieldsLayout.Controls.Add(nameHintLabel, 0, 2);
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 3;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.TabIndex = 0;
        //
        // nameCaptionLabel
        //
        nameCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        nameCaptionLabel.Name = "nameCaptionLabel";
        nameCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        nameCaptionLabel.TabIndex = 0;
        nameCaptionLabel.Text = "Название";
        //
        // nameTextBox
        //
        nameTextBox.BorderStyle = BorderStyle.FixedSingle;
        nameTextBox.Dock = DockStyle.Fill;
        nameTextBox.Margin = new Padding(0, 0, 0, 6);
        nameTextBox.MaxLength = 200;
        nameTextBox.Name = "nameTextBox";
        nameTextBox.TabIndex = 1;
        //
        // nameHintLabel
        //
        nameHintLabel.Margin = new Padding(0);
        nameHintLabel.Name = "nameHintLabel";
        nameHintLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        nameHintLabel.TabIndex = 2;
        nameHintLabel.Text = "Не длиннее 200 символов, без повторов среди жанров.";
        //
        // GenreEditorForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        ClientSize = new Size(468, 220);
        Name = "GenreEditorForm";
        Text = "Жанр";
        bodyPanel.ResumeLayout(false);
        bodyPanel.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel fieldsLayout;
    private BookStore.GUI.Controls.ThemedLabel nameCaptionLabel;
    private TextBox nameTextBox;
    private BookStore.GUI.Controls.ThemedLabel nameHintLabel;
}
