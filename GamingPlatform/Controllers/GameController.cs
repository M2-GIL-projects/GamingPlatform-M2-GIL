using GamingPlatform.Models;
using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatform.Controllers{
public class GameController : Controller
{

        private readonly GameService _gameService;
        private readonly LobbyService _lobbyService;

        public GameController(GameService gameService, LobbyService lobbyService)
        {
            _gameService = gameService;
            _lobbyService = lobbyService;
        }

        public IActionResult Index()
        {
            var games = _gameService.GetAvailableGames(); // Impl�mentez GetAvailableGames dans le service
            return View(games);
        }

        // Affiche les d�tails d'un jeu en fonction de son code.
        public async Task<IActionResult> Details(Guid id)
        {
            // R�cup�rer le jeu par le code
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                // Retourner une page d'erreur si le jeu n'existe pas
                return NotFound("Le jeu sp�cifi� n'existe pas.");
            }

            // R�cup�rer les lobbies associ�s au jeu
            var lobbies = await _lobbyService.GetLobbiesByGameAsync(game.Code);

            // Cr�er un mod�le de vue combin�
            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Lobbies = lobbies
            };

            // Passer le mod�le de vue � la vue
            return View(viewModel);
        }


        // Affiche les d�tails d'un jeu en fonction de son code.
        public async Task<IActionResult> LobbiesByGameCode(string gameCode)
        {
            // R�cup�rer le jeu par le code
            var game = _gameService.GetGameByCode(gameCode);
            if (game == null)
            {
                // Retourner une page d'erreur si le jeu n'existe pas
                return NotFound("Le jeu sp�cifi� n'existe pas.");
            }

            // R�cup�rer les lobbies associ�s au jeu
            var lobbies = await _lobbyService.GetLobbiesByGameAsync(gameCode);

            // Cr�er un mod�le de vue combin�
            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Lobbies = lobbies
            };

            // Passer le mod�le de vue � la vue
            return View(viewModel);
        }

        //public IActionResult Play(string gameCode, Guid lobbyId)
        //{
        //    var game = _gameService.GetGameByCode(gameCode);
        //    var lobby = _lobbyService.GetLobbyWithGameAndPlayers(lobbyId);

        //    if (game == null || lobby == null)
        //    {
        //        return NotFound();
        //    }

        //    // Logique pour charger le plateau du jeu
        //    //return View("GameBoard", new GameViewModel { Game = game, Lobby = lobby });
        //}

    }
}

public class GameDetailsViewModel
{
    public Game Game { get; set; }
    public List<Lobby> Lobbies { get; set; }
}
