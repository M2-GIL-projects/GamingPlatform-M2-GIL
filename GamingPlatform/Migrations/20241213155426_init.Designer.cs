﻿// <auto-generated />
using System;
using GamingPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GamingPlatform.Migrations
{
    [DbContext(typeof(GamingPlatformContext))]
    [Migration("20241213155426_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GamingPlatform.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Game");
                });

            modelBuilder.Entity("GamingPlatform.Models.Lobby", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("GameCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GameType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameCode");

                    b.ToTable("Lobby");
                });

            modelBuilder.Entity("GamingPlatform.Models.LobbyPlayer", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<Guid>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("PlayerId", "LobbyId");

                    b.HasIndex("LobbyId");

                    b.ToTable("LobbyPlayer");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PetitBacGameId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PetitBacGameId");

                    b.ToTable("PetitBacCategories");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorPseudo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsGameStarted")
                        .HasColumnType("bit");

                    b.PrimitiveCollection<string>("Letters")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PlayerCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LobbyId")
                        .IsUnique();

                    b.ToTable("PetitBacGames");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsReady")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PetitBacGameId")
                        .HasColumnType("int");

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponsesJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<string>("SessionToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PetitBacGameId");

                    b.ToTable("PetitBacPlayer");
                });

            modelBuilder.Entity("GamingPlatform.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("GamingPlatform.Models.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Accuracy")
                        .HasColumnType("real");

                    b.Property<DateTime>("DatePlayed")
                        .HasColumnType("datetime2");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WPM")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Score");
                });

            modelBuilder.Entity("GamingPlatform.Models.Sentence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sentences");
                });

            modelBuilder.Entity("GamingPlatform.Models.Lobby", b =>
                {
                    b.HasOne("GamingPlatform.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameCode")
                        .HasPrincipalKey("Code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GamingPlatform.Models.LobbyPlayer", b =>
                {
                    b.HasOne("GamingPlatform.Models.Lobby", "Lobby")
                        .WithMany("LobbyPlayers")
                        .HasForeignKey("LobbyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamingPlatform.Models.Player", "Player")
                        .WithMany("LobbyPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lobby");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacCategory", b =>
                {
                    b.HasOne("GamingPlatform.Models.PetitBacGame", "PetitBacGame")
                        .WithMany("Categories")
                        .HasForeignKey("PetitBacGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PetitBacGame");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacGame", b =>
                {
                    b.HasOne("GamingPlatform.Models.Lobby", "Lobby")
                        .WithOne("PetitBacGame")
                        .HasForeignKey("GamingPlatform.Models.PetitBacGame", "LobbyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lobby");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacPlayer", b =>
                {
                    b.HasOne("GamingPlatform.Models.PetitBacGame", "PetitBacGame")
                        .WithMany("Players")
                        .HasForeignKey("PetitBacGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PetitBacGame");
                });

            modelBuilder.Entity("GamingPlatform.Models.Lobby", b =>
                {
                    b.Navigation("LobbyPlayers");

                    b.Navigation("PetitBacGame");
                });

            modelBuilder.Entity("GamingPlatform.Models.PetitBacGame", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("GamingPlatform.Models.Player", b =>
                {
                    b.Navigation("LobbyPlayers");
                });
#pragma warning restore 612, 618
        }
    }
}
