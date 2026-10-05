using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

public sealed class BooksPage : ListPage
{
    private BooksPage(IWebDriver browser)
        : base(browser)
    {
    }

    public static BooksPage Open(IWebDriver browser)
    {
        var page = new BooksPage(browser);

        page.OpenSection("admin/books", "Книги");

        return page;
    }

    public ModalDialog OpenCreateForm() => ClickPageAction("Добавить книгу", "Новая книга");

    public ModalDialog OpenEditForm(string title) => ClickRowAction(title, "Изменить", "Изменение книги");

    public ModalDialog OpenDeleteDialog(string title) => ClickRowAction(title, "Удалить", "Удаление книги");
}
