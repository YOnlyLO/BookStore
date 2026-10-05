using System.Net;

namespace BookStore.GUI.Services;

/// <summary>
/// Ошибка обращения к BookStore.API с сообщением, готовым для показа пользователю.
/// </summary>
public sealed class ApiException : Exception
{
    public ApiException(
        string message,
        HttpStatusCode? statusCode = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    /// <summary>Код ответа; null — ответа не было (сервер недоступен, таймаут).</summary>
    public HttpStatusCode? StatusCode { get; }

    public bool IsServerError => (int?)StatusCode >= 500;
}
