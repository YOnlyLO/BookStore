using CSharpFunctionalExtensions;

namespace BookStore.Core.Models;

public class User
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    private User(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
    {
        Id = Guid.NewGuid();

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static Result<User> Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<User>(
                "Имя пользователя не может быть пустым.");

        if (firstName.Length > 100)
            return Result.Failure<User>(
                "Имя пользователя не может содержать более 100 символов.");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<User>(
                "Фамилия пользователя не может быть пустой.");

        if (lastName.Length > 100)
            return Result.Failure<User>(
                "Фамилия пользователя не может содержать более 100 символов.");

        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<User>(
                "Email пользователя не может быть пустым.");

        if (email.Length > 320)
            return Result.Failure<User>(
                "Email пользователя не может содержать более 320 символов.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure<User>(
                "Хеш пароля не может быть пустым.");

        return Result.Success(
            new User(
                firstName,
                lastName,
                email,
                passwordHash));
    }

    public Result ChangeName(
        string firstName,
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure(
                "Имя пользователя не может быть пустым.");

        if (firstName.Length > 100)
            return Result.Failure(
                "Имя пользователя не может содержать более 100 символов.");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure(
                "Фамилия пользователя не может быть пустой.");

        if (lastName.Length > 100)
            return Result.Failure(
                "Фамилия пользователя не может содержать более 100 символов.");

        FirstName = firstName;
        LastName = lastName;

        return Result.Success();
    }

    public Result ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure(
                "Email пользователя не может быть пустым.");

        if (email.Length > 320)
            return Result.Failure(
                "Email пользователя не может содержать более 320 символов.");

        Email = email;

        return Result.Success();
    }

    public Result ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure(
                "Хеш пароля не может быть пустым.");

        PasswordHash = passwordHash;

        return Result.Success();
    }
}
