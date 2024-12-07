﻿using System;
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

        public DbSet<Player> Player { get; set; }
        public DbSet<Lobby> Lobby { get; set; }
        public DbSet<Game> Game { get; set; }

        public DbSet<LobbyPlayer> LobbyPlayer { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<Sentence> Sentences { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Contrainte d'unicité pour le champ Code dans Games
            modelBuilder.Entity<Game>()
                .HasIndex(g => g.Code)
                .IsUnique();

            // Configuration de la relation plusieurs-à-plusieurs entre Player et Lobby
            modelBuilder.Entity<LobbyPlayer>()
                .HasKey(lp => new { lp.PlayerId, lp.LobbyId });

            modelBuilder.Entity<LobbyPlayer>()
                .HasOne(lp => lp.Player)
                .WithMany(p => p.LobbyPlayers)
                .HasForeignKey(lp => lp.PlayerId);

            modelBuilder.Entity<LobbyPlayer>()
                .HasOne(lp => lp.Lobby)
                .WithMany(l => l.LobbyPlayers)
                .HasForeignKey(lp => lp.LobbyId);

            // Relation entre Lobby et Game via GameCode
            modelBuilder.Entity<Lobby>()
                .HasOne(l => l.Game)
                .WithMany()
                .HasForeignKey(l => l.GameCode)
                .HasPrincipalKey(g => g.Code)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
            
    }
}
