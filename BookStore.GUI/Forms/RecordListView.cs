using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

/// <summary>
/// Рабочая область раздела. Горячие клавиши: Insert/Ctrl+N — добавить, Enter — изменить,
/// Delete — удалить, F5 — обновить, Ctrl+F — поиск.
/// </summary>
public partial class RecordListView : UserControl, IRecordListView
{
    private bool _isBusy;
    private bool _canEdit;
    private bool _canDelete;
    private bool _isBinding;
    private string _countText = string.Empty;

    public RecordListView()
    {
        InitializeComponent();


        addButton.Click += (_, _) => AddRequested?.Invoke(this, EventArgs.Empty);
        editButton.Click += (_, _) => EditRequested?.Invoke(this, EventArgs.Empty);
        deleteButton.Click += (_, _) => DeleteRequested?.Invoke(this, EventArgs.Empty);
        refreshButton.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        searchBox.QueryChanged += (_, _) => SearchTextChanged?.Invoke(this, EventArgs.Empty);

        recordsGrid.SelectionChanged += (_, _) =>
        {
            if (!_isBinding)
                SelectionChanged?.Invoke(this, EventArgs.Empty);
        };

        recordsGrid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0 && editButton.Enabled)
                EditRequested?.Invoke(this, EventArgs.Empty);
        };

        recordsGrid.KeyDown += OnGridKeyDown;
    }

    public event EventHandler? AddRequested;

    public event EventHandler? EditRequested;

    public event EventHandler? DeleteRequested;

    public event EventHandler? RefreshRequested;

    public event EventHandler? SearchTextChanged;

    public event EventHandler? SelectionChanged;

    public string SearchText => searchBox.Query;

    public Guid? SelectedId =>
        recordsGrid.SelectedRows.Count > 0 &&
        recordsGrid.SelectedRows[0].DataBoundItem is IRecordRow row
            ? row.Id
            : null;

    public void Configure(string title, string subtitle, IReadOnlyList<GridColumn> columns)
    {
        titleLabel.Text = title;
        subtitleLabel.Text = subtitle;

        recordsGrid.Columns.Clear();

        foreach (var column in columns)
        {
            var alignment = column.Alignment switch
            {
                ColumnAlignment.Right => DataGridViewContentAlignment.MiddleRight,
                ColumnAlignment.Center => DataGridViewContentAlignment.MiddleCenter,
                _ => DataGridViewContentAlignment.MiddleLeft
            };

            var gridColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = column.Property,
                HeaderText = column.Header,
                Name = column.Property,
                FillWeight = column.FillWeight,
                MinimumWidth = 60,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            gridColumn.DefaultCellStyle.Alignment = alignment;
            gridColumn.HeaderCell.Style.Alignment = alignment;

            if (column.Format is not null)
                gridColumn.DefaultCellStyle.Format = column.Format;

            recordsGrid.Columns.Add(gridColumn);
        }
    }

    public void ShowRows<TRow>(IReadOnlyList<TRow> rows, int totalCount, string emptyText)
        where TRow : IRecordRow
    {
        _isBinding = true;

        try
        {
            // Список конкретного типа строк: по нему DataGridView находит свойства для колонок.
            recordsGrid.DataSource = rows.ToList();
            recordsGrid.EmptyText = emptyText;
        }
        finally
        {
            _isBinding = false;
        }

        _countText = rows.Count == totalCount
            ? $"Записей: {totalCount}"
            : $"Найдено: {rows.Count} из {totalCount}";

        if (!_isBusy)
            countLabel.Text = _countText;

        SelectionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SelectRow(Guid id)
    {
        foreach (DataGridViewRow row in recordsGrid.Rows)
        {
            if (row.DataBoundItem is not IRecordRow record || record.Id != id)
                continue;

            recordsGrid.CurrentCell = row.Cells[0];
            row.Selected = true;
            return;
        }
    }

    public void SetBusy(bool isBusy)
    {
        _isBusy = isBusy;

        countLabel.Text = isBusy ? "Загрузка…" : _countText;
        UseWaitCursor = isBusy;
        UpdateButtons();
    }

    public void SetCommandsEnabled(bool canEdit, bool canDelete)
    {
        _canEdit = canEdit;
        _canDelete = canDelete;
        UpdateButtons();
    }

    public void ShowError(string? message)
    {
        errorBanner.ShowMessage(message);
    }

    public void ShowAlert(string title, string message)
    {
        MessageDialog.ShowAlert(FindForm(), title, message);
    }

    public bool Confirm(string title, string message, string confirmText)
    {
        return MessageDialog.Confirm(FindForm(), title, message, confirmText);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // При открытии раздела фокус получает таблица: можно сразу листать записи стрелками.
        ActiveControl = recordsGrid;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.F5 when refreshButton.Enabled:
                refreshButton.PerformClick();
                return true;

            case Keys.Insert when addButton.Enabled:
            case Keys.Control | Keys.N when addButton.Enabled:
                addButton.PerformClick();
                return true;

            case Keys.Control | Keys.F:
                searchBox.FocusInput();
                return true;

            case Keys.Escape when searchBox.HasInputFocus && searchBox.Query.Length > 0:
                searchBox.Clear();
                return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void OnGridKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && editButton.Enabled)
        {
            editButton.PerformClick();
            e.SuppressKeyPress = true;
        }
        else if (e.KeyCode == Keys.Delete && deleteButton.Enabled)
        {
            deleteButton.PerformClick();
            e.SuppressKeyPress = true;
        }
    }

    private void UpdateButtons()
    {
        addButton.Enabled = !_isBusy;
        refreshButton.Enabled = !_isBusy;
        editButton.Enabled = !_isBusy && _canEdit;
        deleteButton.Enabled = !_isBusy && _canDelete;
    }
}
