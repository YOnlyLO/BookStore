using System.Net.Mail;
using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class UserEditorPresenter : EditorPresenter<IUserEditorView>
{
    private readonly IUserService _userService;
    private readonly IReadOnlyList<User> _existingUsers;
    private readonly User? _user;

    /// <param name="user">Изменяемый пользователь; null — создание нового.</param>
    public UserEditorPresenter(
        IUserEditorView view,
        IUserService userService,
        IReadOnlyList<User> existingUsers,
        User? user)
        : base(view)
    {
        _userService = userService;
        _existingUsers = existingUsers;
        _user = user;

        View.Title = user is null ? "Новый пользователь" : "Изменение пользователя";
        View.FirstName = user?.FirstName ?? string.Empty;
        View.LastName = user?.LastName ?? string.Empty;
        View.Email = user?.Email ?? string.Empty;
        View.IsPasswordVisible = user is null;
    }

    private string FirstName => View.FirstName.Trim();

    private string LastName => View.LastName.Trim();

    private string Email => View.Email.Trim();

    protected override ValidationError? Validate()
    {
        return CheckText(nameof(View.LastName), LastName, "Фамилия", 100)
            ?? CheckText(nameof(View.FirstName), FirstName, "Имя", 100)
            ?? CheckText(nameof(View.Email), Email, "Email", 320)
            ?? CheckEmail()
            ?? CheckPassword();
    }

    private ValidationError? CheckEmail()
    {
        if (!MailAddress.TryCreate(Email, out var address) || address.Address != Email)
            return new ValidationError(nameof(View.Email), "Введите корректный адрес электронной почты.");

        // В БД на email уникальный индекс: без этой проверки API ответит ошибкой 500.
        var isDuplicate = _existingUsers.Any(user =>
            user.Id != _user?.Id &&
            string.Equals(user.Email, Email, StringComparison.OrdinalIgnoreCase));

        return isDuplicate
            ? new ValidationError(nameof(View.Email), "Пользователь с таким email уже существует.")
            : null;
    }

    private ValidationError? CheckPassword() =>
        _user is null && string.IsNullOrWhiteSpace(View.Password)
            ? new ValidationError(nameof(View.Password), "Задайте пароль пользователя.")
            : null;

    protected override async Task<Guid> SaveAsync()
    {
        if (_user is null)
        {
            var created = await _userService.CreateAsync(new CreateUserRequest(
                FirstName, LastName, Email, PasswordHasher.Hash(View.Password)));

            return created.Id;
        }

        await _userService.UpdateAsync(_user.Id, new UpdateUserRequest(
            FirstName, LastName, Email));

        return _user.Id;
    }
}
