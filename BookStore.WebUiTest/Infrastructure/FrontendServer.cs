using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Dev-сервер Angular (ng serve) для BookStore.Front в конфигурации uitest:
/// она подменяет environment.ts, чтобы фронтенд обращался к тестовому API.
/// </summary>
public sealed class FrontendServer : IDisposable
{
    private readonly StringBuilder _output = new();

    private Process? _process;

    public async Task StartAsync()
    {
        if (IsPortInUse(UiTestSettings.FrontendPort))
        {
            throw new InvalidOperationException(
                $"Порт {UiTestSettings.FrontendPort} уже занят. Остановите процесс, который его слушает " +
                "(например, оставшийся от прошлого прогона ng serve), и запустите тесты снова.");
        }

        var frontendDirectory = FindFrontendDirectory();
        var angularCli = Path.Combine(frontendDirectory, "node_modules", "@angular", "cli", "bin", "ng.js");

        if (!File.Exists(angularCli))
        {
            throw new InvalidOperationException(
                $"Не найден Angular CLI ({angularCli}). Выполните npm install в каталоге {frontendDirectory}.");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "node",
            WorkingDirectory = frontendDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        startInfo.ArgumentList.Add(angularCli);
        startInfo.ArgumentList.Add("serve");
        startInfo.ArgumentList.Add("--configuration=uitest");
        startInfo.ArgumentList.Add($"--port={UiTestSettings.FrontendPort}");
        startInfo.ArgumentList.Add("--live-reload=false");
        startInfo.Environment["NG_CLI_ANALYTICS"] = "false";

        _process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Не удалось запустить ng serve.");

        _process.OutputDataReceived += (_, e) => AppendOutput(e.Data);
        _process.ErrorDataReceived += (_, e) => AppendOutput(e.Data);
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        await WaitUntilReadyAsync();
    }

    public void Dispose()
    {
        if (_process is null)
        {
            return;
        }

        // node запускает дочерние процессы (сборщик, esbuild) — останавливаем всё дерево.
        if (!_process.HasExited)
        {
            _process.Kill(entireProcessTree: true);
            _process.WaitForExit();
        }

        _process.Dispose();
        _process = null;
    }

    private async Task WaitUntilReadyAsync()
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        var deadline = DateTime.UtcNow + UiTestSettings.FrontendStartTimeout;

        while (DateTime.UtcNow < deadline)
        {
            if (_process!.HasExited)
            {
                throw new InvalidOperationException(
                    $"ng serve завершился с кодом {_process.ExitCode}. Вывод:{Environment.NewLine}{GetOutput()}");
            }

            try
            {
                var html = await http.GetStringAsync(UiTestSettings.FrontendUrl);

                if (html.Contains("<app-root", StringComparison.Ordinal))
                {
                    return;
                }
            }
            catch (HttpRequestException)
            {
                // Сервер ещё не начал слушать порт.
            }
            catch (TaskCanceledException)
            {
                // Первая сборка ещё идёт.
            }

            await Task.Delay(TimeSpan.FromMilliseconds(500));
        }

        throw new TimeoutException(
            $"Фронтенд не ответил на {UiTestSettings.FrontendUrl} за {UiTestSettings.FrontendStartTimeout}. " +
            $"Вывод ng serve:{Environment.NewLine}{GetOutput()}");
    }

    private void AppendOutput(string? line)
    {
        if (line is null)
        {
            return;
        }

        lock (_output)
        {
            _output.AppendLine(line);
        }
    }

    private string GetOutput()
    {
        lock (_output)
        {
            return _output.ToString();
        }
    }

    private static bool IsPortInUse(int port)
    {
        try
        {
            using var client = new TcpClient();

            client.Connect("localhost", port);

            return true;
        }
        catch (SocketException)
        {
            return false;
        }
    }

    /// <summary>
    /// Ищет каталог BookStore.Front, поднимаясь от каталога сборки тестов к корню решения.
    /// </summary>
    private static string FindFrontendDirectory()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            var candidate = Path.Combine(directory.FullName, "BookStore.Front");

            if (File.Exists(Path.Combine(candidate, "angular.json")))
            {
                return candidate;
            }
        }

        throw new DirectoryNotFoundException(
            $"Каталог BookStore.Front не найден выше {AppContext.BaseDirectory}.");
    }
}
