using BookStore.GUITest.Infrastructure;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace BookStore.GUITest.Screens;

/// <summary>
/// Дашборд: карточки с числом записей в разделах, распределение заказов по статусам,
/// выручка и последние заказы. Карточки и строки статусов рисуются вручную —
/// их текст (например, «Жанры: 2») приложение сообщает через UI Automation.
/// </summary>
public sealed class DashboardSection
{
    private const string LoadingText = "Загрузка…";

    private readonly WindowsDriver _app;

    private readonly AppiumElement _root;

    private DashboardSection(WindowsDriver app, AppiumElement root)
    {
        _app = app;
        _root = root;
    }

    public string Title => Find("titleLabel").Text;

    /// <summary>Выручка по завершённым заказам («900,00 ₽»).</summary>
    public string Revenue => UiText.Normalize(Find("revenueLabel").Text);

    public GridElement RecentOrders => new(Find("recentGrid"));

    /// <summary>Ждёт, пока раздел появится и сводка загрузится.</summary>
    public static DashboardSection WaitFor(WindowsDriver app)
    {
        var root = Waits.Until(
            app,
            _ => app.FindElement(Locators.Id("dashboardView")),
            "Не показан дашборд");

        var dashboard = new DashboardSection(app, root);

        dashboard.WaitUntilLoaded();

        return dashboard;
    }

    /// <summary>Текст карточки раздела (genresCard, booksCard, usersCard, ordersCard): «Жанры: 2».</summary>
    public string GetCard(string cardId)
    {
        return Find(cardId).Text;
    }

    /// <summary>Строки «Заказы по статусам»: «Новый: 1», «Подтверждён: 0»…</summary>
    public IReadOnlyList<string> GetOrderStatuses()
    {
        return new[] { "statusMeter1", "statusMeter2", "statusMeter3", "statusMeter4" }
            .Select(id => Find(id).Text)
            .ToList();
    }

    /// <summary>Нажимает «Обновить» и ждёт новой сводки.</summary>
    public void Refresh()
    {
        Find("refreshButton").Click();

        WaitUntilLoaded();
    }

    /// <summary>Нажимает карточку раздела — приложение переходит в этот раздел.</summary>
    public void ClickCard(string cardId)
    {
        Find(cardId).Click();
    }

    private void WaitUntilLoaded()
    {
        Waits.Until(
            _app,
            _ => Find("refreshButton") is { Enabled: true, Text: not LoadingText },
            "Дашборд не загрузил сводку");
    }

    private AppiumElement Find(string automationId)
    {
        return _root.FindElement(Locators.Id(automationId));
    }
}
