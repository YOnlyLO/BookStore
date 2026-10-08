namespace BookStore.GUITest.Infrastructure;

/// <summary>
/// Настройки прогона GUI-тестов. Значения по умолчанию подходят для локального запуска;
/// переменные окружения позволяют подключиться к уже запущенному серверу Appium или другому PostgreSQL.
/// </summary>
public static class GuiTestSettings
{
    /// <summary>Сервер PostgreSQL, на котором создаётся временная база (как в BookStore.Test и BookStore.WebUiTest).</summary>
    public const string ConnectionStringVariable = "BOOKSTORE_TEST_CONNECTION";

    /// <summary>
    /// Адрес уже запущенного сервера Appium. Если не задан, тесты используют сервер на 127.0.0.1:4723,
    /// а если там никто не отвечает — запускают Appium сами и останавливают его после прогона.
    /// </summary>
    public const string AppiumUrlVariable = "BOOKSTORE_GUITEST_APPIUM_URL";

    /// <summary>Путь к BookStore.GUI.exe. По умолчанию — сборка проекта BookStore.GUI в той же конфигурации, что и тесты.</summary>
    public const string AppPathVariable = "BOOKSTORE_GUITEST_APP";

    public const string DefaultConnectionString =
        "Host=localhost;Port=5432;Username=postgres;Password=1025";

    /// <summary>Драйвер Appium для настольных приложений Windows (npm-пакет appium-novawindows-driver).</summary>
    public const string AutomationName = "NovaWindows";

    /// <summary>Порт тестового экземпляра API. Отличается от 5255, чтобы не мешать API, запущенному для разработки.</summary>
    public const int ApiPort = 5298;

    public static readonly Uri DefaultAppiumUrl = new("http://127.0.0.1:4723/");

    /// <summary>Адрес API в том виде, в котором приложение показывает его в боковой панели.</summary>
    public static readonly string ApiUrl = $"http://localhost:{ApiPort}";

    /// <summary>Сколько ждать появления окна, элемента или нужного состояния интерфейса.</summary>
    public static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(10);

    /// <summary>Сколько ждать ответа сервера Appium на одну команду (запуск приложения занимает несколько секунд).</summary>
    public static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(60);

    public static string ConnectionString =>
        Environment.GetEnvironmentVariable(ConnectionStringVariable) ?? DefaultConnectionString;

    public static Uri? ExternalAppiumUrl =>
        Environment.GetEnvironmentVariable(AppiumUrlVariable) is { Length: > 0 } url ? new Uri(url) : null;

    public static string AppPath =>
        Environment.GetEnvironmentVariable(AppPathVariable) ?? FindApp();

    /// <summary>
    /// Ищет BookStore.GUI.exe рядом с проектом тестов. Оба проекта нацелены на net10.0-windows,
    /// поэтому путь сборки приложения отличается от пути сборки тестов только каталогом проекта:
    /// BookStore.GUITest\bin\Debug\net10.0-windows → BookStore.GUI\bin\Debug\net10.0-windows.
    /// </summary>
    private static string FindApp()
    {
        var testsDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        for (var directory = testsDirectory; directory is not null; directory = directory.Parent)
        {
            if (!File.Exists(Path.Combine(directory.FullName, "BookStore.GUITest.csproj")))
            {
                continue;
            }

            var outputPath = Path.GetRelativePath(directory.FullName, testsDirectory.FullName);
            var path = Path.Combine(directory.Parent!.FullName, "BookStore.GUI", outputPath, "BookStore.GUI.exe");

            return File.Exists(path)
                ? path
                : throw new FileNotFoundException(
                    $"Не найден {path}. Соберите проект BookStore.GUI или укажите путь в {AppPathVariable}.");
        }

        throw new DirectoryNotFoundException(
            $"Каталог проекта BookStore.GUITest не найден выше {AppContext.BaseDirectory}. " +
            $"Укажите путь к BookStore.GUI.exe в {AppPathVariable}.");
    }
}
