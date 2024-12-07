using GamingPlatform.Models;
using GamingPlatform.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace GamingPlatform.Hubs
{
    public class SpeedTypingHub : Hub
    {
        private readonly SpeedTyping _game;
        private readonly LobbyService _lobbyService;
        private readonly ILogger<SpeedTypingHub> _logger;
        private readonly ConcurrentDictionary<string, string> _connectionToPseudo;

        public SpeedTypingHub(
            SpeedTyping game,
            LobbyService lobbyService,
            ILogger<SpeedTypingHub> logger,
           ConcurrentDictionary<string, string> connectionToPseudo)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionToPseudo = connectionToPseudo ?? throw new ArgumentNullException(nameof(connectionToPseudo));
            _logger.LogInformation("SpeedTypingHub instantiated successfully.");
        }

        public async Task StartGame(string difficulty)
        {
            if (Enum.TryParse<Difficulty>(difficulty, true, out Difficulty parsedDifficulty))
            {
                try
                {
                    _logger.LogInformation("Starting game with difficulty: {Difficulty}", parsedDifficulty);
                    _game.InitializeBoard(parsedDifficulty);
                    _logger.LogInformation("Game initialized with text: {TextToType} and time limit: {TimeLimit}", _game.TextToType, _game.TimeLimit);
                    await Clients.All.SendAsync("GameStarted", _game.TextToType, _game.TimeLimit);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception in StartGame for difficulty: {Difficulty}", parsedDifficulty);
                    throw;
                }
            }
            else
            {
                _logger.LogWarning("Invalid difficulty received: {Difficulty}", difficulty);
                throw new ArgumentException("Invalid difficulty", nameof(difficulty));
            }
        }

        public async Task JoinLobby(string lobbyId, string pseudo)
        {
            if (string.IsNullOrWhiteSpace(lobbyId)) throw new ArgumentNullException(nameof(lobbyId));
            if (string.IsNullOrWhiteSpace(pseudo)) throw new ArgumentNullException(nameof(pseudo));

            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
                _connectionToPseudo[Context.ConnectionId] = pseudo; // Associe le ConnectionId au pseudo
                _logger.LogInformation("Player '{Pseudo}' joined lobby '{LobbyId}'", pseudo, lobbyId);
                await Clients.Group(lobbyId).SendAsync("PlayerJoined", pseudo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding player '{Pseudo}' to lobby '{LobbyId}'", pseudo, lobbyId);
                throw;
            }
        }

        public async Task InitializeLobbyPlayers(string lobbyId)
        {
            if (string.IsNullOrWhiteSpace(lobbyId)) throw new ArgumentNullException(nameof(lobbyId));

            try
            {
                var lobby = _lobbyService.GetLobbyWithGameAndPlayers(Guid.Parse(lobbyId));
                if (lobby != null)
                {
                    foreach (var lobbyPlayer in lobby.LobbyPlayers)
                    {
                        if (lobbyPlayer.Player != null)
                        {
                            _connectionToPseudo[Context.ConnectionId] = lobbyPlayer.Player.Pseudo;
                            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
                        }
                    }
                    _logger.LogInformation("Initialized players for lobby '{LobbyId}'", lobbyId);
                }
                else
                {
                    _logger.LogWarning("Lobby not found for ID: {LobbyId}", lobbyId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing players for lobby '{LobbyId}'", lobbyId);
                throw;
            }
        }


        public async Task UpdateProgress(string typedText)
        {
            try
            {
                await _game.CheckProgress(Context.ConnectionId, typedText);
                _logger.LogInformation("Progress updated for connection '{ConnectionId}'", Context.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating progress for connection '{ConnectionId}'", Context.ConnectionId);
                throw;
            }
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            try
            {
                string pseudo = _connectionToPseudo.GetValueOrDefault(Context.ConnectionId, "Unknown");
                await Clients.All.SendAsync("ReceiveMessage", pseudo, message);
                _logger.LogInformation("Message sent from '{Pseudo}': {Message}", pseudo, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from '{ConnectionId}'", Context.ConnectionId);
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                if (Context.ConnectionId == null)
                {
                    _logger.LogWarning("ConnectionId null detected.");
                    throw new InvalidOperationException("Client not connected.");
                }

                _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during client connection: {ConnectionId}", Context.ConnectionId);
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connectionToPseudo.TryRemove(Context.ConnectionId, out var pseudo))
            {
                _logger.LogInformation("Client déconnecté : {ConnectionId}, Pseudo : {Pseudo}", Context.ConnectionId, pseudo);
            }

            await base.OnDisconnectedAsync(exception);
        }


        public async Task EndGame(List<PlayerScore> leaderboard)
        {
            try
            {
                _logger.LogInformation("Game Over. Sending leaderboard to clients.");
                await Clients.All.SendAsync("GameOver", leaderboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending leaderboard.");
                throw;
            }
        }
    }
}
