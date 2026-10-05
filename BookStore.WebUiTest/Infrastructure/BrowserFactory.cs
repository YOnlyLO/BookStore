using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Создаёт WebDriver для выбранного браузера. Драйвер (msedgedriver, chromedriver, geckodriver)
/// скачивать не нужно: Selenium Manager сам подбирает версию под установленный браузер.
/// </summary>
public static class BrowserFactory
{
    private const int WindowWidth = 1440;

    private const int WindowHeight = 900;

    public static IWebDriver Create()
    {
        IWebDriver driver = UiTestSettings.Browser switch
        {
            "edge" => new EdgeDriver(Configure(new EdgeOptions())),
            "chrome" => new ChromeDriver(Configure(new ChromeOptions())),
            "firefox" => new FirefoxDriver(Configure(new FirefoxOptions())),
            var other => throw new NotSupportedException(
                $"Браузер «{other}» не поддерживается. Укажите в {UiTestSettings.BrowserVariable} edge, chrome или firefox.")
        };

        return driver;
    }

    private static TOptions Configure<TOptions>(TOptions options)
        where TOptions : ChromiumOptions
    {
        if (UiTestSettings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArgument($"--window-size={WindowWidth},{WindowHeight}");

        return options;
    }

    private static FirefoxOptions Configure(FirefoxOptions options)
    {
        if (UiTestSettings.Headless)
        {
            options.AddArgument("-headless");
        }

        options.AddArgument($"-width={WindowWidth}");
        options.AddArgument($"-height={WindowHeight}");

        return options;
    }
}
