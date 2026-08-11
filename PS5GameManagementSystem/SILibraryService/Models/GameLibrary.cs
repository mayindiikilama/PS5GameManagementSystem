using System.ComponentModel.DataAnnotations;

namespace SILibraryService.Models
{
    public class GameLibrary
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = "";

        public int GameId { get; set; }

        public string GameTitle { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
