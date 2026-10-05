namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Окружение всего прогона: временная база, API и фронтенд поднимаются один раз перед первым тестом
/// и останавливаются после последнего. Поэтому запускать BookStore.API и ng serve вручную не нужно,
/// а уже запущенные для разработки экземпляры (порты 5255 и 4200) тестам не мешают.
/// </summary>
[TestClass]
public static class TestEnvironment
{
    private static TestDatabase? _database;

    private static ApiServer? _api;

    private static FrontendServer? _frontend;

    public static TestDatabase Database =>
        _database ?? throw new InvalidOperationException("Тестовое окружение не запущено.");

    [AssemblyInitialize]
    public static async Task StartAsync(TestContext context)
    {
        _database = new TestDatabase();
        await _database.CreateAsync();

        _api = new ApiServer(_database.ConnectionString);
        _api.StartServer();

        _frontend = new FrontendServer();
        await _frontend.StartAsync();
    }

    [AssemblyCleanup]
    public static async Task StopAsync()
    {
        _frontend?.Dispose();

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
