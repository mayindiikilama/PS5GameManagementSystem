using System.Net.Http.Json;

const string gameCatalogUrl = "https://localhost:7201";
const string libraryServiceUrl = "https://localhost:7087";

using var httpClient = new HttpClient();

while (true)
{
    Console.Clear();

    Console.WriteLine("====================================");
    Console.WriteLine("       PS5 GAME MANAGEMENT SYSTEM");
    Console.WriteLine("====================================");
    Console.WriteLine("1. View Games");
    Console.WriteLine("2. Purchase Game");
    Console.WriteLine("3. View My Library");
    Console.WriteLine("4. Remove Game From Library");
    Console.WriteLine("5. Exit");
    Console.WriteLine("====================================");

    Console.Write("Select an option: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await ViewGames();
            break;

        case "2":
            await PurchaseGame();
            break;

        case "3":
            await ViewLibrary();
            break;

        case "4":
            await RemoveGame();
            break;

        case "5":
            return;

        default:
            Console.WriteLine("Invalid option.");
            Pause();
            break;
    }
}

async Task ViewGames()
{
    try
    {
        var games = await httpClient.GetFromJsonAsync<List<GameDTO>>(
            $"{gameCatalogUrl}/api/games");

        Console.Clear();

        Console.WriteLine("========== PS5 GAMES ==========");
        Console.WriteLine();

        if (games == null || games.Count == 0)
        {
            Console.WriteLine("No games available.");
            Pause();
            return;
        }

        foreach (var game in games)
        {
            Console.WriteLine($"ID:          {game.Id}");
            Console.WriteLine($"Title:       {game.Title}");
            Console.WriteLine($"Genre:       {game.Genre}");
            Console.WriteLine($"Publisher:   {game.Publisher}");
            Console.WriteLine($"Price:       ${game.Price:F2}");
            Console.WriteLine($"Release:     {game.ReleaseDate:yyyy-MM-dd}");
            Console.WriteLine($"In Stock:    {(game.InStock ? "Yes" : "No")}");
            Console.WriteLine("--------------------------------");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Pause();
}

async Task PurchaseGame()
{
    try
    {
        Console.Clear();

        Console.WriteLine("========== PURCHASE GAME ==========");
        Console.WriteLine();

        Console.Write("Enter your username: ");
        var username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            Pause();
            return;
        }

        Console.Write("Enter Game ID: ");

        if (!int.TryParse(Console.ReadLine(), out int gameId))
        {
            Console.WriteLine("Invalid Game ID.");
            Pause();
            return;
        }

        var purchaseRequest = new PurchaseRequest
        {
            UserName = username,
            GameId = gameId
        };

        var response = await httpClient.PostAsJsonAsync(
            $"{libraryServiceUrl}/api/library/purchase",
            purchaseRequest);

        if (response.IsSuccessStatusCode)
        {
            var purchasedGame =
                await response.Content.ReadFromJsonAsync<LibraryGameDTO>();

            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine("       PURCHASE SUCCESSFUL!");
            Console.WriteLine("====================================");

            Console.WriteLine($"Game:  {purchasedGame?.GameTitle}");
            Console.WriteLine($"Price: ${purchasedGame?.Price:F2}");
            Console.WriteLine($"User:  {purchasedGame?.UserName}");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();

            Console.WriteLine();
            Console.WriteLine("Purchase failed.");
            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Message: {error}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Pause();
}

async Task ViewLibrary()
{
    try
    {
        Console.Clear();

        Console.WriteLine("========== MY LIBRARY ==========");
        Console.WriteLine();

        Console.Write("Enter your username: ");

        var username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            Pause();
            return;
        }

        var games = await httpClient.GetFromJsonAsync<List<LibraryGameDTO>>(
            $"{libraryServiceUrl}/api/library/{username}");

        if (games == null || games.Count == 0)
        {
            Console.WriteLine("Your library is empty.");
            Pause();
            return;
        }

        foreach (var game in games)
        {
            Console.WriteLine($"Library ID:   {game.Id}");
            Console.WriteLine($"Game:         {game.GameTitle}");
            Console.WriteLine($"Price:        ${game.Price:F2}");
            Console.WriteLine($"Purchase:     {game.PurchaseDate:yyyy-MM-dd}");
            Console.WriteLine("--------------------------------");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Pause();
}

async Task RemoveGame()
{
    try
    {
        Console.Clear();

        Console.WriteLine("====== REMOVE GAME FROM LIBRARY ======");
        Console.WriteLine();

        Console.Write("Enter Library Game ID: ");

        if (!int.TryParse(Console.ReadLine(), out int libraryGameId))
        {
            Console.WriteLine("Invalid ID.");
            Pause();
            return;
        }

        var response = await httpClient.DeleteAsync(
            $"{libraryServiceUrl}/api/library/{libraryGameId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Game successfully removed from library.");
        }
        else
        {
            Console.WriteLine($"Failed to remove game: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Pause();
}

void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press ENTER to continue...");
    Console.ReadLine();
}

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

public class PurchaseRequest
{
    public string UserName { get; set; } = "";

    public int GameId { get; set; }
}

public class LibraryGameDTO
{
    public int Id { get; set; }

    public string UserName { get; set; } = "";

    public int GameId { get; set; }

    public string GameTitle { get; set; } = "";

    public decimal Price { get; set; }

    public DateTime PurchaseDate { get; set; }
}
