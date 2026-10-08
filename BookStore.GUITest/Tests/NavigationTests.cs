using BookStore.Core.Enums;
using BookStore.GUITest.Infrastructure;
using BookStore.GUITest.Screens;

namespace BookStore.GUITest.Tests;

[TestClass]
public sealed class NavigationTests : GuiTestBase
{
    [TestMethod]
    public void Startup_ShowsDashboardAndTestApiAddress()
    {
        Assert.AreEqual(MainWindow.ExpectedTitle, MainWindow.Title);
        Assert.AreEqual("Дашборд", MainWindow.Dashboard.Title);
        // Приложение запущено с аргументом --api-url и работает с тестовым экземпляром API
        Assert.AreEqual(GuiTestSettings.ApiUrl, MainWindow.ApiAddress);
    }

    [TestMethod]
    [DataRow("genresNavButton", "genresView", "Жанры", "Справочник жанров, к которым относятся книги")]
    [DataRow("booksNavButton", "booksView", "Книги", "Каталог: авторы, цены, остатки на складе")]
    [DataRow("usersNavButton", "usersView", "Пользователи", "Учётные записи покупателей")]
    [DataRow("ordersNavButton", "ordersView", "Заказы", "Состав заказов и их статусы")]
    public void SidebarButton_OpensSection(string buttonId, string viewId, string title, string subtitle)
    {
        // Щелчок по пункту бокового меню
        MainWindow.ClickNavButton(buttonId);

        var section = RecordListSection.WaitFor(App, viewId);

        Assert.AreEqual(title, section.Title);
        Assert.AreEqual(subtitle, section.Subtitle);
        Assert.AreEqual("Записей: 0", section.CountText);
        // Одновременно виден только один раздел
        Assert.IsFalse(MainWindow.IsSectionVisible("dashboardView"));
    }

    [TestMethod]
    public void DashboardButton_ReturnsToDashboard()
    {
        MainWindow.OpenGenres();

        var dashboard = MainWindow.OpenDashboard();

        Assert.AreEqual("Дашборд", dashboard.Title);
        Assert.IsFalse(MainWindow.IsSectionVisible("genresView"));
    }

    [TestMethod]
    public async Task Dashboard_Refresh_ShowsRecordCountsAndRevenue()
    {
        var dashboard = MainWindow.Dashboard;

        // Приложение запущено с пустой базой
        Assert.AreEqual("Жанры: 0", dashboard.GetCard("genresCard"));

        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedGenreAsync("Детектив");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис", price: 450m);
        await Database.SeedBookAsync(fiction, "Эдем");
        var user = await Database.SeedUserAsync();
        await Database.SeedOrderAsync(user, OrderStatus.New, (solaris, 1));
        await Database.SeedOrderAsync(user, OrderStatus.Completed, (solaris, 2));

        // «Обновить» перечитывает данные из API
        dashboard.Refresh();

        Assert.AreEqual("Жанры: 2", dashboard.GetCard("genresCard"));
        Assert.AreEqual("Книги: 2", dashboard.GetCard("booksCard"));
        Assert.AreEqual("Пользователи: 1", dashboard.GetCard("usersCard"));
        Assert.AreEqual("Заказы: 2", dashboard.GetCard("ordersCard"));
        CollectionAssert.AreEqual(
            new[] { "Новый: 1", "Подтверждён: 0", "Завершён: 1", "Отменён: 0" },
            dashboard.GetOrderStatuses().ToArray());
        // Выручка считается только по завершённым заказам: 2 × 450 ₽
        Assert.AreEqual("900,00 ₽", dashboard.Revenue);
        Assert.AreEqual(2, dashboard.RecentOrders.RowCount);
    }

    [TestMethod]
    public void DashboardCard_OpensSection()
    {
        MainWindow.Dashboard.ClickCard("booksCard");

        var books = MainWindow.Books;

        Assert.AreEqual("Книги", books.Title);
        Assert.IsFalse(MainWindow.IsSectionVisible("dashboardView"));
    }
}
