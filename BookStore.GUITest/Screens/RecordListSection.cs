using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Рабочая область раздела (жанры, книги, пользователи, заказы): заголовок и счётчик записей,
/// кнопки «Добавить», «Изменить», «Удалить», «Обновить», поле поиска и таблица записей.
/// Все разделы — экземпляры одного представления RecordListView, поэтому элементы
/// ищутся внутри представления конкретного раздела.
/// </summary>
public class RecordListSection
{
    private const string LoadingText = "Загрузка…";

    private readonly string? _editorFormId;

    protected RecordListSection(WindowsDriver app, AppiumElement root, string? editorFormId)
    {
        App = app;
        Root = root;
        _editorFormId = editorFormId;
    }

    public string Title => Find("titleLabel").Text;

    public string Subtitle => Find("subtitleLabel").Text;

    /// <summary>Счётчик над таблицей: «Записей: 2» или при поиске «Найдено: 1 из 2».</summary>
    public string CountText => Find("countLabel").Text;

    public GridElement Grid => new(Find("recordsGrid"));

    /// <summary>Доступна ли кнопка «Изменить» (она активна, только когда выделена запись).</summary>
    public bool CanEdit => Find("editButton").Enabled;

    public bool CanDelete => Find("deleteButton").Enabled;

    protected WindowsDriver App { get; }

    protected AppiumElement Root { get; }

    private string EditorFormId =>
        _editorFormId ?? throw new InvalidOperationException("Для раздела не задана форма записи.");

    /// <summary>Ждёт, пока раздел станет видимым и загрузит записи.</summary>
    /// <param name="viewId">AutomationId представления раздела: genresView, booksView…</param>
    /// <param name="editorFormId">
    /// Форма, которая открывается кнопками «Добавить» и «Изменить»; не нужна, если тест только проверяет раздел.
    /// </param>
    public static RecordListSection WaitFor(WindowsDriver app, string viewId, string? editorFormId = null)
    {
        var section = new RecordListSection(app, FindView(app, viewId), editorFormId);

        section.WaitUntilLoaded();

        return section;
    }

    /// <summary>Ждёт, пока счётчик записей станет равен ожидаемому, и возвращает его текст.</summary>
    public string WaitForCount(string expected)
    {
        return Waits.ForText(App, () => CountText, expected);
    }

    public IReadOnlyList<IReadOnlyList<string>> GetRows() => Grid.GetRows();

    /// <summary>Ждёт строку с указанным текстом в первой колонке и возвращает тексты её ячеек.</summary>
    public IReadOnlyList<string> GetRow(string key)
    {
        WaitForRow(key);

        return Grid.GetRow(key);
    }

    /// <summary>
    /// Ждёт, пока ячейка строки станет равна ожидаемому значению (после сохранения список перечитывается
    /// не сразу), и возвращает последний прочитанный текст ячейки.
    /// </summary>
    public string WaitForCell(string key, int column, string expected)
    {
        return Waits.ForText(App, () => Grid.GetRow(key)[column], expected);
    }

    public bool HasRow(string key) => Grid.HasRow(key);

    public void WaitForRow(string key)
    {
        Waits.Until(App, _ => Grid.HasRow(key), $"В таблице не появилась строка «{key}»");
    }

    public void WaitForRowToDisappear(string key)
    {
        Waits.Until(App, _ => !Grid.HasRow(key), $"Строка «{key}» не исчезла из таблицы");
    }

    public void SelectRow(string key)
    {
        WaitForRow(key);
        Grid.SelectRow(key);

        Waits.Until(App, _ => CanEdit, $"Строка «{key}» не выделилась");
    }

    /// <summary>Вводит текст в поле поиска над таблицей.</summary>
    public void Search(string text)
    {
        // Внутри поля поиска — обычный TextBox без имени.
        Find("searchBox").FindElement(By.XPath("./Edit")).ReplaceText(text);
    }

    /// <summary>Нажимает «Добавить» и ждёт окно создания записи.</summary>
    public EditorDialog OpenCreateForm()
    {
        Find("addButton").Click();

        return EditorDialog.WaitFor(App, EditorFormId);
    }

    /// <summary>Нажимает «Добавить», когда приложение должно не открыть форму, а показать сообщение.</summary>
    public MessageDialog ClickAddExpectingMessage()
    {
        Find("addButton").Click();

        return MessageDialog.WaitFor(App);
    }

    /// <summary>Выделяет строку, нажимает «Изменить» и ждёт окно изменения записи.</summary>
    public EditorDialog OpenEditForm(string key)
    {
        ClickEdit(key);

        return EditorDialog.WaitFor(App, EditorFormId);
    }

    /// <summary>Выделяет строку, нажимает «Удалить» и ждёт окно с вопросом (или сообщение, что удалить нельзя).</summary>
    public MessageDialog OpenDeleteDialog(string key)
    {
        SelectRow(key);
        Find("deleteButton").Click();

        return MessageDialog.WaitFor(App);
    }

    protected void ClickEdit(string key)
    {
        SelectRow(key);
        Find("editButton").Click();
    }

    protected static AppiumElement FindView(WindowsDriver app, string viewId)
    {
        return Waits.Until(
            app,
            _ => app.FindElement(Locators.Id(viewId)),
            $"Не показан раздел {viewId}");
    }

    protected void WaitUntilLoaded()
    {
        Waits.Until(
            App,
            _ => CountText != LoadingText && Find("refreshButton").Enabled,
            "Раздел не загрузил записи");
    }

    protected AppiumElement Find(string automationId)
    {
        return Root.FindElement(Locators.Id(automationId));
    }
}
