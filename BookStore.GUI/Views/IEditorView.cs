namespace BookStore.GUI.Views;

/// <summary>Общая часть модальных окон создания и изменения записи.</summary>
public interface IEditorView : IDisposable
{
    event EventHandler SaveRequested;

    string Title { set; }

    /// <summary>Показывает окно модально. true — запись сохранена.</summary>
    bool ShowModal();

    /// <summary>Закрывает окно с результатом «сохранено».</summary>
    void Accept();

    void SetBusy(bool isBusy);

    /// <summary>Показывает ошибку в окне; null — скрывает её.</summary>
    void ShowError(string? message);

    /// <summary>
    /// Показывает ошибку проверки и переводит фокус в поле.
    /// <paramref name="field"/> — имя свойства представления (nameof).
    /// </summary>
    void ShowFieldError(string field, string message);
}
