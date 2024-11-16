using System;
using System.Collections.Generic;
using GamingPlatform.Data;
using GamingPlatform.Models;
using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamingPlatform.Models
{
    public class GameService : IGameService
    {
        private readonly GamingPlatformContext _context;

        public GameService(GamingPlatformContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetGameTypes()
{
    return _context.TypeGames
        .Select(gt => new SelectListItem { Value = gt.Id.ToString(), Text = gt.Name })
        .ToList();
}




        public List<Game> GetAvailableGames()
        {
            return _context.Games.ToList();
        }
        public Game CreateGame(GameCreateModel model)
        {
            Game newGame;
            switch (model.GameType)
            {
                case "SpeedTyping":
                    newGame = new SpeedTypingGame();
                    break;
                case "Morpion":
                    newGame = new MorpionGame();
                    break;
                // Ajoutez d'autres cas pour les différents types de jeux
                default:
                    throw new ArgumentException("Type de jeu non reconnu");
            }

            newGame.Id = Guid.NewGuid();
            newGame.Name = model.Name;
            newGame.Description = model.Description;
            newGame.ImageUrl = model.ImageUrl;
            newGame.Status = GameStatus.NotStarted;
            // Ne définissez pas de LobbyId ici

            _context.Games.Add(newGame);
            _context.SaveChanges();
            return newGame;
        }

        public Game UpdateGame(Guid id, GameUpdateModel model)
        {
            var game = _context.Games.Find(id);
            if (game == null)
                return null;

            game.Name = model.Name;
            game.Description = model.Description;
            game.ImageUrl = model.ImageUrl;

            _context.SaveChanges();
            return game;
        }

        public bool DeleteGame(Guid id)
        {
            var game = _context.Games.Find(id);
            if (game == null)
                return false;

            _context.Games.Remove(game);
            _context.SaveChanges();
            return true;
        }

       
        public void CreateGameType(string name)
    {
        var gameType = new GameType 
        { 
            Name = name
        };
        _context.TypeGames.Add(gameType);
        _context.SaveChanges();
    }


        public Game GetGameById(Guid id)
        {
            return _context.Games.Find(id);
        }
    }



    public class GameCreateModel
    {
        public string GameType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GameUpdateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
