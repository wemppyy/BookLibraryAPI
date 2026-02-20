using BookLibraryAPI.Abstracts;
using BookLibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryAPI.Controllers
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

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Create([FromBody] BookRequest bookRequest)
        {
            Console.WriteLine($"[DEBUG] BooksController.Create called with Title: {bookRequest?.Title}");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = _bookService.Add(bookRequest);
            if (id == 0) return BadRequest("Service error");
            return Ok(new { id });
        }

        [HttpPost("{id}/delete")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int id)
        {
            Console.WriteLine($"[DEBUG] BooksController.Delete called with id: {id}");
            if (id <= 0) return BadRequest("Invalid book ID");
            if (!_bookService.Delete(id)) return BadRequest("Service error");
            return Ok();
        }

        [HttpPost("{id}/update")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, [FromBody] BookRequest bookRequest)
        {
            Console.WriteLine($"[DEBUG] BooksController.Update called with id: {id}, Title: {bookRequest?.Title}");
            if (id <= 0) return BadRequest("Invalid book ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_bookService.Update(bookRequest, id)) return BadRequest("Service error");
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Console.WriteLine($"[DEBUG] BooksController.GetById called with id: {id}");
            if (id <= 0) return BadRequest("Invalid book ID");
            var book = _bookService.GetById(id);
            if (book == null) return BadRequest("Book not found");
            return Ok(book);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            Console.WriteLine("[DEBUG] BooksController.GetAll called");
            var books = _bookService.GetAll();
            return Ok(books);
        }
    }
}
