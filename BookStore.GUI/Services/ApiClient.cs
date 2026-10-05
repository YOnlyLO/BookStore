using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace BookStore.GUI.Services;

/// <summary>
/// Обёртка над HttpClient: сериализация JSON и перевод ошибок HTTP в <see cref="ApiException"/>.
/// </summary>
public sealed class ApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetAsync<T>(
        string uri,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Get, uri, null, cancellationToken);

        return await ReadAsync<T>(response, cancellationToken);
    }

    public async Task<T> PostAsync<T>(
        string uri,
        object body,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Post, uri, body, cancellationToken);

        return await ReadAsync<T>(response, cancellationToken);
    }

    public async Task PostAsync(
        string uri,
        object? body = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Post, uri, body, cancellationToken);
    }

    public async Task PutAsync(
        string uri,
        object body,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Put, uri, body, cancellationToken);
    }

    public async Task DeleteAsync(
        string uri,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Delete, uri, null, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string uri,
        object? body,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, uri);

        if (body is not null)
            request.Content = JsonContent.Create(body, body.GetType(), options: JsonOptions);

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            throw new ApiException(
                $"Не удалось связаться с сервером {_httpClient.BaseAddress}. Проверьте, что BookStore.API запущен.",
                innerException: exception);
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            throw new ApiException(
                "Сервер не ответил вовремя. Повторите попытку позже.",
                innerException: exception);
        }

        if (response.IsSuccessStatusCode)
            return response;

        using (response)
        {
            var message = await ReadErrorMessageAsync(response, cancellationToken);

            throw new ApiException(message, response.StatusCode);
        }
    }

    private static async Task<T> ReadAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var value = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);

        return value ?? throw new ApiException("Сервер вернул пустой ответ.", response.StatusCode);
    }

    /// <summary>
    /// Доменные ошибки API возвращает строкой (BadRequest(result.Error)),
    /// ошибки привязки модели — как ValidationProblemDetails.
    /// </summary>
    private static async Task<string> ReadErrorMessageAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var statusCode = (int)response.StatusCode;

        if (statusCode >= 500)
            return $"Внутренняя ошибка сервера ({statusCode}). Подробности — в логе BookStore.API.";

        if (response.StatusCode == HttpStatusCode.NotFound)
            return "Запись не найдена. Возможно, она уже удалена.";

        var content = (await response.Content.ReadAsStringAsync(cancellationToken)).Trim();

        try
        {
            if (content.StartsWith('"'))
                return JsonSerializer.Deserialize<string>(content) ?? content;

            if (content.StartsWith('{'))
            {
                var problem = JsonSerializer.Deserialize<ProblemDetails>(content, JsonOptions);
                var firstError = problem?.Errors?.Values.SelectMany(errors => errors).FirstOrDefault();

                if (firstError is not null)
                    return $"Некорректные данные запроса: {firstError}";

                if (problem?.Title is not null)
                    return problem.Title;
            }
        }
        catch (JsonException)
        {
            // Тело не JSON — покажем его как есть.
        }

        return content.Length > 0
            ? content
            : $"Ошибка запроса ({statusCode}).";
    }

    private sealed record ProblemDetails(
        string? Title,
        Dictionary<string, string[]>? Errors);
}
