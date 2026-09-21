using BookStore.API.DTOs.Genres;
using BookStore.API.Mappings;
using BookStore.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly GenreService _genreService;

    public GenresController(GenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GenreResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var genres = await _genreService.GetAllAsync(cancellationToken);

        return Ok(genres.Select(genre => genre.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GenreResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var genre = await _genreService.GetByIdAsync(id, cancellationToken);

        if (genre is null)
            return NotFound();

        return Ok(genre.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<GenreResponse>> Create(
        CreateGenreRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _genreService.CreateAsync(
            request.Name,
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
        UpdateGenreRequest request,
        CancellationToken cancellationToken)
    {
        var genre = await _genreService.GetByIdAsync(id, cancellationToken);

        if (genre is null)
            return NotFound();

        var renameResult = genre.Rename(request.Name);

        if (renameResult.IsFailure)
            return BadRequest(renameResult.Error);

        var result = await _genreService.UpdateAsync(
            genre,
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
        var result = await _genreService.DeleteAsync(
            id,
            cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return NoContent();
    }
}
