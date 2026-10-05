using BookStore.WebUiTest.Infrastructure;
using BookStore.WebUiTest.Pages;

namespace BookStore.WebUiTest.Tests;

[TestClass]
public sealed class UsersTests : UiTestBase
{
    private const string RequiredError = "Обязательное поле.";

    [TestMethod]
    public void Create_AddsUserToTable()
    {
        var page = UsersPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("firstName", "Иван");
        form.Fill("lastName", "Петров");
        form.Fill("email", "ivan@example.com");
        form.Fill("passwordHash", "5f4dcc3b5aa765d61d8327deb882cf99");
        form.Submit();

        form.WaitUntilClosed();

        // В первой ячейке — аватар с инициалами и имя «Фамилия Имя»
        CollectionAssert.AreEqual(
            new[] { "ИП Петров Иван", "ivan@example.com", "0" },
            page.GetRow("Петров Иван").Take(3).ToArray());
    }

    [TestMethod]
    public void Create_EmptyForm_ShowsRequiredErrors()
    {
        var page = UsersPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Submit();

        Assert.AreEqual(RequiredError, form.FieldError("firstName"));
        Assert.AreEqual(RequiredError, form.FieldError("lastName"));
        Assert.AreEqual(RequiredError, form.FieldError("email"));
        Assert.AreEqual(RequiredError, form.FieldError("passwordHash"));
    }

    [TestMethod]
    public void Create_InvalidEmail_ShowsError()
    {
        var page = UsersPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("email", "ivan.example.com");
        form.Submit();

        Assert.AreEqual("Некорректный адрес электронной почты.", form.FieldError("email"));
    }

    [TestMethod]
    public async Task Create_DuplicateEmailInDifferentCase_ShowsError()
    {
        await Database.SeedUserAsync(email: "ivan@example.com");

        var page = UsersPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("firstName", "Иван");
        form.Fill("lastName", "Сидоров");
        form.Fill("email", "IVAN@example.com");
        form.Fill("passwordHash", "hash");
        form.Submit();

        Assert.AreEqual("Пользователь с email IVAN@example.com уже существует.", form.ErrorAlert);
    }

    [TestMethod]
    public async Task Edit_HidesPassword_AndSavesChanges()
    {
        await Database.SeedUserAsync("Иван", "Петров", "ivan@example.com");

        var page = UsersPage.Open(Browser);

        var form = page.OpenEditForm("Петров Иван");

        // Пароль через API не меняется — поля нет в форме изменения
        Assert.IsFalse(form.HasField("passwordHash"));
        Assert.AreEqual("ivan@example.com", form.GetValue("email"));

        form.Fill("lastName", "Сидоров");
        form.Submit();

        form.WaitUntilClosed();
        CollectionAssert.AreEqual(
            new[] { "ИС Сидоров Иван", "ivan@example.com" },
            page.GetRow("Сидоров Иван").Take(2).ToArray());
        Assert.IsFalse(page.HasRow("Петров Иван"));
    }

    [TestMethod]
    public async Task Delete_RemovesUser()
    {
        await Database.SeedUserAsync("Иван", "Петров");

        var page = UsersPage.Open(Browser);

        var dialog = page.OpenDeleteDialog("Петров Иван");
        Assert.AreEqual("Удалить пользователя Петров Иван? Это действие нельзя отменить.", dialog.Message);

        dialog.Click("Удалить");

        dialog.WaitUntilClosed();
        page.WaitForRowToDisappear("Петров Иван");
        Assert.AreEqual("Пользователей пока нет.", page.EmptyStateText);
    }

    [TestMethod]
    public async Task Delete_UserWithOrders_IsBlocked()
    {
        var user = await Database.SeedUserAsync("Иван", "Петров");
        await Database.SeedOrderAsync(user);

        var page = UsersPage.Open(Browser);

        Assert.AreEqual("1", page.GetRow("Петров Иван")[2]);

        var dialog = page.OpenDeleteDialog("Петров Иван");

        Assert.AreEqual(
            "Заказов у пользователя: 1. Пока они существуют, сервер не даст удалить пользователя.",
            dialog.WarningAlert);
        Assert.IsFalse(dialog.Button("Удалить").Enabled);
    }
}
