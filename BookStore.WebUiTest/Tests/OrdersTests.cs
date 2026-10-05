using BookStore.Core.Enums;
using BookStore.WebUiTest.Infrastructure;
using BookStore.WebUiTest.Pages;

namespace BookStore.WebUiTest.Tests;

[TestClass]
public sealed class OrdersTests : UiTestBase
{
    private const string RequiredError = "Обязательное поле.";

    [TestMethod]
    public void Create_WithoutUsers_ShowsHint()
    {
        var page = OrdersPage.Open(Browser);

        var form = page.OpenCreateForm();

        Assert.AreEqual("Сначала добавьте пользователя — заказ оформляется на покупателя.", form.InfoAlert);
    }

    [TestMethod]
    public async Task Create_WithoutCustomer_ShowsRequiredError()
    {
        await Database.SeedUserAsync();

        var page = OrdersPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Submit();

        Assert.AreEqual(RequiredError, form.FieldError("userId"));
    }

    [TestMethod]
    public async Task Create_OpensEditorForEmptyOrder()
    {
        await Database.SeedUserAsync("Иван", "Петров", "ivan@example.com");

        var page = OrdersPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Select("userId", "Петров Иван · ivan@example.com");
        form.Submit();

        // Заказ создаётся пустым, и сразу открывается его карточка для добавления книг
        var editor = OrderEditor.WaitForNewOrder(Browser);

        Assert.AreEqual("Петров Иван", editor.Customer);
        Assert.AreEqual("Новый", editor.Status);
        Assert.AreEqual("0,00 ₽", editor.Total);
        Assert.AreEqual("В заказе пока нет книг. Чтобы подтвердить заказ, добавьте хотя бы одну.", editor.EmptyItemsText);
        Assert.IsFalse(editor.CanConfirm);

        var number = editor.Title["Заказ ".Length..];
        editor.Close();

        // В таблице: номер, покупатель, дата, статус, позиций, сумма
        var row = page.GetRow(number);
        Assert.AreEqual("Петров Иван", row[1]);
        CollectionAssert.AreEqual(new[] { "Новый", "0", "0,00 ₽" }, row.Skip(3).Take(3).ToArray());
    }

    [TestMethod]
    public async Task AddItem_TakesPriceFromCatalog_AndUpdatesTotal()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", price: 450m);
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user);

        var page = OrdersPage.Open(Browser);
        var editor = page.OpenEditor(order.Id);

        // При выборе книги цена подставляется из каталога
        editor.SelectBook("Солярис");
        Assert.AreEqual("450", editor.Dialog.GetValue("unitPrice"));

        editor.AddItem("Солярис", 3);

        Assert.HasCount(1, editor.Items);
        CollectionAssert.AreEqual(
            new[] { "Солярис Станислав Лем", "3", "450,00 ₽", "1 350,00 ₽" },
            editor.Items[0].ToArray());
        Assert.AreEqual("1 350,00 ₽", editor.Total);
        Assert.IsTrue(editor.CanConfirm);
    }

    [TestMethod]
    public async Task AddItem_SameBookTwice_SumsQuantity()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис", price: 450m);
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user, items: (solaris, 1));

        var page = OrdersPage.Open(Browser);
        var editor = page.OpenEditor(order.Id);

        editor.AddItem("Солярис", 2);

        Assert.HasCount(1, editor.Items);
        Assert.AreEqual("3", editor.Items[0][1]);
        Assert.AreEqual("1 350,00 ₽", editor.Total);
    }

    [TestMethod]
    public async Task AddItem_WithoutBook_ShowsRequiredErrors()
    {
        await Database.SeedGenreAsync("Фантастика");
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user);

        var page = OrdersPage.Open(Browser);
        var editor = page.OpenEditor(order.Id);

        editor.ClickAddItem();

        Assert.AreEqual(RequiredError, editor.Dialog.FieldError("bookId"));
        Assert.AreEqual(RequiredError, editor.Dialog.FieldError("unitPrice"));
    }

    [TestMethod]
    public async Task RemoveItem_LeavesOrderEmpty()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис", price: 450m);
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user, items: (solaris, 2));

        var page = OrdersPage.Open(Browser);
        var editor = page.OpenEditor(order.Id);
        Assert.AreEqual("900,00 ₽", editor.Total);

        editor.RemoveItem("Солярис");

        Assert.AreEqual("0,00 ₽", editor.Total);
        Assert.HasCount(0, editor.Items);
        Assert.IsFalse(editor.CanConfirm);
    }

    [TestMethod]
    public async Task ConfirmAndComplete_ChangeStatus()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис");
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user, items: (solaris, 1));

        var page = OrdersPage.Open(Browser);
        var editor = page.OpenEditor(order.Id);

        editor.Confirm();

        // Подтверждённый заказ менять нельзя, но можно завершить или отменить
        Assert.AreEqual("Подтверждён", editor.Status);
        Assert.IsFalse(editor.CanEditItems);
        Assert.AreEqual("Состав можно менять только у заказа в статусе «Новый».", editor.ReadOnlyNotice);
        Assert.IsTrue(editor.HasButton("Завершить"));
        Assert.IsTrue(editor.HasButton("Отменить заказ"));

        editor.Complete();

        Assert.AreEqual("Завершён", editor.Status);
        Assert.IsFalse(editor.HasButton("Завершить"));
        Assert.IsFalse(editor.HasButton("Отменить заказ"));

        editor.Close();
        page.WaitForRow(OrdersPage.NumberOf(order.Id));
        Assert.AreEqual("Завершён", page.GetRow(OrdersPage.NumberOf(order.Id))[3]);
    }

    [TestMethod]
    public async Task Cancel_RequiresConfirmation()
    {
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user, OrderStatus.New);

        var page = OrdersPage.Open(Browser);
        var editor = page.OpenEditor(order.Id);

        // Отказ от отмены оставляет заказ как был
        var confirmation = editor.AskCancel();
        Assert.AreEqual("Отменить заказ? Отменённый заказ нельзя вернуть в работу.", confirmation.Message);
        confirmation.Click("Не отменять");
        confirmation.WaitUntilClosed();
        Assert.AreEqual("Новый", editor.Status);

        editor.ConfirmCancel(editor.AskCancel());

        Assert.AreEqual("Отменён", editor.Status);
        Assert.IsFalse(editor.HasButton("Отменить заказ"));
    }

    [TestMethod]
    public async Task Delete_RemovesOrder()
    {
        var user = await Database.SeedUserAsync();
        var order = await Database.SeedOrderAsync(user);
        var number = OrdersPage.NumberOf(order.Id);

        var page = OrdersPage.Open(Browser);

        var dialog = page.OpenDeleteDialog(order.Id);
        Assert.AreEqual(
            $"Удалить заказ {number} вместе со всеми позициями? Это действие нельзя отменить.",
            dialog.Message);

        dialog.Click("Удалить");

        dialog.WaitUntilClosed();
        page.WaitForRowToDisappear(number);
        Assert.AreEqual("Заказов пока нет.", page.EmptyStateText);
    }
}
