using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Models;

namespace GamingPlatform.Data
{
    public class GamingPlatformContext : DbContext
    {
        public GamingPlatformContext(DbContextOptions<GamingPlatformContext> options)
            : base(options)
        {
        }

       public DbSet<GamingPlatform.Models.User> User { get; set; } = default!;
        public DbSet<Joueur> LesPlayers { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<PlayerLobby> PlayerLobbies { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameType> TypeGames { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Sentence> Sentences { get; set; }

        // Utilisation d'un DbSet générique pour permettre l'héritage des jeux.
        public DbSet<SpeedTypingGame> SpeedTypingGames => Set<SpeedTypingGame>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerLobby>()
                .HasKey(pl => new { pl.JoueurId, pl.LobbyId });

            modelBuilder.Entity<PlayerLobby>()
                .HasOne(pl => pl.Joueur)
                .WithMany(u => u.PlayerLobbies)
                .HasForeignKey(pl => pl.JoueurId);

            modelBuilder.Entity<PlayerLobby>()
                .HasOne(pl => pl.Lobby)
                .WithMany(l => l.Players)
                .HasForeignKey(pl => pl.LobbyId);

            modelBuilder.Entity<Lobby>()
                .Property(l => l.Code)
                .IsRequired()
                .HasMaxLength(6);

            modelBuilder.Entity<Lobby>()
                .Property(l => l.Name)
                .IsRequired();

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Lobby)
                .WithMany()
                .HasForeignKey(g => g.LobbyId)
                .IsRequired(false);  // Ceci rend la relation optionnelle

            modelBuilder.Entity<Game>()
                .HasOne(g => g.GameType)
                .WithMany()
                .HasForeignKey(g => g.GameTypeId);

            // Héritage
            modelBuilder.Entity<Game>()
                .HasDiscriminator<string>("GameTypes")
                .HasValue<SpeedTypingGame>("SpeedTyping")
                .HasValue<MorpionGame>("Morpion");

            base.OnModelCreating(modelBuilder);
        }
    }
}
