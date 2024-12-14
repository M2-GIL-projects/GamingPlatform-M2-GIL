// Établir la connexion SignalR
var connection = new signalR.HubConnectionBuilder().withUrl("/labyrinthHub").build();

// Démarrer la connexion SignalR
connection.start().catch(function (err) {
    return console.error(err.toString());
});

// Fonction pour générer et envoyer le labyrinthe
function generateAndSendLabyrinth() {
    var nRows = 25;
    var nCols = 25;

    // Générer le labyrinthe
    var labyrinth = labyrinthgenerator(nRows, nCols);

    // Convertir le labyrinthe en JSON
    var labyrinthJson = JSON.stringify(labyrinth);

    // Envoyer le labyrinthe au serveur
    connection.invoke("SaveLabyrinth", labyrinthJson).catch(function (err) {
        return console.error(err.toString());
    });
}

// Ajouter un écouteur d'événements au bouton
document.getElementById("generateLabyrinthButton").addEventListener("click", generateAndSendLabyrinth);
