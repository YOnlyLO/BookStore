using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Views;

/// <summary>
/// Рабочая область раздела: список записей и кнопки «Добавить», «Изменить», «Удалить».
/// Одна реализация обслуживает все разделы — колонки задаёт презентер.
/// </summary>
public interface IRecordListView
{
    event EventHandler AddRequested;

    event EventHandler EditRequested;

    event EventHandler DeleteRequested;

    event EventHandler RefreshRequested;

    event EventHandler SearchTextChanged;

    event EventHandler SelectionChanged;

    string SearchText { get; }

    Guid? SelectedId { get; }

    void Configure(
        string title,
        string subtitle,
        IReadOnlyList<GridColumn> columns);

    /// <summary>
    /// Показывает строки. <paramref name="totalCount"/> — число записей без учёта поиска,
    /// <paramref name="emptyText"/> — текст на месте пустой таблицы.
    /// </summary>
    void ShowRows<TRow>(
        IReadOnlyList<TRow> rows,
        int totalCount,
        string emptyText)
        where TRow : IRecordRow;

    void SelectRow(Guid id);

    void SetBusy(bool isBusy);

    void SetCommandsEnabled(bool canEdit, bool canDelete);

    /// <summary>Показывает сообщение об ошибке над таблицей; null — скрывает его.</summary>
    void ShowError(string? message);

    /// <summary>Модальное сообщение об ошибке действия.</summary>
    void ShowAlert(string title, string message);

    bool Confirm(string title, string message, string confirmText);
}
