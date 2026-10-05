namespace BookStore.GUI.ViewModels;

public enum ColumnAlignment
{
    Left,
    Center,
    Right
}

/// <summary>
/// Описание колонки таблицы записей.
/// <paramref name="Property"/> — имя свойства строки (<see cref="IRecordRow"/>),
/// <paramref name="Format"/> — строка формата .NET (например, "C2" или "dd.MM.yyyy HH:mm").
/// </summary>
public sealed record GridColumn(
    string Property,
    string Header,
    int FillWeight = 100,
    ColumnAlignment Alignment = ColumnAlignment.Left,
    string? Format = null);
