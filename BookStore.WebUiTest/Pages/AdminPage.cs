using BookStore.WebUiTest.Infrastructure;
using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

/// <summary>
/// Любая страница админ-панели: боковое меню, заголовок раздела и заголовок вкладки браузера.
/// Page Object скрывает от тестов селекторы: тест описывает действия пользователя,
/// а не устройство разметки.
/// </summary>
public class AdminPage
{
    private static readonly By HeadingLocator = By.CssSelector("h1.page-title");

    private static readonly By ActiveNavLinkLocator = By.CssSelector("nav .nav-link[aria-current='page']");

    public AdminPage(IWebDriver browser)
    {
        Browser = browser;
    }

    protected IWebDriver Browser { get; }

    public string Heading => Browser.FindElement(HeadingLocator).Text;

    public string DocumentTitle => Browser.Title;

    public string Path => new Uri(Browser.Url).AbsolutePath;

    public string ActiveNavLink => Browser.FindElement(ActiveNavLinkLocator).Text;

    public void ClickNavLink(string text)
    {
        Browser
            .FindElement(By.XPath($"//nav//a[normalize-space()={UiText.XPathLiteral(text)}]"))
            .Click();
    }

    /// <summary>
    /// Ждёт, пока роутер Angular загрузит раздел с указанным заголовком.
    /// </summary>
    public void WaitForHeading(string heading)
    {
        Waits.Until(
            Browser,
            driver => driver.FindElement(HeadingLocator).Text == heading,
            $"Не открылась страница «{heading}». Текущий адрес: {Browser.Url}");
    }

    protected void Navigate(string path)
    {
        Browser.Navigate().GoToUrl(new Uri(UiTestSettings.FrontendUrl, path));
    }
}
