using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace BookStore.GUITest.Infrastructure;

public static class Locators
{
    /// <summary>
    /// Контрол WinForms по имени (свойство Name, например nameTextBox): UI Automation показывает его
    /// как AutomationId, а Appium ищет по нему стратегией accessibility id — как FindElementByAccessibilityId в методичке.
    /// </summary>
    public static By Id(string automationId)
    {
        return MobileBy.AccessibilityId(automationId);
    }

    /// <summary>
    /// Окно формы по имени её класса (например, GenreEditorForm). Модальные окна в дереве UI Automation
    /// вложены в окно-владельца. Поиск ограничен элементами Window: AutomationId формы есть ещё у системного меню окна.
    /// </summary>
    public static By Window(string automationId)
    {
        return By.XPath($"//Window[@AutomationId={UiText.XPathLiteral(automationId)}]");
    }
}
