import { AbstractControl, ValidatorFn } from '@angular/forms';

/** Как Validators.required, но строка из одних пробелов тоже считается пустой — так же, как в домене. */
export const notBlank: ValidatorFn = (control) =>
  typeof control.value === 'string' && control.value.trim().length === 0
    ? { required: true }
    : null;

export const integer: ValidatorFn = (control) =>
  control.value === null || control.value === '' || Number.isInteger(control.value)
    ? null
    : { integer: true };

/** Текст первой ошибки валидации поля, после того как пользователь с ним поработал. */
export function getFieldError(control: AbstractControl): string | null {
  const errors = control.errors;

  if (!errors || !(control.touched || control.dirty)) {
    return null;
  }

  if (errors['required']) {
    return 'Обязательное поле.';
  }

  if (errors['maxlength']) {
    return `Не более ${errors['maxlength'].requiredLength} символов.`;
  }

  if (errors['min']) {
    return `Значение должно быть не меньше ${Number(errors['min'].min).toLocaleString('ru-RU')}.`;
  }

  if (errors['email']) {
    return 'Некорректный адрес электронной почты.';
  }

  if (errors['integer']) {
    return 'Введите целое число.';
  }

  return 'Некорректное значение.';
}
