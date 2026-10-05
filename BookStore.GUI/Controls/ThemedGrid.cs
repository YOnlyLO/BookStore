using System.ComponentModel;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Controls;

/// <summary>
/// Таблица тёмной темы: только чтение, выделение строкой, подсветка строки под курсором,
/// метки <see cref="Badge"/> в ячейках и текст-заглушка для пустого списка.
/// </summary>
public class ThemedGrid : DataGridView
{
    private int _hoveredRow = -1;
    private string _emptyText = string.Empty;

    public ThemedGrid()
    {
        DoubleBuffered = true;

        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        AllowUserToResizeRows = false;
        AutoGenerateColumns = false;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        BackgroundColor = Palette.Surface;
        BorderStyle = BorderStyle.None;
        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        ColumnHeadersHeight = 40;
        EnableHeadersVisualStyles = false;
        GridColor = Palette.Border;
        MultiSelect = false;
        ReadOnly = true;
        RowHeadersVisible = false;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        StandardTab = true;

        RowTemplate.Height = 38;

        ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Palette.Surface,
            ForeColor = Palette.TextMuted,
            SelectionBackColor = Palette.Surface,
            SelectionForeColor = Palette.TextMuted,
            Font = Fonts.Label,
            Padding = new Padding(8, 0, 8, 0),
            WrapMode = DataGridViewTriState.False
        };

        DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Palette.Surface,
            ForeColor = Palette.Text,
            SelectionBackColor = Palette.Selection,
            SelectionForeColor = Palette.Text,
            Font = Fonts.Body,
            Padding = new Padding(8, 0, 8, 0),
            WrapMode = DataGridViewTriState.False
        };
    }

    /// <summary>Текст по центру таблицы, когда в ней нет строк.</summary>
    [Category("Appearance")]
    [DefaultValue("")]
    public string EmptyText
    {
        get => _emptyText;
        set
        {
            _emptyText = value ?? string.Empty;
            Invalidate();
        }
    }

    protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
    {
        SetHoveredRow(e.RowIndex);
        base.OnCellMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        SetHoveredRow(-1);
        base.OnMouseLeave(e);
    }

    protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex == _hoveredRow && e.CellStyle is not null)
            e.CellStyle.BackColor = Palette.Surface2;

        base.OnCellFormatting(e);
    }

    protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
    {
        base.OnCellPainting(e);

        if (e.Handled || e.Graphics is null)
            return;

        if (e.ColumnIndex < 0)
            return;

        if (e.RowIndex == -1)
            PaintColumnHeader(e);
        else if (e.RowIndex >= 0)
            PaintBodyCell(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (Rows.Count > 0 || EmptyText.Length == 0)
            return;

        var area = Rectangle.FromLTRB(0, ColumnHeadersVisible ? ColumnHeadersHeight : 0, Width, Height);

        TextRenderer.DrawText(e.Graphics, EmptyText, Fonts.Body, area, Palette.TextSubtle,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
    }

    protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
    {
        base.OnRowsAdded(e);
        Invalidate();
    }

    protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
    {
        base.OnRowsRemoved(e);
        Invalidate();
    }

    private void SetHoveredRow(int rowIndex)
    {
        if (rowIndex == _hoveredRow)
            return;

        var previous = _hoveredRow;
        _hoveredRow = rowIndex;

        if (previous >= 0 && previous < Rows.Count)
            InvalidateRow(previous);

        if (rowIndex >= 0 && rowIndex < Rows.Count)
            InvalidateRow(rowIndex);
    }

    // Ячейки рисуем сами: так у заголовка и строк одна нижняя линия цвета темы,
    // а метки Badge рисуются поверх фона ячейки.
    private void PaintColumnHeader(DataGridViewCellPaintingEventArgs e)
    {
        var graphics = e.Graphics!;

        using (var brush = new SolidBrush(ColumnHeadersDefaultCellStyle.BackColor))
        {
            graphics.FillRectangle(brush, e.CellBounds);
        }

        e.PaintContent(e.ClipBounds);
        DrawBottomLine(graphics, e.CellBounds, Palette.BorderStrong);
        e.Handled = true;
    }

    private void PaintBodyCell(DataGridViewCellPaintingEventArgs e)
    {
        var graphics = e.Graphics!;
        var isSelected = (e.State & DataGridViewElementStates.Selected) != 0;
        var style = e.CellStyle ?? DefaultCellStyle;
        var background = isSelected ? style.SelectionBackColor : style.BackColor;

        using (var brush = new SolidBrush(background))
        {
            graphics.FillRectangle(brush, e.CellBounds);
        }

        if (e.Value is Badge badge)
        {
            var bounds = Rectangle.FromLTRB(
                e.CellBounds.Left + style.Padding.Left,
                e.CellBounds.Top,
                e.CellBounds.Right - style.Padding.Right,
                e.CellBounds.Bottom);

            Painting.DrawBadge(graphics, bounds, badge, Fonts.Small, background);
        }
        else
        {
            e.PaintContent(e.ClipBounds);
        }

        DrawBottomLine(graphics, e.CellBounds, GridColor);
        e.Handled = true;
    }

    private static void DrawBottomLine(Graphics graphics, Rectangle bounds, Color color)
    {
        using var pen = new Pen(color);

        graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
    }
}
