using BookStore.WebUiTest.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookStore.WebUiTest.Pages;

/// <summary>
/// Модальное окно на нативном &lt;dialog&gt; (формы создания и изменения, подтверждение удаления).
/// Поля формы ищутся по атрибуту formcontrolname: Angular оставляет его в разметке,
/// и он совпадает с именем поля в модели формы — такой локатор не ломается при смене вёрстки.
/// </summary>
public sealed class ModalDialog
{
    private const string OpenDialogs = "dialog[open]";

    private readonly IWebDriver _browser;

    private readonly IWebElement _root;

    private ModalDialog(IWebDriver browser, IWebElement root)
    {
        _browser = browser;
        _root = root;

        // Заголовок запоминается сразу: после закрытия окна прочитать его из DOM уже нельзя.
        Title = root.FindElement(By.CssSelector(".modal-title")).Text;
    }

    public string Title { get; }

    /// <summary>Текст сообщения в окне подтверждения.</summary>
    public string Message => _root.FindElement(By.CssSelector(".modal-body > p")).Text;

    public string ErrorAlert => WaitForAlert("alert-error");

    public string WarningAlert => WaitForAlert("alert-warning");

    public string InfoAlert => WaitForAlert("alert-info");

    public bool HasErrorAlert => _root.FindElements(By.CssSelector(".alert-error")).Count > 0;

    public bool IsOpen
    {
        get
        {
            try
            {
                return _root.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                // Angular убрал окно из DOM.
                return false;
            }
        }
    }

    /// <summary>
    /// Ждёт открытия окна с указанным заголовком. Окна бывают вложенными
    /// (подтверждение поверх редактора заказа), поэтому окно определяется по заголовку.
    /// </summary>
    public static ModalDialog WaitFor(IWebDriver browser, string title)
    {
        return WaitFor(browser, actual => actual == title, $"«{title}»");
    }

    public static ModalDialog WaitForTitleStartingWith(IWebDriver browser, string prefix)
    {
        return WaitFor(browser, actual => actual.StartsWith(prefix, StringComparison.Ordinal), $"«{prefix}…»");
    }

    public IWebElement Field(string name)
    {
        return _root.FindElement(FieldLocator(name));
    }

    public bool HasField(string name)
    {
        return _root.FindElements(FieldLocator(name)).Count > 0;
    }

    public string GetValue(string name)
    {
        return Field(name).GetDomProperty("value") ?? string.Empty;
    }

    /// <summary>
    /// Заменяет значение поля. Clear() не генерирует событие input, и Angular не узнал бы
    /// об очистке, поэтому старое значение выделяется и удаляется с клавиатуры, как это сделал бы пользователь.
    /// </summary>
    public void Fill(string name, string value)
    {
        var field = Field(name);

        field.SendKeys(Keys.Control + "a");
        field.SendKeys(Keys.Delete);
        field.SendKeys(value);
    }

    public void Select(string name, string optionText, bool partialMatch = false)
    {
        new SelectElement(Field(name)).SelectByText(optionText, partialMatch);
    }

    public string GetSelectedOption(string name)
    {
        return new SelectElement(Field(name)).SelectedOption.Text;
    }

    /// <summary>Ждёт сообщения об ошибке под полем и возвращает его текст.</summary>
    public string FieldError(string name)
    {
        Waits.ForRender(_browser);

        return Waits.Until(
            _browser,
            _ => FieldMessage(name, "field-error")?.Text,
            $"Под полем «{name}» не появилась ошибка");
    }

    public bool HasFieldError(string name)
    {
        Waits.ForRender(_browser);

        return FieldMessage(name, "field-error") is not null;
    }

    public string? FieldHint(string name)
    {
        return FieldMessage(name, "field-hint")?.Text;
    }

    public IWebElement Button(string text)
    {
        return _root.FindElement(By.XPath($".//button[normalize-space()={UiText.XPathLiteral(text)}]"));
    }

    public void Click(string buttonText)
    {
        Button(buttonText).Click();
    }

    /// <summary>Отправляет форму кнопкой «Создать» / «Сохранить».</summary>
    public void Submit()
    {
        _root.FindElement(By.CssSelector("button[type='submit']")).Click();
    }

    /// <summary>Закрывает окно крестиком в заголовке.</summary>
    public void Close()
    {
        _root.FindElement(By.CssSelector(".modal-header button[aria-label='Закрыть']")).Click();
    }

    public void WaitUntilClosed()
    {
        Waits.Until(_browser, _ => !IsOpen, $"Модальное окно «{Title}» не закрылось");
    }

    internal IWebElement Find(By locator)
    {
        return _root.FindElement(locator);
    }

    internal IReadOnlyCollection<IWebElement> FindAll(By locator)
    {
        return _root.FindElements(locator);
    }

    private static ModalDialog WaitFor(IWebDriver browser, Func<string, bool> matchesTitle, string description)
    {
        var root = Waits.Until(
            browser,
            driver => driver
                .FindElements(By.CssSelector(OpenDialogs))
                .LastOrDefault(dialog => matchesTitle(dialog.FindElement(By.CssSelector(".modal-title")).Text)),
            $"Не открылось модальное окно {description}");

        return new ModalDialog(browser, root);
    }

    private static By FieldLocator(string name)
    {
        return By.CssSelector($"[formcontrolname='{name}']");
    }

    private IWebElement? FieldMessage(string name, string cssClass)
    {
        return Field(name)
            .FindElements(By.XPath($"./ancestor::label[1]//span[contains(@class, '{cssClass}')]"))
            .FirstOrDefault();
    }

    private string WaitForAlert(string cssClass)
    {
        return Waits.Until(
            _browser,
            _ => _root.FindElement(By.CssSelector($".{cssClass}")).Text,
            $"В окне «{Title}» не появилось сообщение .{cssClass}");
    }
}
