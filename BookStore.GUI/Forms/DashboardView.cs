using BookStore.GUI.Controls;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class DashboardView : UserControl, IDashboardView
{
    private readonly StatusMeter[] _statusMeters;

    public DashboardView()
    {
        InitializeComponent();

        _statusMeters = [statusMeter1, statusMeter2, statusMeter3, statusMeter4];
        revenueDivider.BackColor = Palette.Border;

        refreshButton.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);

        genresCard.Click += (_, _) => SectionRequested?.Invoke(this, AppSection.Genres);
        booksCard.Click += (_, _) => SectionRequested?.Invoke(this, AppSection.Books);
        usersCard.Click += (_, _) => SectionRequested?.Invoke(this, AppSection.Users);
        ordersCard.Click += (_, _) => SectionRequested?.Invoke(this, AppSection.Orders);
        allOrdersButton.Click += (_, _) => SectionRequested?.Invoke(this, AppSection.Orders);

        recentGrid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
                SectionRequested?.Invoke(this, AppSection.Orders);
        };
    }

    public event EventHandler? RefreshRequested;

    public event EventHandler<AppSection>? SectionRequested;

    public void SetLoading(bool isLoading)
    {
        refreshButton.Enabled = !isLoading;
        refreshButton.Text = isLoading ? "Загрузка…" : "Обновить";
        UseWaitCursor = isLoading;
    }

    public void ShowSummary(DashboardSummary summary)
    {
        genresCard.Value = summary.GenreCount.ToString("N0");
        booksCard.Value = summary.BookCount.ToString("N0");
        usersCard.Value = summary.UserCount.ToString("N0");
        ordersCard.Value = summary.OrderCount.ToString("N0");

        for (var index = 0; index < _statusMeters.Length; index++)
        {
            var meter = _statusMeters[index];
            var stat = index < summary.OrderStatuses.Count ? summary.OrderStatuses[index] : null;

            meter.Visible = stat is not null;

            if (stat is null)
                continue;

            meter.Badge = stat.Status;
            meter.SetValue(stat.Count, summary.OrderCount);
        }

        revenueLabel.Text = summary.Revenue.ToString("C2");

        recentGrid.DataSource = summary.RecentOrders.ToList();
        recentGrid.ClearSelection();
    }

    public void ShowError(string? message)
    {
        errorBanner.ShowMessage(message);
    }
}
