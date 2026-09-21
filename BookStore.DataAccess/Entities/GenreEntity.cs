namespace BookStore.DataAccess.Entities;

public class GenreEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<BookEntity> Books { get; set; }
        = new List<BookEntity>();
}