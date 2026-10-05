namespace BookStore.GUI.Theme;

/// <summary>Шрифты интерфейса. Иконки берутся из системного шрифта Segoe MDL2 Assets (Windows 10/11).</summary>
public static class Fonts
{
    private const string Family = "Segoe UI";
    private const string SemiboldFamily = "Segoe UI Semibold";
    private const string IconFamily = "Segoe MDL2 Assets";

    public static readonly Font Body = new(Family, 9.75F);
    public static readonly Font Small = new(Family, 8.75F);
    public static readonly Font Label = new(SemiboldFamily, 9F);
    public static readonly Font Strong = new(SemiboldFamily, 9.75F);
    public static readonly Font Heading = new(SemiboldFamily, 12F);
    public static readonly Font Title = new(SemiboldFamily, 18F);
    public static readonly Font Metric = new(SemiboldFamily, 24F);

    public static readonly Font Icon = new(IconFamily, 10F);
    public static readonly Font IconLarge = new(IconFamily, 14F);
}

/// <summary>Коды иконок шрифта Segoe MDL2 Assets.</summary>
public static class Glyphs
{
    public const string Add = "\uE710";
    public const string Edit = "\uE70F";
    public const string Delete = "\uE74D";
    public const string Refresh = "\uE72C";
    public const string Search = "\uE721";
    public const string Dashboard = "\uE80F";
    public const string Genres = "\uE8EC";
    public const string Books = "\uE82D";
    public const string Users = "\uE716";
    public const string Orders = "\uE7BF";
    public const string ChevronRight = "\uE76C";
    public const string Accept = "\uE8FB";
    public const string Cancel = "\uE711";
    public const string Completed = "\uE930";
    public const string Error = "\uE783";
    public const string Store = "\uE719";
}
