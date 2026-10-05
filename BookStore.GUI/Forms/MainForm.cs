using BookStore.GUI.Controls;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class MainForm : Form, IMainView
{
    private readonly Dictionary<AppSection, (ThemedButton Button, Control View)> _sections;

    public MainForm()
    {
        InitializeComponent();

        DarkTheme.Apply(this);
        sidebarLayout.BackColor = Palette.BackgroundElevated;
        sidebarBorder.BackColor = Palette.Border;
        logoLabel.BackColor = Palette.Soft(Palette.Primary, Palette.BackgroundElevated, 0.16);
        logoLabel.ForeColor = Palette.Primary;

        _sections = new Dictionary<AppSection, (ThemedButton, Control)>
        {
            [AppSection.Dashboard] = (dashboardNavButton, dashboardView),
            [AppSection.Genres] = (genresNavButton, genresView),
            [AppSection.Books] = (booksNavButton, booksView),
            [AppSection.Users] = (usersNavButton, usersView),
            [AppSection.Orders] = (ordersNavButton, ordersView)
        };

        foreach (var (section, (button, _)) in _sections)
            button.Click += (_, _) => SectionSelected?.Invoke(this, section);
    }

    public event EventHandler? Started;

    public event EventHandler<AppSection>? SectionSelected;

    public IDashboardView Dashboard => dashboardView;

    public IRecordListView Genres => genresView;

    public IRecordListView Books => booksView;

    public IRecordListView Users => usersView;

    public IRecordListView Orders => ordersView;

    public void ShowSection(AppSection section)
    {
        foreach (var (key, (button, view)) in _sections)
        {
            button.IsSelected = key == section;
            view.Visible = key == section;
        }

        _sections[section].View.Focus();
    }

    public void SetApiAddress(string address)
    {
        apiAddressLabel.Text = address;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Started?.Invoke(this, EventArgs.Empty);
    }

    // Ctrl+1…Ctrl+5 — быстрый переход между разделами.
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        var section = keyData switch
        {
            Keys.Control | Keys.D1 => AppSection.Dashboard,
            Keys.Control | Keys.D2 => AppSection.Genres,
            Keys.Control | Keys.D3 => AppSection.Books,
            Keys.Control | Keys.D4 => AppSection.Users,
            Keys.Control | Keys.D5 => AppSection.Orders,
            _ => (AppSection?)null
        };

        if (section is { } target)
        {
            SectionSelected?.Invoke(this, target);
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
