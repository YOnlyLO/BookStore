namespace BookStore.GUITest.Infrastructure;

/// <summary>
/// Окружение всего прогона: временная база, API и сервер Appium поднимаются один раз перед первым тестом
/// и останавливаются после последнего. Запускать BookStore.API и Appium вручную не нужно,
/// а API, запущенный для разработки (порт 5255), тестам не мешает.
/// </summary>
[TestClass]
public static class TestEnvironment
{
    private static TestDatabase? _database;

    private static ApiServer? _api;

    private static AppiumServer? _appium;

    public static TestDatabase Database =>
        _database ?? throw new InvalidOperationException("Тестовое окружение не запущено.");

    public static Uri AppiumUrl =>
        _appium?.Url ?? throw new InvalidOperationException("Тестовое окружение не запущено.");

    [AssemblyInitialize]
    public static async Task StartAsync(TestContext context)
    {
        _database = new TestDatabase();
        await _database.CreateAsync();

        _api = new ApiServer(_database.ConnectionString);
        _api.StartServer();

        _appium = new AppiumServer();
        await _appium.StartAsync();
    }

    [AssemblyCleanup]
    public static async Task StopAsync()
    {
        _appium?.Dispose();

        if (_api is not null)
        {
            await _api.DisposeAsync();
        }

        if (_database is not null)
        {
            await _database.DropAsync();
        }
    }
}
