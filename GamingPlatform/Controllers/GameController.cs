using GamingPlatform.Models;
using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatform.Controllers{
public class GameController : Controller
{

        private readonly GameService _gameService;
        private readonly LobbyService _lobbyService;
        private readonly PlayerService _playerService;

        public GameController(GameService gameService, LobbyService lobbyService, PlayerService playerService)
        {
            _gameService = gameService;
            _lobbyService = lobbyService;
            _playerService = playerService;
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
            int? playerId = null;
            var player = await GetCurrentPlayer();
            if (player != null)
            {
                playerId = player.Id;
            }
            ViewBag.CurrentUserId = playerId;
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
            int? playerId = null;
            var player = await GetCurrentPlayer();
            if (player != null)
            {
                playerId = player.Id;
            }
            ViewBag.CurrentUserId = playerId;
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

        public async Task<Player> GetCurrentPlayer()
        {
            // R�cup�rer l'ID du joueur depuis la session
            var playerId = HttpContext.Session.GetInt32("PlayerId");

            if (playerId.HasValue)
            {
                return await _playerService.GetPlayerByIdAsync(playerId.Value);
            }

            return null;
        }
    }

}

public class GameDetailsViewModel
{
    public Game Game { get; set; }
    public List<Lobby> Lobbies { get; set; }
}
