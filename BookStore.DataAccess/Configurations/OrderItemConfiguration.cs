using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.DataAccess.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(item => item.Book)
            .WithMany(book => book.OrderItems)
            .HasForeignKey(item => item.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(item => item.BookId);

        builder.HasIndex(item => new
        {
            item.OrderId,
            item.BookId
        })
        .IsUnique();
    }
}