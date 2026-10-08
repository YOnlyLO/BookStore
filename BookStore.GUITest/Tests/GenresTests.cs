using BookStore.GUITest.Infrastructure;

namespace BookStore.GUITest.Tests;

[TestClass]
public sealed class GenresTests : GuiTestBase
{
    [TestMethod]
    public void EmptyList_ShowsNoRecords()
    {
        var genres = MainWindow.OpenGenres();

        Assert.AreEqual("Записей: 0", genres.CountText);
        Assert.AreEqual(0, genres.Grid.RowCount);
        // Без выделенной записи изменять и удалять нечего
        Assert.IsFalse(genres.CanEdit);
        Assert.IsFalse(genres.CanDelete);
    }

    [TestMethod]
    public async Task List_ShowsGenresSortedByNameWithBookCounts()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedGenreAsync("Детектив");
        await Database.SeedBookAsync(fiction, "Солярис");
        await Database.SeedBookAsync(fiction, "Эдем");

        var genres = MainWindow.OpenGenres();

        Assert.AreEqual("Записей: 2", genres.CountText);
        var rows = genres.GetRows();
        CollectionAssert.AreEqual(new[] { "Детектив", "0" }, rows[0].ToArray());
        CollectionAssert.AreEqual(new[] { "Фантастика", "2" }, rows[1].ToArray());
    }

    [TestMethod]
    public void Create_AddsGenreToList()
    {
        var genres = MainWindow.OpenGenres();

        // Нажатие «Добавить», ввод названия и «Сохранить»
        var form = genres.OpenCreateForm();
        Assert.AreEqual("Новый жанр", form.Title);

        form.Fill("nameTextBox", "Фантастика");
        form.Save();

        // Окно закрывается, а жанр появляется в таблице
        form.WaitUntilClosed();
        CollectionAssert.AreEqual(new[] { "Фантастика", "0" }, genres.GetRow("Фантастика").ToArray());
        Assert.AreEqual("Записей: 1", genres.WaitForCount("Записей: 1"));
    }

    [TestMethod]
    public void Create_TrimsName()
    {
        var genres = MainWindow.OpenGenres();

        var form = genres.OpenCreateForm();
        form.Fill("nameTextBox", "  Фантастика  ");
        form.Save();

        form.WaitUntilClosed();
        genres.WaitForRow("Фантастика");
    }

    [TestMethod]
    public void Create_EmptyName_ShowsError()
    {
        var genres = MainWindow.OpenGenres();

        var form = genres.OpenCreateForm();
        form.Save();

        // Запрос к API не отправляется: окно остаётся открытым с сообщением об ошибке
        Assert.AreEqual("Заполните поле «Название».", form.Error);
        Assert.IsTrue(form.IsOpen);

        form.Cancel();
        form.WaitUntilClosed();
        Assert.AreEqual("Записей: 0", genres.CountText);
    }

    [TestMethod]
    public async Task Create_DuplicateName_ShowsError()
    {
        await Database.SeedGenreAsync("Фантастика");

        var genres = MainWindow.OpenGenres();

        // Сравнение без учёта регистра
        var form = genres.OpenCreateForm();
        form.Fill("nameTextBox", "фантастика");
        form.Save();

        Assert.AreEqual("Жанр с таким названием уже существует.", form.Error);
    }

    [TestMethod]
    public void Create_Cancel_DoesNotAddGenre()
    {
        var genres = MainWindow.OpenGenres();

        var form = genres.OpenCreateForm();
        form.Fill("nameTextBox", "Фантастика");
        form.Cancel();

        form.WaitUntilClosed();
        Assert.AreEqual("Записей: 0", genres.CountText);
        Assert.AreEqual(0, genres.Grid.RowCount);
    }

    [TestMethod]
    public async Task Edit_RenamesGenre()
    {
        await Database.SeedGenreAsync("Фантастика");

        var genres = MainWindow.OpenGenres();

        // Окно изменения открывается с текущим названием
        var form = genres.OpenEditForm("Фантастика");
        Assert.AreEqual("Изменение жанра", form.Title);
        Assert.AreEqual("Фантастика", form.GetText("nameTextBox"));

        form.Fill("nameTextBox", "Научная фантастика");
        form.Save();

        form.WaitUntilClosed();
        genres.WaitForRow("Научная фантастика");
        Assert.IsFalse(genres.HasRow("Фантастика"));
        Assert.AreEqual("Записей: 1", genres.CountText);
    }

    [TestMethod]
    public async Task Delete_AsksConfirmation_AndRemovesGenre()
    {
        await Database.SeedGenreAsync("Фантастика");

        var genres = MainWindow.OpenGenres();

        var dialog = genres.OpenDeleteDialog("Фантастика");
        Assert.AreEqual("Удаление записи", dialog.Title);
        Assert.AreEqual("Удалить жанр «Фантастика»?", dialog.Message);
        Assert.AreEqual("Удалить", dialog.ConfirmText);

        dialog.Confirm();

        genres.WaitForRowToDisappear("Фантастика");
        Assert.AreEqual("Записей: 0", genres.WaitForCount("Записей: 0"));
    }

    [TestMethod]
    public async Task Delete_Cancel_KeepsGenre()
    {
        await Database.SeedGenreAsync("Фантастика");

        var genres = MainWindow.OpenGenres();

        var dialog = genres.OpenDeleteDialog("Фантастика");
        dialog.Cancel();

        Assert.IsTrue(genres.HasRow("Фантастика"));
        Assert.AreEqual("Записей: 1", genres.CountText);
    }

    [TestMethod]
    public async Task Delete_GenreWithBooks_IsBlocked()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction);

        var genres = MainWindow.OpenGenres();

        // Вместо вопроса об удалении приложение объясняет, почему удалить нельзя
        var dialog = genres.OpenDeleteDialog("Фантастика");
        Assert.AreEqual("Удаление невозможно", dialog.Title);
        Assert.AreEqual(
            "К жанру «Фантастика» относятся книги (1). Сначала удалите их или перенесите в другой жанр.",
            dialog.Message);
        Assert.AreEqual("Понятно", dialog.ConfirmText);
        Assert.IsFalse(dialog.HasCancel);

        dialog.Confirm();

        Assert.IsTrue(genres.HasRow("Фантастика"));
    }

    [TestMethod]
    public async Task Search_FiltersList()
    {
        await Database.SeedGenreAsync("Фантастика");
        await Database.SeedGenreAsync("Детектив");

        var genres = MainWindow.OpenGenres();

        genres.Search("фант");

        Assert.AreEqual("Найдено: 1 из 2", genres.WaitForCount("Найдено: 1 из 2"));
        CollectionAssert.AreEqual(new[] { "Фантастика" }, genres.Grid.GetKeys().ToArray());

        // Пустой запрос снова показывает все записи
        genres.Search(string.Empty);

        Assert.AreEqual("Записей: 2", genres.WaitForCount("Записей: 2"));
    }
}
