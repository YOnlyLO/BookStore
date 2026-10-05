namespace BookStore.GUI.Views;

public interface IUserEditorView : IEditorView
{
    string FirstName { get; set; }

    string LastName { get; set; }

    string Email { get; set; }

    string Password { get; }

    /// <summary>Пароль задаётся только при создании: API не меняет его при обновлении.</summary>
    bool IsPasswordVisible { set; }
}
