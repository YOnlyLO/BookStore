namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Настройки прогона UI-тестов. Порты фиксированы: адрес API зашит в сборку фронтенда
/// (BookStore.Front/src/environments/environment.uitest.ts), а адрес фронтенда должен
/// быть разрешён в CORS-политике API.
/// </summary>
public static class UiTestSettings
{
    /// <summary>Сервер PostgreSQL, на котором создаётся временная база (как в BookStore.Test).</summary>
    public const string ConnectionStringVariable = "BOOKSTORE_TEST_CONNECTION";

    /// <summary>Браузер: edge (по умолчанию), chrome или firefox.</summary>
    public const string BrowserVariable = "BOOKSTORE_UITEST_BROWSER";

    /// <summary>true — запускать браузер без окна (например, на сервере сборки).</summary>
    public const string HeadlessVariable = "BOOKSTORE_UITEST_HEADLESS";

    public const string DefaultConnectionString =
        "Host=localhost;Port=5432;Username=postgres;Password=1025";

    public const int ApiPort = 5299;

    public const int FrontendPort = 4299;

    public static readonly Uri FrontendUrl = new($"http://localhost:{FrontendPort}/");

    /// <summary>Сколько ждать появления элемента или нужного состояния страницы.</summary>
    public static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(10);

    /// <summary>Сколько ждать первой сборки фронтенда dev-сервером Angular.</summary>
    public static readonly TimeSpan FrontendStartTimeout = TimeSpan.FromMinutes(3);

    public static string ConnectionString =>
        Environment.GetEnvironmentVariable(ConnectionStringVariable) ?? DefaultConnectionString;

    public static string Browser =>
        Environment.GetEnvironmentVariable(BrowserVariable)?.Trim().ToLowerInvariant() ?? "edge";

    public static bool Headless =>
        Environment.GetEnvironmentVariable(HeadlessVariable)?.Trim().ToLowerInvariant() is "1" or "true";
}
