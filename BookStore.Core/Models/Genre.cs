using CSharpFunctionalExtensions;

namespace BookStore.Core.Models;

public class Genre
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    private Genre(string name)
    {
        Id = Guid.NewGuid();

        Name = name;
    }

    private Genre(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<Genre> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Genre>(
                "Название жанра не может быть пустым.");

        if (name.Length > 200)
            return Result.Failure<Genre>(
                "Название жанра не может содержать более 200 символов.");

        return Result.Success(
            new Genre(name));
    }

    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(
                "Название жанра не может быть пустым.");

        if (name.Length > 200)
            return Result.Failure(
                "Название жанра не может содержать более 200 символов.");

        Name = name;

        return Result.Success();
    }

    public static Result<Genre> Restore(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result.Failure<Genre>("Genre id cannot be empty.");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Genre>("Genre name cannot be empty.");

        if (name.Length > 200)
            return Result.Failure<Genre>("Genre name cannot exceed 200 characters.");

        return Result.Success(
            new Genre(id, name));
    }
}
