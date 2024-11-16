using GamingPlatform.Data;
using GamingPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamingPlatform.Controllers
{
    public class LobbyController : Controller
    {
        private readonly GamingPlatformContext _context;

        public LobbyController(GamingPlatformContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLobby(Lobby lobby)
        {
            lobby.Id = Guid.NewGuid();
            lobby.Code = GenerateLobbyCode();
            lobby.Status = LobbyStatus.Waiting;
            lobby.CreatedAt = DateTime.Now;

            if (lobby.IsPrivate && string.IsNullOrWhiteSpace(lobby.Password))
            {
                return BadRequest("Les lobbies privés doivent avoir un mot de passe.");
            }

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            return RedirectToAction("Join", new { code = lobby.Code });
        }

        public IActionResult Join(string code)
        {
            var lobby = _context.Lobbies.FirstOrDefault(l => l.Code == code);
            if (lobby == null)
            {
                return NotFound("Lobby non trouvé.");
            }
            return View(lobby);
        }

        [HttpPost]
        public async Task<IActionResult> JoinLobby(string lobbyCode, string playerName, string password)
        {
            var lobby = await _context.Lobbies
                .Include(l => l.Players)
                .FirstOrDefaultAsync(l => l.Code == lobbyCode);

            if (lobby == null)
            {
                return NotFound("Lobby non trouvé.");
            }

            if (lobby.IsPrivate && lobby.Password != password)
            {
                return BadRequest("Mot de passe incorrect pour ce lobby privé.");
            }

          var player = new Joueur { Pseudo = playerName };
            
            // Ajoutez le joueur à la table de jonction PlayerLobby
            var playerLobbyEntry = new PlayerLobby { JoueurId = player.Id, LobbyId = lobby.Id };
            
            _context.PlayerLobbies.Add(playerLobbyEntry); // Assurez-vous que PlayerLobby est bien configuré
            await _context.SaveChangesAsync();

            return Json(new { success = true, redirectUrl = $"/Game/Play?code={lobbyCode}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableLobbies()
        {
            var lobbies = await _context.Lobbies
                .Where(l => l.Status == LobbyStatus.Waiting)
                .Select(l => new 
                {
                    l.Code,
                    l.Name,
                    l.GameType,
                    l.IsPrivate,
                    PlayerCount = l.Players.Count
                })
                .ToListAsync();

            return Json(lobbies);
        }

        [HttpPost]
        public async Task<IActionResult> StartGame(string lobbyCode)
        {
            var lobby = await _context.Lobbies.FirstOrDefaultAsync(l => l.Code == lobbyCode);
            if (lobby == null)
            {
                return NotFound("Lobby non trouvé.");
            }

            lobby.Status = LobbyStatus.InProgress;
            await _context.SaveChangesAsync();

            // Ici, vous pouvez ajouter la logique pour démarrer le jeu
            return RedirectToAction("Play", "Game", new { code = lobbyCode });
        }

    /*
        [HttpPost]
        public async Task<IActionResult> LeaveLobby(string lobbyCode, string playerName)
        {
            var lobby = await _context.Lobbies
                .Include(l => l.Players)
                .FirstOrDefaultAsync(l => l.Code == lobbyCode);

            if (lobby == null)
            {
                return NotFound("Lobby non trouvé.");
            }

            var player = lobby.Players.FirstOrDefault(p => p.Pseudo == playerName);
            if (player != null)
            {
                lobby.Players.Remove(player);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }*/

        private string GenerateLobbyCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
