using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Views;

/// <summary>Главное окно: боковое меню и рабочая область с представлениями разделов.</summary>
public interface IMainView
{
    /// <summary>Окно показано пользователю — можно загружать данные.</summary>
    event EventHandler Started;

    event EventHandler<AppSection> SectionSelected;

    IDashboardView Dashboard { get; }

    IRecordListView Genres { get; }

    IRecordListView Books { get; }

    IRecordListView Users { get; }

    IRecordListView Orders { get; }

    void ShowSection(AppSection section);

    void SetApiAddress(string address);
}
