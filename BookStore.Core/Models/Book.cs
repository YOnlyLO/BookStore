using CSharpFunctionalExtensions;

namespace BookStore.Core.Models;

public class Book
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Author { get; private set; }

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public int StockQuantity { get; private set; }

    public Guid GenreId { get; private set; }

    private Book(
        string title,
        string author,
        string? description,
        decimal price,
        int stockQuantity,
        Guid genreId)
    {
        Id = Guid.NewGuid();

        Title = title;
        Author = author;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        GenreId = genreId;
    }

    private Book(
        Guid id,
        string title,
        string author,
        string? description,
        decimal price,
        int stockQuantity,
        Guid genreId)
    {
        Id = id;
        Title = title;
        Author = author;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        GenreId = genreId;
    }

    public static Result<Book> Create(
        string title,
        string author,
        string? description,
        decimal price,
        int stockQuantity,
        Guid genreId)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<Book>(
                "Название книги не может быть пустым.");

        if (title.Length > 200)
            return Result.Failure<Book>(
                "Название книги не может содержать более 200 символов.");

        if (string.IsNullOrWhiteSpace(author))
            return Result.Failure<Book>(
                "Автор книги не может быть пустым.");

        if (author.Length > 200)
            return Result.Failure<Book>(
                "Имя автора не может содержать более 200 символов.");

        if (description is not null && description.Length > 2000)
            return Result.Failure<Book>(
                "Описание книги не может содержать более 2000 символов.");

        if (price <= 0)
            return Result.Failure<Book>(
                "Цена книги должна быть больше нуля.");

        if (stockQuantity < 0)
            return Result.Failure<Book>(
                "Количество книг на складе не может быть отрицательным.");

        if (genreId == Guid.Empty)
            return Result.Failure<Book>(
                "Идентификатор жанра не может быть пустым.");

        return Result.Success(
            new Book(
                title,
                author,
                description,
                price,
                stockQuantity,
                genreId));
    }

    public Result ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure(
                "Название книги не может быть пустым.");

        if (title.Length > 200)
            return Result.Failure(
                "Название книги не может содержать более 200 символов.");

        Title = title;

        return Result.Success();
    }

    public Result ChangeAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            return Result.Failure(
                "Автор книги не может быть пустым.");

        if (author.Length > 200)
            return Result.Failure(
                "Имя автора не может содержать более 200 символов.");

        Author = author;

        return Result.Success();
    }

    public Result ChangeDescription(string? description)
    {
        if (description is not null && description.Length > 2000)
            return Result.Failure(
                "Описание книги не может содержать более 2000 символов.");

        Description = description;

        return Result.Success();
    }

    public Result ChangePrice(decimal price)
    {
        if (price <= 0)
            return Result.Failure(
                "Цена книги должна быть больше нуля.");

        Price = price;

        return Result.Success();
    }

    public Result ChangeGenre(Guid genreId)
    {
        if (genreId == Guid.Empty)
            return Result.Failure(
                "Идентификатор жанра не может быть пустым.");

        GenreId = genreId;

        return Result.Success();
    }

    public Result IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(
                "Количество для увеличения остатка должно быть больше нуля.");

        StockQuantity += quantity;

        return Result.Success();
    }

    public Result DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(
                "Количество для уменьшения остатка должно быть больше нуля.");

        if (quantity > StockQuantity)
            return Result.Failure(
                "Нельзя уменьшить остаток больше текущего количества книг.");

        StockQuantity -= quantity;

        return Result.Success();
    }

    public static Result<Book> Restore(
        Guid id,
        string title,
        string author,
        string? description,
        decimal price,
        int stockQuantity,
        Guid genreId)
    {
    if (id == Guid.Empty)
        return Result.Failure<Book>("Book id cannot be empty.");

    if (string.IsNullOrWhiteSpace(title))
        return Result.Failure<Book>("Book title cannot be empty.");

    if (title.Length > 200)
        return Result.Failure<Book>("Book title cannot exceed 200 characters.");

    if (string.IsNullOrWhiteSpace(author))
        return Result.Failure<Book>("Book author cannot be empty.");

    if (author.Length > 200)
        return Result.Failure<Book>("Book author cannot exceed 200 characters.");

    if (description is not null && description.Length > 2000)
        return Result.Failure<Book>("Book description cannot exceed 2000 characters.");

    if (price <= 0)
        return Result.Failure<Book>("Book price must be greater than zero.");

    if (stockQuantity < 0)
        return Result.Failure<Book>("Book stock quantity cannot be negative.");

    if (genreId == Guid.Empty)
        return Result.Failure<Book>("Genre id cannot be empty.");

    return Result.Success(
        new Book(
            id,
            title,
            author,
            description,
            price,
            stockQuantity,
            genreId));
    }
}