using BookStore.Core.Enums;
using BookStore.GUITest.Infrastructure;

namespace BookStore.GUITest.Tests;

[TestClass]
public sealed class BooksTests : GuiTestBase
{
    [TestMethod]
    public async Task List_ShowsBookDetails()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", "Станислав Лем", price: 450m, stockQuantity: 10);

        var books = MainWindow.OpenBooks();

        // Цена выводится в рублях, жанр — названием, а не идентификатором
        CollectionAssert.AreEqual(
            new[] { "Солярис", "Станислав Лем", "Фантастика", "450,00 ₽", "10" },
            books.GetRow("Солярис").ToArray());
    }

    [TestMethod]
    public async Task Create_AddsBook()
    {
        await Database.SeedGenreAsync("Детектив");
        await Database.SeedGenreAsync("Фантастика");

        var books = MainWindow.OpenBooks();

        var form = books.OpenCreateForm();
        Assert.AreEqual("Новая книга", form.Title);

        form.Fill("titleTextBox", "Солярис");
        form.Fill("authorTextBox", "Станислав Лем");
        form.Select("genreComboBox", "Фантастика");
        form.FillNumber("priceUpDown", "450,5");
        form.FillNumber("stockUpDown", "12");
        form.Fill("descriptionTextBox", "Роман о планете-океане");
        form.Save();

        form.WaitUntilClosed();
        CollectionAssert.AreEqual(
            new[] { "Солярис", "Станислав Лем", "Фантастика", "450,50 ₽", "12" },
            books.GetRow("Солярис").ToArray());
    }

    [TestMethod]
    public void Create_WithoutGenres_ShowsMessage()
    {
        var books = MainWindow.OpenBooks();

        // Книга обязана относиться к жанру, поэтому без жанров окно создания не открывается
        var message = books.ClickAddExpectingMessage();

        Assert.AreEqual("Нет жанров", message.Title);
        Assert.AreEqual("Книга должна относиться к жанру. Сначала создайте хотя бы один жанр.", message.Message);

        message.Confirm();
    }

    [TestMethod]
    public async Task Create_ZeroPrice_ShowsError()
    {
        await Database.SeedGenreAsync("Фантастика");

        var books = MainWindow.OpenBooks();

        var form = books.OpenCreateForm();
        form.Fill("titleTextBox", "Солярис");
        form.Fill("authorTextBox", "Станислав Лем");
        form.Select("genreComboBox", "Фантастика");
        form.Save();

        Assert.AreEqual("Цена книги должна быть больше нуля.", form.Error);
        Assert.IsTrue(form.IsOpen);
    }

    [TestMethod]
    public async Task Create_WithoutGenre_ShowsError()
    {
        await Database.SeedGenreAsync("Фантастика");

        var books = MainWindow.OpenBooks();

        var form = books.OpenCreateForm();
        form.Fill("titleTextBox", "Солярис");
        form.Fill("authorTextBox", "Станислав Лем");
        form.FillNumber("priceUpDown", "450");
        form.Save();

        Assert.AreEqual("Выберите жанр книги.", form.Error);
    }

    [TestMethod]
    public async Task Edit_ChangesPrice_StockIsReadOnly()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", price: 450m, stockQuantity: 10);

        var books = MainWindow.OpenBooks();

        var form = books.OpenEditForm("Солярис");
        Assert.AreEqual("Изменение книги", form.Title);
        Assert.AreEqual("Солярис", form.GetText("titleTextBox"));
        Assert.AreEqual("450,00", form.GetNumber("priceUpDown"));
        // Остаток задаётся только при создании книги
        Assert.IsFalse(form.Field("stockUpDown").Enabled);
        Assert.IsTrue(form.HasField("stockHintLabel"));

        form.FillNumber("priceUpDown", "520");
        form.Save();

        form.WaitUntilClosed();
        Assert.AreEqual("520,00 ₽", books.WaitForCell("Солярис", 3, "520,00 ₽"));
    }

    [TestMethod]
    public async Task Delete_AsksConfirmation_AndRemovesBook()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", "Станислав Лем");

        var books = MainWindow.OpenBooks();

        var dialog = books.OpenDeleteDialog("Солярис");
        Assert.AreEqual("Удалить книгу «Солярис» (Станислав Лем)?", dialog.Message);

        dialog.Confirm();

        books.WaitForRowToDisappear("Солярис");
        Assert.AreEqual("Записей: 0", books.WaitForCount("Записей: 0"));
    }

    [TestMethod]
    public async Task Delete_BookInOrder_IsBlocked()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис");
        var user = await Database.SeedUserAsync();
        await Database.SeedOrderAsync(user, OrderStatus.New, (solaris, 1));

        var books = MainWindow.OpenBooks();

        var dialog = books.OpenDeleteDialog("Солярис");
        Assert.AreEqual("Удаление невозможно", dialog.Title);
        Assert.AreEqual(
            "Книга «Солярис» входит в заказы (1). Сначала уберите её из заказов или удалите эти заказы.",
            dialog.Message);

        dialog.Confirm();

        Assert.IsTrue(books.HasRow("Солярис"));
    }
}
