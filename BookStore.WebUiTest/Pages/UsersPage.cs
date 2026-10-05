using OpenQA.Selenium;

namespace BookStore.WebUiTest.Pages;

public sealed class UsersPage : ListPage
{
    private UsersPage(IWebDriver browser)
        : base(browser)
    {
    }

    public static UsersPage Open(IWebDriver browser)
    {
        var page = new UsersPage(browser);

        page.OpenSection("admin/users", "Пользователи");

        return page;
    }

    public ModalDialog OpenCreateForm() => ClickPageAction("Добавить пользователя", "Новый пользователь");

    /// <param name="fullName">Как в таблице: «Фамилия Имя».</param>
    public ModalDialog OpenEditForm(string fullName) =>
        ClickRowAction(fullName, "Изменить", "Изменение пользователя");

    public ModalDialog OpenDeleteDialog(string fullName) =>
        ClickRowAction(fullName, "Удалить", "Удаление пользователя");
}
