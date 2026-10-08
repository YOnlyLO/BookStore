using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Окно сообщения приложения (форма MessageDialog — замена MessageBox в тёмной теме):
/// либо сообщение с кнопкой «Понятно», либо вопрос с кнопками подтверждения и «Отмена».
/// </summary>
public sealed class MessageDialog
{
    private static readonly By Locator = Locators.Window("MessageDialog");

    private readonly WindowsDriver _app;

    private readonly AppiumElement _window;

    private MessageDialog(WindowsDriver app, AppiumElement window)
    {
        _app = app;
        _window = window;

        Title = Find("titleLabel").Text;
    }

    public string Title { get; }

    public string Message => Find("messageLabel").Text;

    /// <summary>Текст кнопки подтверждения: «Понятно», «Удалить», «Отменить заказ».</summary>
    public string ConfirmText => Find("confirmButton").Text;

    /// <summary>Есть ли кнопка «Отмена» (в простом сообщении она скрыта).</summary>
    public bool HasCancel => _window.Has("cancelButton");

    public bool IsOpen => _app.FindElements(Locator).Count > 0;

    public static MessageDialog WaitFor(WindowsDriver app)
    {
        var window = Waits.Until(
            app,
            _ => app.FindElement(Locator),
            "Не открылось окно сообщения");

        return new MessageDialog(app, window);
    }

    public void Confirm()
    {
        Find("confirmButton").Click();
        WaitUntilClosed();
    }

    public void Cancel()
    {
        Find("cancelButton").Click();
        WaitUntilClosed();
    }

    private void WaitUntilClosed()
    {
        Waits.Until(_app, _ => !IsOpen, $"Окно «{Title}» не закрылось");
    }

    private AppiumElement Find(string automationId)
    {
        return _window.FindElement(Locators.Id(automationId));
    }
}
