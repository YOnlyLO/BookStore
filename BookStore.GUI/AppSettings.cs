using System.Text.Json;

namespace BookStore.GUI;

/// <summary>
/// Настройки из appsettings.json рядом с исполняемым файлом.
/// </summary>
public sealed class AppSettings
{
    private const string FileName = "appsettings.json";

    public ApiSettings Api { get; init; } = new();

    public static AppSettings Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, FileName);

        if (!File.Exists(path))
            return new AppSettings();

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? new AppSettings();
    }
}

public sealed class ApiSettings
{
    public string BaseUrl { get; init; } = "http://localhost:5255";

    public int TimeoutSeconds { get; init; } = 15;
}
