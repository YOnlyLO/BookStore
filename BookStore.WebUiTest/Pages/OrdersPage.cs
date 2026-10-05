using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

public sealed class OrdersPage : ListPage
{
    private OrdersPage(IWebDriver browser)
        : base(browser)
    {
    }

    public static OrdersPage Open(IWebDriver browser)
    {
        var page = new OrdersPage(browser);

        page.OpenSection("admin/orders", "Заказы");

        return page;
    }

    /// <summary>Номер заказа, как его показывает интерфейс: «#» и первые 8 символов идентификатора.</summary>
    public static string NumberOf(Guid orderId) => $"#{orderId.ToString("N")[..8].ToUpperInvariant()}";

    public ModalDialog OpenCreateForm() => ClickPageAction("Создать заказ", "Новый заказ");

    public OrderEditor OpenEditor(Guid orderId)
    {
        var number = NumberOf(orderId);

        ClickRowAction(number, "Изменить", $"Заказ {number}");

        return OrderEditor.WaitFor(Browser, number);
    }

    public ModalDialog OpenDeleteDialog(Guid orderId) =>
        ClickRowAction(NumberOf(orderId), "Удалить", "Удаление заказа");
}
