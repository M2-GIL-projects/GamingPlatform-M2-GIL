using System.Collections.Generic;
using GamingPlatform.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamingPlatform.Services
{
   public interface IGameService
    {
        //creer un type de jeux
        void CreateGameType(string name);

         List<SelectListItem> GetGameTypes();

        // Récupère la liste des jeux disponibles sur la plateforme.
        List<Game> GetAvailableGames();

        // Créer un nouveau jeu
        Game CreateGame(GameCreateModel model);

        // Modifier un jeu existant
        Game UpdateGame(Guid id, GameUpdateModel model);

        // Supprimer un jeu
        bool DeleteGame(Guid id);

        // Récupérer un jeu spécifique
        Game GetGameById(Guid id);
    }
    
    
}
