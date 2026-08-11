using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SILibraryService.Data;
using SILibraryService.Models;

namespace SILibraryService.Controllers
{

    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public LibraryController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/library
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameLibrary>>> GetLibrary()
        {
            return await _context.GameLibraries.ToListAsync();
        }

        // GET: api/library/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<GameLibrary>>> GetUserLibrary(
            string username)
        {
            var games = await _context.GameLibraries
                .Where(g => g.UserName == username)
                .ToListAsync();

            return Ok(games);
        }

        // DELETE: api/library/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveGame(int id)
        {
            var game = await _context.GameLibraries.FindAsync(id);

            if (game == null)
            {
                return NotFound(new
                {
                    message = "Game not found in library."
                });
            }

            _context.GameLibraries.Remove(game);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
