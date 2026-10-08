using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Раздел «Заказы». «Добавить» открывает окно выбора покупателя (OrderCreateForm), после создания
/// пустого заказа сразу открывается окно заказа; «Изменить» открывает окно заказа (OrderEditorForm).
/// </summary>
public sealed class OrdersSection : RecordListSection
{
    private OrdersSection(WindowsDriver app, AppiumElement root)
        : base(app, root, "OrderCreateForm")
    {
    }

    public static OrdersSection WaitFor(WindowsDriver app)
    {
        var section = new OrdersSection(app, FindView(app, "ordersView"));

        section.WaitUntilLoaded();

        return section;
    }

    /// <summary>Выделяет заказ по номеру (первые 8 символов идентификатора) и открывает окно заказа.</summary>
    public OrderEditorDialog OpenOrder(string number)
    {
        ClickEdit(number);

        return OrderEditorDialog.WaitFor(App);
    }
}
