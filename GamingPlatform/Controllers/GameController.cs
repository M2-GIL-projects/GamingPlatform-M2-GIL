using GamingPlatform.Models;
using GamingPlatform.Data;
using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatform.Controllers
{
    public class GameController : Controller
    {
        private readonly GameService _gameService;
        private readonly LobbyService _lobbyService;
        private readonly GamingPlatformContext _context;

        // Constructeur avec injection de dépendances
        public GameController(GameService gameService, LobbyService lobbyService, GamingPlatformContext context)
        {
            _gameService = gameService;
            _lobbyService = lobbyService;
            _context = context;
        }

        // Page d'accueil des jeux disponibles
        public IActionResult Index()
        {
            var games = _gameService.GetAvailableGames(); // Implémentez GetAvailableGames dans GameService
            return View(games);
        }

        // Affiche les détails d'un jeu en fonction de son ID
        public async Task<IActionResult> Details(Guid id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound("Le jeu spécifié n'existe pas.");
            }

            var lobbies = await _lobbyService.GetLobbiesByGameAsync(game.Code);

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Lobbies = lobbies
            };

            return View(viewModel);
        }

        // Affiche les lobbies d'un jeu en fonction de son code
        public async Task<IActionResult> LobbiesByGameCode(string gameCode)
        {
            var game = _gameService.GetGameByCode(gameCode);
            if (game == null)
            {
                return NotFound("Le jeu spécifié n'existe pas.");
            }

            var lobbies = await _lobbyService.GetLobbiesByGameAsync(gameCode);

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Lobbies = lobbies
            };

            return View(viewModel);
        }

        // Action pour jouer à un jeu
       [HttpGet]
//public IActionResult Play(Guid id)
//{
    // Charger les données nécessaires pour le jeu
    //var lobby = _context.Lobby // Utilisez le nom correct ici
      //  .Include(l => l.Game)
        //.Include(l => l.LobbyPlayers)
        //.ThenInclude(lp => lp.Player) // Inclure les joueurs dans le lobby
        //.FirstOrDefault(l => l.Id == id);

    //if (lobby == null)
    //{
      //  return NotFound("Le lobby spécifié n'existe pas.");
    //}

    // Passer les données à la vue
    //return View(lobby);
//}

  //  }
  public IActionResult Start(Guid id)
{
    var lobby = _context.Lobby
        .Include(l => l.Game)
        .FirstOrDefault(l => l.Id == id);

    if (lobby == null || lobby.Game == null)
    {
        return NotFound("Lobby ou jeu introuvable.");
    }

    // Redirection spécifique en fonction du jeu
    if (lobby.Game.Name == "Petit Bac")
    {
        return RedirectToAction("Configure", "PetitBac", new { lobbyId = id });
    }
    else
    {
        // Ajouter la logique pour d'autres jeux ici
        return RedirectToAction("Play", lobby.Game.Code, new { lobbyId = id });
    }
}
    }

   
}
