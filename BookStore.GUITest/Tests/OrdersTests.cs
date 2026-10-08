using BookStore.Core.Enums;
using BookStore.GUITest.Infrastructure;
using BookStore.GUITest.Screens;

namespace BookStore.GUITest.Tests;

[TestClass]
public sealed class OrdersTests : GuiTestBase
{
    [TestMethod]
    public void Create_WithoutUsers_ShowsMessage()
    {
        var orders = MainWindow.OpenOrders();

        var message = orders.ClickAddExpectingMessage();

        Assert.AreEqual("Нет покупателей", message.Title);
        Assert.AreEqual(
            "Заказ оформляется на пользователя. Сначала добавьте хотя бы одного пользователя.",
            message.Message);

        message.Confirm();
    }

    [TestMethod]
    public async Task Create_WithoutCustomer_ShowsError()
    {
        await Database.SeedUserAsync();

        var orders = MainWindow.OpenOrders();

        var form = orders.OpenCreateForm();
        form.Save();

        Assert.AreEqual("Выберите покупателя.", form.Error);
    }

    [TestMethod]
    public async Task Create_OpensNewEmptyOrder()
    {
        await Database.SeedUserAsync("Анна", "Смирнова", "anna@example.com");
        await Database.SeedUserAsync("Иван", "Петров", "ivan@example.com");

        var orders = MainWindow.OpenOrders();

        // Выбор покупателя в окне «Новый заказ»
        var form = orders.OpenCreateForm();
        Assert.AreEqual("Новый заказ", form.Title);

        form.Select("userComboBox", "Петров Иван");
        form.Save();

        // Заказ создаётся пустым, и сразу открывается окно заказа, чтобы добавить книги
        var order = OrderEditorDialog.WaitFor(App);
        Assert.AreEqual("Новый", order.Status);
        Assert.AreEqual("Петров Иван", order.Customer);
        Assert.AreEqual("0,00 ₽", order.Total);
        Assert.AreEqual(0, order.Items.RowCount);
        Assert.IsTrue(order.CanEditItems);
        // Пустой заказ подтвердить нельзя
        Assert.IsFalse(order.CanConfirm);

        order.Close();

        Assert.AreEqual("Записей: 1", orders.WaitForCount("Записей: 1"));
        var row = orders.GetRows()[0];
        Assert.AreEqual("Петров Иван", row[1]);
        Assert.AreEqual("Новый", row[3]);
    }

    [TestMethod]
    public async Task AddItem_FillsPriceFromBook_AndUpdatesTotal()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", "Станислав Лем", price: 450m);
        var user = await Database.SeedUserAsync();
        var seeded = await Database.SeedOrderAsync(user, OrderStatus.New);

        var order = MainWindow.OpenOrders().OpenOrder(Number(seeded.Id));
        Assert.AreEqual($"Заказ #{Number(seeded.Id)}", order.Title);

        // Цена за штуку подставляется из карточки книги
        order.SelectBook("Солярис — Станислав Лем");
        Assert.AreEqual("450,00", order.UnitPrice);

        order.SetQuantity(2);
        order.AddItem();

