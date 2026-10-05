using System.ComponentModel;
using BookStore.GUI.Theme;

namespace BookStore.GUI.Controls;

/// <summary>
/// Панель-«карточка»: скруглённый фон Surface с рамкой. Дочерние элементы
/// наследуют её цвет фона, поэтому внутри карточки не нужно задавать цвета вручную.
/// </summary>
public class CardPanel : Panel
{
    private const int Radius = 10;

    public CardPanel()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        BackColor = Palette.Surface;
        Padding = new Padding(16);
    }

    [DefaultValue(typeof(Color), "22, 27, 36")]
    public override Color BackColor
    {
        get => base.BackColor;
        set => base.BackColor = value;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? Palette.Background);

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

        Painting.FillRounded(e.Graphics, bounds, Radius, BackColor);
        Painting.DrawRounded(e.Graphics, bounds, Radius, Palette.Border);
    }
}
