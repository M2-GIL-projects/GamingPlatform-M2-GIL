// Ajout de l'événement pour le bouton "Rejoindre"
document.getElementById('joinButton').addEventListener('click', function () {
    // Récupérer le champ du pseudo
    const pseudoField = document.getElementById('Pseudo');
    const pseudoValue = pseudoField.value.trim();
    const pseudoError = document.getElementById('pseudoError');

    // Récupérer le pseudo du créateur
    const creatorPseudoField = document.getElementById('creatorPseudo');
    const creatorPseudo = creatorPseudoField ? creatorPseudoField.value.trim() : '';

    // Réinitialiser l'affichage des erreurs
    pseudoError.classList.add("d-none");

    // Vérification : Le pseudo ne doit pas être identique à celui du créateur
    if (creatorPseudo && pseudoValue.toLowerCase() === creatorPseudo.toLowerCase()) {
        pseudoError.textContent = 'Votre pseudo ne peut pas être le même que celui du créateur.';
        pseudoError.classList.remove("d-none");
        return;
    }

    // Vérifications supplémentaires
    if (pseudoValue === '') {
        pseudoError.textContent = 'Veuillez saisir un pseudo avant de rejoindre la partie.';
        pseudoError.classList.remove("d-none");
        return;
    }

    if (pseudoValue.length < 5) {
        pseudoError.textContent = 'Le pseudo doit contenir au moins 5 caractères.';
        pseudoError.classList.remove("d-none");
        return;
    }

    if (!/^[a-zA-Z]/.test(pseudoValue)) {
        pseudoError.textContent = 'Le pseudo doit commencer par une lettre.';
        pseudoError.classList.remove("d-none");
        return;
    }

    // Si tout est valide, soumettre le formulaire
    document.getElementById('joinForm').submit();
});

// Initialisation de SignalR et gestion des événements
document.addEventListener("DOMContentLoaded", () => {
    // Récupérer les données injectées dans la vue
    const gameInfo = document.getElementById("game-info");
    const gameId = gameInfo.getAttribute("data-game-id");
    const sessionToken = gameInfo.getAttribute("data-session-token");

    console.log("Game ID récupéré :", gameId);
    console.log("Session Token récupéré :", sessionToken);

    // Initialiser la connexion SignalR
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/petitbacHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Gestion de l'événement SignalR "PlayerStatusUpdated"
    connection.on("PlayerStatusUpdated", (playerPseudo, status) => {
        console.log(`[SignalR] Événement reçu : Joueur=${playerPseudo}, Statut=${status}`);

        const playerList = document.getElementById("player-list");
        if (playerList) {
            // Rechercher une ligne existante pour ce joueur
            let playerRow = [...playerList.rows].find(row => row.cells[0]?.innerText === playerPseudo);

            if (playerRow) {
                // Mettre à jour le statut si le joueur existe déjà
                playerRow.cells[1].innerText = status;
            } else {
                // Ajouter une nouvelle ligne pour le joueur
                const newRow = playerList.insertRow();
                const nameCell = newRow.insertCell(0);
                const statusCell = newRow.insertCell(1);
                nameCell.innerText = playerPseudo;
                statusCell.innerText = status;
            }
        } else {
            console.warn("Table 'player-list' introuvable.");
        }
    });

    // Connexion au hub SignalR
    connection.start().then(() => {
        console.log("Connexion SignalR établie avec succès.");
    }).catch(err => {
        console.error("Erreur de connexion à SignalR :", err);
    });
});
