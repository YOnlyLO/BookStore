using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Views;

public interface IOrderCreateView : IEditorView
{
    Guid? UserId { get; }

    void SetUsers(IReadOnlyList<LookupItem> users);
}
