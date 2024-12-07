﻿using Microsoft.AspNetCore.Mvc;
using GamingPlatform.Services;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Models;
using GamingPlatform.Data;


namespace GamingPlatform.Controllers
{
    public class LobbyController : Controller
    {
        private readonly LobbyService _lobbyService;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamingPlatformContext _context;

        public LobbyController(LobbyService lobbyService, GameService gameService, PlayerService playerService, GamingPlatformContext context)
        {
            _lobbyService = lobbyService;
            _gameService = gameService;
            _playerService = playerService;
            _context = context;
        }

        public async Task<IActionResult> Index(string? name, string? gameCode, LobbyStatus? status)
        {
            // Charger les lobbies
            var lobbies = await _lobbyService.GetAllLobbies();

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
        public async Task<IActionResult> CreateWithSelect(string name, Guid gameId, bool isPrivate, string? password)
        {
            int? playerId = null;
            var player = await GetCurrentPlayer();
            if (player != null)
            {
                playerId = player.Id;
            }
            try
            {
                var lobby = _lobbyService.CreateLobby(name, gameId, isPrivate, playerId, password);
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
        public async Task<IActionResult> CreateFromGame(string name, Guid gameId, bool isPrivate, string? password)
        {
            int? playerId = null;
            var player = await GetCurrentPlayer();
            if (player != null)
            {
                playerId = player.Id;
            }
            try
            {
                var lobby = _lobbyService.CreateLobby(name, gameId, isPrivate, playerId ,password);
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

        public async Task<Player> GetCurrentPlayer()
        {
            // Récupérer l'ID du joueur depuis la session
            var playerId = HttpContext.Session.GetInt32("PlayerId");

            if (playerId.HasValue)
            {
                return await _playerService.GetPlayerByIdAsync(playerId.Value);
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> JoinPrivateLobby(Guid id, string password)
        {
            var lobby = _lobbyService.GetLobbyById(id);

            if (lobby == null)
            {
                return NotFound();
            }

            // Vérifier si le mot de passe correspond pour un lobby privé
            if (lobby.IsPrivate && lobby.Password != password)
            {
                return Unauthorized();
            }

            // Logique pour ajouter un joueur au lobby
            var player = await GetCurrentPlayer();
            if (player != null)
            {
                // Ajouter le joueur au lobby
                _lobbyService.AddPlayerToLobby(id, player.Id);
            }

            return RedirectToAction("Details", "Lobby", new { id = lobby.Id });
        }

       // [HttpGet]
       // public async Task<IActionResult> JoinLobby(Guid id)
        //{
          //  var lobby = _lobbyService.GetLobbyById(id);

            //if (lobby == null)
            //{
              //  return NotFound();
            //}

            // Logique pour ajouter un joueur au lobby
            //var player = await GetCurrentPlayer();
            //if (player != null)
            //{
                // Ajouter le joueur au lobby
              //  _lobbyService.AddPlayerToLobby(id, player.Id);
            //}

            //return RedirectToAction("Details", "Lobby", new { id = lobby.Id });
        //}
    //}
//}


[HttpGet]
public async Task<IActionResult> JoinLobby(Guid id)
{
    // Récupérer le lobby par son ID
    var lobby = _lobbyService.GetLobbyById(id);

    if (lobby == null)
    {
        return NotFound("Lobby introuvable.");
    }

    // Récupérer le joueur actuel
    var player = await GetCurrentPlayer();
    if (player != null)
    {
        // Ajouter le joueur au lobby
        _lobbyService.AddPlayerToLobby(id, player.Id);
    }

    // Vérifier le GameCode pour déterminer le type de jeu
    if (lobby.GameCode == "PTB")
    {
        // Récupérer le jeu Petit Bac associé au lobby
        var game = _context.PetitBacGames
            .Include(g => g.Categories)
            .Include(g => g.Players)
            .FirstOrDefault(g => g.LobbyId == lobby.Id);

        if (game == null)
        {
            return NotFound("Partie Petit Bac introuvable.");
        }

        // Rediriger vers la page RecapitulatifJoin
        return View("RecapitulatifJoin", game);
    }

    // Redirection standard pour les autres jeux
    return RedirectToAction("Details", "Lobby", new { id = lobby.Id });
}
    }
    }