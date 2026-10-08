using System.Net.Http.Json;
using System.Text.Json;
using OpenQA.Selenium.Appium.Service;

namespace BookStore.GUITest.Infrastructure;

/// <summary>
/// Сервер Appium — посредник между тестами и приложением (в методичке эту роль играет WinAppDriver).
/// Если сервер уже запущен (например, вручную командой appium), тесты подключаются к нему;
/// иначе запускают свой экземпляр через AppiumLocalService и останавливают его после прогона.
/// </summary>
public sealed class AppiumServer : IDisposable
{
    private AppiumLocalService? _service;

    public Uri Url { get; private set; } = GuiTestSettings.DefaultAppiumUrl;

    public async Task StartAsync()
    {
        if (GuiTestSettings.ExternalAppiumUrl is { } externalUrl)
        {
            Url = externalUrl;

            if (!await IsReadyAsync(externalUrl))
            {
                throw new InvalidOperationException(
                    $"Сервер Appium не отвечает по адресу {externalUrl} (задан в {GuiTestSettings.AppiumUrlVariable}).");
            }

            return;
        }

        if (await IsReadyAsync(Url))
        {
            return;
        }

        // AppiumLocalService сам находит Appium, установленный через npm (npm install -g appium),
        // запускает его на node и ждёт, пока сервер начнёт принимать команды.
        _service = new AppiumServiceBuilder()
            .WithIPAddress(Url.Host)
            .UsingPort(Url.Port)
            .WithLogFile(new FileInfo(Path.Combine(AppContext.BaseDirectory, "appium.log")))
            .Build();

        try
        {
            _service.Start();
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                "Не удалось запустить сервер Appium. Установите его и драйвер для Windows-приложений:" +
                $"{Environment.NewLine}  npm install -g appium" +
                $"{Environment.NewLine}  appium driver install --source=npm appium-novawindows-driver",
                exception);
        }
    }

    public void Dispose()
    {
        _service?.Dispose();
        _service = null;
    }

    private static async Task<bool> IsReadyAsync(Uri url)
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };

        try
        {
            var status = await http.GetFromJsonAsync<AppiumStatus>(new Uri(url, "status"));

            return status?.Value?.Ready == true;
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            return false;
        }
    }

    private sealed record AppiumStatus(AppiumStatusValue? Value);

    private sealed record AppiumStatusValue(bool Ready);
}
