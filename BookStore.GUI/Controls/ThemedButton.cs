using System.ComponentModel;
using BookStore.GUI.Theme;

namespace BookStore.GUI.Controls;

public enum ButtonVariant
{
    /// <summary>Главное действие окна: «Добавить», «Сохранить».</summary>
    Primary,

    /// <summary>Обычное действие с рамкой.</summary>
    Secondary,

    /// <summary>Разрушающее действие: «Удалить».</summary>
    Danger,

    /// <summary>Второстепенное действие без фона.</summary>
    Ghost,

    /// <summary>Пункт бокового меню: текст слева, выбранный пункт подсвечен.</summary>
    Navigation
}

/// <summary>
/// Кнопка тёмной темы со скруглёнными углами и иконкой из Segoe MDL2 Assets.
/// Наследует Button, поэтому работают клавиатура, AcceptButton/CancelButton и DialogResult.
/// </summary>
public class ThemedButton : Button
{
    private const int Radius = 7;
    private const int GlyphGap = 8;

    private ButtonVariant _variant = ButtonVariant.Secondary;
    private string _glyph = string.Empty;
    private bool _isSelected;
    private bool _isHovered;
    private bool _isPressed;

    public ThemedButton()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        UseVisualStyleBackColor = false;
        Cursor = Cursors.Hand;
        Padding = new Padding(14, 0, 14, 0);
    }

    [Category("Appearance")]
    [DefaultValue(ButtonVariant.Secondary)]
    public ButtonVariant Variant
    {
        get => _variant;
        set
        {
            _variant = value;
            Invalidate();
        }
    }

    /// <summary>Иконка — символ шрифта Segoe MDL2 Assets (см. <see cref="Glyphs"/>).</summary>
    [Category("Appearance")]
    [DefaultValue("")]
    public string Glyph
    {
        get => _glyph;
        set
        {
            _glyph = value ?? string.Empty;
            Invalidate();
        }
    }

    /// <summary>Выбранный пункт меню (для <see cref="ButtonVariant.Navigation"/>).</summary>
    [Category("Appearance")]
    [DefaultValue(false)]
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            Invalidate();
        }
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        var textSize = TextRenderer.MeasureText(Text, Font);
        var glyphWidth = Glyph.Length > 0
            ? TextRenderer.MeasureText(Glyph, Fonts.Icon).Width + GlyphGap
            : 0;

        return new Size(
            Padding.Horizontal + glyphWidth + textSize.Width,
            Math.Max(Height, textSize.Height + 16));
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
        _isPressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        if (mevent.Button == MouseButtons.Left)
        {
            _isPressed = true;
            Invalidate();
        }

        base.OnMouseDown(mevent);
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        _isPressed = false;
        Invalidate();
        base.OnMouseUp(mevent);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        Cursor = Enabled ? Cursors.Hand : Cursors.Default;
        Invalidate();
        base.OnEnabledChanged(e);
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        var graphics = pevent.Graphics;
        var parentColor = Parent?.BackColor ?? Palette.Background;

        graphics.Clear(parentColor);

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        var (back, fore, border) = GetColors(parentColor);

        if (back != Color.Empty)
            Painting.FillRounded(graphics, bounds, Radius, back);

        if (border != Color.Empty)
            Painting.DrawRounded(graphics, bounds, Radius, border);

        if (Variant == ButtonVariant.Navigation && IsSelected)
            Painting.FillRounded(graphics, new Rectangle(0, 8, 3, Height - 16), 1, Palette.Primary);

        if (Focused && ShowFocusCues)
            Painting.DrawRounded(graphics, Rectangle.Inflate(bounds, -2, -2), Radius - 2, Palette.Primary);

        DrawContent(graphics, fore);
    }

    private void DrawContent(Graphics graphics, Color color)
    {
        var textSize = TextRenderer.MeasureText(graphics, Text, Font);
        var glyphSize = Glyph.Length > 0
            ? TextRenderer.MeasureText(graphics, Glyph, Fonts.Icon)
            : Size.Empty;

        var contentWidth = textSize.Width + (Glyph.Length > 0 ? glyphSize.Width + GlyphGap : 0);
        var left = Variant == ButtonVariant.Navigation
            ? Padding.Left
            : (Width - contentWidth) / 2;

        if (Glyph.Length > 0)
        {
            var glyphBounds = new Rectangle(left, 0, glyphSize.Width, Height);

            TextRenderer.DrawText(graphics, Glyph, Fonts.Icon, glyphBounds, color,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            left += glyphSize.Width + GlyphGap;
        }

        var textBounds = new Rectangle(left, 0, Math.Max(0, Width - left - Padding.Right / 2), Height);

        TextRenderer.DrawText(graphics, Text, Font, textBounds, color,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    private (Color Back, Color Fore, Color Border) GetColors(Color parentColor)
    {
        if (!Enabled)
        {
            return Variant is ButtonVariant.Ghost or ButtonVariant.Navigation
                ? (Color.Empty, Palette.TextSubtle, Color.Empty)
                : (Palette.Surface2, Palette.TextSubtle, Palette.Border);
        }

        return Variant switch
        {
            ButtonVariant.Primary => (
                _isPressed ? Palette.Primary : _isHovered ? Palette.PrimaryHover : Palette.Primary,
                Palette.OnPrimary,
                Color.Empty),

            ButtonVariant.Danger => (
                Palette.Soft(Palette.Danger, parentColor, _isHovered ? 0.24 : 0.14),
                _isHovered ? Palette.DangerHover : Palette.Danger,
                Color.Empty),

            ButtonVariant.Ghost => (
                _isHovered ? Palette.Surface3 : Color.Empty,
                _isHovered ? Palette.Text : Palette.TextMuted,
                Color.Empty),

            ButtonVariant.Navigation => (
                IsSelected ? Palette.Soft(Palette.Primary, parentColor, 0.14)
                    : _isHovered ? Palette.Surface2 : Color.Empty,
                IsSelected || _isHovered ? Palette.Text : Palette.TextMuted,
                Color.Empty),

            _ => (
                _isPressed ? Palette.Surface2 : _isHovered ? Palette.Surface3 : Palette.Surface2,
                Palette.Text,
                _isHovered ? Palette.BorderStrong : Palette.Border)
        };
    }
}
