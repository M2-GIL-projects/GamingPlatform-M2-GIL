using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Data;
using GamingPlatform.Models;
using GamingPlatform.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace GamingPlatform.Controllers
{
    public class PetitBacController : Controller
    {
        private readonly GamingPlatformContext _context;
        private readonly IHubContext<PetitBacHub> _hubContext;

        public PetitBacController(GamingPlatformContext context, IHubContext<PetitBacHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
    [HttpGet]
    public IActionResult Configure(Guid lobbyId)
{
    // Récupérer le lobby
    var lobby = _context.Lobby.Include(l => l.Game).FirstOrDefault(l => l.Id == lobbyId);
    if (lobby == null)
    {
        Console.WriteLine("Erreur : Lobby introuvable.");
        return NotFound("Lobby introuvable.");
    }

    // Récupérer le pseudo du joueur connecté depuis la session
    var creatorPseudo = HttpContext.Session.GetString("PlayerPseudo");
    if (string.IsNullOrEmpty(creatorPseudo))
    {
        Console.WriteLine("Erreur : Aucun pseudo trouvé dans la session.");
        return RedirectToAction("Index", "Home"); // Rediriger vers la page d'accueil ou de connexion si la session est vide
    }

    // Vérifier que le joueur est bien dans le lobby (optionnel)
    var playerInLobby = _context.LobbyPlayer.Any(lp => lp.LobbyId == lobbyId && lp.Player.Pseudo == creatorPseudo);
    if (!playerInLobby)
    {
        Console.WriteLine("Erreur : Le joueur n'appartient pas à ce lobby.");
        return Unauthorized("Vous n'avez pas accès à ce lobby.");
    }

    // Créer le modèle de configuration du jeu
    var model = new PetitBacGame
    {
        LobbyId = lobbyId,
        Letters = new List<char>(), // Laisser vide pour permettre la personnalisation
        PlayerCount = 2, // Définir un nombre par défaut (modifiable par le créateur)
        CreatorPseudo = creatorPseudo,
        Categories = _context.PetitBacCategories.ToList()
    };

    // Générer un lien pour inviter un joueur
    string linkPlayer2 = $"{Request.Scheme}://{Request.Host}/PetitBac/Join?code={lobby.Game.Code}&playerId=2";
    ViewBag.LinkPlayer2 = linkPlayer2;

    Console.WriteLine($"Configuration initialisée pour le lobby : {lobbyId}, Créateur : {creatorPseudo}");
    return View("Configuration", model);
}

[HttpPost]
public async Task<IActionResult> ConfigureGame(PetitBacGame model, string[] SelectedCategories, string[] SelectedLetters)
{
    try
    {
        // Récupérer le lobby
        var lobby = _context.Lobby.Include(l => l.Game).FirstOrDefault(l => l.Id == model.LobbyId);
        if (lobby == null)
        {
            Console.WriteLine("Erreur : Lobby introuvable lors de la configuration.");
            return NotFound("Lobby introuvable.");
        }

        model.Lobby = lobby;

        // Validation des lettres sélectionnées
        if (SelectedLetters == null || SelectedLetters.Length == 0)
        {
            ModelState.AddModelError("Letters", "Vous devez sélectionner au moins une lettre.");
            return View("Configuration", model);
        }

        model.Letters = SelectedLetters.Select(l => l[0]).ToList();

        // Validation des catégories sélectionnées
        if (SelectedCategories == null || SelectedCategories.Length == 0)
        {
            ModelState.AddModelError("SelectedCategories", "Vous devez sélectionner au moins une catégorie.");
            return View("Configuration", model);
        }

        foreach (var category in SelectedCategories)
        {
            model.Categories.Add(new PetitBacCategory { Name = category });
        }

        // Ajouter le jeu à la base de données
        _context.PetitBacGames.Add(model);
        _context.SaveChanges();

        Console.WriteLine($"Partie configurée avec succès : {model.Id}");

        // Notification via SignalR
        await _hubContext.Clients.Group(model.LobbyId.ToString())
            .SendAsync("GameConfiguredNotification", $"La partie dans le lobby {lobby.Name} a été configurée par {model.CreatorPseudo}.");

        return RedirectToAction("Recapitulatif", new { gameId = model.Id });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la configuration de la partie : {ex.Message}");
        return BadRequest(new { message = "Erreur lors de la configuration du jeu.", error = ex.Message });
    }
}



        public IActionResult Recapitulatif(int gameId)
{
    // Récupérer les informations de base sur la partie, sans inclure les statuts des joueurs
    var game = _context.PetitBacGames
        .Include(g => g.Categories)
        .Include(g => g.Lobby)
        .FirstOrDefault(g => g.Id == gameId);

    if (game == null)
    {
        Console.WriteLine($"Erreur : Partie introuvable pour gameId {gameId}");
        return NotFound("Partie introuvable.");
    }

    // Préparer une liste de joueurs avec des statuts vides pour un affichage initial
    ViewBag.Players = new List<object>(); // Liste vide pour laisser SignalR mettre à jour dynamiquement

    // Préparer le lien pour inviter des joueurs
    string linkPlayer2 = $"{Request.Scheme}://{Request.Host}/PetitBac/Join?code={game.Lobby.Code}";
    ViewBag.LinkPlayer2 = linkPlayer2;

    Console.WriteLine($"Recapitulatif chargé pour la partie {gameId} sans inclure les statuts des joueurs.");
    return View(game);
}


        [HttpGet]
        public async Task<IActionResult> Join(string code)
        {
            var lobby = _context.Lobby.FirstOrDefault(l => l.Code == code);

            if (lobby == null)
            {
                Console.WriteLine($"Erreur : Lobby introuvable pour code {code}");
                return NotFound("Lobby introuvable.");
            }
              ViewBag.Players = new List<object>();

            var game = _context.PetitBacGames
                .Include(g => g.Categories)
                .Include(g => g.Players)
                .FirstOrDefault(g => g.LobbyId == lobby.Id);

            if (game == null)
            {
                Console.WriteLine($"Erreur : Partie introuvable pour lobbyId {lobby.Id}");
                return NotFound("Partie introuvable.");
            }

            if (game.Players.Count >= game.PlayerCount)
            {
                TempData["Error"] = "Le nombre maximal de joueurs pour cette partie a été atteint.";
                Console.WriteLine("Erreur : Nombre maximal de joueurs atteint.");
                return RedirectToAction("ErrorPage");
            }

            Console.WriteLine($"Un joueur rejoint la partie avec le code {code}.");
            await _hubContext.Clients.Group(game.Id.ToString())
                .SendAsync("PlayerJoinedNotification", $"Un joueur a rejoint la partie : {code}");

            ViewBag.GameId = game.Id;
            return View("RecapitulatifJoin", game);
        }

 [HttpPost]
public async Task<IActionResult> RegisterPlayer(int gameId, string pseudo)
{
    
    // Vérifier si le pseudo est vide
    if (string.IsNullOrWhiteSpace(pseudo))
    {
        return Json(new { success = false, message = "Le pseudo ne peut pas être vide." });
    }

    // Récupérer le jeu et vérifier s'il existe
    var game = _context.PetitBacGames
        .Include(g => g.Players)
        .FirstOrDefault(g => g.Id == gameId);

    if (game == null)
    {
        return Json(new { success = false, message = "Jeu introuvable." });
    }

    // Vérifier si un joueur avec le même pseudo existe déjà dans la partie
    var existingPlayer = _context.PetitBacPlayer
        .FirstOrDefault(p => p.Pseudo == pseudo && p.PetitBacGameId == gameId);
    if (existingPlayer != null)
    {
        return Json(new { success = false, message = "Un joueur avec ce pseudo est déjà inscrit dans cette partie." });
    }

    var player = new PetitBacPlayer
    {
        Pseudo = pseudo,
        PetitBacGameId = gameId,
        SessionToken = Guid.NewGuid().ToString(),
        JoinedAt = DateTime.UtcNow
    };

    _context.PetitBacPlayer.Add(player);
    _context.SaveChanges();

    Console.WriteLine($"Joueur {pseudo} ajouté à la partie {gameId}");

    // Notifier le créateur via SignalR
    await _hubContext.Clients.Group(gameId.ToString())
        .SendAsync("PlayerStatusUpdated", pseudo, "Inactive");

    string url = $"/PetitBac/Play?gameId={gameId}&sessionToken={player.SessionToken}";
    return Redirect(url);
}


 [HttpGet]
        public async Task<IActionResult> Play(int gameId, string sessionToken)
        {
            // Charger le jeu
            var game = _context.PetitBacGames
                .Include(g => g.Categories)
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
                return NotFound("Jeu introuvable.");

            // Rechercher le joueur avec le token de session
            var player = _context.PetitBacPlayer
                .FirstOrDefault(p => p.SessionToken == sessionToken && p.PetitBacGameId == gameId);

            if (player == null)
                return NotFound("Session de joueur introuvable ou invalide.");

            // Notifier le créateur via SignalR que le joueur est en cours
            await _hubContext.Clients.Group(gameId.ToString())
                .SendAsync("PlayerStatusUpdated", player.Pseudo, "En cours");


            Console.WriteLine($"[SignalR] Notification pour {player.Pseudo} : En cours. Groupe : {gameId}");


            // Préparer les données pour la vue
            var viewModel = new PlayViewModel
            {
                GameId = gameId,
                PlayerId = player.Id,
                Letters = game.Letters,
                Categories = game.Categories.Select(c => c.Name).ToList()
            };

            return View(viewModel);
        }
[HttpPost]
public async Task<IActionResult> SubmitAnswers(int gameId, int playerId, Dictionary<char, Dictionary<string, string>> answers)
{
    var player = _context.PetitBacPlayer
        .FirstOrDefault(p => p.Id == playerId && p.PetitBacGameId == gameId);

    if (player == null)
    {
        return NotFound("Session de joueur introuvable ou invalide.");
    }

    try
    {
        // Enregistrer les réponses en base de données
        player.ResponsesJson = Newtonsoft.Json.JsonConvert.SerializeObject(answers);

        // Sauvegarder les modifications
        _context.SaveChanges();

        // Notifier via SignalR que le joueur a terminé
        await _hubContext.Clients.Group(gameId.ToString())
            .SendAsync("PlayerStatusUpdated", player.Pseudo, "Terminé");

        Console.WriteLine($"Joueur {player.Pseudo} a soumis ses réponses et est maintenant Terminé.");

        // Rediriger vers une vue de confirmation
        return RedirectToAction("Confirmation", new { gameId = gameId, playerId = playerId });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la soumission des réponses : {ex.Message}");
        return BadRequest("Une erreur est survenue lors de la soumission des réponses.");
    }
}

       [HttpGet]
        public IActionResult Confirmation(int gameId, int playerId)
        {
            var player = _context.PetitBacPlayer
                .FirstOrDefault(p => p.Id == playerId && p.PetitBacGameId == gameId);

            if (player == null)
            {
                return NotFound("Joueur introuvable ou invalide.");
            }

            var game = _context.PetitBacGames
                .Include(g => g.Categories)
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return NotFound("Partie introuvable.");
            }

            var viewModel = new ConfirmationViewModel
            {
                GameId = gameId,
                PlayerId = playerId,
                PlayerPseudo = player.Pseudo,
                Categories = game.Categories.Select(c => c.Name).ToList(),
                Score = player.Score
            };

            return View(viewModel);
        }
[HttpPost]
[Route("PetitBac/SubmitScore")]
public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreRequest request)
{
    if (request == null)
    {
        Console.WriteLine("Requête invalide reçue.");
        return BadRequest(new { message = "Requête invalide." });
    }

    // Validation du score
    if (request.Score < 0 || request.Score > 100)
    {
        Console.WriteLine($"Score invalide : {request.Score}");
        return BadRequest(new { message = "Le score doit être entre 0 et 100." });
    }

    // Récupérer le joueur en utilisant PlayerPseudo et GameId
    var player = _context.PetitBacPlayer
        .FirstOrDefault(p => p.Pseudo == request.PlayerPseudo && p.PetitBacGameId == request.GameId);

    if (player == null)
    {
        Console.WriteLine($"Joueur introuvable : Pseudo={request.PlayerPseudo}, GameId={request.GameId}");
        return NotFound(new { message = "Joueur introuvable ou invalide." });
    }

    // Vérifier si le score a déjà été attribué pour éviter les mises à jour multiples
    if (player.Score > 0)
    {
        Console.WriteLine($"Le score a déjà été attribué : Pseudo={player.Pseudo}, Score={player.Score}");
        return BadRequest(new { message = "Le score a déjà été attribué." });
    }

    // Mise à jour du score dans la base de données
    player.Score = request.Score;
    _context.SaveChanges();

    // Log des données mises à jour
    Console.WriteLine($"Score mis à jour : Pseudo={player.Pseudo}, GameId={request.GameId}, Score={request.Score}");

    // Envoyer le score via SignalR
    await _hubContext.Clients.Group($"Game-{request.GameId}").SendAsync("ReceiveScore", player.Pseudo, request.Score);

    // Retourner une réponse de succès
    Console.WriteLine($"Score envoyé via SignalR : Pseudo={player.Pseudo}, Score={request.Score}");
    return Ok(new { message = "Score mis à jour et envoyé avec succès." });
}


[HttpGet("api/petitbac/getPlayerAnswersByPseudo")]
public IActionResult GetPlayerAnswersByPseudo(string playerPseudo)
{
    try
    {
        // Rechercher le joueur par son pseudo
        var player = _context.PetitBacPlayer.FirstOrDefault(p => p.Pseudo == playerPseudo);
        if (player == null)
        {
            Console.WriteLine($"Joueur introuvable pour le pseudo : {playerPseudo}");
            return NotFound(new { message = "Joueur introuvable." });
        }

        // Vérifier si le joueur a des réponses
        if (string.IsNullOrEmpty(player.ResponsesJson))
        {
            Console.WriteLine($"Aucune réponse trouvée pour le joueur : {playerPseudo}");
            return Ok(new { pseudo = player.Pseudo, responses = new Dictionary<char, Dictionary<string, string>>() });
        }

        // Désérialiser les réponses
        var responses = JsonSerializer.Deserialize<Dictionary<char, Dictionary<string, string>>>(player.ResponsesJson);

        return Ok(new { pseudo = player.Pseudo, responses });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la récupération des réponses : {ex.Message}");
        return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des réponses." });
    }
}


}
}
