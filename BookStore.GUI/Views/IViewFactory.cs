namespace BookStore.GUI.Views;

/// <summary>
/// Создаёт модальные окна. Презентеры получают только интерфейсы и не зависят от WinForms.
/// </summary>
public interface IViewFactory
{
    IGenreEditorView CreateGenreEditor();

    IBookEditorView CreateBookEditor();

    IUserEditorView CreateUserEditor();

    IOrderCreateView CreateOrderCreator();

    IOrderEditorView CreateOrderEditor();
}
