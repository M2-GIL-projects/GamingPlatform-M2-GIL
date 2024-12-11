using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using GamingPlatform.Models;

namespace GamingPlatform.Hubs
{
    public class MorpionHub : Hub
    {
        private static readonly Morpion GameBoard = new Morpion();
        private static string CurrentPlayer = "X"; // X commence toujours

        public MorpionHub()
        {
            GameBoard.InitializeBoard();
        }

        public async Task JoinGame(string lobbyId, string playerName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
            await Clients.Group(lobbyId).SendAsync("PlayerJoined", playerName);
        }

        public async Task MakeMove(string lobbyId, int x, int y, string playerSymbol)
{
    try
    {
        // Vérifie si le joueur actuel peut jouer
        if (playerSymbol != CurrentPlayer)
        {
            await Clients.Caller.SendAsync("Error", "Ce n'est pas votre tour !");
            return;
        }

        // Effectue le mouvement sur le plateau
        GameBoard.MakeMove(x, y, playerSymbol);

        // Vérifie si le jeu est terminé
        if (GameBoard.IsGameOver())
        {
            await Clients.Group(lobbyId).SendAsync("GameOver", $"{playerSymbol} a gagné !");
        }
        else
        {
            // Alterne le joueur actuel
            CurrentPlayer = CurrentPlayer == "X" ? "O" : "X";

            // Notifie les clients du groupe avec le plateau mis à jour
            await Clients.Group(lobbyId).SendAsync("UpdateGame", GameBoard.RenderBoard(), CurrentPlayer);
        }
    }
    catch (Exception ex)
    {
        await Clients.Caller.SendAsync("Error", ex.Message);
    }
}

    }
}
