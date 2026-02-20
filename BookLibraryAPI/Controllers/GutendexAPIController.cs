using BookLibraryAPI.Abstracts;
using BookLibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class GutendexAPIController : ControllerBase
    {
        private readonly IGutendexAPIService _gatendexAPIService;
        public GutendexAPIController(IGutendexAPIService gatendexAPIService)
        {
            _gatendexAPIService = gatendexAPIService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks(int page = 1, string search = null)
        {
            Console.WriteLine($"[DEBUG] GuntexAPIController.GetAllBooks called with page: {page}, search: {search}");
            if (page < 1) return BadRequest("Page must be greater than 0");
            
            var result = await _gatendexAPIService.GetAllBooks(page, search);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Console.WriteLine($"[DEBUG] GuntexAPIController.GetById called with id: {id}");
            if (id <= 0) return BadRequest("Invalid book ID");
            var book = await _gatendexAPIService.GetById(id);
            if (book == null) return NotFound($"Book with ID {id} not found");
            return Ok(book);
        }
    }
}
