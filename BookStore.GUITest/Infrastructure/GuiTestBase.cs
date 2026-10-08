using BookStore.GUITest.Screens;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Infrastructure;

/// <summary>
/// Основа GUI-тестов. Как в методичке: перед каждым тестом через Appium запускается тестируемое
/// приложение ([TestInitialize]), после теста оно закрывается ([TestCleanup]). Дополнительно база
/// очищается перед каждым тестом, а при падении теста сохраняется дерево элементов окна.
/// </summary>
public abstract class GuiTestBase
{
    public TestContext TestContext { get; set; } = null!;

    /// <summary>Сессия Appium, управляющая запущенным приложением (winDriver в методичке).</summary>
    protected WindowsDriver App { get; private set; } = null!;

    protected MainWindow MainWindow { get; private set; } = null!;

    protected static TestDatabase Database => TestEnvironment.Database;

    [TestInitialize]
    public async Task StartAppAsync()
    {
        await Database.ResetAsync();

        var options = new AppiumOptions
        {
            PlatformName = "Windows",
            AutomationName = GuiTestSettings.AutomationName,
            // Путь к тестируемому приложению
            App = GuiTestSettings.AppPath
        };

        // Приложение работает с тестовым экземпляром API, а не с адресом из своего appsettings.json
        options.AddAdditionalAppiumOption("appArguments", $"--api-url {GuiTestSettings.ApiUrl}");
        // После упавшего теста может остаться открытым модальное окно, и обычное закрытие
        // главного окна не сработает — поэтому процесс приложения завершается принудительно
        options.AddAdditionalAppiumOption("ms:forcequit", true);
        // Щелчок эмулируется настоящей мышью; пауза после него даёт приложению обработать щелчок
        // до следующей команды — иначе тест может прочитать состояние окна до щелчка
        options.AddAdditionalAppiumOption("delayAfterClick", 150);

        // Подключение к серверу Appium и запуск приложения
        App = new WindowsDriver(TestEnvironment.AppiumUrl, options, GuiTestSettings.CommandTimeout);

        MainWindow = MainWindow.WaitForStart(App);
    }

    [TestCleanup]
    public void CloseApp()
    {
        if (App is null)
        {
            return;
        }

        if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
        {
            SavePageSource();
        }

        // По завершении каждого теста закрыть тестируемое приложение
        App.Quit();
    }

    /// <summary>
    /// Сохраняет дерево элементов окна (XML со всеми AutomationId, именами и состояниями) —
    /// по нему видно, в каком состоянии был интерфейс, когда тест упал.
    /// </summary>
    private void SavePageSource()
    {
        try
        {
            var directory = TestContext.TestResultsDirectory ?? Path.GetTempPath();
            var path = Path.Combine(directory, $"{TestContext.TestName}_{DateTime.Now:HHmmss_fff}.xml");

            Directory.CreateDirectory(directory);
            File.WriteAllText(path, App.PageSource);

            TestContext.AddResultFile(path);
        }
        catch (Exception exception)
        {
            TestContext.WriteLine($"Не удалось сохранить дерево элементов: {exception.Message}");
        }
    }
}
