using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Модальное окно записи (GenreEditorForm, BookEditorForm, UserEditorForm, OrderCreateForm):
/// заголовок, плашка ошибки, поля и кнопки «Сохранить» / «Отмена». Поля ищутся по AutomationId —
/// имени контрола в форме (nameTextBox, priceUpDown…), как элементы tb_a, btn_Go в методичке.
/// </summary>
public sealed class EditorDialog
{
    private readonly WindowsDriver _app;

    private readonly AppiumElement _window;

    private readonly By _locator;

    private EditorDialog(WindowsDriver app, AppiumElement window, By locator)
    {
        _app = app;
        _window = window;
        _locator = locator;

        // Заголовок запоминается сразу: после закрытия окна прочитать его уже нельзя.
        Title = window.Text;
    }

    /// <summary>Заголовок окна: «Новый жанр», «Изменение книги»…</summary>
    public string Title { get; }

    public bool IsOpen => _app.FindElements(_locator).Count > 0;

    /// <summary>Ждёт сообщение об ошибке над полями и возвращает его текст.</summary>
    public string Error => Waits.Until(
        _app,
        _ => Find("errorBanner").Text is { Length: > 0 } text ? text : null,
        $"В окне «{Title}» не появилось сообщение об ошибке");

    public bool HasError => _window.Has("errorBanner");

    public static EditorDialog WaitFor(WindowsDriver app, string formId)
    {
        var locator = Locators.Window(formId);

        var window = Waits.Until(
            app,
            _ => app.FindElement(locator),
            $"Не открылось окно {formId}");

        return new EditorDialog(app, window, locator);
    }

    public AppiumElement Field(string automationId)
    {
        return Find(automationId);
    }

    /// <summary>Есть ли поле в окне (скрытые поля в дереве UI Automation отсутствуют).</summary>
    public bool HasField(string automationId)
    {
        return _window.Has(automationId);
    }

    /// <summary>Текст поля ввода.</summary>
    public string GetText(string automationId)
    {
        return Find(automationId).Text;
    }

    /// <summary>Заменяет текст поля ввода.</summary>
    public void Fill(string automationId, string value)
    {
        Find(automationId).ReplaceText(value);
    }

    /// <summary>Текст числового поля (NumericUpDown): «450,00».</summary>
    public string GetNumber(string automationId)
    {
        return NumberEdit(automationId).Text;
    }

    /// <summary>
    /// Вводит число в NumericUpDown. Текст вводится во вложенное поле ввода; само значение
    /// NumericUpDown разбирает, когда фокус уходит из поля (например, при нажатии «Сохранить»).
    /// </summary>
    public void FillNumber(string automationId, string value)
    {
        NumberEdit(automationId).ReplaceText(value);
    }

    /// <summary>Выбирает пункт выпадающего списка по началу его текста.</summary>
    public void Select(string automationId, string itemText)
    {
        Find(automationId).SelectItem(itemText);
    }

    public void Save()
    {
        Find("saveButton").Click();
    }

    public void Cancel()
    {
        Find("cancelButton").Click();
    }

    public void WaitUntilClosed()
    {
        Waits.Until(_app, _ => !IsOpen, $"Окно «{Title}» не закрылось");
    }

    private AppiumElement NumberEdit(string automationId)
    {
        return Find(automationId).FindElement(By.XPath("./Edit"));
    }

    private AppiumElement Find(string automationId)
    {
        return _window.FindElement(Locators.Id(automationId));
    }
}
