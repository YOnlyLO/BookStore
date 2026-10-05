using BookStore.WebUiTest.Infrastructure;
using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

/// <summary>
/// Окно заказа: сводка (покупатель, статус, сумма), состав, добавление книг и смена статуса.
/// Каждое действие отправляет запрос к API и перечитывает заказ, поэтому методы ждут,
/// пока результат появится на экране.
/// </summary>
public sealed class OrderEditor
{
    private const string AddItemButton = "Добавить";

    private const string ConfirmButton = "Подтвердить";

    private const string CompleteButton = "Завершить";

    private const string CancelButton = "Отменить заказ";

    private readonly IWebDriver _browser;

    private OrderEditor(IWebDriver browser, ModalDialog dialog)
    {
        _browser = browser;
        Dialog = dialog;

        Waits.Until(browser, _ => Dialog.Find(By.CssSelector("dl.summary")), "Заказ не загрузился");
    }

    public ModalDialog Dialog { get; }

    public string Title => Dialog.Title;

    public string Customer => Summary("Покупатель");

    public string Status => Summary("Статус");

    public string Total => Summary("Итого");

    public string EmptyItemsText => Dialog.Find(By.CssSelector(".empty-items")).Text;

    /// <summary>Сообщение о том, что состав заказа менять нельзя.</summary>
    public string ReadOnlyNotice => Dialog.Find(By.CssSelector(".alert-info")).Text;

    /// <summary>Позиции заказа: книга (название и автор), количество, цена, сумма.</summary>
    public IReadOnlyList<IReadOnlyList<string>> Items =>
        Dialog
            .FindAll(By.CssSelector(".items tbody tr"))
            .Select(row => row
                .FindElements(By.TagName("td"))
                .Select(cell => UiText.Normalize(cell.Text))
                .Take(4)
                .ToList())
            .ToList();

    public static OrderEditor WaitFor(IWebDriver browser, string orderNumber)
    {
        return new OrderEditor(browser, ModalDialog.WaitFor(browser, $"Заказ {orderNumber}"));
    }

    /// <summary>Ждёт окно только что созданного заказа, номер которого тест заранее не знает.</summary>
    public static OrderEditor WaitForNewOrder(IWebDriver browser)
    {
        return new OrderEditor(browser, ModalDialog.WaitForTitleStartingWith(browser, "Заказ #"));
    }

    public bool HasButton(string text)
    {
        return Dialog.FindAll(By.XPath($".//button[normalize-space()={UiText.XPathLiteral(text)}]")).Count > 0;
    }

    public bool CanConfirm => Dialog.Button(ConfirmButton).Enabled;

    public bool CanEditItems => Dialog.HasField("bookId");

    /// <summary>Выбирает книгу в списке «Книга» (достаточно названия — в пункте ещё автор, цена и остаток).</summary>
    public void SelectBook(string title)
    {
        Dialog.Select("bookId", title, partialMatch: true);
    }

    public void AddItem(string bookTitle, int quantity)
    {
        SelectBook(bookTitle);
        Dialog.Fill("quantity", quantity.ToString());

        ChangeTotal(() => Dialog.Click(AddItemButton));
    }

    public void ClickAddItem()
    {
        Dialog.Click(AddItemButton);
    }

    public void RemoveItem(string bookTitle)
    {
        var removeButton = Dialog.Find(By.XPath(
            $".//*[contains(@class, 'items')]//tr[.//*[contains(@class, 'cell-main') and normalize-space()={UiText.XPathLiteral(bookTitle)}]]" +
            "//button[@aria-label='Убрать из заказа']"));

        ChangeTotal(removeButton.Click);
    }

    public void Confirm()
    {
        ChangeStatus(() => Dialog.Click(ConfirmButton));
    }

    public void Complete()
    {
        ChangeStatus(() => Dialog.Click(CompleteButton));
    }

    /// <summary>Нажимает «Отменить заказ» и возвращает окно подтверждения отмены.</summary>
    public ModalDialog AskCancel()
    {
        Dialog.Click(CancelButton);

        return ModalDialog.WaitFor(_browser, "Отмена заказа");
    }

    /// <summary>Подтверждает отмену в окне подтверждения и ждёт смены статуса.</summary>
    public void ConfirmCancel(ModalDialog confirmation)
    {
        ChangeStatus(() => confirmation.Click(CancelButton));

        confirmation.WaitUntilClosed();
    }

    public void Close()
    {
        Dialog.Click("Закрыть");
        Dialog.WaitUntilClosed();
    }

    private string Summary(string label)
    {
        return UiText.Normalize(Dialog
            .Find(By.XPath($".//dl[contains(@class, 'summary')]/div[dt[normalize-space()={UiText.XPathLiteral(label)}]]/dd"))
            .Text);
    }

    private void ChangeTotal(Action action)
    {
        var total = Total;

        action();

        Waits.UntilChanged(_browser, () => Total, total, $"Сумма заказа не изменилась (было {total})");
    }

    private void ChangeStatus(Action action)
    {
        var status = Status;

        action();

        Waits.UntilChanged(_browser, () => Status, status, $"Статус заказа не изменился (был «{status}»)");
    }
}
