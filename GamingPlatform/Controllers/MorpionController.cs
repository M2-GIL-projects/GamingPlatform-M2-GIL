using Microsoft.AspNetCore.Mvc;
using GamingPlatform.Services;
using GamingPlatform.Models;
using System;
using System.Linq;

namespace GamingPlatform.Controllers
{
    [Route("Game/Morpion")]
    public class MorpionController : Controller
    {
        private readonly LobbyService _lobbyService;
        private readonly ILogger<MorpionController> _logger;

        public MorpionController(LobbyService lobbyService, ILogger<MorpionController> logger)
        {
            _lobbyService = lobbyService;
            _logger = logger;
        }

        [HttpGet("Play/{lobbyId}")]
        public IActionResult Play(Guid lobbyId)
        {
            var lobby = _lobbyService.GetLobbyWithGameAndPlayers(lobbyId);
            if (lobby == null)
            {
                _logger.LogError($"Lobby ID {lobbyId} introuvable.");
                return NotFound("Lobby introuvable.");
            }

            // Passer les informations nécessaires à la vue via ViewData
            ViewData["LobbyId"] = lobbyId;
            ViewData["PlayerPseudos"] = lobby.LobbyPlayers.Select(p => p.Player.Pseudo).ToList();

            _logger.LogInformation($"Lobby {lobbyId} chargé pour Morpion avec les joueurs : {string.Join(", ", ViewData["PlayerPseudos"])}.");
            return View("Play");
        }
    }
}
