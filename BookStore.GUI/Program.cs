using System.Globalization;
using BookStore.GUI.Forms;
using BookStore.GUI.Presenters;
using BookStore.GUI.Services;

namespace BookStore.GUI;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Цены в рублях и даты в русском формате независимо от настроек Windows.
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");

        ApplicationConfiguration.Initialize();
        Application.SetColorMode(SystemColorMode.Dark);
        Application.ThreadException += (_, e) =>
            MessageDialog.ShowAlert(Form.ActiveForm, "Непредвиденная ошибка", e.Exception.Message);

        var settings = AppSettings.Load();

        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri(settings.Api.BaseUrl),
            Timeout = TimeSpan.FromSeconds(settings.Api.TimeoutSeconds)
        };

        // Компоновка MVP: модель (сервисы API) → представление (формы) → презентеры.
        var apiClient = new ApiClient(httpClient);
        var mainForm = new MainForm();

        _ = new MainPresenter(
            mainForm,
            new FormsViewFactory(mainForm),
            new GenreService(apiClient),
            new BookService(apiClient),
            new UserService(apiClient),
            new OrderService(apiClient),
            settings.Api.BaseUrl);

        Application.Run(mainForm);
    }
}
