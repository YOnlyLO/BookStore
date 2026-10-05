using BookStore.WebUiTest.Infrastructure;
using BookStore.WebUiTest.Pages;

namespace BookStore.WebUiTest.Tests;

[TestClass]
public sealed class BooksTests : UiTestBase
{
    private const string RequiredError = "Обязательное поле.";

    [TestMethod]
    public void EmptyList_ShowsEmptyState()
    {
        var page = BooksPage.Open(Browser);

        Assert.AreEqual("Книг пока нет.", page.EmptyStateText);
        Assert.AreEqual(0, page.Counter);
    }

    [TestMethod]
    public async Task Create_AddsBookToTable()
    {
        await Database.SeedGenreAsync("Фантастика");

        var page = BooksPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("title", "Солярис");
        form.Fill("author", "Станислав Лем");
        form.Select("genreId", "Фантастика");
        form.Fill("price", "1350.5");
        form.Fill("stockQuantity", "10");
        form.Fill("description", "Роман о планете-океане");
        form.Submit();

        form.WaitUntilClosed();

        // Цена выводится по-русски: разделитель тысяч, запятая и знак рубля
        CollectionAssert.AreEqual(
            new[] { "Солярис Станислав Лем", "Фантастика", "1 350,50 ₽", "10" },
            page.GetRow("Солярис").Take(4).ToArray());
    }

    [TestMethod]
    public async Task Create_EmptyForm_ShowsRequiredErrors()
    {
        await Database.SeedGenreAsync("Фантастика");

        var page = BooksPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Submit();

        Assert.AreEqual(RequiredError, form.FieldError("title"));
        Assert.AreEqual(RequiredError, form.FieldError("author"));
        Assert.AreEqual(RequiredError, form.FieldError("genreId"));
        Assert.AreEqual(RequiredError, form.FieldError("price"));
        // Остаток по умолчанию 0 — это корректное значение, а описание необязательно
        Assert.IsFalse(form.HasFieldError("stockQuantity"));
        Assert.IsFalse(form.HasFieldError("description"));
    }

    [TestMethod]
    [DataRow("price", "0", "Значение должно быть не меньше 0,01.")]
    [DataRow("price", "-5", "Значение должно быть не меньше 0,01.")]
    [DataRow("stockQuantity", "-1", "Значение должно быть не меньше 0.")]
    [DataRow("stockQuantity", "2.5", "Введите целое число.")]
    public async Task Create_InvalidNumber_ShowsError(string field, string value, string expectedError)
    {
        await Database.SeedGenreAsync("Фантастика");

        var page = BooksPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill(field, value);
        form.Submit();

        Assert.AreEqual(expectedError, form.FieldError(field));
    }

    [TestMethod]
    public void Create_WithoutGenres_ShowsHint()
    {
        var page = BooksPage.Open(Browser);

        var form = page.OpenCreateForm();

        Assert.AreEqual(
            "Сначала создайте хотя бы один жанр — книга обязана к нему относиться.",
            form.InfoAlert);
    }

    [TestMethod]
    public async Task List_BookWithZeroStock_ShowsOutOfStockBadge()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", stockQuantity: 0);

        var page = BooksPage.Open(Browser);

        Assert.AreEqual("Нет в наличии", page.GetRow("Солярис")[3]);
    }

    [TestMethod]
    public async Task Edit_ChangesPrice_StockIsReadOnly()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис", price: 450m, stockQuantity: 10);

        var page = BooksPage.Open(Browser);

        var form = page.OpenEditForm("Солярис");

        // API не умеет менять остаток, поэтому поле заблокировано и это объяснено подсказкой
        Assert.AreEqual("Фантастика", form.GetSelectedOption("genreId"));
        Assert.IsFalse(form.Field("stockQuantity").Enabled);
        Assert.AreEqual("API не поддерживает изменение остатка.", form.FieldHint("stockQuantity"));

        form.Fill("price", "500");
        form.Submit();

        form.WaitUntilClosed();
        Waits.Until(Browser, _ => page.GetRow("Солярис")[2] == "500,00 ₽", "Цена в таблице не обновилась");
        Assert.AreEqual("10", page.GetRow("Солярис")[3]);
    }

    [TestMethod]
    public async Task Delete_RemovesBook()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction, "Солярис");

        var page = BooksPage.Open(Browser);

        var dialog = page.OpenDeleteDialog("Солярис");
        Assert.AreEqual("Удалить книгу «Солярис»? Это действие нельзя отменить.", dialog.Message);

        dialog.Click("Удалить");

        dialog.WaitUntilClosed();
        page.WaitForRowToDisappear("Солярис");
        Assert.AreEqual("Книг пока нет.", page.EmptyStateText);
    }

    [TestMethod]
    public async Task Delete_BookInOrder_IsBlocked()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        var solaris = await Database.SeedBookAsync(fiction, "Солярис");
        var user = await Database.SeedUserAsync();
        await Database.SeedOrderAsync(user, items: (solaris, 1));

        var page = BooksPage.Open(Browser);

        var dialog = page.OpenDeleteDialog("Солярис");

        Assert.AreEqual(
            "Книга есть в заказах — пока на неё ссылаются позиции заказов, сервер не даст её удалить.",
            dialog.WarningAlert);
        Assert.IsFalse(dialog.Button("Удалить").Enabled);
    }
}
