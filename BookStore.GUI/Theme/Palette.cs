using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Theme;

/// <summary>
/// Цвета тёмной темы. Совпадают с дизайн-токенами BookStore.Front (styles.css),
/// чтобы веб- и десктоп-клиент выглядели одинаково.
/// </summary>
public static class Palette
{
    public static readonly Color Background = ColorTranslator.FromHtml("#0d1016");
    public static readonly Color BackgroundElevated = ColorTranslator.FromHtml("#11151d");
    public static readonly Color Surface = ColorTranslator.FromHtml("#161b24");
    public static readonly Color Surface2 = ColorTranslator.FromHtml("#1b212c");
    public static readonly Color Surface3 = ColorTranslator.FromHtml("#242b38");
    public static readonly Color Border = ColorTranslator.FromHtml("#252d3a");
    public static readonly Color BorderStrong = ColorTranslator.FromHtml("#343e4f");

    public static readonly Color Text = ColorTranslator.FromHtml("#e7eaf0");
    public static readonly Color TextMuted = ColorTranslator.FromHtml("#9ba4b4");
    public static readonly Color TextSubtle = ColorTranslator.FromHtml("#6c7689");

    public static readonly Color Primary = ColorTranslator.FromHtml("#7c8cff");
    public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#97a4ff");
    public static readonly Color OnPrimary = ColorTranslator.FromHtml("#0b0e18");

    public static readonly Color Danger = ColorTranslator.FromHtml("#f06c75");
    public static readonly Color DangerHover = ColorTranslator.FromHtml("#ff8a92");
    public static readonly Color Success = ColorTranslator.FromHtml("#4fd1a1");
    public static readonly Color Warning = ColorTranslator.FromHtml("#f2bb5c");
    public static readonly Color Info = ColorTranslator.FromHtml("#5bb8ff");
    public static readonly Color Accent = ColorTranslator.FromHtml("#c08cff");

    /// <summary>Выделенная строка таблицы — основной цвет, приглушённый до фона.</summary>
    public static readonly Color Selection = Soft(Primary, Surface, 0.22);

    public static Color ToneColor(Tone tone) => tone switch
    {
        Tone.Primary => Primary,
        Tone.Info => Info,
        Tone.Accent => Accent,
        Tone.Success => Success,
        Tone.Warning => Warning,
        Tone.Danger => Danger,
        _ => TextMuted
    };

    /// <summary>Мягкий фон для метки цвета <paramref name="tone"/> поверх <paramref name="background"/>.</summary>
    public static Color ToneSoft(Tone tone, Color background) =>
        Soft(ToneColor(tone), background, tone == Tone.Neutral ? 0.16 : 0.14);

    /// <summary>
    /// Полупрозрачный цвет, заранее смешанный с фоном: WinForms не умеет прозрачность
    /// у фона элементов, поэтому вместо rgb(... / 0.14) из CSS используем смешение.
    /// </summary>
    public static Color Soft(Color color, Color background, double opacity)
    {
        static int Mix(int front, int back, double alpha) =>
            (int)Math.Round(front * alpha + back * (1 - alpha));

        return Color.FromArgb(
            Mix(color.R, background.R, opacity),
            Mix(color.G, background.G, opacity),
            Mix(color.B, background.B, opacity));
    }
}
