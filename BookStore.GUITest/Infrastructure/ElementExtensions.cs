using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace BookStore.GUITest.Infrastructure;

public static class ElementExtensions
{
    /// <summary>
    /// Значение элемента из шаблона UI Automation Value. Нужно для ячеек таблицы: их Name — это
    /// «колонка + строка» («Название Строка 1»), а текст ячейки доступен только как Value.
    /// </summary>
    public static string GetValue(this AppiumElement element)
    {
        var value = ((IJavaScriptExecutor)element.WrappedDriver).ExecuteScript("windows: getValue", element);

        return UiText.Normalize(value as string);
    }

    /// <summary>
    /// Заменяет текст поля: очищает его и вводит новый текст с клавиатуры, как это сделал бы пользователь.
    /// </summary>
    public static void ReplaceText(this AppiumElement field, string text)
    {
        field.Clear();

        if (text.Length > 0)
        {
            field.SendKeys(text);
        }
    }

    /// <summary>
    /// Выбирает пункт выпадающего списка (ComboBox), название которого начинается с <paramref name="itemText"/>:
    /// первый щелчок раскрывает список, второй выбирает пункт.
    /// </summary>
    public static void SelectItem(this AppiumElement comboBox, string itemText)
    {
        comboBox.Click();

        var item = Waits.Until(
            comboBox.WrappedDriver,
            _ => comboBox.FindElement(By.XPath($".//ListItem[starts-with(@Name, {UiText.XPathLiteral(itemText)})]")),
            $"В раскрывшемся списке нет пункта «{itemText}»");

        item.Click();
    }

    /// <summary>Есть ли внутри элемента потомок с указанным AutomationId (скрытые контролы в дереве отсутствуют).</summary>
    public static bool Has(this ISearchContext context, string automationId)
    {
        return context.FindElements(Locators.Id(automationId)).Count > 0;
    }
}
