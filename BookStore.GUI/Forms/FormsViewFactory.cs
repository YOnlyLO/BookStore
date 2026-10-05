using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

/// <summary>
/// Реализация <see cref="IViewFactory"/> на WinForms: модальные окна открываются поверх главного окна.
/// </summary>
public sealed class FormsViewFactory : IViewFactory
{
    private readonly IWin32Window _owner;

    public FormsViewFactory(IWin32Window owner)
    {
        _owner = owner;
    }

    public IGenreEditorView CreateGenreEditor() => new GenreEditorForm { ModalOwner = _owner };

    public IBookEditorView CreateBookEditor() => new BookEditorForm { ModalOwner = _owner };

    public IUserEditorView CreateUserEditor() => new UserEditorForm { ModalOwner = _owner };

    public IOrderCreateView CreateOrderCreator() => new OrderCreateForm { ModalOwner = _owner };

    public IOrderEditorView CreateOrderEditor() => new OrderEditorForm { ModalOwner = _owner };
}
