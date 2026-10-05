using System.ComponentModel;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Controls;

/// <summary>
/// Строка распределения: метка статуса, полоса доли от общего числа и само число.
/// </summary>
public class StatusMeter : Control
{
    private const int BadgeColumnWidth = 130;
    private const int CountColumnWidth = 44;
    private const int BarHeight = 8;

    private Badge _badge = new(string.Empty, Tone.Neutral);
    private int _count;
    private int _total;

    public StatusMeter()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        SetStyle(ControlStyles.Selectable, false);

        Height = 34;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Badge Badge
    {
        get => _badge;
        set
        {
            _badge = value;
            Invalidate();
        }
    }

    public void SetValue(int count, int total)
    {
        _count = count;
        _total = total;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var graphics = e.Graphics;
        var background = Parent?.BackColor ?? Palette.Surface;

        graphics.Clear(background);

        Painting.DrawBadge(graphics, new Rectangle(0, 0, BadgeColumnWidth, Height), Badge, Fonts.Small, background);

        var trackLeft = BadgeColumnWidth + 8;
        var trackWidth = Math.Max(0, Width - trackLeft - CountColumnWidth - 8);
        var track = new Rectangle(trackLeft, (Height - BarHeight) / 2, trackWidth, BarHeight);

        Painting.FillRounded(graphics, track, BarHeight / 2, Palette.Surface3);

        if (_total > 0 && _count > 0)
        {
            var fillWidth = Math.Max(BarHeight, (int)Math.Round(trackWidth * (double)_count / _total));

            Painting.FillRounded(graphics, track with { Width = fillWidth }, BarHeight / 2, Palette.ToneColor(Badge.Tone));
        }

        var countBounds = new Rectangle(Width - CountColumnWidth, 0, CountColumnWidth, Height);

        TextRenderer.DrawText(graphics, _count.ToString(), Fonts.Strong, countBounds, Palette.Text,
            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
    }
}
