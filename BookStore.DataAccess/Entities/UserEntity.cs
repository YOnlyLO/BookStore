namespace BookStore.DataAccess.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public ICollection<OrderEntity> Orders { get; set; }
        = new List<OrderEntity>();
}