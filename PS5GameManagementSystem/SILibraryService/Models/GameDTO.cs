namespace SILibraryService.Models
{
    public class GameDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Genre { get; set; } = "";

        public string Publisher { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool InStock { get; set; }
    }
}
