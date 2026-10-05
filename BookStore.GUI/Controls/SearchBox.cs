using System.ComponentModel;
using BookStore.GUI.Theme;

namespace BookStore.GUI.Controls;

/// <summary>
/// Поле поиска в стиле темы: скруглённая рамка, иконка лупы и подсказка.
/// Внутри — обычный TextBox без рамки: у стандартной рамки в тёмном режиме светлый цвет.
/// </summary>
public class SearchBox : Control
{
    private const int Radius = 7;
    private const int IconWidth = 34;
    private const int RightPadding = 10;

    private readonly TextBox _textBox;

    public SearchBox()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        _textBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Palette.Surface2,
            ForeColor = Palette.Text,
            Font = Fonts.Body
        };

        _textBox.TextChanged += (_, _) => QueryChanged?.Invoke(this, EventArgs.Empty);
        _textBox.GotFocus += (_, _) => Invalidate();
        _textBox.LostFocus += (_, _) => Invalidate();

        Controls.Add(_textBox);

        Cursor = Cursors.IBeam;
        Size = new Size(300, 36);
    }

    /// <summary>Текст поиска изменился.</summary>
    public event EventHandler? QueryChanged;

    [Browsable(false)]
    public string Query => _textBox.Text;

    [Browsable(false)]
    public bool HasInputFocus => _textBox.Focused;

    [Category("Appearance")]
    [DefaultValue("")]
    public string PlaceholderText
    {
        get => _textBox.PlaceholderText;
        set => _textBox.PlaceholderText = value;
    }

    public void FocusInput()
    {
        _textBox.Focus();
        _textBox.SelectAll();
    }

    public void Clear() => _textBox.Clear();

    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);

        _textBox.SetBounds(
            IconWidth,
            (Height - _textBox.Height) / 2,
            Math.Max(0, Width - IconWidth - RightPadding),
            _textBox.Height);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _textBox.Focus();
        base.OnMouseDown(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var graphics = e.Graphics;

        graphics.Clear(Parent?.BackColor ?? Palette.Background);

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

        Painting.FillRounded(graphics, bounds, Radius, Palette.Surface2);
        Painting.DrawRounded(graphics, bounds, Radius, _textBox.Focused ? Palette.Primary : Palette.BorderStrong);

        TextRenderer.DrawText(graphics, Glyphs.Search, Fonts.Icon, new Rectangle(0, 0, IconWidth, Height),
            Palette.TextSubtle, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}
