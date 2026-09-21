using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using BookStore.DataAccess.Context;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BookStoreDbContext _context;

    public UserRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.Id == id,
                cancellationToken);

        return entity is null
            ? null
            : MapToDomain(entity);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities
            .Select(MapToDomain)
            .ToList();
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(user);

        await _context.Users.AddAsync(
            entity,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(user);

        _context.Users.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(
                user => user.Id == id,
                cancellationToken);

        if (entity is null)
            return;

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static User MapToDomain(UserEntity entity)
    {
        var result = User.Restore(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.Email,
            entity.PasswordHash);

        return result.Value;
    }

    private static UserEntity MapToEntity(User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };
    }
}
