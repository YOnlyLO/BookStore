using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Таблица DataGridView. В дереве UI Automation это элемент DataGrid: первая строка — заголовки (Header),
/// остальные строки — элементы Custom с ячейками DataItem. Текст ячейки читается через шаблон Value.
/// </summary>
public sealed class GridElement
{
    private static readonly By RowsLocator = By.XPath("./Custom[DataItem]");

    private static readonly By FirstCellsLocator = By.XPath("./Custom/DataItem[1]");

    private static readonly By CellsLocator = By.XPath("./DataItem");

    private readonly AppiumElement _grid;

    public GridElement(AppiumElement grid)
    {
        _grid = grid;
    }

    public int RowCount => _grid.FindElements(RowsLocator).Count;

    /// <summary>Тексты ячеек всех строк, сверху вниз.</summary>
    public IReadOnlyList<IReadOnlyList<string>> GetRows()
    {
        return _grid.FindElements(RowsLocator).Select(ReadCells).ToList();
    }

    /// <summary>Тексты ячеек первой колонки (название, ФИО, номер заказа) — по ним строки находятся в тестах.</summary>
    public IReadOnlyList<string> GetKeys()
    {
        return _grid.FindElements(FirstCellsLocator).Select(cell => cell.GetValue()).ToList();
    }

    public bool HasRow(string key)
    {
        return GetKeys().Contains(key);
    }

    /// <summary>Строка, в первой ячейке которой указанный текст, или null.</summary>
    public AppiumElement? FindRow(string key)
    {
        var index = GetKeys().ToList().IndexOf(key);

        return index < 0 ? null : _grid.FindElements(RowsLocator)[index];
    }

    public IReadOnlyList<string> GetRow(string key)
    {
        var row = FindRow(key) ?? throw new NoSuchElementException($"В таблице нет строки «{key}»");

        return ReadCells(row);
    }

    /// <summary>Выделяет строку щелчком по её первой ячейке.</summary>
    public void SelectRow(string key)
    {
        var row = FindRow(key) ?? throw new NoSuchElementException($"В таблице нет строки «{key}»");

        row.FindElement(CellsLocator).Click();
    }

    private static IReadOnlyList<string> ReadCells(AppiumElement row)
    {
        return row.FindElements(CellsLocator).Select(cell => cell.GetValue()).ToList();
    }
}
