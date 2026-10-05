using BookStore.GUI.Theme;

namespace BookStore.GUI.Controls;

/// <summary>
/// Плашка с сообщением об ошибке. Высота подстраивается под текст,
/// поэтому плашку удобно класть в строку TableLayoutPanel с AutoSize.
/// </summary>
public class AlertBanner : Control
{
    private const int Radius = 8;
    private static readonly Padding Inner = new(40, 10, 14, 10);

    private const TextFormatFlags TextFlags =
        TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.NoPadding;

    public AlertBanner()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        SetStyle(ControlStyles.Selectable, false);

        AutoSize = true;
        Font = Fonts.Body;
    }

    /// <summary>Показывает сообщение; null или пустая строка скрывают плашку.</summary>
    public void ShowMessage(string? message)
    {
        Text = message ?? string.Empty;
        Visible = !string.IsNullOrEmpty(message);
        Parent?.PerformLayout(this, nameof(Text));
        Invalidate();
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        var width = proposedSize.Width is > 0 and < int.MaxValue ? proposedSize.Width : Width;
        var textWidth = Math.Max(1, width - Inner.Horizontal);
        var textSize = TextRenderer.MeasureText(Text, Font, new Size(textWidth, int.MaxValue), TextFlags);

        return new Size(width, textSize.Height + Inner.Vertical);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var graphics = e.Graphics;
        var background = Parent?.BackColor ?? Palette.Background;

        graphics.Clear(background);

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

        Painting.FillRounded(graphics, bounds, Radius, Palette.Soft(Palette.Danger, background, 0.12));
        Painting.DrawRounded(graphics, bounds, Radius, Palette.Soft(Palette.Danger, background, 0.35));

        TextRenderer.DrawText(graphics, Glyphs.Error, Fonts.Icon,
            new Rectangle(14, Inner.Top, 20, Fonts.Body.Height), Palette.Danger,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        var textBounds = new Rectangle(Inner.Left, Inner.Top, Width - Inner.Horizontal, Height - Inner.Vertical);

        TextRenderer.DrawText(graphics, Text, Font, textBounds, Palette.Danger, TextFlags);
    }
}
