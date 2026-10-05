namespace BookStore.GUI.Theme;

/// <summary>
/// Раскрашивает стандартные элементы WinForms в цвета темы.
/// Собственные элементы (Controls/*) раскрашивают себя сами.
/// </summary>
public static class DarkTheme
{
    /// <summary>Фон окна и цвета полей ввода внутри <paramref name="root"/>.</summary>
    public static void Apply(Control root)
    {
        root.BackColor = Palette.Background;
        root.ForeColor = Palette.Text;

        ApplyToInputs(root);
    }

    private static void ApplyToInputs(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            switch (control)
            {
                case TextBoxBase or NumericUpDown:
                    control.BackColor = Palette.Surface2;
                    control.ForeColor = Palette.Text;
                    break;

                case ComboBox comboBox:
                    comboBox.BackColor = Palette.Surface2;
                    comboBox.ForeColor = Palette.Text;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;
            }

            ApplyToInputs(control);
        }
    }
}
