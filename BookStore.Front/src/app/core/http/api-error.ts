import { HttpErrorResponse } from '@angular/common/http';
import { Resource } from '@angular/core';

interface ProblemDetails {
  title?: string;
  errors?: Record<string, string[]>;
}

/**
 * Превращает ошибку HTTP-запроса в сообщение для пользователя.
 * Доменные ошибки API возвращает строкой (BadRequest(result.Error)),
 * ошибки привязки модели — как ValidationProblemDetails.
 */
export function getErrorMessage(error: unknown): string {
  if (!(error instanceof HttpErrorResponse)) {
    return 'Произошла непредвиденная ошибка.';
  }

  if (error.status === 0) {
    return 'Не удалось связаться с сервером. Проверьте, что BookStore.API запущен.';
  }

  if (error.status >= 500) {
    return 'Внутренняя ошибка сервера. Подробности — в логе BookStore.API.';
  }

  if (typeof error.error === 'string' && error.error.trim()) {
    return error.error;
  }

  const problem = error.error as ProblemDetails | null;
  const firstValidationError = Object.values(problem?.errors ?? {}).flat()[0];

  if (firstValidationError) {
    return `Некорректные данные запроса: ${firstValidationError}`;
  }

  if (error.status === 404) {
    return 'Запись не найдена. Возможно, она уже удалена.';
  }

  return problem?.title ?? `Ошибка запроса (${error.status}).`;
}

/** Сообщение о первой ошибке среди ресурсов страницы или null, если ошибок нет. */
export function getResourceError(...resources: Pick<Resource<unknown>, 'error'>[]): string | null {
  const failed = resources.find((resource) => resource.error());

  return failed ? getErrorMessage(failed.error()) : null;
}
