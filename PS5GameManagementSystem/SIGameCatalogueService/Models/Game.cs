using System.ComponentModel.DataAnnotations;

namespace SIGameCatalogueService.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string Genre { get; set; } = "";

        public string Publisher { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool InStock { get; set; }
    }
}

