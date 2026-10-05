using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

/// <summary>
/// Общая логика рабочей области: загрузка списка, поиск, добавление, изменение и удаление записи.
/// Наследники описывают только то, чем разделы отличаются друг от друга.
/// </summary>
public abstract class RecordListPresenter<TRecord, TRow> : ISectionPresenter
    where TRecord : class
    where TRow : IRecordRow
{
    private IReadOnlyList<TRecord> _records = [];
    private string? _loadError;
    private bool _isBusy;

    protected RecordListPresenter(
        IRecordListView view,
        string title,
        string subtitle,
        IReadOnlyList<GridColumn> columns)
    {
        View = view;

        View.Configure(title, subtitle, columns);

        View.AddRequested += OnAddRequested;
        View.EditRequested += OnEditRequested;
        View.DeleteRequested += OnDeleteRequested;
        View.RefreshRequested += async (_, _) => await ReloadAsync(View.SelectedId);
        View.SearchTextChanged += (_, _) => ShowRecords(View.SelectedId);
        View.SelectionChanged += (_, _) => UpdateCommands();

        UpdateCommands();
    }

    protected IRecordListView View { get; }

    /// <summary>Записи, загруженные при последнем обновлении списка.</summary>
    protected IReadOnlyList<TRecord> Records => _records;

    /// <summary>Текст на месте пустого списка.</summary>
    protected abstract string EmptyText { get; }

    /// <summary>Подсказка, если сервер не смог удалить запись из-за связанных данных.</summary>
    protected virtual string? DeleteConflictHint => null;

    public Task ActivateAsync() => ReloadAsync(View.SelectedId);

    /// <summary>Загружает записи (и справочники, нужные для отображения) в порядке показа.</summary>
    protected abstract Task<IReadOnlyList<TRecord>> LoadAsync();

    protected abstract Guid GetId(TRecord record);

    protected abstract TRow ToRow(TRecord record);

    /// <summary>Тексты записи, по которым работает поиск.</summary>
    protected abstract IEnumerable<string?> GetSearchableText(TRecord record);

    /// <summary>Открывает окно создания. Возвращает идентификатор новой записи или null.</summary>
    protected abstract Guid? OpenCreateEditor();

    /// <summary>Открывает окно изменения. true — данные изменились и список нужно обновить.</summary>
    protected abstract bool OpenEditEditor(TRecord record);

    protected abstract string GetDeleteQuestion(TRecord record);

    protected abstract Task DeleteAsync(Guid id);

    /// <summary>Причина, по которой запись нельзя удалить; null — удалять можно.</summary>
    protected virtual Task<string?> GetDeleteBlockerAsync(TRecord record) =>
        Task.FromResult<string?>(null);

    private TRecord? SelectedRecord =>
        View.SelectedId is { } id
            ? _records.FirstOrDefault(record => GetId(record) == id)
            : null;

    private async Task ReloadAsync(Guid? selectId)
    {
        if (_isBusy)
            return;

        SetBusy(true);
        View.ShowError(null);

        try
        {
            _records = await LoadAsync();
            _loadError = null;
        }
        catch (ApiException exception)
        {
            // Устаревший список не показываем: по нему нельзя корректно изменять записи.
            _records = [];
            _loadError = exception.Message;
            View.ShowError(exception.Message);
        }
        finally
        {
            SetBusy(false);
        }

        ShowRecords(selectId);
    }

    private void ShowRecords(Guid? selectId)
    {
        var search = View.SearchText.Trim();

        var rows = _records
            .Where(record => search.Length == 0 || Matches(record, search))
            .Select(ToRow)
            .ToList();

        var emptyText = _loadError is not null ? "Не удалось загрузить данные"
            : _records.Count == 0 ? EmptyText
            : "По запросу ничего не найдено";

        View.ShowRows(rows, _records.Count, emptyText);

        if (selectId is { } id)
            View.SelectRow(id);

        UpdateCommands();
    }

    private bool Matches(TRecord record, string search) =>
        GetSearchableText(record).Any(text =>
            text?.Contains(search, StringComparison.CurrentCultureIgnoreCase) == true);

    private async void OnAddRequested(object? sender, EventArgs e)
    {
        if (_isBusy)
            return;

        var createdId = OpenCreateEditor();

        if (createdId is not null)
            await ReloadAsync(createdId);
    }

    private async void OnEditRequested(object? sender, EventArgs e)
    {
        if (_isBusy || SelectedRecord is not { } record)
            return;

        if (OpenEditEditor(record))
            await ReloadAsync(GetId(record));
    }

    private async void OnDeleteRequested(object? sender, EventArgs e)
    {
        if (_isBusy || SelectedRecord is not { } record)
            return;

        SetBusy(true);

        string? blocker;

        try
        {
            blocker = await GetDeleteBlockerAsync(record);
        }
        catch (ApiException exception)
        {
            View.ShowAlert("Не удалось проверить запись", exception.Message);
            return;
        }
        finally
        {
            SetBusy(false);
        }

        if (blocker is not null)
        {
            View.ShowAlert("Удаление невозможно", blocker);
            return;
        }

        if (!View.Confirm("Удаление записи", GetDeleteQuestion(record), "Удалить"))
            return;

        SetBusy(true);

        try
        {
            await DeleteAsync(GetId(record));
        }
        catch (ApiException exception)
        {
            var message = exception.IsServerError && DeleteConflictHint is not null
                ? $"{exception.Message}{Environment.NewLine}{Environment.NewLine}{DeleteConflictHint}"
                : exception.Message;

            View.ShowAlert("Не удалось удалить запись", message);
        }
        finally
        {
            SetBusy(false);
        }

        await ReloadAsync(null);
    }

    private void SetBusy(bool isBusy)
    {
        _isBusy = isBusy;
        View.SetBusy(isBusy);
    }

    private void UpdateCommands()
    {
        var hasSelection = SelectedRecord is not null;

        View.SetCommandsEnabled(canEdit: hasSelection, canDelete: hasSelection);
    }
}
