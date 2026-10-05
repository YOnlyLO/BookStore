using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

/// <summary>
/// Главный презентер: создаёт презентеры разделов и переключает их по команде из меню.
/// </summary>
public sealed class MainPresenter
{
    private readonly IMainView _view;
    private readonly IReadOnlyDictionary<AppSection, ISectionPresenter> _sections;

    public MainPresenter(
        IMainView view,
        IViewFactory viewFactory,
        IGenreService genreService,
        IBookService bookService,
        IUserService userService,
        IOrderService orderService,
        string apiAddress)
    {
        _view = view;

        var dashboard = new DashboardPresenter(
            view.Dashboard, genreService, bookService, userService, orderService);

        dashboard.SectionRequested += (_, section) => Navigate(section);

        _sections = new Dictionary<AppSection, ISectionPresenter>
        {
            [AppSection.Dashboard] = dashboard,
            [AppSection.Genres] = new GenresPresenter(view.Genres, viewFactory, genreService, bookService),
            [AppSection.Books] = new BooksPresenter(view.Books, viewFactory, bookService, genreService, orderService),
            [AppSection.Users] = new UsersPresenter(view.Users, viewFactory, userService, orderService),
            [AppSection.Orders] = new OrdersPresenter(view.Orders, viewFactory, orderService, userService, bookService)
        };

        _view.SetApiAddress(apiAddress);
        _view.Started += (_, _) => Navigate(AppSection.Dashboard);
        _view.SectionSelected += (_, section) => Navigate(section);
    }

    private async void Navigate(AppSection section)
    {
        _view.ShowSection(section);

        // Данные могли измениться в другом разделе — перечитываем их при каждом переходе.
        await _sections[section].ActivateAsync();
    }
}
