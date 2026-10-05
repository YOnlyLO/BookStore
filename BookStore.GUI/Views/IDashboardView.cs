using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Views;

public interface IDashboardView
{
    event EventHandler RefreshRequested;

    /// <summary>Пользователь нажал на карточку раздела.</summary>
    event EventHandler<AppSection> SectionRequested;

    void SetLoading(bool isLoading);

    void ShowSummary(DashboardSummary summary);

    /// <summary>Показывает сообщение об ошибке над карточками; null — скрывает его.</summary>
    void ShowError(string? message);
}
