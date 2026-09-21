using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.DataAccess.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(book => book.Author)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(book => book.Description)
            .HasMaxLength(2000);

        builder.Property(book => book.Price)
            .HasPrecision(18, 2);

        builder.Property(book => book.StockQuantity)
            .IsRequired();

        builder.HasOne(book => book.Genre)
            .WithMany(genre => genre.Books)
            .HasForeignKey(book => book.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(book => book.GenreId);
    }
}