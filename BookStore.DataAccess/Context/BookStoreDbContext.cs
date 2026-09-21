using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Context;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(
        DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<BookEntity> Books => Set<BookEntity>();

    public DbSet<GenreEntity> Genres => Set<GenreEntity>();

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BookStoreDbContext).Assembly);
    }
}
