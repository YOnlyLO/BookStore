using System.Drawing.Drawing2D;
using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Theme;

/// <summary>Общие процедуры рисования: скруглённые прямоугольники и метки-«пилюли».</summary>
public static class Painting
{
    public static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        var diameter = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));

        if (diameter <= 1)
        {
            path.AddRectangle(bounds);
            return path;
        }

        var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

        path.AddArc(arc, 180, 90);
        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();

        return path;
    }

    public static void FillRounded(Graphics graphics, Rectangle bounds, int radius, Color color)
    {
        using var path = RoundedRectangle(bounds, radius);
        using var brush = new SolidBrush(color);

        Smoothly(graphics, () => graphics.FillPath(brush, path));
    }

    public static void DrawRounded(Graphics graphics, Rectangle bounds, int radius, Color color, float width = 1F)
    {
        using var path = RoundedRectangle(bounds, radius);
        using var pen = new Pen(color, width);

        Smoothly(graphics, () => graphics.DrawPath(pen, path));
    }

    /// <summary>
    /// Рисует со сглаживанием и возвращает прежний режим. Graphics общий для всех ячеек таблицы:
    /// если оставить сглаживание включённым, следующие прямоугольники получат полупрозрачные края.
    /// </summary>
    private static void Smoothly(Graphics graphics, Action draw)
    {
        var previous = graphics.SmoothingMode;

        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        try
        {
            draw();
        }
        finally
        {
            graphics.SmoothingMode = previous;
        }
    }

    // Внутренние отступы метки: слева до точки, размер точки, от точки до текста, справа после текста.
    private const int BadgeLeft = 10;
    private const int BadgeDot = 6;
    private const int BadgeGap = 6;
    private const int BadgeRight = 11;

    public static Size MeasureBadge(Badge badge, Font font)
    {
        var textSize = TextRenderer.MeasureText(badge.Text, font, Size.Empty, TextFormatFlags.NoPadding);

        return new Size(
            BadgeLeft + BadgeDot + BadgeGap + textSize.Width + BadgeRight,
            textSize.Height + 8);
    }

    /// <summary>
    /// Рисует метку статуса: мягкий фон, цветная точка и текст.
    /// Метка выравнивается по левому краю и по центру <paramref name="bounds"/> по вертикали.
    /// </summary>
    public static void DrawBadge(Graphics graphics, Rectangle bounds, Badge badge, Font font, Color background)
    {
        var size = MeasureBadge(badge, font);
        var width = Math.Min(size.Width, bounds.Width);
        var pill = new Rectangle(bounds.Left, bounds.Top + (bounds.Height - size.Height) / 2, width, size.Height);
        var toneColor = Palette.ToneColor(badge.Tone);

        FillRounded(graphics, pill, pill.Height / 2, Palette.ToneSoft(badge.Tone, background));

        var dot = new Rectangle(pill.Left + BadgeLeft, pill.Top + pill.Height / 2 - BadgeDot / 2, BadgeDot, BadgeDot);

        using (var brush = new SolidBrush(toneColor))
        {
            Smoothly(graphics, () => graphics.FillEllipse(brush, dot));
        }

        var textBounds = Rectangle.FromLTRB(dot.Right + BadgeGap, pill.Top, pill.Right - BadgeRight + 4, pill.Bottom);

        TextRenderer.DrawText(
            graphics,
            badge.Text,
            font,
            textBounds,
            toneColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }
}
