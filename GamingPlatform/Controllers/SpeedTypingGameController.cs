using GamingPlatform.Data;
using GamingPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamingPlatform.Controllers
{
    public class SpeedTypingGameController : Controller
    {
        private readonly GamingPlatformContext _context;
        private static readonly Random random = new Random();

        public SpeedTypingGameController(GamingPlatformContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SpeedTyping(string lobbyCode, string playerName)
        {
            ViewBag.LobbyCode = lobbyCode;
            ViewBag.PlayerName = playerName;

            // Récupérer un texte à taper depuis la base de données
            ViewBag.TextToType = await GenerateRandomTextAsync();

            return View();
        }

        public async Task<IActionResult> GetPlayersInLobby(string lobbyCode)
        {
            var players = await _context.Lobbies
                .Where(l => l.Code == lobbyCode)
                .SelectMany(l => l.Players)
                .Select(pl => pl.Joueur) // Assurez-vous que PlayerLobby a une propriété User
                .ToListAsync();

            return Json(players);
        }

        public async Task<IActionResult> Start(int lobbyId)
        {
            var lobby = await _context.Lobbies.FindAsync(lobbyId);
            if (lobby == null)
            {
                return NotFound("Lobby not found");
            }

            var gameSession = new SpeedTypingGame
            {
                StartTime = DateTime.Now,
                TextToType = await GenerateRandomTextAsync()
            };

            // Assurez-vous d'associer le jeu à un lobby
            gameSession.LobbyId = lobby.Id; // Ajoutez cette ligne pour lier le jeu au lobby

            _context.SpeedTypingGames.Add(gameSession);
            await _context.SaveChangesAsync();

            return View(gameSession);
        }

        private async Task<string> GenerateRandomTextAsync()
        {
            int sentenceCount = random.Next(3, 6);
            var sentences = await _context.Sentences
                .OrderBy(r => Guid.NewGuid())
                .Take(sentenceCount)
                .Select(s => s.Text)
                .ToListAsync();

            return string.Join(" ", sentences);
        }

        [HttpPost]
        public async Task<IActionResult> JoinLobby(string lobbyCode, string playerName)
        {
            var lobby = await _context.Lobbies.FirstOrDefaultAsync(l => l.Code == lobbyCode);
            if (lobby == null)
            {
                return NotFound("Lobby not found");
            }

            var player = new Joueur { Pseudo = playerName };
            
            // Ajoutez le joueur à la table de jonction PlayerLobby
            var playerLobbyEntry = new PlayerLobby { JoueurId = player.Id, LobbyId = lobby.Id };
            
            _context.PlayerLobbies.Add(playerLobbyEntry); // Assurez-vous que PlayerLobby est bien configuré
            await _context.SaveChangesAsync();

            return Ok(new { message = "Joined lobby successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScore(Guid GameId, int userId, int score, double timeTaken)
        {
            var player = await _context.LesPlayers.FindAsync(userId);
            
            if (player == null)
            {
                return NotFound("Player not found");
            }

            var scoreEntry = new Score
            {
                GameId = GameId,
                UserId = player.Id,
                Value = score,
                TimeTaken = timeTaken,
                Date = DateTime.Now // Ajoutez la date du score
            };

            _context.Scores.Add(scoreEntry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Score enregistré avec succès" });
        }

        public async Task<IActionResult> GetTopPlayers()
        {
            var topPlayers = await _context.Scores
                .OrderByDescending(s => s.Value) // Changez cela pour trier par points plutôt que par temps
                .Take(5)
                .Select(s => new { s.Joueur.Pseudo, s.Value }) // Incluez le pseudo du joueur dans les résultats
                .ToListAsync();

            return Json(topPlayers);
        }
    }
}
