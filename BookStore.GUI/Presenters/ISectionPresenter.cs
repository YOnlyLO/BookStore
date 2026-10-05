namespace BookStore.GUI.Presenters;

/// <summary>Презентер раздела, который открывается из бокового меню.</summary>
public interface ISectionPresenter
{
    /// <summary>Раздел стал видимым — презентер загружает актуальные данные.</summary>
    Task ActivateAsync();
}
