using Library.Application.Mediator.Commands;
using Library.Application.Models.In;
using Library.Application.Models.Out;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Deletes the book with a given id
    /// </summary>
    /// <param name="id">The identifier of the book to be deleted.</param>
    /// <response code="200">Deleted with success.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Book not found.</response>
    [HttpDelete("[action]/{id}")]
    [Authorize(Policy = "Librarian")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteBookCommand(id));

        return GetActionStatus(result);
    }

    /// <summary>
    /// Updates the book with a given id
    /// </summary>
    /// <param name="id">The identifier of the book to be updated.</param>
    /// <param name="BookModel">The book model with the changes.</param>
    /// <response code="200">Updated with success.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Book not found.</response>
    [HttpPatch("[action]/{id}")]
    [Authorize(Policy = "Librarian")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> Update(int id, BookModel book)
    {
        var result = await _mediator.Send(new UpdateBookCommand(id, book));

        return GetActionStatus(result);
    }

    /// <summary>
    /// Creates a given book
    /// </summary>
    /// <param name="BookModel">The book model.</param>
    /// <response code="200">Created with success.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Book or authors not found.</response>
    [HttpPost("[action]")]
    [Authorize(Policy = "Librarian")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> Create([FromBody] BookModel book)
    {
        var result = await _mediator.Send(new CreateBookCommand(book));

        return GetActionStatus(result);
    }

    /// <summary>
    /// Gets all books
    /// </summary>
    /// <returns>
    /// List of all books
    /// </returns>
    /// <response code="200">Gets all books</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="204">No books found.</response>
    [HttpGet("all")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookViewModel>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllBooksCommand());

        if (!result?.Any() ?? true)
        {
            return NoContent();
        }

        return Ok(result!.Select(book => new BookViewModel
        {
            Authors = book.Authors.Select(author => author.Author.Name),
            Title = book.Title
        }));
    }

    /// <summary>
    /// Searches for the books with the given parameters
    /// </summary>
    /// <param name="SearchModel">The search model.</param>
    /// <response code="200">All books that corresponds to the given criteria.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="204">No books found.</response>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookViewModel>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
    public async Task<IActionResult> Search([FromQuery] SearchModel request)
    {
        var result = await _mediator.Send(new SearchBookCommand(request.Title, request.Author));

        if (!result?.Any() ?? true)
        {
            return NoContent();
        }

        return Ok(result!.Select(book => new BookViewModel
        {
            Authors = book.Authors.Select(author => author.Author.Name),
            Title = book.Title
        }));
    }
}
