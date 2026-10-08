using System.ComponentModel;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Controls;

/// <summary>
/// Карточка дашборда: иконка раздела, название, крупное число и описание.
/// Нажатие (мышью, Enter или пробелом) вызывает событие Click.
/// </summary>
public class StatCard : Control
{
    private const int Radius = 12;
    private const int Inset = 18;

    private string _title = string.Empty;
    private string _value = "—";
    private string _description = string.Empty;
    private string _glyph = string.Empty;
    private Tone _tone = Tone.Primary;
    private bool _isHovered;

    public StatCard()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.Selectable,
            true);

        Cursor = Cursors.Hand;
        TabStop = true;
    }

    [Category("Appearance")]
    [DefaultValue("")]
    public string Title
    {
        get => _title;
        set => SetAndRepaint(ref _title, value);
    }

    [Category("Appearance")]
    [DefaultValue("—")]
    public string Value
    {
        get => _value;
        set => SetAndRepaint(ref _value, value);
    }

    [Category("Appearance")]
    [DefaultValue("")]
    public string Description
    {
        get => _description;
        set => SetAndRepaint(ref _description, value);
    }

    [Category("Appearance")]
    [DefaultValue("")]
    public string Glyph
    {
        get => _glyph;
        set => SetAndRepaint(ref _glyph, value);
    }

    [Category("Appearance")]
    [DefaultValue(Tone.Primary)]
    public Tone Tone
    {
        get => _tone;
        set
        {
            _tone = value;
            Invalidate();
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _isHovered = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _isHovered = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Focus();
        base.OnMouseDown(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Enter or Keys.Space)
        {
            OnClick(EventArgs.Empty);
            e.Handled = true;
        }

        base.OnKeyDown(e);
    }

    protected override void OnGotFocus(EventArgs e)
    {
        Invalidate();
        base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
        Invalidate();
        base.OnLostFocus(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var graphics = e.Graphics;
        var parentColor = Parent?.BackColor ?? Palette.Background;
        var background = _isHovered ? Palette.Surface2 : Palette.Surface;
        var toneColor = Palette.ToneColor(Tone);

        graphics.Clear(parentColor);

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

        Painting.FillRounded(graphics, bounds, Radius, background);
        Painting.DrawRounded(graphics, bounds, Radius,
            Focused && ShowFocusCues ? Palette.Primary : _isHovered ? Palette.BorderStrong : Palette.Border);

        // Иконка раздела в квадрате мягкого цвета.
        var iconBounds = new Rectangle(Inset, Inset, 40, 40);

        Painting.FillRounded(graphics, iconBounds, 9, Palette.ToneSoft(Tone, background));
        TextRenderer.DrawText(graphics, Glyph, Fonts.IconLarge, iconBounds, toneColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // Стрелка «перейти» в правом верхнем углу.
        var arrowBounds = new Rectangle(Width - Inset - 20, Inset, 20, 40);

        TextRenderer.DrawText(graphics, Glyphs.ChevronRight, Fonts.Icon, arrowBounds,
            _isHovered ? Palette.Text : Palette.TextSubtle,
            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);

        var textWidth = Width - Inset * 2;
        var top = iconBounds.Bottom + 12;

        top += DrawLine(graphics, Title, Fonts.Body, Palette.TextMuted, top, textWidth);
        top += DrawLine(graphics, Value, Fonts.Metric, Palette.Text, top, textWidth);

        var descriptionBounds = new Rectangle(Inset, top + 2, textWidth, Math.Max(0, Height - top - Inset));

        TextRenderer.DrawText(graphics, Description, Fonts.Small, descriptionBounds, Palette.TextSubtle,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    private static int DrawLine(Graphics graphics, string text, Font font, Color color, int top, int width)
    {
        var height = TextRenderer.MeasureText(graphics, "Ag", font).Height;

        TextRenderer.DrawText(graphics, text, font, new Rectangle(Inset, top, width, height), color,
            TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

        return height;
    }

    // Карточка рисуется целиком, поэтому без своего AccessibleObject экранный диктор
    // и UI-тесты видят безымянную панель вместо названия раздела и числа.
    protected override AccessibleObject CreateAccessibilityInstance() => new StatCardAccessibleObject(this);

    private void SetAndRepaint(ref string field, string? value)
    {
        field = value ?? string.Empty;
        Invalidate();
    }

    private sealed class StatCardAccessibleObject : ControlAccessibleObject
    {
        private readonly StatCard _card;

        public StatCardAccessibleObject(StatCard card)
            : base(card)
        {
            _card = card;
        }

        public override string? Name => _card.AccessibleName ?? $"{_card.Title}: {_card.Value}";

        public override AccessibleRole Role =>
            _card.AccessibleRole == AccessibleRole.Default ? AccessibleRole.PushButton : _card.AccessibleRole;
    }
}
