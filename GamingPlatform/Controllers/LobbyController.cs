using Microsoft.AspNetCore.Mvc;
using GamingPlatform.Services;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Models;

namespace GamingPlatform.Controllers
{
    public class LobbyController : Controller
    {
        private readonly LobbyService _lobbyService;
        private readonly GameService _gameService;

        public LobbyController(LobbyService lobbyService, GameService gameService)
        {
            _lobbyService = lobbyService;
            _gameService = gameService;
        }

        public IActionResult Index(string? name, string? gameCode, LobbyStatus? status)
        {
            // Charger les lobbies
            var lobbies = _lobbyService.GetAllLobbies();

            // Appliquer les filtres
            if (!string.IsNullOrEmpty(name))
            {
                lobbies = lobbies.Where(l => l.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(gameCode))
            {
                lobbies = lobbies.Where(l => l.GameCode == gameCode);
            }

            if (status.HasValue)
            {
                lobbies = lobbies.Where(l => l.Status == status.Value);
            }

            // Charger les jeux pour les filtres
            ViewBag.Games = _gameService.GetAvailableGames();

            return View(lobbies);
        }

        public IActionResult Details(Guid id)
        {
            var lobby = _lobbyService.GetLobbyWithGameAndPlayers(id);
            if (lobby == null)
            {
                return NotFound();
            }

            return View(lobby);
        }


        // Affiche un formulaire avec un select pour choisir un jeu
        [HttpGet]
        public IActionResult CreateWithSelect()
        {
            // Récupérer tous les jeux pour la liste déroulante
            var games = _gameService.GetAvailableGames();
            ViewBag.Games = games;
            return View();
        }

        // Crée un lobby à partir du formulaire CreateWithSelect
        [HttpPost]
        public IActionResult CreateWithSelect(string name, Guid gameId, bool isPrivate, string? password)
        {
            try
            {
                var lobby = _lobbyService.CreateLobby(name, gameId, isPrivate, password);
                return RedirectToAction("Details", new { id = lobby.Id });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Games = _gameService.GetAvailableGames();
                return View();
            }
        }

        // Crée un lobby à partir d'un jeu (le code du jeu est passé en paramètre)
        [HttpGet]
        public IActionResult CreateFromGame(string gameCode)
        {
            var game = _gameService.GetGameByCode(gameCode);
            if (game == null)
            {
                return NotFound("Le jeu spécifié n'existe pas.");
            }

            return View(game);
        }

        // Traite la création d'un lobby à partir d'un jeu
        [HttpPost]
        public IActionResult CreateFromGame(string name, Guid gameId, bool isPrivate, string? password)
        {
            try
            {
                var lobby = _lobbyService.CreateLobby(name, gameId, isPrivate, password);
                return RedirectToAction("Details", new { id = lobby.Id });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var game = _gameService.GetGameById(gameId);
                if (game == null)
                {
                    return NotFound("Le jeu spécifié n'existe pas.");
                }

                return View(game);
            }
        }

        [HttpPost]
        public IActionResult Start(Guid id)
        {
            _lobbyService.StartGame(id);

            return Ok();
        }
    }
}
