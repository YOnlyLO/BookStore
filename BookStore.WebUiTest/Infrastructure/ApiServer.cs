using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Экземпляр BookStore.API внутри процесса тестов. В отличие от обычного WebApplicationFactory
/// (сервер в памяти) он слушает настоящий порт через Kestrel — иначе браузер не смог бы к нему обратиться.
/// </summary>
public sealed class ApiServer : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public ApiServer(string connectionString)
    {
        _connectionString = connectionString;

        UseKestrel(UiTestSettings.ApiPort);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _connectionString);
        builder.UseSetting("Cors:AllowedOrigins:0", UiTestSettings.FrontendUrl.GetLeftPart(UriPartial.Authority));

        builder.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning));
    }
}
