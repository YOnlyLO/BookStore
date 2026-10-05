using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class DashboardPresenter : ISectionPresenter
{
    private const int RecentOrderCount = 6;

    private readonly IDashboardView _view;
    private readonly IGenreService _genreService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    private bool _isLoading;

    public DashboardPresenter(
        IDashboardView view,
        IGenreService genreService,
        IBookService bookService,
        IUserService userService,
        IOrderService orderService)
    {
        _view = view;
        _genreService = genreService;
        _bookService = bookService;
        _userService = userService;
        _orderService = orderService;

        _view.RefreshRequested += async (_, _) => await ActivateAsync();
        _view.SectionRequested += (_, section) => SectionRequested?.Invoke(this, section);
    }

    /// <summary>Пользователь выбрал раздел на дашборде — переход выполняет главный презентер.</summary>
    public event EventHandler<AppSection>? SectionRequested;

    public async Task ActivateAsync()
    {
        if (_isLoading)
            return;

        _isLoading = true;
        _view.ShowError(null);
        _view.SetLoading(true);

        try
        {
            var genresTask = _genreService.GetAllAsync();
            var booksTask = _bookService.GetAllAsync();
            var usersTask = _userService.GetAllAsync();
            var ordersTask = _orderService.GetAllAsync();

            await Task.WhenAll(genresTask, booksTask, usersTask, ordersTask);

            _view.ShowSummary(BuildSummary(
                genresTask.Result,
                booksTask.Result,
                usersTask.Result,
                ordersTask.Result));
        }
        catch (ApiException exception)
        {
            _view.ShowError(exception.Message);
        }
        finally
        {
            _view.SetLoading(false);
            _isLoading = false;
        }
    }

    private static DashboardSummary BuildSummary(
        IReadOnlyList<Genre> genres,
        IReadOnlyList<Book> books,
        IReadOnlyList<User> users,
        IReadOnlyList<Order> orders)
    {
        var statuses = Enum.GetValues<OrderStatus>()
            .Select(status => new StatusStat(
                OrderPresentation.StatusBadge(status),
                orders.Count(order => order.Status == status)))
            .ToList();

        var revenue = orders
            .Where(order => order.Status == OrderStatus.Completed)
            .Sum(order => order.TotalPrice);

        var userNames = OrderPresentation.UserNames(users);

        var recentOrders = orders
            .OrderByDescending(order => order.CreatedAt)
            .Take(RecentOrderCount)
            .Select(order => OrderPresentation.ToRow(order, userNames))
            .ToList();

        return new DashboardSummary(
            genres.Count,
            books.Count,
            users.Count,
            orders.Count,
            statuses,
            revenue,
            recentOrders);
    }
}
