using GamingPlatform.Hubs;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Data;
using GamingPlatform.Services;
using GamingPlatform.Models;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GamingPlatformContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GamingPlatformContext") ?? throw new InvalidOperationException("Connection string 'GamingPlatformContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<LobbyService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<GameSeeder>();
builder.Services.AddSingleton<SpeedTyping>();
builder.Services.AddSingleton(new ConcurrentDictionary<string, string>());

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(90); // Dur�e de la session
    options.Cookie.HttpOnly = true; // S�curiser le cookie
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Initialiser les donn�es
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "game-details",
    pattern: "game/details/{id?}",
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

app.MapHub<ChatHub>("/chatHub");
app.MapHub<LabyrinthHub>("/labyrinthHub");

// Redirection pour le speed Typing game
app.MapHub<SpeedTypingHub>("/SpeedTypingHub");

app.Run();