        Assert.AreEqual("900,00 ₽", order.WaitForTotal("900,00 ₽"));
        CollectionAssert.AreEqual(
            new[] { "Солярис", "2", "450,00 ₽", "900,00 ₽" },
            order.Items.GetRow("Солярис").ToArray());
        Assert.IsTrue(order.CanConfirm);
    }

    [TestMethod]
    public async Task RemoveItem_ClearsOrder()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис", price: 450m);
        var user = await Database.SeedUserAsync();
        var seeded = await Database.SeedOrderAsync(user, OrderStatus.New, (solaris, 2));

        var order = MainWindow.OpenOrders().OpenOrder(Number(seeded.Id));
        Assert.AreEqual("900,00 ₽", order.Total);

        order.RemoveItem("Солярис");

        Assert.AreEqual("0,00 ₽", order.WaitForTotal("0,00 ₽"));
        Assert.AreEqual(0, order.Items.RowCount);
        Assert.IsFalse(order.CanConfirm);
    }

    [TestMethod]
    public async Task ConfirmAndComplete_ChangeStatus()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис", price: 450m);
        var user = await Database.SeedUserAsync();
        var seeded = await Database.SeedOrderAsync(user, OrderStatus.New, (solaris, 1));

        var order = MainWindow.OpenOrders().OpenOrder(Number(seeded.Id));
        Assert.AreEqual("Новый", order.Status);
        Assert.IsFalse(order.CanComplete);

        order.ConfirmOrder();

        // У подтверждённого заказа состав больше не меняется
        Assert.AreEqual("Подтверждён", order.WaitForStatus("Подтверждён"));
        Assert.IsFalse(order.CanEditItems);
        Assert.AreEqual("Состав можно менять только у заказа в статусе «Новый».", order.LockedHint);
        Assert.IsFalse(order.CanConfirm);
        Assert.IsTrue(order.CanComplete);

        order.CompleteOrder();

        // Завершённый заказ нельзя ни завершить повторно, ни отменить
        Assert.AreEqual("Завершён", order.WaitForStatus("Завершён"));
        Assert.IsFalse(order.CanComplete);
        Assert.IsFalse(order.CanCancelOrder);

        order.Close();

        var orders = MainWindow.Orders;
        Assert.AreEqual("Завершён", orders.WaitForCell(Number(seeded.Id), 3, "Завершён"));
    }

    [TestMethod]
    public async Task CancelOrder_AsksConfirmation()
    {
        var user = await Database.SeedUserAsync();
        var seeded = await Database.SeedOrderAsync(user, OrderStatus.Confirmed);
        var number = Number(seeded.Id);

        var order = MainWindow.OpenOrders().OpenOrder(number);

        var dialog = order.CancelOrder();
        Assert.AreEqual("Отмена заказа", dialog.Title);
        Assert.AreEqual(
            $"Отменить заказ #{number}? Отменённый заказ нельзя будет изменить или снова подтвердить.",
            dialog.Message);
        Assert.AreEqual("Отменить заказ", dialog.ConfirmText);

        dialog.Confirm();

        Assert.AreEqual("Отменён", order.WaitForStatus("Отменён"));
        Assert.IsFalse(order.CanCancelOrder);
        Assert.IsFalse(order.CanComplete);
    }

    [TestMethod]
    public async Task CancelOrder_Declined_KeepsStatus()
    {
        var user = await Database.SeedUserAsync();
        var seeded = await Database.SeedOrderAsync(user, OrderStatus.Confirmed);

        var order = MainWindow.OpenOrders().OpenOrder(Number(seeded.Id));

        order.CancelOrder().Cancel();

        Assert.AreEqual("Подтверждён", order.Status);
        Assert.IsTrue(order.CanCancelOrder);
    }

    [TestMethod]
    public async Task Delete_AsksConfirmation_AndRemovesOrder()
    {
        var user = await Database.SeedUserAsync("Иван", "Петров");
        var seeded = await Database.SeedOrderAsync(user, OrderStatus.New);
        var number = Number(seeded.Id);

        var orders = MainWindow.OpenOrders();

        var dialog = orders.OpenDeleteDialog(number);
        Assert.AreEqual(
            $"Удалить заказ #{number} (Петров Иван)? Позиции заказа будут удалены вместе с ним.",
            dialog.Message);

        dialog.Confirm();

        orders.WaitForRowToDisappear(number);
        Assert.AreEqual("Записей: 0", orders.WaitForCount("Записей: 0"));
    }

    /// <summary>Номер заказа, как его показывает приложение: первые 8 символов идентификатора.</summary>
    private static string Number(Guid orderId)
    {
        return orderId.ToString()[..8].ToUpperInvariant();
    }
}
