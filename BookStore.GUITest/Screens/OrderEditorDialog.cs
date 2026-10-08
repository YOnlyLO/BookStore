using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Окно заказа (OrderEditorForm): шапка со статусом, покупателем и суммой, состав заказа,
/// блок добавления книги и кнопки смены статуса. Каждое действие сразу отправляется в API,
/// после чего окно перечитывает заказ, — поэтому результат действий нужно дожидаться.
/// </summary>
public sealed class OrderEditorDialog
{
    private const string NotLoadedText = "—";

    private static readonly By Locator = Locators.Window("OrderEditorForm");

    private readonly WindowsDriver _app;

    private readonly AppiumElement _window;

    private OrderEditorDialog(WindowsDriver app, AppiumElement window)
    {
        _app = app;
        _window = window;
    }

    /// <summary>Заголовок окна: «Заказ #9EE79E48».</summary>
    public string Title => _window.Text;

    /// <summary>Статус заказа: «Новый», «Подтверждён», «Завершён», «Отменён».</summary>
    public string Status => Find("statusBadge").Text;

    public string Customer => Find("customerValueLabel").Text;

    public string Total => UiText.Normalize(Find("totalValueLabel").Text);

    public GridElement Items => new(Find("itemsGrid"));

    /// <summary>Виден ли блок добавления книги (только у заказа в статусе «Новый»).</summary>
    public bool CanEditItems => _window.Has("addItemCard");

    public string LockedHint => Find("lockedHintLabel").Text;

    public bool CanConfirm => Find("confirmButton").Enabled;

    public bool CanComplete => Find("completeButton").Enabled;

    public bool CanCancelOrder => Find("cancelOrderButton").Enabled;

    public bool IsOpen => _app.FindElements(Locator).Count > 0;

    /// <summary>Цена за штуку в блоке добавления книги («450,00»).</summary>
    public string UnitPrice => Find("unitPriceUpDown").FindElement(By.XPath("./Edit")).Text;

    public string Error => Waits.Until(
        _app,
        _ => Find("errorBanner").Text is { Length: > 0 } text ? text : null,
        "В окне заказа не появилось сообщение об ошибке");

    /// <summary>Ждёт открытия окна заказа и загрузки заказа из API.</summary>
    public static OrderEditorDialog WaitFor(WindowsDriver app)
    {
        var window = Waits.Until(
            app,
            _ => app.FindElement(Locator),
            "Не открылось окно заказа");

        var dialog = new OrderEditorDialog(app, window);

        Waits.Until(app, _ => dialog.Customer != NotLoadedText, "Окно не загрузило заказ");

        return dialog;
    }

    /// <summary>Ждёт, пока статус станет ожидаемым, и возвращает его.</summary>
    public string WaitForStatus(string expected)
    {
        return Waits.ForText(_app, () => Status, expected);
    }

    /// <summary>Ждёт, пока сумма заказа станет ожидаемой, и возвращает её.</summary>
    public string WaitForTotal(string expected)
    {
        return Waits.ForText(_app, () => Total, expected);
    }

    /// <summary>Выбирает книгу в блоке добавления по началу текста пункта («Солярис — Станислав Лем»).</summary>
    public void SelectBook(string itemText)
    {
        Find("bookComboBox").SelectItem(itemText);
    }

    public void SetQuantity(int quantity)
    {
        Find("quantityUpDown").FindElement(By.XPath("./Edit")).ReplaceText(quantity.ToString());
    }

    public void AddItem()
    {
        Find("addItemButton").Click();
    }

    /// <summary>Выделяет позицию по названию книги и нажимает «Убрать позицию».</summary>
    public void RemoveItem(string bookTitle)
    {
        Items.SelectRow(bookTitle);
        Find("removeItemButton").Click();
    }

    public void ConfirmOrder()
    {
        Find("confirmButton").Click();
    }

    public void CompleteOrder()
    {
        Find("completeButton").Click();
    }

    /// <summary>Нажимает «Отменить заказ» и ждёт вопроса о подтверждении.</summary>
    public MessageDialog CancelOrder()
    {
        Find("cancelOrderButton").Click();

        return MessageDialog.WaitFor(_app);
    }

    public void Close()
    {
        Find("closeButton").Click();

        Waits.Until(_app, _ => !IsOpen, "Окно заказа не закрылось");
    }

    private AppiumElement Find(string automationId)
    {
        return _window.FindElement(Locators.Id(automationId));
    }
}
