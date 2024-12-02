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
        public DbSet<Game> Game { get; set; } 
        public DbSet<Lobby> Lobby { get; set; }
        public DbSet<LobbyPlayer> LobbyPlayer { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<PetitBacGame> PetitBacGames { get; set; }
        public DbSet<Answer> Answers { get; set; }



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

                // Ajouter kes realation de petitBacplayer et petitbac        

    // Configuration pour PetitBacGame
    modelBuilder.Entity<PetitBacGame>()
        .HasOne(pbg => pbg.Lobby)
        .WithOne(l => l.PetitBacGame)
        .HasForeignKey<PetitBacGame>(pbg => pbg.LobbyId);

    // Configuration pour PetitBacPlayer
    modelBuilder.Entity<PetitBacPlayer>()
        .HasOne(pbp => pbp.PetitBacGame)
        .WithMany(pbg => pbg.Players)
        .HasForeignKey(pbp => pbp.PetitBacGameId);

    base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);

    //Configurez la relation entre Answer et PetitBacPlayer 
    modelBuilder.Entity<Answer>()
        .HasOne(a => a.PetitBacPlayer)
        .WithMany(p => p.Answers)
        .HasForeignKey(a => a.PetitBacPlayerId);

        }
    }
    }



