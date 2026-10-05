namespace BookStore.GUI.Forms;

partial class RecordListView
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
        rootLayout = new TableLayoutPanel();
        headerLayout = new TableLayoutPanel();
        titleLabel = new BookStore.GUI.Controls.ThemedLabel();
        subtitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        countLabel = new BookStore.GUI.Controls.ThemedLabel();
        toolbarLayout = new TableLayoutPanel();
        buttonsPanel = new FlowLayoutPanel();
        addButton = new BookStore.GUI.Controls.ThemedButton();
        editButton = new BookStore.GUI.Controls.ThemedButton();
        deleteButton = new BookStore.GUI.Controls.ThemedButton();
        refreshButton = new BookStore.GUI.Controls.ThemedButton();
        searchBox = new BookStore.GUI.Controls.SearchBox();
        errorBanner = new BookStore.GUI.Controls.AlertBanner();
        gridCard = new BookStore.GUI.Controls.CardPanel();
        recordsGrid = new BookStore.GUI.Controls.ThemedGrid();
        rootLayout.SuspendLayout();
        headerLayout.SuspendLayout();
        toolbarLayout.SuspendLayout();
        buttonsPanel.SuspendLayout();
        gridCard.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)recordsGrid).BeginInit();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(headerLayout, 0, 0);
        rootLayout.Controls.Add(toolbarLayout, 0, 1);
        rootLayout.Controls.Add(errorBanner, 0, 2);
        rootLayout.Controls.Add(gridCard, 0, 3);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Margin = new Padding(0);
        rootLayout.Name = "rootLayout";
        rootLayout.RowCount = 4;
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
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
        headerLayout.Controls.Add(countLabel, 1, 1);
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
        titleLabel.Text = "Раздел";
        //
        // subtitleLabel
        //
        subtitleLabel.Margin = new Padding(0);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "Описание раздела";
        //
        // countLabel
        //
        countLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        countLabel.Margin = new Padding(16, 0, 0, 0);
        countLabel.Name = "countLabel";
        countLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        countLabel.TabIndex = 2;
        countLabel.Text = "Записей: 0";
        //
        // toolbarLayout
        //
        toolbarLayout.AutoSize = true;
        toolbarLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        toolbarLayout.ColumnCount = 2;
        toolbarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        toolbarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
        toolbarLayout.Controls.Add(buttonsPanel, 0, 0);
        toolbarLayout.Controls.Add(searchBox, 1, 0);
        toolbarLayout.Dock = DockStyle.Fill;
        toolbarLayout.Margin = new Padding(0, 0, 0, 14);
        toolbarLayout.Name = "toolbarLayout";
        toolbarLayout.RowCount = 1;
        toolbarLayout.RowStyles.Add(new RowStyle());
        toolbarLayout.TabIndex = 1;
        //
        // buttonsPanel
        //
        buttonsPanel.AutoSize = true;
        buttonsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttonsPanel.Controls.Add(addButton);
        buttonsPanel.Controls.Add(editButton);
        buttonsPanel.Controls.Add(deleteButton);
        buttonsPanel.Controls.Add(refreshButton);
        buttonsPanel.Dock = DockStyle.Fill;
        buttonsPanel.Margin = new Padding(0);
        buttonsPanel.Name = "buttonsPanel";
        buttonsPanel.TabIndex = 0;
        buttonsPanel.WrapContents = false;
        //
        // addButton
        //
        addButton.Glyph = "\uE710";
        addButton.Margin = new Padding(0, 0, 8, 0);
        addButton.Name = "addButton";
        addButton.Size = new Size(124, 36);
        addButton.TabIndex = 0;
        addButton.Text = "Добавить";
        addButton.Variant = BookStore.GUI.Controls.ButtonVariant.Primary;
        //
        // editButton
        //
        editButton.Glyph = "\uE70F";
        editButton.Margin = new Padding(0, 0, 8, 0);
        editButton.Name = "editButton";
        editButton.Size = new Size(124, 36);
        editButton.TabIndex = 1;
        editButton.Text = "Изменить";
        //
        // deleteButton
        //
        deleteButton.Glyph = "\uE74D";
        deleteButton.Margin = new Padding(0, 0, 8, 0);
        deleteButton.Name = "deleteButton";
        deleteButton.Size = new Size(116, 36);
        deleteButton.TabIndex = 2;
        deleteButton.Text = "Удалить";
        deleteButton.Variant = BookStore.GUI.Controls.ButtonVariant.Danger;
        //
        // refreshButton
        //
        refreshButton.Glyph = "\uE72C";
        refreshButton.Margin = new Padding(0);
        refreshButton.Name = "refreshButton";
        refreshButton.Size = new Size(124, 36);
        refreshButton.TabIndex = 3;
        refreshButton.Text = "Обновить";
        refreshButton.Variant = BookStore.GUI.Controls.ButtonVariant.Ghost;
        //
        // searchBox
        //
        searchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        searchBox.Margin = new Padding(0);
        searchBox.Name = "searchBox";
        searchBox.PlaceholderText = "Поиск по списку…  (Ctrl+F)";
        searchBox.Size = new Size(300, 36);
        searchBox.TabIndex = 1;
        //
        // errorBanner
        //
        errorBanner.Dock = DockStyle.Fill;
        errorBanner.Margin = new Padding(0, 0, 0, 14);
        errorBanner.Name = "errorBanner";
        errorBanner.TabIndex = 2;
        errorBanner.Visible = false;
        //
        // gridCard
        //
        gridCard.Controls.Add(recordsGrid);
        gridCard.Dock = DockStyle.Fill;
        gridCard.Margin = new Padding(0);
        gridCard.Name = "gridCard";
        gridCard.Padding = new Padding(6, 4, 6, 6);
        gridCard.TabIndex = 3;
        //
        // recordsGrid
        //
        recordsGrid.Dock = DockStyle.Fill;
        recordsGrid.Name = "recordsGrid";
        recordsGrid.TabIndex = 0;
        //
        // RecordListView
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(rootLayout);
        Name = "RecordListView";
        Size = new Size(980, 720);
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        headerLayout.ResumeLayout(false);
        headerLayout.PerformLayout();
        toolbarLayout.ResumeLayout(false);
        toolbarLayout.PerformLayout();
        buttonsPanel.ResumeLayout(false);
        gridCard.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)recordsGrid).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel rootLayout;
    private TableLayoutPanel headerLayout;
    private BookStore.GUI.Controls.ThemedLabel titleLabel;
    private BookStore.GUI.Controls.ThemedLabel subtitleLabel;
    private BookStore.GUI.Controls.ThemedLabel countLabel;
    private TableLayoutPanel toolbarLayout;
    private FlowLayoutPanel buttonsPanel;
    private BookStore.GUI.Controls.ThemedButton addButton;
    private BookStore.GUI.Controls.ThemedButton editButton;
    private BookStore.GUI.Controls.ThemedButton deleteButton;
    private BookStore.GUI.Controls.ThemedButton refreshButton;
    private BookStore.GUI.Controls.SearchBox searchBox;
    private BookStore.GUI.Controls.AlertBanner errorBanner;
    private BookStore.GUI.Controls.CardPanel gridCard;
    private BookStore.GUI.Controls.ThemedGrid recordsGrid;
}
