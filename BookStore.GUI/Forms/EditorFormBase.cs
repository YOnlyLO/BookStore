using System.ComponentModel;
using BookStore.GUI.Theme;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

/// <summary>
/// Базовое модальное окно записи: заголовок, плашка ошибки, поля (bodyPanel) и кнопки
/// «Сохранить»/«Отмена». Наследники добавляют поля в bodyPanel и сопоставляют их
/// с именами свойств в <see cref="GetFieldControl"/>.
/// </summary>
/// <remarks>
/// Класс не абстрактный: дизайнер WinForms не умеет открывать наследников абстрактной формы.
/// Свойства представлений реализованы явно (IXxxView.Property): презентер работает только
/// через интерфейс, а дизайнер не пытается сериализовать эти свойства (WFO1000).
/// </remarks>
public partial class EditorFormBase : Form, IEditorView
{
    private bool _isBusy;

    public EditorFormBase()
    {
        InitializeComponent();

        saveButton.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? SaveRequested;

    string IEditorView.Title
    {
        set
        {
            Text = value;
            titleLabel.Text = value;
        }
    }

    /// <summary>Окно, поверх которого открывается диалог; задаёт фабрика представлений.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IWin32Window? ModalOwner { get; set; }

    public bool ShowModal() => ShowDialog(ModalOwner) == DialogResult.OK;

    public void Accept()
    {
        DialogResult = DialogResult.OK;
    }

    public void SetBusy(bool isBusy)
    {
        _isBusy = isBusy;

        bodyPanel.Enabled = !isBusy;
        saveButton.Enabled = !isBusy;
        cancelButton.Enabled = !isBusy;
        saveButton.Text = isBusy ? "Сохранение…" : "Сохранить";
        UseWaitCursor = isBusy;
    }

    public void ShowError(string? message)
    {
        errorBanner.ShowMessage(message);
    }

    public void ShowFieldError(string field, string message)
    {
        ShowError(message);

        if (GetFieldControl(field) is { } control)
        {
            control.Focus();

            if (control is TextBoxBase textBox)
                textBox.SelectAll();
        }
    }

    /// <summary>Поле ввода для свойства представления с именем <paramref name="field"/>.</summary>
    protected virtual Control? GetFieldControl(string field) => null;

    protected override void OnLoad(EventArgs e)
    {
        // Здесь, а не в конструкторе: к этому моменту наследник уже добавил свои поля.
        DarkTheme.Apply(this);
        base.OnLoad(e);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Пока запрос не завершён, окно не закрываем: иначе результат сохранения потеряется.
        if (_isBusy)
            e.Cancel = true;

        base.OnFormClosing(e);
    }
}
