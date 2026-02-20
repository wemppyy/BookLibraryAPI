using BookLibraryAPI.Abstracts;
using BookLibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Create([FromBody] AuthorRequest authorRequest)
        {
            Console.WriteLine($"[DEBUG] AuthorsController.Create called with FirstName: {authorRequest?.FirstName}");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = _authorService.Add(authorRequest);
            return Ok(new { id });
        }

        [HttpPost("{id}/delete")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int id)
        {
            Console.WriteLine($"[DEBUG] AuthorsController.Delete called with id: {id}");
            if (id <= 0) return BadRequest("Invalid author ID");
            if (!_authorService.Delete(id)) return BadRequest("Service error");
            return Ok();
        }

        [HttpPost("{id}/update")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, [FromBody] AuthorRequest authorRequest)
        {
            Console.WriteLine($"[DEBUG] AuthorsController.Update called with id: {id}, FirstName: {authorRequest?.FirstName}");
            if (id <= 0) return BadRequest("Invalid author ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_authorService.Update(authorRequest, id)) return BadRequest("Service error");
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Console.WriteLine($"[DEBUG] AuthorsController.GetById called with id: {id}");
            if (id <= 0) return BadRequest("Invalid author ID");
            var author = _authorService.GetById(id);
            if (author == null) return BadRequest("Author not found");
            return Ok(author);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            Console.WriteLine("[DEBUG] AuthorsController.GetAll called");
            var authors = _authorService.GetAll();
            return Ok(authors);
        }
    }
}