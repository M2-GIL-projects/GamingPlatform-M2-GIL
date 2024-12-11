using GamingPlatform.Data;
using GamingPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GamingPlatform.Hubs;
using Microsoft.Data.SqlClient;

namespace GamingPlatform.Services
{
    public class LobbyService
    {
        private readonly GamingPlatformContext _context;

        private readonly IServiceProvider _serviceProvider;
        public LobbyService(GamingPlatformContext context, IServiceProvider serviceProvider)
        {
            _context = context;
             _serviceProvider = serviceProvider;
        }

        // Crée un nouveau lobby.
        public Lobby CreateLobby(string name, Guid gameId, bool isPrivate, int? playerId = null, string? password = null)
        {
            var game = _context.Game.Find(gameId);
            if (game == null)
            {
                throw new Exception("Le jeu spécifié n'existe pas.");
            }

            var lobby = new Lobby
            {
                Id = Guid.NewGuid(),
                Name = name,
                GameCode = game.Code,
                Game = game,
                IsPrivate = isPrivate,
                Password = isPrivate ? password : null,
                Status = LobbyStatus.Waiting,
                CreatedAt = DateTime.Now
            };

            // Vérifier si un joueur est connecté
            if (playerId.HasValue)
            {
                Player? player = _context.Player.Find(playerId.Value);
                if (player != null)
                {
                    var lobbyPlayer = new LobbyPlayer
                    {
                        LobbyId = lobby.Id,
                        PlayerId = player.Id
                    };

                    lobby.LobbyPlayers.Add(lobbyPlayer);
                }
            }

            _context.Lobby.Add(lobby);
            _context.SaveChanges();

            return lobby;
        }

        // Met à jour le lobby
        public void UpdateLobby(Lobby lobby)
        {
            _context.Lobby.Update(lobby); // Marque le lobby comme modifié
            _context.SaveChanges(); // Enregistre les modifications dans la base
        }

        // Récupère un lobby par son ID.
        public Lobby? GetLobbyById(Guid id)
        {
            return _context.Lobby
                .Where(l => l.Id == id)
                .FirstOrDefault();
        }

        // Méthode pour récupérer un lobby et charger les joueurs associés
        public Lobby GetLobbyWithGameAndPlayers(Guid lobbyId)
        {
            var lobby = _context.Lobby
                .Where(l => l.Id == lobbyId)
                .Include(l => l.Game)
                .Include(l => l.LobbyPlayers)  // Inclure les joueurs du lobby
                .ThenInclude(lp => lp.Player)  // Inclure les informations sur le joueur
                .FirstOrDefault();

            return lobby;  // Retourne le lobby avec les joueurs associés
        }


        // Récupère tous les lobbies disponibles.
        public async Task<IEnumerable<Lobby>> GetAllLobbies()
        {
            return await _context.Lobby
                .Include(l => l.Game) // Inclut l'objet Game lié au Lobby
                .Include(l => l.LobbyPlayers) // Inclut la collection LobbyPlayers
                    .ThenInclude(lp => lp.Player) // Inclut les objets Player dans LobbyPlayers
                .ToListAsync();
        }

        public async Task<List<Lobby>> GetLobbiesByGameAsync(string gameCode)
        {
            return await _context.Lobby
                .Include(l => l.Game) // Inclure les détails du jeu
                .Include(l => l.LobbyPlayers) // Inclure les joueurs du lobby
                    .ThenInclude(lp => lp.Player) // Inclure les détails des joueurs
                .Where(l => l.GameCode == gameCode)
                .ToListAsync();
        }

        // Ajoute un joueur à un lobby.
        public void AddPlayerToLobby(Guid lobbyId, int playerId)
        {
            var lobby = _context.Lobby.Find(lobbyId);
            var player = _context.Player.Find(playerId);

            if (lobby == null || player == null)
            {
                throw new Exception("Le lobby ou le joueur n'existe pas.");
            }

            if (lobby.LobbyPlayers.Any(lp => lp.PlayerId == playerId))
            {
                throw new PlayerAlreadyInLobbyException("Le joueur est déjà dans le lobby.");
            }

            var lobbyPlayer = new LobbyPlayer
            {
                LobbyId = lobbyId,
                PlayerId = playerId
            };

            _context.LobbyPlayer.Add(lobbyPlayer);
            _context.SaveChanges();
        }

        //
        public IActionResult StartGame(Guid lobbyId)
        {
            try
            {
                var lobby = GetLobbyWithGameAndPlayers(lobbyId);

                if (lobby == null)
                {
                    return new NotFoundObjectResult("Lobby introuvable");
                }

                if (lobby.Status != LobbyStatus.Waiting)
                {
                    return new BadRequestObjectResult("Le lobby n'est pas en attente.");
                }

                lobby.Status = LobbyStatus.InProgress;
                UpdateLobby(lobby);
                string redirectUrl = null;
                switch (lobby.Code)
                {
                    case "SPT":
                        {
                            var hubContext = _serviceProvider.GetRequiredService<IHubContext<SpeedTypingHub>>();
                            var speedTypingGame = new SpeedTyping(hubContext);
                            speedTypingGame.InitializeBoard();

                            lobby.Game.Name = "SpeedTyping";
                            UpdateLobby(lobby);

                            hubContext.Clients.Group(lobbyId.ToString()).SendAsync("GameStarted", speedTypingGame.TextToType, speedTypingGame.TimeLimit);
                            
                            redirectUrl = $"/Game/SpeedTyping/Play/{lobbyId}";
                            return new OkObjectResult("Partie de Speed Typing démarrée");
                        }

                    default:
                        return new BadRequestObjectResult($"Le type de jeu avec le code {lobby.Code} n'est pas pris en charge.");
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Une erreur est survenue : {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
public class PlayerAlreadyInLobbyException : Exception
{
    public PlayerAlreadyInLobbyException() { }

    public PlayerAlreadyInLobbyException(string message) : base(message) { }

    public PlayerAlreadyInLobbyException(string message, Exception inner) : base(message, inner) { }
}
