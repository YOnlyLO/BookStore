using BookStore.Core.Enums;
using BookStore.WebUiTest.Infrastructure;
using BookStore.WebUiTest.Pages;

namespace BookStore.WebUiTest.Tests;

[TestClass]
public sealed class NavigationTests : UiTestBase
{
    [TestMethod]
    public void RootUrl_RedirectsToDashboard()
    {
        // Открываем корень сайта — роутер должен перенаправить на дашборд админ-панели
        Browser.Navigate().GoToUrl(UiTestSettings.FrontendUrl);

        var page = new AdminPage(Browser);
        page.WaitForHeading(DashboardPage.HeadingText);

        Assert.AreEqual("/admin", page.Path);
        Assert.AreEqual("Дашборд · BookStore", page.DocumentTitle);
        Assert.AreEqual("Дашборд", page.ActiveNavLink);
    }

    [TestMethod]
    public void UnknownUrl_RedirectsToDashboard()
    {
        Browser.Navigate().GoToUrl(new Uri(UiTestSettings.FrontendUrl, "no/such/page"));

        var page = new AdminPage(Browser);
        page.WaitForHeading(DashboardPage.HeadingText);

        Assert.AreEqual("/admin", page.Path);
    }

    [TestMethod]
    [DataRow("Жанры", "/admin/genres")]
    [DataRow("Книги", "/admin/books")]
    [DataRow("Пользователи", "/admin/users")]
    [DataRow("Заказы", "/admin/orders")]
    public void SidebarLink_OpensSection(string section, string expectedPath)
    {
        var page = DashboardPage.Open(Browser);

        // Щелчок по пункту бокового меню
        page.ClickNavLink(section);
        page.WaitForHeading(section);

        Assert.AreEqual(expectedPath, page.Path);
        Assert.AreEqual($"{section} · BookStore", page.DocumentTitle);
        // Активный пункт меню помечен aria-current="page" — так его видят и скринридеры
        Assert.AreEqual(section, page.ActiveNavLink);
    }

    [TestMethod]
    public async Task Dashboard_ShowsRecordCountsAndOrderStatuses()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedGenreAsync("Детектив");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис");
        await Database.SeedBookAsync(fiction, "Непобедимый");
        await Database.SeedBookAsync(fiction, "Эдем");
        var user = await Database.SeedUserAsync();
        await Database.SeedOrderAsync(user, OrderStatus.New, (solaris, 1));
        await Database.SeedOrderAsync(user, OrderStatus.Completed, (solaris, 2));

        var page = DashboardPage.Open(Browser);

        Assert.AreEqual(2, page.GetCardCount("Жанры"));
        Assert.AreEqual(3, page.GetCardCount("Книги"));
        Assert.AreEqual(1, page.GetCardCount("Пользователи"));
        Assert.AreEqual(2, page.GetCardCount("Заказы"));

        CollectionAssert.AreEqual(
            new[] { ("Новый", 1), ("Подтверждён", 0), ("Завершён", 1), ("Отменён", 0) },
            page.GetOrderStats().ToArray());
    }

    [TestMethod]
    public void DashboardCard_OpensSection()
    {
        var page = DashboardPage.Open(Browser);

        page.OpenCard("Пользователи");
        page.WaitForHeading("Пользователи");

        Assert.AreEqual("/admin/users", page.Path);
    }
}
