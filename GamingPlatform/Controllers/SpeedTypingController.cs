using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using GamingPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamingPlatform.Controllers
{
    [Route("Game/SpeedTyping")]
    public class SpeedTypingController : Controller
    {
        private readonly LobbyService _lobbyService;
        private readonly PlayerService _playerService;
        private readonly ILogger<SpeedTypingController> _logger;

        public SpeedTypingController(LobbyService lobbyService, ILogger<SpeedTypingController> logger, PlayerService playerService)
        {
            _lobbyService = lobbyService;
            _logger = logger;
            _playerService = playerService;
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

        [HttpGet("Play/{lobbyId}")]
        public async Task<IActionResult> Play(Guid lobbyId)
        {
            var lobby = _lobbyService.GetLobbyWithGameAndPlayers(lobbyId);
            if (lobby == null)
            {
                _logger.LogError($"Lobby ID {lobbyId} not found.");
                return NotFound("Lobby not found.");
            }

            var currentPlayer = await GetCurrentPlayer();
            if (currentPlayer == null)
            {
                return RedirectToAction("Player", "Home");
            }

            var model = new SpeedTypingViewModel
            {
                LobbyId = lobby.Id,
                Name = lobby.Name,
                PlayerPseudos = lobby.LobbyPlayers.Select(lp => lp.Player.Pseudo).ToList(),
                CurrentPlayerPseudo = currentPlayer.Pseudo
            };
            return View(model);
        }



    }

}
