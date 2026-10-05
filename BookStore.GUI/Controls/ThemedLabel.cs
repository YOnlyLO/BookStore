using System.ComponentModel;
using BookStore.GUI.Theme;

namespace BookStore.GUI.Controls;

public enum LabelStyle
{
    /// <summary>Обычный текст.</summary>
    Body,

    /// <summary>Пояснения и подзаголовки.</summary>
    Muted,

    /// <summary>Мелкий второстепенный текст.</summary>
    Subtle,

    /// <summary>Подпись поля формы.</summary>
    Caption,

    /// <summary>Заголовок блока.</summary>
    Heading,

    /// <summary>Заголовок страницы.</summary>
    Title
}

/// <summary>Надпись с типографикой тёмной темы: шрифт и цвет задаются стилем.</summary>
public class ThemedLabel : Label
{
    private LabelStyle _style = LabelStyle.Body;

    public ThemedLabel()
    {
        AutoSize = true;
        ApplyStyle();
    }

    [Category("Appearance")]
    [DefaultValue(LabelStyle.Body)]
    public LabelStyle Style
    {
        get => _style;
        set
        {
            _style = value;
            ApplyStyle();
        }
    }

    [DefaultValue(true)]
    public override bool AutoSize
    {
        get => base.AutoSize;
        set => base.AutoSize = value;
    }

    private void ApplyStyle()
    {
        (Font, ForeColor) = _style switch
        {
            LabelStyle.Muted => (Fonts.Body, Palette.TextMuted),
            LabelStyle.Subtle => (Fonts.Small, Palette.TextSubtle),
            LabelStyle.Caption => (Fonts.Label, Palette.TextMuted),
            LabelStyle.Heading => (Fonts.Heading, Palette.Text),
            LabelStyle.Title => (Fonts.Title, Palette.Text),
            _ => (Fonts.Body, Palette.Text)
        };
    }
}
