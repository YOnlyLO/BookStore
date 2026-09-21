using BookStore.API.DTOs.Books;
using BookStore.API.Mappings;
using BookStore.Core.Models;
using BookStore.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var books = await _bookService.GetAllAsync(cancellationToken);

        return Ok(books.Select(book => book.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var book = await _bookService.GetByIdAsync(id, cancellationToken);

        if (book is null)
            return NotFound();

        return Ok(book.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create(
        CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _bookService.CreateAsync(
            request.Title,
            request.Author,
            request.Description,
            request.Price,
            request.StockQuantity,
            request.GenreId,
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
        UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await _bookService.GetByIdAsync(id, cancellationToken);

        if (book is null)
            return NotFound();

        var titleResult = book.ChangeTitle(request.Title);
        if (titleResult.IsFailure)
            return BadRequest(titleResult.Error);

        var authorResult = book.ChangeAuthor(request.Author);
        if (authorResult.IsFailure)
            return BadRequest(authorResult.Error);

        var descriptionResult = book.ChangeDescription(request.Description);
        if (descriptionResult.IsFailure)
            return BadRequest(descriptionResult.Error);

        var priceResult = book.ChangePrice(request.Price);
        if (priceResult.IsFailure)
            return BadRequest(priceResult.Error);

        var genreResult = book.ChangeGenre(request.GenreId);
        if (genreResult.IsFailure)
            return BadRequest(genreResult.Error);

        var result = await _bookService.UpdateAsync(book, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _bookService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return NoContent();
    }
}
