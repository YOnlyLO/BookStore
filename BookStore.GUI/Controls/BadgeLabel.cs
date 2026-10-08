using System.ComponentModel;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Controls;

/// <summary>Отдельная метка статуса (например, в шапке окна заказа). Ширина подстраивается под текст.</summary>
public class BadgeLabel : Control
{
    private const int MinimumHeight = 28;

    private Badge? _badge;

    public BadgeLabel()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        SetStyle(ControlStyles.Selectable, false);

        AutoSize = true;
        Size = new Size(120, MinimumHeight);
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Badge? Badge
    {
        get => _badge;
        set
        {
            _badge = value;
            Parent?.PerformLayout(this, nameof(Badge));
            Invalidate();
        }
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        if (Badge is null)
            return new Size(0, MinimumHeight);

        var size = Painting.MeasureBadge(Badge, Fonts.Small);

        return new Size(size.Width + 1, Math.Max(size.Height, MinimumHeight));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var background = Parent?.BackColor ?? Palette.Surface;

        e.Graphics.Clear(background);

        if (Badge is not null)
            Painting.DrawBadge(e.Graphics, ClientRectangle, Badge, Fonts.Small, background);
    }

    // Метка рисуется вручную — её текст сообщаем экранному диктору и UI-тестам явно.
    protected override AccessibleObject CreateAccessibilityInstance() => new BadgeLabelAccessibleObject(this);

    private sealed class BadgeLabelAccessibleObject : ControlAccessibleObject
    {
        private readonly BadgeLabel _label;

        public BadgeLabelAccessibleObject(BadgeLabel label)
            : base(label)
        {
            _label = label;
        }

        public override string? Name => _label.AccessibleName ?? _label.Badge?.Text;

        public override AccessibleRole Role =>
            _label.AccessibleRole == AccessibleRole.Default ? AccessibleRole.StaticText : _label.AccessibleRole;
    }
}
