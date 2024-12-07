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
        private readonly ILogger<SpeedTypingController> _logger;

        public SpeedTypingController(LobbyService lobbyService, ILogger<SpeedTypingController> logger)
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
                _logger.LogError($"Lobby ID {lobbyId} not found.");
                return NotFound("Lobby not found.");
            }


            var model = new SimpleViewModel
            {
                LobbyId = lobbyId,
                PlayerPseudos = lobby.LobbyPlayers.Select(p => p.Player.Pseudo).ToList()
            };
            return View(model);
        }
    }

}
