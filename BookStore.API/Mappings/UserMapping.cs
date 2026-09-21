using BookStore.API.DTOs.Users;
using BookStore.Core.Models;

namespace BookStore.API.Mappings;

public static class UserMapping
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email);
    }
}