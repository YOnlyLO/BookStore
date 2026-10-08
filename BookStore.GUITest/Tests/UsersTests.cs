using BookStore.Core.Enums;
using BookStore.GUITest.Infrastructure;

namespace BookStore.GUITest.Tests;

[TestClass]
public sealed class UsersTests : GuiTestBase
{
    [TestMethod]
    public void Create_AddsUser()
    {
        var users = MainWindow.OpenUsers();

        var form = users.OpenCreateForm();
        Assert.AreEqual("Новый пользователь", form.Title);

        form.Fill("lastNameTextBox", "Петров");
        form.Fill("firstNameTextBox", "Иван");
        form.Fill("emailTextBox", "ivan@example.com");
        form.Fill("passwordTextBox", "secret-123");
        form.Save();

        // В таблице — фамилия и имя одной колонкой, email и число заказов
        form.WaitUntilClosed();
        CollectionAssert.AreEqual(
            new[] { "Петров Иван", "ivan@example.com", "0" },
            users.GetRow("Петров Иван").ToArray());
    }

    [TestMethod]
    [DataRow("ivan.example.com")]
    [DataRow("ivan@")]
    [DataRow("Иван Петров <ivan@example.com>")]
    public void Create_InvalidEmail_ShowsError(string email)
    {
        var users = MainWindow.OpenUsers();

        var form = users.OpenCreateForm();
        form.Fill("lastNameTextBox", "Петров");
        form.Fill("firstNameTextBox", "Иван");
        form.Fill("emailTextBox", email);
        form.Fill("passwordTextBox", "secret-123");
        form.Save();

        Assert.AreEqual("Введите корректный адрес электронной почты.", form.Error);
    }

    [TestMethod]
    public async Task Create_DuplicateEmail_ShowsError()
    {
        await Database.SeedUserAsync(email: "ivan@example.com");

        var users = MainWindow.OpenUsers();

        // Email сравнивается без учёта регистра
        var form = users.OpenCreateForm();
        form.Fill("lastNameTextBox", "Сидоров");
        form.Fill("firstNameTextBox", "Иван");
        form.Fill("emailTextBox", "IVAN@example.com");
        form.Fill("passwordTextBox", "secret-123");
        form.Save();

        Assert.AreEqual("Пользователь с таким email уже существует.", form.Error);
    }

    [TestMethod]
    public void Create_WithoutPassword_ShowsError()
    {
        var users = MainWindow.OpenUsers();

        var form = users.OpenCreateForm();
        form.Fill("lastNameTextBox", "Петров");
        form.Fill("firstNameTextBox", "Иван");
        form.Fill("emailTextBox", "ivan@example.com");
        form.Save();

        Assert.AreEqual("Задайте пароль пользователя.", form.Error);
    }

    [TestMethod]
    public async Task Edit_ChangesName_WithoutPasswordField()
    {
        await Database.SeedUserAsync("Иван", "Петров", "ivan@example.com");

        var users = MainWindow.OpenUsers();

        var form = users.OpenEditForm("Петров Иван");
        Assert.AreEqual("Изменение пользователя", form.Title);
        Assert.AreEqual("Петров", form.GetText("lastNameTextBox"));
        Assert.AreEqual("ivan@example.com", form.GetText("emailTextBox"));
        // Пароль задаётся только при регистрации — при изменении поля нет
        Assert.IsFalse(form.HasField("passwordTextBox"));

        form.Fill("firstNameTextBox", "Пётр");
        form.Save();

        form.WaitUntilClosed();
        users.WaitForRow("Петров Пётр");
        Assert.IsFalse(users.HasRow("Петров Иван"));
    }

    [TestMethod]
    public async Task Delete_UserWithOrders_IsBlocked()
    {
        var user = await Database.SeedUserAsync("Иван", "Петров");
        await Database.SeedOrderAsync(user, OrderStatus.New);

        var users = MainWindow.OpenUsers();

        CollectionAssert.AreEqual(
            new[] { "Петров Иван", "ivan@example.com", "1" },
            users.GetRow("Петров Иван").ToArray());

        var dialog = users.OpenDeleteDialog("Петров Иван");
        Assert.AreEqual("Удаление невозможно", dialog.Title);
        Assert.AreEqual("У пользователя Петров Иван есть заказы (1). Сначала удалите их.", dialog.Message);

        dialog.Confirm();

        Assert.IsTrue(users.HasRow("Петров Иван"));
    }

    [TestMethod]
    public async Task Delete_AsksConfirmation_AndRemovesUser()
    {
        await Database.SeedUserAsync("Иван", "Петров", "ivan@example.com");

        var users = MainWindow.OpenUsers();

        var dialog = users.OpenDeleteDialog("Петров Иван");
        Assert.AreEqual("Удалить пользователя Петров Иван (ivan@example.com)?", dialog.Message);

        dialog.Confirm();

        users.WaitForRowToDisappear("Петров Иван");
        Assert.AreEqual("Записей: 0", users.WaitForCount("Записей: 0"));
    }
}
