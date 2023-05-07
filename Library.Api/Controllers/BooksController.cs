using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet] // Admin / Regular
        public async Task<ActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{bookId}")] // Admin / Regular
        public async Task<ActionResult<Book>> GetBookById(Guid bookId)
        {
            var book = await _bookService.GetSingleBook(bookId);
            return Ok(book);
        }

        [HttpPost] // Admin 
        public async Task<ActionResult> CreateBook([FromBody] BookCreateDto book)
        {
            await _bookService.AddBook(book);
            return NoContent();
        }

        [HttpPut] // Admin
        public async Task<ActionResult> UpdateBook([FromBody] BookUpdateDto book)
        {
            await _bookService.UpdateBook(book);
            return NoContent();
        }

        [HttpDelete("{bookId}")] // Admin
        public async Task<ActionResult> DeleteBook(Guid bookId)
        {
            await _bookService.DeleteBook(bookId);
            return NoContent();
        }

        [HttpGet("search")] // admin
        public async Task<ActionResult<IEnumerable<Book>>> SearchByTerm(string? isbn, string? title, string? author, string? publisher, string? yearPublished)
        {
            var searchResult = await _bookService.Search(isbn, title, author, publisher, yearPublished);
            return Ok(searchResult);
        }

        [HttpPost("review")] // user
        public async Task<ActionResult> AddReview(ReviewCreationDto review)
        {
            await _bookService.AddReviewForBook(review);
            return NoContent();

        }
    }
}
