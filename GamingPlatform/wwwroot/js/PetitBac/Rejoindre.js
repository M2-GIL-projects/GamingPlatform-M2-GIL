document.addEventListener("DOMContentLoaded", () => {
    const copyButton = document.getElementById('copyButton');
    const playerList = document.getElementById("player-list");
    const modal = document.getElementById("modalCorrection");
    const gameInfo = document.getElementById("game-info");
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/petitbacHub")
        .build();

    // Bouton copier
    copyButton.addEventListener('click', function () {
        const linkField = document.getElementById('playerLink');
        const copyAlert = document.getElementById('copyAlert');

        navigator.clipboard.writeText(linkField.value)
            .then(() => {
                copyAlert.textContent = 'Lien copié dans le presse-papier !';
                copyAlert.style.display = 'block';
                setTimeout(() => copyAlert.style.display = 'none', 3000);
            })
            .catch(err => {
                copyAlert.textContent = 'Erreur lors de la copie : ' + err;
                copyAlert.style.display = 'block';
                setTimeout(() => copyAlert.style.display = 'none', 3000);
            });
    });

    // SignalR
    const gameId = gameInfo.getAttribute("data-game-id");
    const sessionToken = gameInfo.getAttribute("data-session-token");

    connection.on("PlayerStatusUpdated", (playerPseudo, status) => {
        let playerRow = [...playerList.rows].find(row => row.cells[0]?.innerText === playerPseudo);

        if (playerRow) {
            playerRow.cells[1].innerText = status;
            const button = playerRow.cells[2].querySelector("button");
            button.disabled = status !== "Terminé";
        } else {
            const newRow = playerList.insertRow();
            newRow.innerHTML = `
                <td>${playerPseudo}</td>
                <td>${status}</td>
                <td>
                    <button class="btn btn-primary btn-sm" ${status !== "Terminé" ? "disabled" : ""}>
                        Corriger
                    </button>
                </td>`;
            const button = newRow.cells[2].querySelector("button");

            button.setAttribute("onclick", `openCorrectionModal('${playerPseudo}', this.parentElement.parentElement)`);
        }
    });

    connection.start()
        .then(() => connection.invoke("JoinGame", gameId, sessionToken))
        .catch(err => console.error("Erreur de connexion SignalR :", err));

    // Modal correction
    document.addEventListener("DOMContentLoaded", () => {
        const modal = document.getElementById("modalCorrection");
    
        modal.addEventListener("show.bs.modal", function (event) {
            const button = event.relatedTarget; // Bouton déclencheur
            const playerId = button.getAttribute("data-player-id");
            const playerPseudo = button.getAttribute("data-player-pseudo");
    
            // Vérifier si l'ID du joueur est correctement récupéré
            if (!playerId || playerId === "0") {
                console.error("ID du joueur manquant ou incorrect :", playerId);
                alert("Erreur : ID du joueur invalide.");
                return;
            }
    
            console.log("ID du joueur :", playerId); // Debug
            console.log("Pseudo du joueur :", playerPseudo); // Debug
    
            document.getElementById("playerName").textContent = playerPseudo;
    
            // Charger les réponses via l'API
            fetch(`/api/petitbac/getPlayerAnswers?playerId=${playerId}`)
                .then((response) => {
                    if (!response.ok) {
                        throw new Error("Erreur lors de la récupération des réponses du joueur.");
                    }
                    return response.json();
                })
                .then((data) => {
                    const container = document.getElementById("answers-container");
                    container.innerHTML = "";
    
                    for (const [letter, categories] of Object.entries(data.responses)) {
                        for (const [category, answer] of Object.entries(categories)) {
                            container.innerHTML += `
                                <div class="mb-3">
                                    <label>${category} (${letter})</label>
                                    <input type="text" class="form-control" value="${answer}" readonly />
                                    <div class="form-check">
                                        <input class="form-check-input correct-checkbox" type="checkbox" />
                                        <label class="form-check-label">Correct</label>
                                    </div>
                                </div>`;
                        }
                    }
                })
                .catch((err) => {
                    console.error(err);
                    const container = document.getElementById("answers-container");
                    container.innerHTML = `<p class="text-danger">Erreur : Impossible de charger les réponses.</p>`;
                });
        });
    });
    

    // Correction soumise
    document.getElementById("submitCorrection").addEventListener("click", () => {
        const checkboxes = document.querySelectorAll(".correct-checkbox");
        const correctCount = [...checkboxes].filter(cb => cb.checked).length;
        const total = checkboxes.length;
        alert(`Score calculé : ${(correctCount / total) * 100}%`);
    });
});

function openCorrectionModal(playerPseudo) {
    // Mettre à jour les informations dans le modal
    const modalTitle = document.getElementById("playerName");
    modalTitle.textContent = playerPseudo;

    // Charger les réponses du joueur via une API basée sur le pseudo
    fetch(`/api/petitbac/getPlayerAnswersByPseudo?playerPseudo=${encodeURIComponent(playerPseudo)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Erreur lors de la récupération des réponses du joueur.");
            }
            return response.json();
        })
        .then(data => {
            const container = document.getElementById("answers-container");
            container.innerHTML = ""; // Nettoyer le conteneur existant

            // Afficher les réponses
            for (const [letter, categories] of Object.entries(data.responses)) {
                for (const [category, answer] of Object.entries(categories)) {
                    container.innerHTML += `
                        <div class="mb-3">
                            <label>${category} (${letter})</label>
                            <input type="text" class="form-control" value="${answer}" readonly />
                            <div class="form-check">
                                <input class="form-check-input correct-checkbox" type="checkbox" />
                                <label class="form-check-label">Correct</label>
                            </div>
                        </div>`;
                }
            }

            // Afficher le modal Bootstrap
            const modal = new bootstrap.Modal(document.getElementById("modalCorrection"));
            modal.show();
        })
        .catch(err => {
            console.error("Erreur lors du chargement des réponses :", err);
            const container = document.getElementById("answers-container");
            container.innerHTML = `<p class="text-danger">Erreur : Impossible de charger les réponses.</p>`;
        });
}
