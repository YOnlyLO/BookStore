namespace BookStore.GUI.Forms;

partial class DashboardView
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        DataGridViewCellStyle createdCellStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle totalCellStyle = new DataGridViewCellStyle();
        rootLayout = new TableLayoutPanel();
        headerLayout = new TableLayoutPanel();
        titleLabel = new BookStore.GUI.Controls.ThemedLabel();
        subtitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        refreshButton = new BookStore.GUI.Controls.ThemedButton();
        errorBanner = new BookStore.GUI.Controls.AlertBanner();
        cardsLayout = new TableLayoutPanel();
        genresCard = new BookStore.GUI.Controls.StatCard();
        booksCard = new BookStore.GUI.Controls.StatCard();
        usersCard = new BookStore.GUI.Controls.StatCard();
        ordersCard = new BookStore.GUI.Controls.StatCard();
        bottomLayout = new TableLayoutPanel();
        statusCard = new BookStore.GUI.Controls.CardPanel();
        statusLayout = new TableLayoutPanel();
        statusTitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        statusHintLabel = new BookStore.GUI.Controls.ThemedLabel();
        statusMeter1 = new BookStore.GUI.Controls.StatusMeter();
        statusMeter2 = new BookStore.GUI.Controls.StatusMeter();
        statusMeter3 = new BookStore.GUI.Controls.StatusMeter();
        statusMeter4 = new BookStore.GUI.Controls.StatusMeter();
        revenueDivider = new Panel();
        revenueCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        revenueLabel = new BookStore.GUI.Controls.ThemedLabel();
        recentCard = new BookStore.GUI.Controls.CardPanel();
        recentLayout = new TableLayoutPanel();
        recentTitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        allOrdersButton = new BookStore.GUI.Controls.ThemedButton();
        recentGrid = new BookStore.GUI.Controls.ThemedGrid();
        numberColumn = new DataGridViewTextBoxColumn();
        customerColumn = new DataGridViewTextBoxColumn();
        createdColumn = new DataGridViewTextBoxColumn();
        statusColumn = new DataGridViewTextBoxColumn();
        totalColumn = new DataGridViewTextBoxColumn();
        rootLayout.SuspendLayout();
        headerLayout.SuspendLayout();
        cardsLayout.SuspendLayout();
        bottomLayout.SuspendLayout();
        statusCard.SuspendLayout();
        statusLayout.SuspendLayout();
        recentCard.SuspendLayout();
        recentLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)recentGrid).BeginInit();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(headerLayout, 0, 0);
        rootLayout.Controls.Add(errorBanner, 0, 1);
        rootLayout.Controls.Add(cardsLayout, 0, 2);
        rootLayout.Controls.Add(bottomLayout, 0, 3);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Margin = new Padding(0);
        rootLayout.Name = "rootLayout";
        rootLayout.RowCount = 4;
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 196F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.Size = new Size(980, 720);
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
        headerLayout.Controls.Add(subtitleLabel, 0, 1);
        headerLayout.Controls.Add(refreshButton, 1, 0);
        headerLayout.Dock = DockStyle.Fill;
        headerLayout.Margin = new Padding(0, 0, 0, 20);
        headerLayout.Name = "headerLayout";
        headerLayout.RowCount = 2;
        headerLayout.RowStyles.Add(new RowStyle());
        headerLayout.RowStyles.Add(new RowStyle());
        headerLayout.TabIndex = 0;
        //
        // titleLabel
        //
        titleLabel.Margin = new Padding(0, 0, 0, 2);
        titleLabel.Name = "titleLabel";
        titleLabel.Style = BookStore.GUI.Controls.LabelStyle.Title;
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Дашборд";
        //
        // subtitleLabel
        //
        subtitleLabel.Margin = new Padding(0);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "Сводка по данным BookStore.API. Нажмите на карточку, чтобы открыть раздел.";
        //
        // refreshButton
        //
        refreshButton.Anchor = AnchorStyles.Right;
        refreshButton.Glyph = "\uE72C";
        refreshButton.Margin = new Padding(16, 0, 0, 0);
        refreshButton.Name = "refreshButton";
        headerLayout.SetRowSpan(refreshButton, 2);
        refreshButton.Size = new Size(124, 36);
        refreshButton.TabIndex = 2;
        refreshButton.Text = "Обновить";
        //
        // errorBanner
        //
        errorBanner.Dock = DockStyle.Fill;
        errorBanner.Margin = new Padding(0, 0, 0, 16);
        errorBanner.Name = "errorBanner";
        errorBanner.TabIndex = 1;
        errorBanner.Visible = false;
        //
        // cardsLayout
        //
        cardsLayout.ColumnCount = 4;
        cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cardsLayout.Controls.Add(genresCard, 0, 0);
        cardsLayout.Controls.Add(booksCard, 1, 0);
        cardsLayout.Controls.Add(usersCard, 2, 0);
        cardsLayout.Controls.Add(ordersCard, 3, 0);
        cardsLayout.Dock = DockStyle.Fill;
        cardsLayout.Margin = new Padding(0, 0, 0, 16);
        cardsLayout.Name = "cardsLayout";
        cardsLayout.RowCount = 1;
        cardsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        cardsLayout.TabIndex = 2;
        //
        // genresCard
        //
        genresCard.Description = "Справочник жанров, к которым относятся книги";
        genresCard.Dock = DockStyle.Fill;
        genresCard.Glyph = "\uE8EC";
        genresCard.Margin = new Padding(0, 0, 12, 0);
        genresCard.Name = "genresCard";
        genresCard.TabIndex = 0;
        genresCard.Title = "Жанры";
        genresCard.Tone = BookStore.GUI.ViewModels.Tone.Accent;
        //
        // booksCard
        //
        booksCard.Description = "Каталог: авторы, цены, остатки на складе";
        booksCard.Dock = DockStyle.Fill;
        booksCard.Glyph = "\uE82D";
        booksCard.Margin = new Padding(6, 0, 6, 0);
        booksCard.Name = "booksCard";
        booksCard.TabIndex = 1;
        booksCard.Title = "Книги";
        //
        // usersCard
        //
        usersCard.Description = "Учётные записи покупателей";
        usersCard.Dock = DockStyle.Fill;
        usersCard.Glyph = "\uE716";
        usersCard.Margin = new Padding(6, 0, 6, 0);
        usersCard.Name = "usersCard";
        usersCard.TabIndex = 2;
        usersCard.Title = "Пользователи";
        usersCard.Tone = BookStore.GUI.ViewModels.Tone.Info;
        //
        // ordersCard
        //
        ordersCard.Description = "Состав заказов и их статусы";
        ordersCard.Dock = DockStyle.Fill;
        ordersCard.Glyph = "\uE7BF";
        ordersCard.Margin = new Padding(12, 0, 0, 0);
        ordersCard.Name = "ordersCard";
        ordersCard.TabIndex = 3;
        ordersCard.Title = "Заказы";
        ordersCard.Tone = BookStore.GUI.ViewModels.Tone.Warning;
        //
        // bottomLayout
        //
        bottomLayout.ColumnCount = 2;
        bottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
        bottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
        bottomLayout.Controls.Add(statusCard, 0, 0);
        bottomLayout.Controls.Add(recentCard, 1, 0);
        bottomLayout.Dock = DockStyle.Fill;
        bottomLayout.Margin = new Padding(0);
        bottomLayout.Name = "bottomLayout";
        bottomLayout.RowCount = 1;
        bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        bottomLayout.TabIndex = 3;
        //
        // statusCard
        //
        statusCard.Controls.Add(statusLayout);
        statusCard.Dock = DockStyle.Fill;
        statusCard.Margin = new Padding(0, 0, 8, 0);
        statusCard.Name = "statusCard";
        statusCard.Padding = new Padding(20, 18, 20, 18);
        statusCard.TabIndex = 0;
        //
        // statusLayout
        //
        statusLayout.ColumnCount = 1;
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        statusLayout.Controls.Add(statusTitleLabel, 0, 0);
        statusLayout.Controls.Add(statusHintLabel, 0, 1);
        statusLayout.Controls.Add(statusMeter1, 0, 2);
        statusLayout.Controls.Add(statusMeter2, 0, 3);
        statusLayout.Controls.Add(statusMeter3, 0, 4);
        statusLayout.Controls.Add(statusMeter4, 0, 5);
        statusLayout.Controls.Add(revenueDivider, 0, 6);
        statusLayout.Controls.Add(revenueCaptionLabel, 0, 7);
        statusLayout.Controls.Add(revenueLabel, 0, 8);
        statusLayout.Dock = DockStyle.Fill;
        statusLayout.Name = "statusLayout";
        statusLayout.RowCount = 10;
        statusLayout.RowStyles.Add(new RowStyle());
        statusLayout.RowStyles.Add(new RowStyle());
        statusLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        statusLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        statusLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        statusLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        statusLayout.RowStyles.Add(new RowStyle());
        statusLayout.RowStyles.Add(new RowStyle());
        statusLayout.RowStyles.Add(new RowStyle());
        statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        statusLayout.TabIndex = 0;
        //
        // statusTitleLabel
        //
        statusTitleLabel.Margin = new Padding(0, 0, 0, 2);
        statusTitleLabel.Name = "statusTitleLabel";
        statusTitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        statusTitleLabel.TabIndex = 0;
        statusTitleLabel.Text = "Заказы по статусам";
        //
        // statusHintLabel
        //
        statusHintLabel.Margin = new Padding(0, 0, 0, 12);
        statusHintLabel.Name = "statusHintLabel";
        statusHintLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        statusHintLabel.TabIndex = 1;
        statusHintLabel.Text = "Доля заказов в каждом статусе";
        //
        // statusMeter1
        //
        statusMeter1.Dock = DockStyle.Fill;
        statusMeter1.Margin = new Padding(0, 2, 0, 2);
        statusMeter1.Name = "statusMeter1";
        statusMeter1.TabIndex = 2;
        //
        // statusMeter2
        //
        statusMeter2.Dock = DockStyle.Fill;
        statusMeter2.Margin = new Padding(0, 2, 0, 2);
        statusMeter2.Name = "statusMeter2";
        statusMeter2.TabIndex = 3;
        //
        // statusMeter3
        //
        statusMeter3.Dock = DockStyle.Fill;
        statusMeter3.Margin = new Padding(0, 2, 0, 2);
        statusMeter3.Name = "statusMeter3";
        statusMeter3.TabIndex = 4;
        //
        // statusMeter4
        //
        statusMeter4.Dock = DockStyle.Fill;
        statusMeter4.Margin = new Padding(0, 2, 0, 2);
        statusMeter4.Name = "statusMeter4";
        statusMeter4.TabIndex = 5;
        //
        // revenueDivider
        //
        revenueDivider.Dock = DockStyle.Fill;
        revenueDivider.Margin = new Padding(0, 14, 0, 0);
        revenueDivider.Name = "revenueDivider";
        revenueDivider.Size = new Size(100, 1);
        revenueDivider.TabIndex = 6;
        //
        // revenueCaptionLabel
        //
        revenueCaptionLabel.Margin = new Padding(0, 14, 0, 2);
        revenueCaptionLabel.Name = "revenueCaptionLabel";
        revenueCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        revenueCaptionLabel.TabIndex = 7;
        revenueCaptionLabel.Text = "Выручка по завершённым заказам";
        //
        // revenueLabel
        //
        revenueLabel.Margin = new Padding(0);
        revenueLabel.Name = "revenueLabel";
        revenueLabel.Style = BookStore.GUI.Controls.LabelStyle.Title;
        revenueLabel.TabIndex = 8;
        revenueLabel.Text = "—";
        //
        // recentCard
        //
        recentCard.Controls.Add(recentLayout);
        recentCard.Dock = DockStyle.Fill;
        recentCard.Margin = new Padding(8, 0, 0, 0);
        recentCard.Name = "recentCard";
        recentCard.Padding = new Padding(20, 14, 12, 8);
        recentCard.TabIndex = 1;
        //
        // recentLayout
        //
        recentLayout.ColumnCount = 2;
        recentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        recentLayout.ColumnStyles.Add(new ColumnStyle());
        recentLayout.Controls.Add(recentTitleLabel, 0, 0);
        recentLayout.Controls.Add(allOrdersButton, 1, 0);
        recentLayout.Controls.Add(recentGrid, 0, 1);
        recentLayout.Dock = DockStyle.Fill;
        recentLayout.Name = "recentLayout";
        recentLayout.RowCount = 2;
        recentLayout.RowStyles.Add(new RowStyle());
        recentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        recentLayout.TabIndex = 0;
        //
        // recentTitleLabel
        //
        recentTitleLabel.Anchor = AnchorStyles.Left;
        recentTitleLabel.Margin = new Padding(0);
        recentTitleLabel.Name = "recentTitleLabel";
        recentTitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        recentTitleLabel.TabIndex = 0;
        recentTitleLabel.Text = "Последние заказы";
        //
        // allOrdersButton
        //
        allOrdersButton.Anchor = AnchorStyles.Right;
        allOrdersButton.Glyph = "\uE76C";
        allOrdersButton.Margin = new Padding(0, 0, 0, 8);
        allOrdersButton.Name = "allOrdersButton";
        allOrdersButton.Size = new Size(124, 34);
        allOrdersButton.TabIndex = 1;
        allOrdersButton.Text = "Все заказы";
        allOrdersButton.Variant = BookStore.GUI.Controls.ButtonVariant.Ghost;
        //
        // recentGrid
        //
        recentGrid.Columns.AddRange(new DataGridViewColumn[] { numberColumn, customerColumn, createdColumn, statusColumn, totalColumn });
        recentLayout.SetColumnSpan(recentGrid, 2);
        recentGrid.Dock = DockStyle.Fill;
        recentGrid.EmptyText = "Заказов пока нет";
        recentGrid.Margin = new Padding(0);
        recentGrid.Name = "recentGrid";
        recentGrid.ScrollBars = ScrollBars.Vertical;
        recentGrid.TabIndex = 2;
        //
        // numberColumn
        //
        numberColumn.DataPropertyName = "Number";
        numberColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        numberColumn.HeaderText = "Номер";
        numberColumn.Name = "numberColumn";
        numberColumn.Width = 92;
        numberColumn.ReadOnly = true;
        numberColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // customerColumn
        //
        customerColumn.DataPropertyName = "Customer";
        customerColumn.MinimumWidth = 100;
        customerColumn.HeaderText = "Покупатель";
        customerColumn.Name = "customerColumn";
        customerColumn.ReadOnly = true;
        customerColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // createdColumn
        //
        createdColumn.DataPropertyName = "CreatedAt";
        createdCellStyle.Format = "dd.MM.yyyy HH:mm";
        createdColumn.DefaultCellStyle = createdCellStyle;
        createdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        createdColumn.HeaderText = "Создан";
        createdColumn.Name = "createdColumn";
        createdColumn.Width = 130;
        createdColumn.ReadOnly = true;
        createdColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // statusColumn
        //
        statusColumn.DataPropertyName = "Status";
        statusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        statusColumn.HeaderText = "Статус";
        statusColumn.Name = "statusColumn";
        statusColumn.Width = 130;
        statusColumn.ReadOnly = true;
        statusColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // totalColumn
        //
        totalColumn.DataPropertyName = "TotalPrice";
        totalCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        totalCellStyle.Format = "C2";
        totalColumn.DefaultCellStyle = totalCellStyle;
        totalColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        totalColumn.HeaderText = "Сумма";
        totalColumn.Name = "totalColumn";
        totalColumn.Width = 104;
        totalColumn.ReadOnly = true;
        totalColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //
        // DashboardView
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(rootLayout);
        Name = "DashboardView";
        Size = new Size(980, 720);
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        headerLayout.ResumeLayout(false);
        headerLayout.PerformLayout();
        cardsLayout.ResumeLayout(false);
        bottomLayout.ResumeLayout(false);
        statusCard.ResumeLayout(false);
        statusLayout.ResumeLayout(false);
        statusLayout.PerformLayout();
        recentCard.ResumeLayout(false);
        recentLayout.ResumeLayout(false);
        recentLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)recentGrid).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel rootLayout;
    private TableLayoutPanel headerLayout;
    private BookStore.GUI.Controls.ThemedLabel titleLabel;
    private BookStore.GUI.Controls.ThemedLabel subtitleLabel;
    private BookStore.GUI.Controls.ThemedButton refreshButton;
    private BookStore.GUI.Controls.AlertBanner errorBanner;
    private TableLayoutPanel cardsLayout;
    private BookStore.GUI.Controls.StatCard genresCard;
    private BookStore.GUI.Controls.StatCard booksCard;
    private BookStore.GUI.Controls.StatCard usersCard;
    private BookStore.GUI.Controls.StatCard ordersCard;
    private TableLayoutPanel bottomLayout;
    private BookStore.GUI.Controls.CardPanel statusCard;
    private TableLayoutPanel statusLayout;
    private BookStore.GUI.Controls.ThemedLabel statusTitleLabel;
    private BookStore.GUI.Controls.ThemedLabel statusHintLabel;
    private BookStore.GUI.Controls.StatusMeter statusMeter1;
    private BookStore.GUI.Controls.StatusMeter statusMeter2;
    private BookStore.GUI.Controls.StatusMeter statusMeter3;
    private BookStore.GUI.Controls.StatusMeter statusMeter4;
    private Panel revenueDivider;
    private BookStore.GUI.Controls.ThemedLabel revenueCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel revenueLabel;
    private BookStore.GUI.Controls.CardPanel recentCard;
    private TableLayoutPanel recentLayout;
    private BookStore.GUI.Controls.ThemedLabel recentTitleLabel;
    private BookStore.GUI.Controls.ThemedButton allOrdersButton;
    private BookStore.GUI.Controls.ThemedGrid recentGrid;
    private DataGridViewTextBoxColumn numberColumn;
    private DataGridViewTextBoxColumn customerColumn;
    private DataGridViewTextBoxColumn createdColumn;
    private DataGridViewTextBoxColumn statusColumn;
    private DataGridViewTextBoxColumn totalColumn;
}
