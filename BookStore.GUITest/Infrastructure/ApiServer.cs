using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace BookStore.GUITest.Infrastructure;

/// <summary>
/// Экземпляр BookStore.API внутри процесса тестов. Он слушает настоящий порт через Kestrel
/// (а не работает в памяти, как обычный WebApplicationFactory): к нему обращается отдельный процесс BookStore.GUI.
/// </summary>
public sealed class ApiServer : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public ApiServer(string connectionString)
    {
        _connectionString = connectionString;

        UseKestrel(GuiTestSettings.ApiPort);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _connectionString);

        builder.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning));
    }
}
