using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Главное окно: боковое меню разделов и рабочая область. В рабочей области виден один раздел —
/// скрытых разделов в дереве UI Automation нет, поэтому раздел ищется по AutomationId его представления.
/// </summary>
public sealed class MainWindow
{
    public const string ExpectedTitle = "BookStore — администрирование";

    private readonly WindowsDriver _app;

    private MainWindow(WindowsDriver app)
    {
        _app = app;
    }

    /// <summary>Заголовок окна.</summary>
    public string Title => _app.Title;

    /// <summary>Адрес API в нижней части бокового меню.</summary>
    public string ApiAddress => _app.FindElement(Locators.Id("apiAddressLabel")).Text;

    public DashboardSection Dashboard => DashboardSection.WaitFor(_app);

    public RecordListSection Genres => RecordListSection.WaitFor(_app, "genresView", "GenreEditorForm");

    public RecordListSection Books => RecordListSection.WaitFor(_app, "booksView", "BookEditorForm");

    public RecordListSection Users => RecordListSection.WaitFor(_app, "usersView", "UserEditorForm");

    public OrdersSection Orders => OrdersSection.WaitFor(_app);

    /// <summary>Ждёт, пока приложение запустится и дашборд загрузит сводку.</summary>
    public static MainWindow WaitForStart(WindowsDriver app)
    {
        var window = new MainWindow(app);

        _ = window.Dashboard;

        return window;
    }

    public DashboardSection OpenDashboard()
    {
        ClickNavButton("dashboardNavButton");

        return Dashboard;
    }

    public RecordListSection OpenGenres()
    {
        ClickNavButton("genresNavButton");

        return Genres;
    }

    public RecordListSection OpenBooks()
    {
        ClickNavButton("booksNavButton");

        return Books;
    }

    public RecordListSection OpenUsers()
    {
        ClickNavButton("usersNavButton");

        return Users;
    }

    public OrdersSection OpenOrders()
    {
        ClickNavButton("ordersNavButton");

        return Orders;
    }

    /// <summary>Нажимает кнопку бокового меню (dashboardNavButton, genresNavButton и т. д.).</summary>
    public void ClickNavButton(string automationId)
    {
        _app.FindElement(Locators.Id(automationId)).Click();
    }

    /// <summary>Есть ли в рабочей области раздел с указанным AutomationId (genresView, booksView…).</summary>
    public bool IsSectionVisible(string viewId)
    {
        return _app.Has(viewId);
    }
}
