using BookStore.WebUiTest.Infrastructure;
using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

/// <summary>
/// Раздел-справочник: панель со счётчиком, таблица записей или заглушка (загрузка, пусто, ошибка),
/// кнопки над таблицей и кнопки «Изменить» / «Удалить» в строках.
/// </summary>
public abstract class ListPage : AdminPage
{
    private const string PanelXPath = "//section[contains(concat(' ', normalize-space(@class), ' '), ' panel ')]";

    private static readonly By RowsLocator = By.CssSelector("section.panel .table-wrap > table > tbody > tr");

    private static readonly By SpinnerLocator = By.CssSelector("section.panel .state .spinner");

    private static readonly By ContentLocator = By.CssSelector("section.panel table, section.panel .state");

    protected ListPage(IWebDriver browser)
        : base(browser)
    {
    }

    public int Counter => int.Parse(Browser.FindElement(By.CssSelector("section.panel .counter")).Text);

    public string EmptyStateText => Browser.FindElement(By.CssSelector("section.panel .state p")).Text;

    public string LoadErrorText => Browser.FindElement(By.CssSelector("section.panel .state-error p")).Text;

    /// <summary>Тексты ячеек всех строк таблицы, сверху вниз.</summary>
    public IReadOnlyList<IReadOnlyList<string>> GetRows()
    {
        return Browser.FindElements(RowsLocator).Select(ReadCells).ToList();
    }

    /// <summary>Тексты ячеек строки, в которой есть ячейка с указанным текстом (название, имя, номер).</summary>
    public IReadOnlyList<string> GetRow(string key)
    {
        return ReadCells(WaitForRow(key));
    }

    public bool HasRow(string key)
    {
        return Browser.FindElements(RowLocator(key)).Count > 0;
    }

    public IWebElement WaitForRow(string key)
    {
        return Waits.Until(
            Browser,
            driver => driver.FindElement(RowLocator(key)),
            $"В таблице не появилась строка «{key}»");
    }

    public void WaitForRowToDisappear(string key)
    {
        Waits.Until(Browser, _ => !HasRow(key), $"Строка «{key}» не исчезла из таблицы");
    }

    public void WaitForLoadError()
    {
        Waits.Until(
            Browser,
            driver => driver.FindElement(By.CssSelector("section.panel .state-error")),
            "Не появилось сообщение об ошибке загрузки");
    }

    public void Retry()
    {
        Browser
            .FindElement(By.XPath($"{PanelXPath}//button[normalize-space()='Повторить']"))
            .Click();

        WaitUntilLoaded();
    }

    /// <summary>
    /// Ждёт окончания первой загрузки: спиннер исчез, а вместо него таблица или заглушка.
    /// </summary>
    protected void WaitUntilLoaded()
    {
        Waits.Until(
            Browser,
            driver => driver.FindElements(SpinnerLocator).Count == 0 && driver.FindElements(ContentLocator).Count > 0,
            "Список не загрузился");
    }

    protected void OpenSection(string path, string heading)
    {
        Navigate(path);
        WaitForHeading(heading);
        WaitUntilLoaded();
    }

    /// <summary>Нажимает кнопку над таблицей и ждёт открытия модального окна.</summary>
    protected ModalDialog ClickPageAction(string buttonText, string dialogTitle)
    {
        Browser
            .FindElement(By.XPath($"//*[contains(@class, 'page-actions')]//button[normalize-space()={UiText.XPathLiteral(buttonText)}]"))
            .Click();

        return ModalDialog.WaitFor(Browser, dialogTitle);
    }

    /// <summary>Нажимает кнопку в строке таблицы и ждёт открытия модального окна.</summary>
    protected ModalDialog ClickRowAction(string key, string buttonText, string dialogTitle)
    {
        WaitForRow(key)
            .FindElement(By.XPath($".//button[normalize-space()={UiText.XPathLiteral(buttonText)}]"))
            .Click();

        return ModalDialog.WaitFor(Browser, dialogTitle);
    }

    private static By RowLocator(string key)
    {
        var text = UiText.XPathLiteral(key);

        return By.XPath(
            $"{PanelXPath}//table/tbody/tr[td[normalize-space()={text}] " +
            $"or .//*[contains(concat(' ', normalize-space(@class), ' '), ' cell-main ') and normalize-space()={text}]]");
    }

    private static IReadOnlyList<string> ReadCells(IWebElement row)
    {
        return row
            .FindElements(By.TagName("td"))
            .Select(cell => UiText.Normalize(cell.Text))
            .ToList();
    }
}
