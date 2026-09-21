using BookStore.API.DTOs.Users;
using BookStore.API.Mappings;
using BookStore.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);

        return Ok(users.Select(user => user.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return NotFound();

        return Ok(user.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PasswordHash,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        var response = result.Value.ToResponse();

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return NotFound();

        var nameResult = user.ChangeName(
            request.FirstName,
            request.LastName);

        if (nameResult.IsFailure)
            return BadRequest(nameResult.Error);

        var emailResult = user.ChangeEmail(request.Email);

        if (emailResult.IsFailure)
            return BadRequest(emailResult.Error);

        var result = await _userService.UpdateAsync(
            user,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteAsync(
            id,
            cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return NoContent();
    }
}
