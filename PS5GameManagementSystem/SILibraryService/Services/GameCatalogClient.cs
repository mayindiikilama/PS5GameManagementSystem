using SILibraryService.Models;

namespace SILibraryService.Services
{
    public class GameCatalogClient
    {
        private readonly HttpClient _httpClient;

        public GameCatalogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GameDTO?> GetGameAsync(int gameId)
        {
            var response = await _httpClient.GetAsync($"api/games/{gameId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GameDTO>();
        }
    }
}
