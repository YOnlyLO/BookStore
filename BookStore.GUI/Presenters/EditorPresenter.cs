using BookStore.GUI.Services;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

/// <summary>Ошибка проверки поля: имя свойства представления и текст для пользователя.</summary>
public sealed record ValidationError(
    string Field,
    string Message);

/// <summary>
/// Общая логика модального окна записи: проверка введённых данных, сохранение через API,
/// показ ошибок сервера и закрытие окна после успешного сохранения.
/// </summary>
public abstract class EditorPresenter<TView>
    where TView : IEditorView
{
    private Guid? _savedId;
    private bool _isSaving;

    protected EditorPresenter(TView view)
    {
        View = view;
        View.SaveRequested += OnSaveRequested;
    }

    protected TView View { get; }

    /// <summary>
    /// Показывает окно. Возвращает идентификатор сохранённой записи
    /// или null, если окно закрыли без сохранения.
    /// </summary>
    public Guid? Run() => View.ShowModal() ? _savedId : null;

    /// <summary>Проверяет введённые данные теми же правилами, что и доменная модель API.</summary>
    protected abstract ValidationError? Validate();

    /// <summary>Сохраняет запись через API и возвращает её идентификатор.</summary>
    protected abstract Task<Guid> SaveAsync();

    protected static ValidationError? CheckText(
        string field,
        string value,
        string fieldTitle,
        int maxLength,
        bool isRequired = true)
    {
        if (isRequired && value.Length == 0)
            return new ValidationError(field, $"Заполните поле «{fieldTitle}».");

        if (value.Length > maxLength)
            return new ValidationError(field, $"Поле «{fieldTitle}» не может содержать более {maxLength} символов.");

        return null;
    }

    private async void OnSaveRequested(object? sender, EventArgs e)
    {
        if (_isSaving)
            return;

        View.ShowError(null);

        if (Validate() is { } error)
        {
            View.ShowFieldError(error.Field, error.Message);
            return;
        }

        _isSaving = true;
        View.SetBusy(true);

        try
        {
            _savedId = await SaveAsync();
        }
        catch (ApiException exception)
        {
            View.ShowError(exception.Message);
            return;
        }
        finally
        {
            _isSaving = false;
            View.SetBusy(false);
        }

        View.Accept();
    }
}
