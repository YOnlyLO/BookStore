namespace BookStore.GUI.ViewModels;

public sealed record StatusStat(
    Badge Status,
    int Count);

public sealed record DashboardSummary(
    int GenreCount,
    int BookCount,
    int UserCount,
    int OrderCount,
    IReadOnlyList<StatusStat> OrderStatuses,
    decimal Revenue,
    IReadOnlyList<OrderRow> RecentOrders);
