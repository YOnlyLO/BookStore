namespace BookStore.GUI.Forms;

partial class OrderEditorForm
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
        DataGridViewCellStyle quantityCellStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle unitPriceCellStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle totalCellStyle = new DataGridViewCellStyle();
        rootLayout = new TableLayoutPanel();
        headerLayout = new TableLayoutPanel();
        titleLabel = new BookStore.GUI.Controls.ThemedLabel();
        statusBadge = new BookStore.GUI.Controls.BadgeLabel();
        infoLayout = new TableLayoutPanel();
        customerCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        createdCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        totalCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        customerValueLabel = new BookStore.GUI.Controls.ThemedLabel();
        createdValueLabel = new BookStore.GUI.Controls.ThemedLabel();
        totalValueLabel = new BookStore.GUI.Controls.ThemedLabel();
        errorBanner = new BookStore.GUI.Controls.AlertBanner();
        itemsHeaderLayout = new TableLayoutPanel();
        itemsTitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        removeItemButton = new BookStore.GUI.Controls.ThemedButton();
        itemsCard = new BookStore.GUI.Controls.CardPanel();
        itemsGrid = new BookStore.GUI.Controls.ThemedGrid();
        bookColumn = new DataGridViewTextBoxColumn();
        quantityColumn = new DataGridViewTextBoxColumn();
        unitPriceColumn = new DataGridViewTextBoxColumn();
        totalColumn = new DataGridViewTextBoxColumn();
        addItemCard = new BookStore.GUI.Controls.CardPanel();
        addItemLayout = new TableLayoutPanel();
        bookCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        quantityCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        unitPriceCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        bookComboBox = new ComboBox();
        quantityUpDown = new NumericUpDown();
        unitPriceUpDown = new NumericUpDown();
        addItemButton = new BookStore.GUI.Controls.ThemedButton();
        lockedHintLabel = new BookStore.GUI.Controls.ThemedLabel();
        footerLayout = new TableLayoutPanel();
        cancelOrderButton = new BookStore.GUI.Controls.ThemedButton();
        actionsPanel = new FlowLayoutPanel();
        closeButton = new BookStore.GUI.Controls.ThemedButton();
        completeButton = new BookStore.GUI.Controls.ThemedButton();
        confirmButton = new BookStore.GUI.Controls.ThemedButton();
        rootLayout.SuspendLayout();
        headerLayout.SuspendLayout();
        infoLayout.SuspendLayout();
        itemsHeaderLayout.SuspendLayout();
        itemsCard.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)itemsGrid).BeginInit();
        addItemCard.SuspendLayout();
        addItemLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)quantityUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)unitPriceUpDown).BeginInit();
        footerLayout.SuspendLayout();
        actionsPanel.SuspendLayout();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(headerLayout, 0, 0);
        rootLayout.Controls.Add(infoLayout, 0, 1);
        rootLayout.Controls.Add(errorBanner, 0, 2);
        rootLayout.Controls.Add(itemsHeaderLayout, 0, 3);
        rootLayout.Controls.Add(itemsCard, 0, 4);
        rootLayout.Controls.Add(addItemCard, 0, 5);
        rootLayout.Controls.Add(lockedHintLabel, 0, 6);
        rootLayout.Controls.Add(footerLayout, 0, 7);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Margin = new Padding(0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(24, 20, 24, 20);
        rootLayout.RowCount = 8;
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.Size = new Size(820, 680);
        rootLayout.TabIndex = 0;
        //
        // headerLayout
        //
        headerLayout.AutoSize = true;
        headerLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        headerLayout.ColumnCount = 2;
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        headerLayout.ColumnStyles.Add(new ColumnStyle());
        headerLayout.Controls.Add(titleLabel, 0, 0);
        headerLayout.Controls.Add(statusBadge, 1, 0);
        headerLayout.Dock = DockStyle.Fill;
        headerLayout.Margin = new Padding(0, 0, 0, 16);
        headerLayout.Name = "headerLayout";
        headerLayout.RowCount = 1;
        headerLayout.RowStyles.Add(new RowStyle());
        headerLayout.TabIndex = 0;
        //
        // titleLabel
        //
        titleLabel.Anchor = AnchorStyles.Left;
        titleLabel.Margin = new Padding(0);
        titleLabel.Name = "titleLabel";
        titleLabel.Style = BookStore.GUI.Controls.LabelStyle.Title;
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Заказ";
        //
        // statusBadge
        //
        statusBadge.Anchor = AnchorStyles.Right;
        statusBadge.Margin = new Padding(16, 0, 0, 0);
        statusBadge.Name = "statusBadge";
        statusBadge.Size = new Size(130, 28);
        statusBadge.TabIndex = 1;
        //
        // infoLayout
        //
        infoLayout.AutoSize = true;
        infoLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        infoLayout.ColumnCount = 3;
        infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        infoLayout.Controls.Add(customerCaptionLabel, 0, 0);
        infoLayout.Controls.Add(createdCaptionLabel, 1, 0);
        infoLayout.Controls.Add(totalCaptionLabel, 2, 0);
        infoLayout.Controls.Add(customerValueLabel, 0, 1);
        infoLayout.Controls.Add(createdValueLabel, 1, 1);
        infoLayout.Controls.Add(totalValueLabel, 2, 1);
        infoLayout.Dock = DockStyle.Fill;
        infoLayout.Margin = new Padding(0, 0, 0, 18);
        infoLayout.Name = "infoLayout";
        infoLayout.RowCount = 2;
        infoLayout.RowStyles.Add(new RowStyle());
        infoLayout.RowStyles.Add(new RowStyle());
        infoLayout.TabIndex = 1;
        //
        // customerCaptionLabel
        //
        customerCaptionLabel.Margin = new Padding(0, 0, 0, 4);
        customerCaptionLabel.Name = "customerCaptionLabel";
        customerCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        customerCaptionLabel.TabIndex = 0;
        customerCaptionLabel.Text = "Покупатель";
        //
        // createdCaptionLabel
        //
        createdCaptionLabel.Margin = new Padding(0, 0, 0, 4);
        createdCaptionLabel.Name = "createdCaptionLabel";
        createdCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        createdCaptionLabel.TabIndex = 1;
        createdCaptionLabel.Text = "Создан";
        //
        // totalCaptionLabel
        //
        totalCaptionLabel.Margin = new Padding(0, 0, 0, 4);
        totalCaptionLabel.Name = "totalCaptionLabel";
        totalCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        totalCaptionLabel.TabIndex = 2;
        totalCaptionLabel.Text = "Сумма заказа";
        //
        // customerValueLabel
        //
        customerValueLabel.Margin = new Padding(0);
        customerValueLabel.Name = "customerValueLabel";
        customerValueLabel.TabIndex = 3;
        customerValueLabel.Text = "—";
        //
        // createdValueLabel
        //
        createdValueLabel.Margin = new Padding(0);
        createdValueLabel.Name = "createdValueLabel";
        createdValueLabel.TabIndex = 4;
        createdValueLabel.Text = "—";
        //
        // totalValueLabel
        //
        totalValueLabel.Margin = new Padding(0);
        totalValueLabel.Name = "totalValueLabel";
        totalValueLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        totalValueLabel.TabIndex = 5;
        totalValueLabel.Text = "—";
        //
        // errorBanner
        //
        errorBanner.Dock = DockStyle.Fill;
        errorBanner.Margin = new Padding(0, 0, 0, 14);
        errorBanner.Name = "errorBanner";
        errorBanner.TabIndex = 2;
        errorBanner.Visible = false;
        //
        // itemsHeaderLayout
        //
        itemsHeaderLayout.AutoSize = true;
        itemsHeaderLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        itemsHeaderLayout.ColumnCount = 2;
        itemsHeaderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        itemsHeaderLayout.ColumnStyles.Add(new ColumnStyle());
        itemsHeaderLayout.Controls.Add(itemsTitleLabel, 0, 0);
        itemsHeaderLayout.Controls.Add(removeItemButton, 1, 0);
        itemsHeaderLayout.Dock = DockStyle.Fill;
        itemsHeaderLayout.Margin = new Padding(0, 0, 0, 8);
        itemsHeaderLayout.Name = "itemsHeaderLayout";
        itemsHeaderLayout.RowCount = 1;
        itemsHeaderLayout.RowStyles.Add(new RowStyle());
        itemsHeaderLayout.TabIndex = 3;
        //
        // itemsTitleLabel
        //
        itemsTitleLabel.Anchor = AnchorStyles.Left;
        itemsTitleLabel.Margin = new Padding(0);
        itemsTitleLabel.Name = "itemsTitleLabel";
        itemsTitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        itemsTitleLabel.TabIndex = 0;
        itemsTitleLabel.Text = "Состав заказа";
        //
        // removeItemButton
        //
        removeItemButton.Anchor = AnchorStyles.Right;
        removeItemButton.Enabled = false;
        removeItemButton.Glyph = "\uE74D";
        removeItemButton.Margin = new Padding(0);
        removeItemButton.Name = "removeItemButton";
        removeItemButton.Size = new Size(170, 34);
        removeItemButton.TabIndex = 1;
        removeItemButton.Text = "Убрать позицию";
        removeItemButton.Variant = BookStore.GUI.Controls.ButtonVariant.Danger;
        //
        // itemsCard
        //
        itemsCard.Controls.Add(itemsGrid);
        itemsCard.Dock = DockStyle.Fill;
        itemsCard.Margin = new Padding(0, 0, 0, 14);
        itemsCard.Name = "itemsCard";
        itemsCard.Padding = new Padding(6, 4, 6, 6);
        itemsCard.TabIndex = 4;
        //
        // itemsGrid
        //
        itemsGrid.Columns.AddRange(new DataGridViewColumn[] { bookColumn, quantityColumn, unitPriceColumn, totalColumn });
        itemsGrid.Dock = DockStyle.Fill;
        itemsGrid.EmptyText = "В заказе пока нет книг";
        itemsGrid.Name = "itemsGrid";
        itemsGrid.TabIndex = 0;
        //
        // bookColumn
        //
        bookColumn.DataPropertyName = "Book";
        bookColumn.FillWeight = 260F;
        bookColumn.HeaderText = "Книга";
        bookColumn.Name = "bookColumn";
        bookColumn.ReadOnly = true;
        bookColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // quantityColumn
        //
        quantityColumn.DataPropertyName = "Quantity";
        quantityCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        quantityColumn.DefaultCellStyle = quantityCellStyle;
        quantityColumn.FillWeight = 70F;
        quantityColumn.HeaderText = "Кол-во";
        quantityColumn.Name = "quantityColumn";
        quantityColumn.ReadOnly = true;
        quantityColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // unitPriceColumn
        //
        unitPriceColumn.DataPropertyName = "UnitPrice";
        unitPriceCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        unitPriceCellStyle.Format = "C2";
        unitPriceColumn.DefaultCellStyle = unitPriceCellStyle;
        unitPriceColumn.FillWeight = 100F;
        unitPriceColumn.HeaderText = "Цена";
        unitPriceColumn.Name = "unitPriceColumn";
        unitPriceColumn.ReadOnly = true;
        unitPriceColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // totalColumn
        //
        totalColumn.DataPropertyName = "TotalPrice";
        totalCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        totalCellStyle.Format = "C2";
        totalColumn.DefaultCellStyle = totalCellStyle;
        totalColumn.FillWeight = 100F;
        totalColumn.HeaderText = "Сумма";
        totalColumn.Name = "totalColumn";
        totalColumn.ReadOnly = true;
        totalColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // addItemCard
        //
        addItemCard.AutoSize = true;
        addItemCard.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        addItemCard.Controls.Add(addItemLayout);
        addItemCard.Dock = DockStyle.Fill;
        addItemCard.Margin = new Padding(0, 0, 0, 18);
        addItemCard.Name = "addItemCard";
        addItemCard.Padding = new Padding(16, 14, 16, 16);
        addItemCard.TabIndex = 5;
        //
        // addItemLayout
        //
        addItemLayout.AutoSize = true;
        addItemLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        addItemLayout.ColumnCount = 4;
        addItemLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        addItemLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
        addItemLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 136F));
        addItemLayout.ColumnStyles.Add(new ColumnStyle());
        addItemLayout.Controls.Add(bookCaptionLabel, 0, 0);
        addItemLayout.Controls.Add(quantityCaptionLabel, 1, 0);
        addItemLayout.Controls.Add(unitPriceCaptionLabel, 2, 0);
        addItemLayout.Controls.Add(bookComboBox, 0, 1);
        addItemLayout.Controls.Add(quantityUpDown, 1, 1);
        addItemLayout.Controls.Add(unitPriceUpDown, 2, 1);
        addItemLayout.Controls.Add(addItemButton, 3, 1);
        addItemLayout.Dock = DockStyle.Top;
        addItemLayout.Margin = new Padding(0);
        addItemLayout.Name = "addItemLayout";
        addItemLayout.RowCount = 2;
        addItemLayout.RowStyles.Add(new RowStyle());
        addItemLayout.RowStyles.Add(new RowStyle());
        addItemLayout.TabIndex = 0;
        //
        // bookCaptionLabel
        //
        bookCaptionLabel.Margin = new Padding(0, 0, 0, 6);
        bookCaptionLabel.Name = "bookCaptionLabel";
        bookCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        bookCaptionLabel.TabIndex = 0;
        bookCaptionLabel.Text = "Книга";
        //
        // quantityCaptionLabel
        //
        quantityCaptionLabel.Margin = new Padding(12, 0, 0, 6);
        quantityCaptionLabel.Name = "quantityCaptionLabel";
        quantityCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        quantityCaptionLabel.TabIndex = 1;
        quantityCaptionLabel.Text = "Кол-во";
        //
        // unitPriceCaptionLabel
        //
        unitPriceCaptionLabel.Margin = new Padding(12, 0, 0, 6);
        unitPriceCaptionLabel.Name = "unitPriceCaptionLabel";
        unitPriceCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Caption;
        unitPriceCaptionLabel.TabIndex = 2;
        unitPriceCaptionLabel.Text = "Цена за шт., ₽";
        //
        // bookComboBox
        //
        bookComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        bookComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        bookComboBox.Margin = new Padding(0);
        bookComboBox.Name = "bookComboBox";
        bookComboBox.TabIndex = 3;
        //
        // quantityUpDown
        //
        quantityUpDown.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        quantityUpDown.BorderStyle = BorderStyle.FixedSingle;
        quantityUpDown.Margin = new Padding(12, 0, 0, 0);
        quantityUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        quantityUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        quantityUpDown.Name = "quantityUpDown";
        quantityUpDown.TabIndex = 4;
        quantityUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // unitPriceUpDown
        //
        unitPriceUpDown.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        unitPriceUpDown.BorderStyle = BorderStyle.FixedSingle;
        unitPriceUpDown.DecimalPlaces = 2;
        unitPriceUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
        unitPriceUpDown.Margin = new Padding(12, 0, 0, 0);
        unitPriceUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        unitPriceUpDown.Name = "unitPriceUpDown";
        unitPriceUpDown.TabIndex = 5;
        unitPriceUpDown.ThousandsSeparator = true;
        //
        // addItemButton
        //
        addItemButton.Glyph = "\uE710";
        addItemButton.Margin = new Padding(12, 0, 0, 0);
        addItemButton.Name = "addItemButton";
        addItemButton.Size = new Size(124, 36);
        addItemButton.TabIndex = 6;
        addItemButton.Text = "Добавить";
        addItemButton.Variant = BookStore.GUI.Controls.ButtonVariant.Primary;
        //
        // lockedHintLabel
        //
        lockedHintLabel.Margin = new Padding(0, 0, 0, 18);
        lockedHintLabel.Name = "lockedHintLabel";
        lockedHintLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        lockedHintLabel.TabIndex = 6;
        lockedHintLabel.Text = "Состав можно менять только у заказа в статусе «Новый».";
        lockedHintLabel.Visible = false;
        //
        // footerLayout
        //
        footerLayout.AutoSize = true;
        footerLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        footerLayout.ColumnCount = 2;
        footerLayout.ColumnStyles.Add(new ColumnStyle());
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        footerLayout.Controls.Add(cancelOrderButton, 0, 0);
        footerLayout.Controls.Add(actionsPanel, 1, 0);
        footerLayout.Dock = DockStyle.Fill;
        footerLayout.Margin = new Padding(0);
        footerLayout.Name = "footerLayout";
        footerLayout.RowCount = 1;
        footerLayout.RowStyles.Add(new RowStyle());
        footerLayout.TabIndex = 7;
        //
        // cancelOrderButton
        //
        cancelOrderButton.Enabled = false;
        cancelOrderButton.Glyph = "\uE711";
        cancelOrderButton.Margin = new Padding(0);
        cancelOrderButton.Name = "cancelOrderButton";
        cancelOrderButton.Size = new Size(170, 36);
        cancelOrderButton.TabIndex = 0;
        cancelOrderButton.Text = "Отменить заказ";
        cancelOrderButton.Variant = BookStore.GUI.Controls.ButtonVariant.Danger;
        //
        // actionsPanel
        //
        actionsPanel.AutoSize = true;
        actionsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionsPanel.Controls.Add(closeButton);
        actionsPanel.Controls.Add(completeButton);
        actionsPanel.Controls.Add(confirmButton);
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.FlowDirection = FlowDirection.RightToLeft;
        actionsPanel.Margin = new Padding(0);
        actionsPanel.Name = "actionsPanel";
        actionsPanel.TabIndex = 1;
        actionsPanel.WrapContents = false;
        //
        // closeButton
        //
        closeButton.DialogResult = DialogResult.Cancel;
        closeButton.Margin = new Padding(8, 0, 0, 0);
        closeButton.Name = "closeButton";
        closeButton.Size = new Size(110, 36);
        closeButton.TabIndex = 2;
        closeButton.Text = "Закрыть";
        //
        // completeButton
        //
        completeButton.Enabled = false;
        completeButton.Glyph = "\uE930";
        completeButton.Margin = new Padding(8, 0, 0, 0);
        completeButton.Name = "completeButton";
        completeButton.Size = new Size(140, 36);
        completeButton.TabIndex = 1;
        completeButton.Text = "Завершить";
        completeButton.Variant = BookStore.GUI.Controls.ButtonVariant.Primary;
        //
        // confirmButton
        //
        confirmButton.Enabled = false;
        confirmButton.Glyph = "\uE8FB";
        confirmButton.Margin = new Padding(0);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new Size(150, 36);
        confirmButton.TabIndex = 0;
        confirmButton.Text = "Подтвердить";
        confirmButton.Variant = BookStore.GUI.Controls.ButtonVariant.Primary;
        //
        // OrderEditorForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        CancelButton = closeButton;
        ClientSize = new Size(820, 680);
        Controls.Add(rootLayout);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "OrderEditorForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Заказ";
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        headerLayout.ResumeLayout(false);
        headerLayout.PerformLayout();
        infoLayout.ResumeLayout(false);
        infoLayout.PerformLayout();
        itemsHeaderLayout.ResumeLayout(false);
        itemsHeaderLayout.PerformLayout();
        itemsCard.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)itemsGrid).EndInit();
        addItemCard.ResumeLayout(false);
        addItemCard.PerformLayout();
        addItemLayout.ResumeLayout(false);
        addItemLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)quantityUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)unitPriceUpDown).EndInit();
        footerLayout.ResumeLayout(false);
        footerLayout.PerformLayout();
        actionsPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel rootLayout;
    private TableLayoutPanel headerLayout;
    private BookStore.GUI.Controls.ThemedLabel titleLabel;
    private BookStore.GUI.Controls.BadgeLabel statusBadge;
    private TableLayoutPanel infoLayout;
    private BookStore.GUI.Controls.ThemedLabel customerCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel createdCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel totalCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel customerValueLabel;
    private BookStore.GUI.Controls.ThemedLabel createdValueLabel;
    private BookStore.GUI.Controls.ThemedLabel totalValueLabel;
    private BookStore.GUI.Controls.AlertBanner errorBanner;
    private TableLayoutPanel itemsHeaderLayout;
    private BookStore.GUI.Controls.ThemedLabel itemsTitleLabel;
    private BookStore.GUI.Controls.ThemedButton removeItemButton;
    private BookStore.GUI.Controls.CardPanel itemsCard;
    private BookStore.GUI.Controls.ThemedGrid itemsGrid;
    private DataGridViewTextBoxColumn bookColumn;
    private DataGridViewTextBoxColumn quantityColumn;
    private DataGridViewTextBoxColumn unitPriceColumn;
    private DataGridViewTextBoxColumn totalColumn;
    private BookStore.GUI.Controls.CardPanel addItemCard;
    private TableLayoutPanel addItemLayout;
    private BookStore.GUI.Controls.ThemedLabel bookCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel quantityCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel unitPriceCaptionLabel;
    private ComboBox bookComboBox;
    private NumericUpDown quantityUpDown;
    private NumericUpDown unitPriceUpDown;
    private BookStore.GUI.Controls.ThemedButton addItemButton;
    private BookStore.GUI.Controls.ThemedLabel lockedHintLabel;
    private TableLayoutPanel footerLayout;
    private BookStore.GUI.Controls.ThemedButton cancelOrderButton;
    private FlowLayoutPanel actionsPanel;
    private BookStore.GUI.Controls.ThemedButton closeButton;
    private BookStore.GUI.Controls.ThemedButton completeButton;
    private BookStore.GUI.Controls.ThemedButton confirmButton;
}
