using GamingPlatform.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingPlatform.Hubs
{
    public class MorpionHub : Hub
    {
        private static readonly Dictionary<string, Morpion> Lobbies = new();

        public async Task JoinLobby(string lobbyId)
        {
            if (!Lobbies.ContainsKey(lobbyId))
            {
                Lobbies[lobbyId] = new Morpion { LobbyId = Guid.NewGuid() };
            }

            var lobby = Lobbies[lobbyId];

            if (string.IsNullOrEmpty(lobby.PlayerXConnectionId))
            {
                lobby.PlayerXConnectionId = Context.ConnectionId;
                lobby.CurrentPlayerConnectionId = Context.ConnectionId;
            }
            else if (string.IsNullOrEmpty(lobby.PlayerOConnectionId))
            {
                lobby.PlayerOConnectionId = Context.ConnectionId;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
            await Clients.Group(lobbyId).SendAsync("PlayerJoined", lobbyId);
        }

        public async Task MakeMove(string lobbyId, int row, int col)
        {
            if (!Lobbies.ContainsKey(lobbyId)) return;

            var lobby = Lobbies[lobbyId];

            if (Context.ConnectionId != lobby.CurrentPlayerConnectionId)
            {
                await Clients.Caller.SendAsync("InvalidMove", "Ce n'est pas votre tour.");
                return;
            }

            try
            {
                lobby.MakeMove(row, col, lobby.CurrentPlayer);
            }
            catch (InvalidOperationException ex)
            {
                await Clients.Caller.SendAsync("InvalidMove", ex.Message);
                return;
            }

            await Clients.Group(lobbyId).SendAsync("ReceiveMove", row, col, lobby.CurrentPlayer);

            if (lobby.CheckForWin(lobby.CurrentPlayer))
            {
                lobby.IsGameOver = true;
                lobby.Winner = lobby.CurrentPlayer;
                await Clients.Group(lobbyId).SendAsync("GameOver", lobby.Winner);
                return;
            }

            if (lobby.IsBoardFull())
            {
                lobby.IsGameOver = true;
                await Clients.Group(lobbyId).SendAsync("GameOver", "null");
                return;
            }

            lobby.CurrentPlayer = lobby.CurrentPlayer == "X" ? "O" : "X";
            lobby.CurrentPlayerConnectionId = lobby.CurrentPlayer == "X" ? lobby.PlayerXConnectionId : lobby.PlayerOConnectionId;
            await Clients.Group(lobbyId).SendAsync("UpdateCurrentPlayer", lobby.CurrentPlayer);
        }

        public async Task ResetGame(string lobbyId)
        {
            if (!Lobbies.ContainsKey(lobbyId)) return;

            var lobby = Lobbies[lobbyId];
            lobby.InitializeBoard();
            lobby.CurrentPlayer = "X";
            lobby.IsGameOver = false;
            lobby.Winner = null;

            await Clients.Group(lobbyId).SendAsync("ReceiveReset");
        }
    }
}
