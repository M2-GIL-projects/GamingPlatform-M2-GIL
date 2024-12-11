using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class PetitBacHub : Hub
{
    
    public async Task Ping()
    {
        Console.WriteLine($"[SignalR] Ping reçu de : {Context.ConnectionId}");
        await Clients.Caller.SendAsync("Pong", "Pong from server!");
    }
    
    // Rejoindre un groupe SignalR basé sur gameId
    public async Task JoinGame(string gameId, string sessionToken)
    {
        Console.WriteLine($"Rejoindre le groupe SignalR pour gameId : {gameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    // Notifier le créateur sur l'état d'un joueur

public async Task NotifyStatusChange(string gameId, string playerPseudo, string status)
{
    Console.WriteLine($"[SignalR] Notification envoyée : GameId={gameId}, Player={playerPseudo}, Status={status}");
    await Clients.Group(gameId).SendAsync("PlayerStatusUpdated", playerPseudo, status);
}


}
