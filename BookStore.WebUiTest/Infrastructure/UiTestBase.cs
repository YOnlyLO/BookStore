using OpenQA.Selenium;

namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Основа UI-тестов. Как в классическом сценарии Selenium: перед каждым тестом открывается
/// новое окно браузера ([TestInitialize]), после теста оно закрывается ([TestCleanup]).
/// Дополнительно база очищается перед каждым тестом, а при падении теста сохраняется скриншот.
/// </summary>
public abstract class UiTestBase
{
    public TestContext TestContext { get; set; } = null!;

    protected IWebDriver Browser { get; private set; } = null!;

    protected static TestDatabase Database => TestEnvironment.Database;

    [TestInitialize]
    public async Task OpenBrowserAsync()
    {
        await Database.ResetAsync();

        Browser = BrowserFactory.Create();
    }

    [TestCleanup]
    public void CloseBrowser()
    {
        if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
        {
            SaveScreenshot();
        }

        // Quit, а не Close: Close закрывает только окно, а процесс драйвера остаётся висеть.
        Browser.Quit();
    }

    private void SaveScreenshot()
    {
        var directory = TestContext.TestResultsDirectory ?? Path.GetTempPath();
        var path = Path.Combine(directory, $"{TestContext.TestName}_{DateTime.Now:HHmmss_fff}.png");

        Directory.CreateDirectory(directory);

        ((ITakesScreenshot)Browser).GetScreenshot().SaveAsFile(path);

        TestContext.AddResultFile(path);
    }
}
