using GamingPlatform.Hubs;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Data;
using GamingPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration du contexte de base de données
builder.Services.AddDbContext<GamingPlatformContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GamingPlatformContext") ?? 
        throw new InvalidOperationException("Connection string 'GamingPlatformContext' not found.")));

// Ajout des services au conteneur
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // Ajouter SignalR une seule fois
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<LobbyService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<GameSeeder>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(90); // Durée de la session
    options.Cookie.HttpOnly = true; // Sécuriser le cookie
});

var app = builder.Build();

// Configurez le pipeline des requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Initialisation des données
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<GameSeeder>();
    seeder.SeedGames();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthorization();

// Mapping des routes de contrôleur
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "game-details",
    pattern: "game/details/{id:guid}",
    defaults: new { controller = "Game", action = "Details" });

app.MapControllerRoute(
    name: "create-lobby-select",
    pattern: "lobby/create/select",
    defaults: new { controller = "Lobby", action = "CreateWithSelect" });

app.MapControllerRoute(
    name: "create-lobby-game",
    pattern: "lobby/create/game/{gameCode}",
    defaults: new { controller = "Lobby", action = "CreateFromGame" });

app.MapControllerRoute(
    name: "gameLobbies",
    pattern: "game/{gameCode}/lobbies",
    defaults: new { controller = "Game", action = "LobbiesByGameCode" });

// Nouvelle route pour PetitBacController.Configure
app.MapControllerRoute(
    name: "petitbac-configure",
    pattern: "petitbac/configure",
    defaults: new { controller = "PetitBac", action = "Configure" });

app.MapControllerRoute(
    name: "recapitulatif",
    pattern: "PetitBac/Recapitulatif/{gameId}",
    defaults: new { controller = "PetitBac", action = "Recapitulatif" });

// Ajout des hubs SignalR
app.MapHub<PetitBacHub>("/petitbachub"); 

// Lancer l'application
app.Run();