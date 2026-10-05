using BookStore.WebUiTest.Infrastructure;
using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

/// <summary>Дашборд: карточки разделов с количеством записей и сводка заказов по статусам.</summary>
public sealed class DashboardPage : AdminPage
{
    public const string HeadingText = "Дашборд";

    private DashboardPage(IWebDriver browser)
        : base(browser)
    {
    }

    public static DashboardPage Open(IWebDriver browser)
    {
        var page = new DashboardPage(browser);

        page.Navigate("admin");
        page.WaitUntilLoaded();

        return page;
    }

    /// <summary>Количество записей на карточке раздела («Жанры», «Книги» и т. д.).</summary>
    public int GetCardCount(string section)
    {
        return int.Parse(Card(section).FindElement(By.CssSelector(".card-value")).Text);
    }

    /// <summary>Количество заказов по каждому статусу в порядке отображения.</summary>
    public IReadOnlyList<(string Status, int Count)> GetOrderStats()
    {
        return Browser
            .FindElements(By.CssSelector(".stats .stat"))
            .Select(stat => (
                stat.FindElement(By.CssSelector(".badge")).Text,
                int.Parse(stat.FindElement(By.CssSelector(".stat-value")).Text)))
            .ToList();
    }

    public void OpenCard(string section)
    {
        Card(section).Click();
    }

    private IWebElement Card(string section)
    {
        return Browser.FindElement(By.XPath(
            $"//a[contains(@class, 'card')][.//*[contains(@class, 'card-label') and normalize-space()={UiText.XPathLiteral(section)}]]"));
    }

    /// <summary>Пока данные не пришли, вместо чисел на карточках показываются заглушки-скелетоны.</summary>
    private void WaitUntilLoaded()
    {
        WaitForHeading(HeadingText);

        Waits.Until(
            Browser,
            driver => driver.FindElements(By.CssSelector(".card .skeleton")).Count == 0
                && driver.FindElements(By.CssSelector(".stats .stat")).Count > 0,
            "Дашборд не загрузил данные");
    }
}
