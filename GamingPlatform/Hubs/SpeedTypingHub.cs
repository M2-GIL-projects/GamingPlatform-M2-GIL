using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace GamingPlatform.Hubs
{
    public class SpeedTypingGameHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> LobbyPlayers = new();
        public async Task UpdateProgress(string lobbyCode, string playerName, double progress)
        {
            // Envoyer la progression à tous les clients du groupe
            await Clients.Group(lobbyCode).SendAsync("ReceiveProgress", playerName, progress);
        }

        public async Task JoinLobby(string lobbyCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
        }

        public async Task LeaveLobby(string lobbyCode)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyCode);
        }

        public async Task JoinGame(string lobbyCode, string playerName)
        {
            // Ajouter le joueur au lobby s'il n'est pas encore présent
            if (!LobbyPlayers.ContainsKey(lobbyCode))
            {
                LobbyPlayers[lobbyCode] = new List<string>();
            }

            LobbyPlayers[lobbyCode].Add(playerName);

            // Envoyer la liste des joueurs actuels aux clients du lobby
            await Clients.Group(lobbyCode).SendAsync("PlayerJoined", new { username = playerName });

            // Ajouter le joueur à un groupe de SignalR
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var lobby in LobbyPlayers)
            {
                if (lobby.Value.Contains(Context.ConnectionId))
                {
                    lobby.Value.Remove(Context.ConnectionId);
                    break;
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}

