using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

public sealed class GenresPage : ListPage
{
    private GenresPage(IWebDriver browser)
        : base(browser)
    {
    }

    public static GenresPage Open(IWebDriver browser)
    {
        var page = new GenresPage(browser);

        page.OpenSection("admin/genres", "Жанры");

        return page;
    }

    public ModalDialog OpenCreateForm() => ClickPageAction("Добавить жанр", "Новый жанр");

    public ModalDialog OpenEditForm(string name) => ClickRowAction(name, "Изменить", "Изменение жанра");

    public ModalDialog OpenDeleteDialog(string name) => ClickRowAction(name, "Удалить", "Удаление жанра");
}
