namespace BookStore.GUI.Forms;

partial class BookEditorForm
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
        titleCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        titleTextBox = new TextBox();
        authorCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        authorTextBox = new TextBox();
        genreCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        genreComboBox = new ComboBox();
        numbersLayout = new TableLayoutPanel();
        priceCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        stockCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        priceUpDown = new NumericUpDown();
        stockUpDown = new NumericUpDown();
        stockHintLabel = new BookStore.GUI.Controls.ThemedLabel();
        descriptionCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        descriptionTextBox = new TextBox();
        bodyPanel.SuspendLayout();
        fieldsLayout.SuspendLayout();
        numbersLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)priceUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)stockUpDown).BeginInit();
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
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 480F));
        fieldsLayout.Controls.Add(titleCaptionLabel, 0, 0);
        fieldsLayout.Controls.Add(titleTextBox, 0, 1);
        fieldsLayout.Controls.Add(authorCaptionLabel, 0, 2);
        fieldsLayout.Controls.Add(authorTextBox, 0, 3);
        fieldsLayout.Controls.Add(genreCaptionLabel, 0, 4);
        fieldsLayout.Controls.Add(genreComboBox, 0, 5);
        fieldsLayout.Controls.Add(numbersLayout, 0, 6);
        fieldsLayout.Controls.Add(descriptionCaptionLabel, 0, 7);
        fieldsLayout.Controls.Add(descriptionTextBox, 0, 8);
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
        // titleCaptionLabel
        //
        titleCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        titleCaptionLabel.Name = "titleCaptionLabel";
        titleCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        titleCaptionLabel.TabIndex = 0;
        titleCaptionLabel.Text = "Название";
        //
        // titleTextBox
        //
        titleTextBox.BorderStyle = BorderStyle.FixedSingle;
        titleTextBox.Dock = DockStyle.Fill;
        titleTextBox.Margin = new Padding(0, 0, 0, 14);
        titleTextBox.MaxLength = 200;
        titleTextBox.Name = "titleTextBox";
        titleTextBox.TabIndex = 1;
        //
        // authorCaptionLabel
        //
        authorCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        authorCaptionLabel.Name = "authorCaptionLabel";
        authorCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        authorCaptionLabel.TabIndex = 2;
        authorCaptionLabel.Text = "Автор";
        //
        // authorTextBox
        //
        authorTextBox.BorderStyle = BorderStyle.FixedSingle;
        authorTextBox.Dock = DockStyle.Fill;
        authorTextBox.Margin = new Padding(0, 0, 0, 14);
        authorTextBox.MaxLength = 200;
        authorTextBox.Name = "authorTextBox";
        authorTextBox.TabIndex = 3;
        //
        // genreCaptionLabel
        //
        genreCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        genreCaptionLabel.Name = "genreCaptionLabel";
        genreCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        genreCaptionLabel.TabIndex = 4;
        genreCaptionLabel.Text = "Жанр";
        //
        // genreComboBox
        //
        genreComboBox.Dock = DockStyle.Fill;
        genreComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        genreComboBox.Margin = new Padding(0, 0, 0, 14);
        genreComboBox.Name = "genreComboBox";
        genreComboBox.TabIndex = 5;
        //
        // numbersLayout
        //
        numbersLayout.AutoSize = true;
        numbersLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        numbersLayout.ColumnCount = 2;
        numbersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        numbersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        numbersLayout.Controls.Add(priceCaptionLabel, 0, 0);
        numbersLayout.Controls.Add(stockCaptionLabel, 1, 0);
        numbersLayout.Controls.Add(priceUpDown, 0, 1);
        numbersLayout.Controls.Add(stockUpDown, 1, 1);
        numbersLayout.Controls.Add(stockHintLabel, 1, 2);
        numbersLayout.Dock = DockStyle.Fill;
        numbersLayout.Margin = new Padding(0, 0, 0, 14);
        numbersLayout.Name = "numbersLayout";
        numbersLayout.RowCount = 3;
        numbersLayout.RowStyles.Add(new RowStyle());
        numbersLayout.RowStyles.Add(new RowStyle());
        numbersLayout.RowStyles.Add(new RowStyle());
        numbersLayout.TabIndex = 6;
        //
        // priceCaptionLabel
        //
        priceCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        priceCaptionLabel.Name = "priceCaptionLabel";
        priceCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        priceCaptionLabel.TabIndex = 0;
        priceCaptionLabel.Text = "Цена, ₽";
        //
        // stockCaptionLabel
        //
        stockCaptionLabel.Margin = new Padding(8, 0, 0, 6);
        stockCaptionLabel.Name = "stockCaptionLabel";
        stockCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        stockCaptionLabel.TabIndex = 1;
        stockCaptionLabel.Text = "Остаток на складе, шт.";
        //
        // priceUpDown
        //
        priceUpDown.BorderStyle = BorderStyle.FixedSingle;
        priceUpDown.DecimalPlaces = 2;
        priceUpDown.Dock = DockStyle.Fill;
        priceUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
        priceUpDown.Margin = new Padding(0, 0, 8, 0);
        priceUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        priceUpDown.Name = "priceUpDown";
        priceUpDown.TabIndex = 2;
        priceUpDown.ThousandsSeparator = true;
        //
        // stockUpDown
        //
        stockUpDown.BorderStyle = BorderStyle.FixedSingle;
        stockUpDown.Dock = DockStyle.Fill;
        stockUpDown.Margin = new Padding(8, 0, 0, 0);
        stockUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        stockUpDown.Name = "stockUpDown";
        stockUpDown.TabIndex = 3;
        stockUpDown.ThousandsSeparator = true;
        //
        // stockHintLabel
        //
        stockHintLabel.Margin = new Padding(8, 6, 0, 0);
        stockHintLabel.Name = "stockHintLabel";
        stockHintLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        stockHintLabel.TabIndex = 4;
        stockHintLabel.Text = "Задаётся только при создании книги";
        stockHintLabel.Visible = false;
        //
        // descriptionCaptionLabel
        //
        descriptionCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        descriptionCaptionLabel.Name = "descriptionCaptionLabel";
        descriptionCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        descriptionCaptionLabel.TabIndex = 7;
        descriptionCaptionLabel.Text = "Описание (необязательно)";
        //
        // descriptionTextBox
        //
        descriptionTextBox.AcceptsReturn = true;
        descriptionTextBox.BorderStyle = BorderStyle.FixedSingle;
        descriptionTextBox.Dock = DockStyle.Fill;
        descriptionTextBox.Margin = new Padding(0);
        descriptionTextBox.MaxLength = 2000;
        descriptionTextBox.Multiline = true;
        descriptionTextBox.Name = "descriptionTextBox";
        descriptionTextBox.ScrollBars = ScrollBars.Vertical;
        descriptionTextBox.Size = new Size(480, 110);
        descriptionTextBox.TabIndex = 8;
        //
        // BookEditorForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        ClientSize = new Size(528, 560);
        Name = "BookEditorForm";
        Text = "Книга";
        bodyPanel.ResumeLayout(false);
        bodyPanel.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        numbersLayout.ResumeLayout(false);
        numbersLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)priceUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)stockUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel fieldsLayout;
    private BookStore.GUI.Controls.ThemedLabel titleCaptionLabel;
    private TextBox titleTextBox;
    private BookStore.GUI.Controls.ThemedLabel authorCaptionLabel;
    private TextBox authorTextBox;
    private BookStore.GUI.Controls.ThemedLabel genreCaptionLabel;
    private ComboBox genreComboBox;
    private TableLayoutPanel numbersLayout;
    private BookStore.GUI.Controls.ThemedLabel priceCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel stockCaptionLabel;
    private NumericUpDown priceUpDown;
    private NumericUpDown stockUpDown;
    private BookStore.GUI.Controls.ThemedLabel stockHintLabel;
    private BookStore.GUI.Controls.ThemedLabel descriptionCaptionLabel;
    private TextBox descriptionTextBox;
}
