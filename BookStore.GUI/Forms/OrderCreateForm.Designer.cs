namespace BookStore.GUI.Forms;

partial class OrderCreateForm
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
        userCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        userComboBox = new ComboBox();
        userHintLabel = new BookStore.GUI.Controls.ThemedLabel();
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
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 440F));
        fieldsLayout.Controls.Add(userCaptionLabel, 0, 0);
        fieldsLayout.Controls.Add(userComboBox, 0, 1);
        fieldsLayout.Controls.Add(userHintLabel, 0, 2);
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 3;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.TabIndex = 0;
        //
        // userCaptionLabel
        //
        userCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        userCaptionLabel.Name = "userCaptionLabel";
        userCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        userCaptionLabel.TabIndex = 0;
        userCaptionLabel.Text = "Покупатель";
        //
        // userComboBox
        //
        userComboBox.Dock = DockStyle.Fill;
        userComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        userComboBox.Margin = new Padding(0, 0, 0, 6);
        userComboBox.Name = "userComboBox";
        userComboBox.TabIndex = 1;
        //
        // userHintLabel
        //
        userHintLabel.Margin = new Padding(0);
        userHintLabel.Name = "userHintLabel";
        userHintLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        userHintLabel.TabIndex = 2;
        userHintLabel.Text = "Заказ создаётся пустым — книги добавляются в следующем окне.";
        //
        // OrderCreateForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        ClientSize = new Size(488, 220);
        Name = "OrderCreateForm";
        Text = "Заказ";
        bodyPanel.ResumeLayout(false);
        bodyPanel.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel fieldsLayout;
    private BookStore.GUI.Controls.ThemedLabel userCaptionLabel;
    private ComboBox userComboBox;
    private BookStore.GUI.Controls.ThemedLabel userHintLabel;
}
