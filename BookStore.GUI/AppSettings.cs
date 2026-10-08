using System.Text.Json;

namespace BookStore.GUI;

/// <summary>
/// Настройки из appsettings.json рядом с исполняемым файлом.
/// Адрес API можно переопределить аргументом командной строки: --api-url http://localhost:5298
/// (так GUI-тесты направляют приложение на свой экземпляр API с временной базой).
/// </summary>
public sealed class AppSettings
{
    private const string FileName = "appsettings.json";

    private const string ApiUrlArgument = "--api-url";

    public ApiSettings Api { get; init; } = new();

    public static AppSettings Load(string[] args)
    {
        var settings = LoadFile();
        var apiUrl = GetArgumentValue(args, ApiUrlArgument);

        return apiUrl is null
            ? settings
            : new AppSettings { Api = settings.Api with { BaseUrl = apiUrl } };
    }

    private static AppSettings LoadFile()
    {
        var path = Path.Combine(AppContext.BaseDirectory, FileName);

        if (!File.Exists(path))
            return new AppSettings();

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? new AppSettings();
    }

    private static string? GetArgumentValue(string[] args, string name)
    {
        var index = Array.IndexOf(args, name);

        return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
    }
}

public sealed record ApiSettings
{
    public string BaseUrl { get; init; } = "http://localhost:5255";

    public int TimeoutSeconds { get; init; } = 15;
}
