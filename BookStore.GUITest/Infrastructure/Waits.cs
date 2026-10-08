using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookStore.GUITest.Infrastructure;

/// <summary>
/// Явные ожидания. Приложение обращается к API асинхронно: после щелчка окно открывается,
/// а список перечитывается не сразу, поэтому проверять результат действия нужно,
/// дождавшись, пока интерфейс придёт в ожидаемое состояние.
/// </summary>
public static class Waits
{
    private static readonly TimeSpan PollingInterval = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// Повторяет проверку, пока она не вернёт true или не-null значение, и возвращает его.
    /// По истечении <see cref="GuiTestSettings.WaitTimeout"/> бросает WebDriverTimeoutException с сообщением.
    /// </summary>
    public static TResult Until<TResult>(IWebDriver driver, Func<IWebDriver, TResult?> condition, string message)
    {
        var wait = new WebDriverWait(driver, GuiTestSettings.WaitTimeout)
        {
            Message = message,
            PollingInterval = PollingInterval
        };

        // Пока приложение перестраивает окно, элемент может ещё не появиться или уже исчезнуть.
        // Драйвер NovaWindows сообщает об исчезнувшем элементе как о неизвестной ошибке (unknown error).
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(UnknownErrorException));

        return wait.Until(condition)!;
    }

    /// <summary>Ждёт, пока прочитанный текст станет равен ожидаемому, и возвращает последнее прочитанное значение.</summary>
    /// <remarks>
    /// Если текст так и не стал ожидаемым, возвращает последнее значение, а не бросает исключение:
    /// тогда Assert.AreEqual в тесте покажет понятную разницу между ожидаемым и фактическим текстом.
    /// </remarks>
    public static string ForText(IWebDriver driver, Func<string> read, string expected)
    {
        var actual = string.Empty;

        try
        {
            Until(driver, _ => (actual = read()) == expected, $"Текст не стал равен «{expected}»");
        }
        catch (WebDriverTimeoutException)
        {
            // Последнее значение вернётся в тест.
        }

        return actual;
    }
}
