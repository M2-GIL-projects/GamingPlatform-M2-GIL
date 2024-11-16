
using Microsoft.AspNetCore.SignalR;

namespace GamingPlatform.Hubs
{

    public class LabyrinthHub : Hub
    {
        // Méthode pour envoyer la matrice d'adjacence au client
        public async Task SendLabyrinth(bool[,] adjacency)
        {
            await Clients.All.SendAsync("ReceiveLabyrinth", adjacency);
        }
        public async Task SendPlayerMovement(int playerId, int newCell)
        {
            await Clients.Others.SendAsync("ReceivePlayerMovement", playerId, newCell);
        }

        public async Task SendGameEnd(string message)
        {
            await Clients.All.SendAsync("EndGame", message);
        }

    }

}
