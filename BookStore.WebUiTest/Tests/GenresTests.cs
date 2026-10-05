using BookStore.WebUiTest.Infrastructure;
using BookStore.WebUiTest.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;

namespace BookStore.WebUiTest.Tests;

[TestClass]
public sealed class GenresTests : UiTestBase
{
    private const string RequiredError = "Обязательное поле.";

    [TestMethod]
    public void EmptyList_ShowsEmptyState()
    {
        var page = GenresPage.Open(Browser);

        Assert.AreEqual("Жанров пока нет.", page.EmptyStateText);
        Assert.AreEqual(0, page.Counter);
    }

    [TestMethod]
    public async Task LoadError_ShowsMessage_AndRetryReloadsList()
    {
        if (Browser is not ChromiumDriver)
        {
            Assert.Inconclusive("Перехват сетевых запросов (CDP) доступен только в Edge и Chrome.");
        }

        await Database.SeedGenreAsync("Фантастика");

        // Selenium 4 умеет подменять ответы сервера: запрос списка жанров получит 500
        var network = Browser.Manage().Network;
        network.AddRequestHandler(new NetworkRequestHandler
        {
            RequestMatcher = request => request.Url?.EndsWith("/api/genres", StringComparison.Ordinal) == true,
            ResponseSupplier = _ => new HttpResponseData
            {
                StatusCode = 500,
                Body = string.Empty,
                // Без CORS-заголовка браузер скрыл бы статус и показал «нет связи с сервером»
                Headers = { ["Access-Control-Allow-Origin"] = UiTestSettings.FrontendUrl.GetLeftPart(UriPartial.Authority) }
            }
        });
        await network.StartMonitoring();

        var page = GenresPage.Open(Browser);

        Assert.AreEqual("Внутренняя ошибка сервера. Подробности — в логе BookStore.API.", page.LoadErrorText);

        // Сервер «восстановился» — кнопка «Повторить» загружает список заново
        network.ClearRequestHandlers();
        page.Retry();

        Assert.IsTrue(page.HasRow("Фантастика"));

        await network.StopMonitoring();
    }

    [TestMethod]
    public async Task List_ShowsGenresSortedByNameWithBookCounts()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedGenreAsync("Детектив");
        await Database.SeedBookAsync(fiction, "Солярис");
        await Database.SeedBookAsync(fiction, "Эдем");

        var page = GenresPage.Open(Browser);

        Assert.AreEqual(2, page.Counter);
        CollectionAssert.AreEqual(new[] { "Детектив", "0" }, page.GetRows()[0].Take(2).ToArray());
        CollectionAssert.AreEqual(new[] { "Фантастика", "2" }, page.GetRows()[1].Take(2).ToArray());
    }

    [TestMethod]
    public void Create_AddsGenreToTable()
    {
        var page = GenresPage.Open(Browser);

        // Открытие формы, ввод названия и нажатие «Создать»
        var form = page.OpenCreateForm();
        form.Fill("name", "Фантастика");
        form.Submit();

        // Форма закрывается, а жанр появляется в таблице
        form.WaitUntilClosed();
        CollectionAssert.AreEqual(new[] { "Фантастика", "0" }, page.GetRow("Фантастика").Take(2).ToArray());
        Assert.AreEqual(1, page.Counter);
    }

    [TestMethod]
    public void Create_TrimsName()
    {
        var page = GenresPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("name", "  Фантастика  ");
        form.Submit();

        form.WaitUntilClosed();
        Assert.IsTrue(page.HasRow("Фантастика"));
    }

    [TestMethod]
    public void Create_EmptyName_ShowsRequiredError()
    {
        var page = GenresPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Submit();

        // Запрос к API не отправляется: форма остаётся открытой с ошибкой под полем
        Assert.AreEqual(RequiredError, form.FieldError("name"));
        Assert.IsTrue(form.IsOpen);

        form.Close();
        form.WaitUntilClosed();
        Assert.AreEqual("Жанров пока нет.", page.EmptyStateText);
    }

    [TestMethod]
    public void Create_WhitespaceName_ShowsRequiredError()
    {
        var page = GenresPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("name", "   ");
        form.Submit();

        Assert.AreEqual(RequiredError, form.FieldError("name"));
    }

    [TestMethod]
    public void Create_NameLongerThan200Characters_ShowsLengthError()
    {
        var page = GenresPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("name", new string('Ж', 201));
        form.Submit();

        Assert.AreEqual("Не более 200 символов.", form.FieldError("name"));
    }

    [TestMethod]
    public async Task Create_DuplicateName_ShowsError()
    {
        await Database.SeedGenreAsync("Фантастика");

        var page = GenresPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("name", "Фантастика");
        form.Submit();

        Assert.AreEqual("Жанр «Фантастика» уже существует.", form.ErrorAlert);
        Assert.AreEqual(1, page.Counter);
    }

    [TestMethod]
    public void Create_Cancel_DoesNotAddGenre()
    {
        var page = GenresPage.Open(Browser);

        var form = page.OpenCreateForm();
        form.Fill("name", "Фантастика");
        form.Click("Отмена");

        form.WaitUntilClosed();
        Assert.AreEqual("Жанров пока нет.", page.EmptyStateText);
    }

    [TestMethod]
    public async Task Edit_RenamesGenre()
    {
        await Database.SeedGenreAsync("Фантастика");

        var page = GenresPage.Open(Browser);

        // Форма изменения открывается с текущим названием
        var form = page.OpenEditForm("Фантастика");
        Assert.AreEqual("Фантастика", form.GetValue("name"));

        form.Fill("name", "Научная фантастика");
        form.Submit();

        form.WaitUntilClosed();
        page.WaitForRow("Научная фантастика");
        Assert.IsFalse(page.HasRow("Фантастика"));
        Assert.AreEqual(1, page.Counter);
    }

    [TestMethod]
    public async Task Delete_RemovesGenre()
    {
        await Database.SeedGenreAsync("Фантастика");

        var page = GenresPage.Open(Browser);

        var dialog = page.OpenDeleteDialog("Фантастика");
        Assert.AreEqual("Удалить жанр «Фантастика»? Это действие нельзя отменить.", dialog.Message);

        dialog.Click("Удалить");

        dialog.WaitUntilClosed();
        page.WaitForRowToDisappear("Фантастика");
        Assert.AreEqual("Жанров пока нет.", page.EmptyStateText);
    }

    [TestMethod]
    public async Task Delete_GenreWithBooks_IsBlocked()
    {
        var fiction = await Database.SeedGenreAsync("Фантастика");
        await Database.SeedBookAsync(fiction);

        var page = GenresPage.Open(Browser);

        var dialog = page.OpenDeleteDialog("Фантастика");

        // Интерфейс заранее объясняет, почему удалить нельзя, и блокирует кнопку
        Assert.AreEqual(
            "Книг в этом жанре: 1. Перенесите их в другой жанр или удалите — " +
            "пока на жанр ссылаются книги, сервер не даст его удалить.",
            dialog.WarningAlert);
        Assert.IsFalse(dialog.Button("Удалить").Enabled);
    }
}
