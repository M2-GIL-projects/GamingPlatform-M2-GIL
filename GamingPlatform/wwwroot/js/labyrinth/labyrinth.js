const connection = new signalR.HubConnectionBuilder()
    .withUrl("/labyrinthHub")
    .build();

// Fonction de génération du labyrinthe
function generateLabyrinth(adjacency, rows, cols) {
    const labyrinth = document.getElementById('labyrinth');
    labyrinth.innerHTML = ''; // Nettoie le conteneur avant d'ajouter le labyrinthe

    for (let i = 0; i < rows; i++) {
        for (let j = 0; j < cols; j++) {
            const cell = document.createElement('div');
            cell.classList.add('cell');

            const currentCell = i * cols + j;
            if (!adjacency[currentCell][currentCell - cols]) {
                cell.classList.add('wall-top');
            }
            if (!adjacency[currentCell][currentCell + 1] && j < cols - 1) {
                cell.classList.add('wall-right');
            }
            if (!adjacency[currentCell][currentCell + cols] && i < rows - 1) {
                cell.classList.add('wall-bottom');
            }
            if (!adjacency[currentCell][currentCell - 1] && j > 0) {
                cell.classList.add('wall-left');
            }

            labyrinth.appendChild(cell);
        }
    }
}

// Recevoir la matrice et appeler la fonction de génération
connection.on("ReceiveLabyrinth", function (adjacency) {
    const rows = 10; // Ajuster en fonction des dimensions du labyrinthe
    const cols = 10;
    generateLabyrinth(adjacency, rows, cols);
});

// Démarrer la connexion
connection.start().catch(function (err) {
    return console.error(err.toString());
});
