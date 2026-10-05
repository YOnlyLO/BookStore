namespace BookStore.GUI.Forms;

partial class MainForm
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
        sidebarLayout = new TableLayoutPanel();
        brandLayout = new TableLayoutPanel();
        logoLabel = new Label();
        brandTitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        brandSubtitleLabel = new BookStore.GUI.Controls.ThemedLabel();
        navigationPanel = new FlowLayoutPanel();
        dashboardNavButton = new BookStore.GUI.Controls.ThemedButton();
        genresNavButton = new BookStore.GUI.Controls.ThemedButton();
        booksNavButton = new BookStore.GUI.Controls.ThemedButton();
        usersNavButton = new BookStore.GUI.Controls.ThemedButton();
        ordersNavButton = new BookStore.GUI.Controls.ThemedButton();
        footerPanel = new FlowLayoutPanel();
        apiCaptionLabel = new BookStore.GUI.Controls.ThemedLabel();
        apiAddressLabel = new BookStore.GUI.Controls.ThemedLabel();
        sidebarBorder = new Panel();
        contentPanel = new Panel();
        dashboardView = new DashboardView();
        genresView = new RecordListView();
        booksView = new RecordListView();
        usersView = new RecordListView();
        ordersView = new RecordListView();
        sidebarLayout.SuspendLayout();
        brandLayout.SuspendLayout();
        navigationPanel.SuspendLayout();
        footerPanel.SuspendLayout();
        contentPanel.SuspendLayout();
        SuspendLayout();
        //
        // sidebarLayout
        //
        sidebarLayout.ColumnCount = 1;
        sidebarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        sidebarLayout.Controls.Add(brandLayout, 0, 0);
        sidebarLayout.Controls.Add(navigationPanel, 0, 1);
        sidebarLayout.Controls.Add(footerPanel, 0, 2);
        sidebarLayout.Dock = DockStyle.Left;
        sidebarLayout.Location = new Point(0, 0);
        sidebarLayout.Name = "sidebarLayout";
        sidebarLayout.Padding = new Padding(16, 20, 16, 16);
        sidebarLayout.RowCount = 3;
        sidebarLayout.RowStyles.Add(new RowStyle());
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        sidebarLayout.RowStyles.Add(new RowStyle());
        sidebarLayout.Size = new Size(236, 780);
        sidebarLayout.TabIndex = 0;
        //
        // brandLayout
        //
        brandLayout.AutoSize = true;
        brandLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        brandLayout.ColumnCount = 2;
        brandLayout.ColumnStyles.Add(new ColumnStyle());
        brandLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        brandLayout.Controls.Add(logoLabel, 0, 0);
        brandLayout.Controls.Add(brandTitleLabel, 1, 0);
        brandLayout.Controls.Add(brandSubtitleLabel, 1, 1);
        brandLayout.Dock = DockStyle.Fill;
        brandLayout.Margin = new Padding(4, 0, 0, 28);
        brandLayout.Name = "brandLayout";
        brandLayout.RowCount = 2;
        brandLayout.RowStyles.Add(new RowStyle());
        brandLayout.RowStyles.Add(new RowStyle());
        brandLayout.TabIndex = 0;
        //
        // logoLabel
        //
        logoLabel.Font = new Font("Segoe MDL2 Assets", 14F);
        logoLabel.Margin = new Padding(0, 0, 12, 0);
        logoLabel.Name = "logoLabel";
        brandLayout.SetRowSpan(logoLabel, 2);
        logoLabel.Size = new Size(40, 40);
        logoLabel.TabIndex = 0;
        logoLabel.Text = "\uE719";
        logoLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // brandTitleLabel
        //
        brandTitleLabel.Margin = new Padding(0);
        brandTitleLabel.Name = "brandTitleLabel";
        brandTitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Heading;
        brandTitleLabel.TabIndex = 1;
        brandTitleLabel.Text = "BookStore";
        //
        // brandSubtitleLabel
        //
        brandSubtitleLabel.Margin = new Padding(0);
        brandSubtitleLabel.Name = "brandSubtitleLabel";
        brandSubtitleLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        brandSubtitleLabel.TabIndex = 2;
        brandSubtitleLabel.Text = "Администрирование";
        //
        // navigationPanel
        //
        navigationPanel.Controls.Add(dashboardNavButton);
        navigationPanel.Controls.Add(genresNavButton);
        navigationPanel.Controls.Add(booksNavButton);
        navigationPanel.Controls.Add(usersNavButton);
        navigationPanel.Controls.Add(ordersNavButton);
        navigationPanel.Dock = DockStyle.Fill;
        navigationPanel.FlowDirection = FlowDirection.TopDown;
        navigationPanel.Margin = new Padding(0);
        navigationPanel.Name = "navigationPanel";
        navigationPanel.TabIndex = 1;
        navigationPanel.WrapContents = false;
        //
        // dashboardNavButton
        //
        dashboardNavButton.Glyph = "\uE80F";
        dashboardNavButton.Margin = new Padding(0, 0, 0, 4);
        dashboardNavButton.Name = "dashboardNavButton";
        dashboardNavButton.Size = new Size(204, 40);
        dashboardNavButton.TabIndex = 0;
        dashboardNavButton.Text = "Дашборд";
        dashboardNavButton.Variant = BookStore.GUI.Controls.ButtonVariant.Navigation;
        //
        // genresNavButton
        //
        genresNavButton.Glyph = "\uE8EC";
        genresNavButton.Margin = new Padding(0, 0, 0, 4);
        genresNavButton.Name = "genresNavButton";
        genresNavButton.Size = new Size(204, 40);
        genresNavButton.TabIndex = 1;
        genresNavButton.Text = "Жанры";
        genresNavButton.Variant = BookStore.GUI.Controls.ButtonVariant.Navigation;
        //
        // booksNavButton
        //
        booksNavButton.Glyph = "\uE82D";
        booksNavButton.Margin = new Padding(0, 0, 0, 4);
        booksNavButton.Name = "booksNavButton";
        booksNavButton.Size = new Size(204, 40);
        booksNavButton.TabIndex = 2;
        booksNavButton.Text = "Книги";
        booksNavButton.Variant = BookStore.GUI.Controls.ButtonVariant.Navigation;
        //
        // usersNavButton
        //
        usersNavButton.Glyph = "\uE716";
        usersNavButton.Margin = new Padding(0, 0, 0, 4);
        usersNavButton.Name = "usersNavButton";
        usersNavButton.Size = new Size(204, 40);
        usersNavButton.TabIndex = 3;
        usersNavButton.Text = "Пользователи";
        usersNavButton.Variant = BookStore.GUI.Controls.ButtonVariant.Navigation;
        //
        // ordersNavButton
        //
        ordersNavButton.Glyph = "\uE7BF";
        ordersNavButton.Margin = new Padding(0, 0, 0, 4);
        ordersNavButton.Name = "ordersNavButton";
        ordersNavButton.Size = new Size(204, 40);
        ordersNavButton.TabIndex = 4;
        ordersNavButton.Text = "Заказы";
        ordersNavButton.Variant = BookStore.GUI.Controls.ButtonVariant.Navigation;
        //
        // footerPanel
        //
        footerPanel.AutoSize = true;
        footerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        footerPanel.Controls.Add(apiCaptionLabel);
        footerPanel.Controls.Add(apiAddressLabel);
        footerPanel.Dock = DockStyle.Fill;
        footerPanel.FlowDirection = FlowDirection.TopDown;
        footerPanel.Margin = new Padding(4, 0, 0, 0);
        footerPanel.Name = "footerPanel";
        footerPanel.TabIndex = 2;
        footerPanel.WrapContents = false;
        //
        // apiCaptionLabel
        //
        apiCaptionLabel.Margin = new Padding(0, 0, 0, 2);
        apiCaptionLabel.Name = "apiCaptionLabel";
        apiCaptionLabel.Style = BookStore.GUI.Controls.LabelStyle.Subtle;
        apiCaptionLabel.TabIndex = 0;
        apiCaptionLabel.Text = "Подключение к API";
        //
        // apiAddressLabel
        //
        apiAddressLabel.Margin = new Padding(0);
        apiAddressLabel.Name = "apiAddressLabel";
        apiAddressLabel.Style = BookStore.GUI.Controls.LabelStyle.Muted;
        apiAddressLabel.TabIndex = 1;
        apiAddressLabel.Text = "http://localhost:5255";
        //
        // sidebarBorder
        //
        sidebarBorder.Dock = DockStyle.Left;
        sidebarBorder.Location = new Point(236, 0);
        sidebarBorder.Name = "sidebarBorder";
        sidebarBorder.Size = new Size(1, 780);
        sidebarBorder.TabIndex = 1;
        //
        // contentPanel
        //
        contentPanel.Controls.Add(dashboardView);
        contentPanel.Controls.Add(genresView);
        contentPanel.Controls.Add(booksView);
        contentPanel.Controls.Add(usersView);
        contentPanel.Controls.Add(ordersView);
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Location = new Point(237, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Padding = new Padding(28, 24, 28, 24);
        contentPanel.Size = new Size(1043, 780);
        contentPanel.TabIndex = 2;
        //
        // dashboardView
        //
        dashboardView.Dock = DockStyle.Fill;
        dashboardView.Name = "dashboardView";
        dashboardView.TabIndex = 0;
        //
        // genresView
        //
        genresView.Dock = DockStyle.Fill;
        genresView.Name = "genresView";
        genresView.TabIndex = 1;
        genresView.Visible = false;
        //
        // booksView
        //
        booksView.Dock = DockStyle.Fill;
        booksView.Name = "booksView";
        booksView.TabIndex = 2;
        booksView.Visible = false;
        //
        // usersView
        //
        usersView.Dock = DockStyle.Fill;
        usersView.Name = "usersView";
        usersView.TabIndex = 3;
        usersView.Visible = false;
        //
        // ordersView
        //
        ordersView.Dock = DockStyle.Fill;
        ordersView.Name = "ordersView";
        ordersView.TabIndex = 4;
        ordersView.Visible = false;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1280, 780);
        Controls.Add(contentPanel);
        Controls.Add(sidebarBorder);
        Controls.Add(sidebarLayout);
        KeyPreview = true;
        MinimumSize = new Size(1080, 680);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "BookStore — администрирование";
        sidebarLayout.ResumeLayout(false);
        sidebarLayout.PerformLayout();
        brandLayout.ResumeLayout(false);
        brandLayout.PerformLayout();
        navigationPanel.ResumeLayout(false);
        footerPanel.ResumeLayout(false);
        footerPanel.PerformLayout();
        contentPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel sidebarLayout;
    private TableLayoutPanel brandLayout;
    private Label logoLabel;
    private BookStore.GUI.Controls.ThemedLabel brandTitleLabel;
    private BookStore.GUI.Controls.ThemedLabel brandSubtitleLabel;
    private FlowLayoutPanel navigationPanel;
    private BookStore.GUI.Controls.ThemedButton dashboardNavButton;
    private BookStore.GUI.Controls.ThemedButton genresNavButton;
    private BookStore.GUI.Controls.ThemedButton booksNavButton;
    private BookStore.GUI.Controls.ThemedButton usersNavButton;
    private BookStore.GUI.Controls.ThemedButton ordersNavButton;
    private FlowLayoutPanel footerPanel;
    private BookStore.GUI.Controls.ThemedLabel apiCaptionLabel;
    private BookStore.GUI.Controls.ThemedLabel apiAddressLabel;
    private Panel sidebarBorder;
    private Panel contentPanel;
    private DashboardView dashboardView;
    private RecordListView genresView;
    private RecordListView booksView;
    private RecordListView usersView;
    private RecordListView ordersView;
}
