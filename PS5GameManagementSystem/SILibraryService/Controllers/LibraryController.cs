using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SILibraryService.Data;
using SILibraryService.Models;
using SILibraryService.Services;

namespace SILibraryService.Controllers
{

    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly GameCatalogClient _gameCatalogClient;

        public LibraryController(LibraryDbContext context, GameCatalogClient gameCatalogClient)
        {
            _context = context;
            _gameCatalogClient = gameCatalogClient;
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

        // POST: api/library/purchase
        [HttpPost("purchase")]
        public async Task<ActionResult<GameLibrary>> PurchaseGame(
            PurchaseRequest request)
        {
            // Call Game Catalog Service
            var game = await _gameCatalogClient.GetGameAsync(request.GameId);

            // Game doesn't exist
            if (game == null)
            {
                return NotFound(new
                {
                    message = "Game was not found in the Game Catalogue."
                });
            }

            // Game isn't available
            if (!game.InStock)
            {
                return BadRequest(new
                {
                    message = "Game is currently out of stock."
                });
            }

            // Check if user already owns the game
            var existingGame = await _context.GameLibraries
                .FirstOrDefaultAsync(g =>
                    g.UserName == request.UserName &&
                    g.GameId == request.GameId);

            if (existingGame != null)
            {
                return Conflict(new
                {
                    message = "User already owns this game."
                });
            }

            // Create library record
            var libraryGame = new GameLibrary()
            {
                UserName = request.UserName,
                GameId = game.Id,
                GameTitle = game.Title,
                Price = game.Price,
                PurchaseDate = DateTime.UtcNow
            };

            _context.GameLibraries.Add(libraryGame);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUserLibrary),
                new { username = request.UserName },
                libraryGame);
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
