using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Явные ожидания. Angular обновляет страницу асинхронно (запросы к API, отрисовка после
/// изменения сигналов), поэтому проверять результат сразу после клика нельзя — нужно дождаться,
/// пока страница придёт в ожидаемое состояние.
/// </summary>
public static class Waits
{
    /// <summary>
    /// Повторяет проверку, пока она не вернёт true или не-null значение, и возвращает его.
    /// По истечении <see cref="UiTestSettings.WaitTimeout"/> бросает WebDriverTimeoutException с сообщением.
    /// </summary>
    public static TResult Until<TResult>(IWebDriver browser, Func<IWebDriver, TResult?> condition, string message)
    {
        var wait = new WebDriverWait(browser, UiTestSettings.WaitTimeout)
        {
            Message = message
        };

        // Пока Angular перерисовывает страницу, элемент может ещё не появиться или уже быть заменён.
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

        return wait.Until(condition)!;
    }

    /// <summary>
    /// Ждёт отрисовки после последнего действия. Приложение работает без zone.js: Angular обновляет DOM
    /// не сразу после события, а в ближайшей макрозадаче или кадре анимации. Без этого ожидания тест может
    /// прочитать текст, оставшийся от промежуточного состояния (например, ошибку «Обязательное поле.»,
    /// показанную, пока поле было очищено перед вводом нового значения).
    /// </summary>
    public static void ForRender(IWebDriver browser)
    {
        ((IJavaScriptExecutor)browser).ExecuteAsyncScript(
            "const done = arguments[arguments.length - 1];" +
            "setTimeout(() => requestAnimationFrame(() => setTimeout(done)));");
    }

    /// <summary>
    /// Ждёт, пока прочитанное значение изменится (например, сумма заказа после добавления позиции).
    /// </summary>
    public static void UntilChanged(IWebDriver browser, Func<string> read, string initialValue, string message)
    {
        Until(browser, _ => read() != initialValue, message);
    }
}
