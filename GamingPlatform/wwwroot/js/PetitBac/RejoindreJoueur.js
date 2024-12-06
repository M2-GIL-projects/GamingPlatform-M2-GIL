document.getElementById('joinButton').addEventListener('click', function () {
    // Récupérer le champ du pseudo
    var pseudoField = document.getElementById('Pseudo');
    var pseudoValue = pseudoField.value.trim();
    
    // Récupérer le conteneur de l'erreur
    var pseudoError = document.getElementById('pseudoError');
    
    // Réinitialiser l'affichage de l'erreur (masquer les messages d'erreur au départ)
    pseudoError.classList.add("d-none");
    
    // Vérifier si le champ du pseudo est vide
    if (pseudoValue === '') {
        pseudoError.textContent = 'Veuillez saisir un pseudo avant de rejoindre la partie.';
        pseudoError.classList.remove("d-none"); // Afficher l'erreur
    } else if (pseudoValue.length < 5) {
        // Vérifier si le pseudo a moins de 5 caractères
        pseudoError.textContent = 'Le pseudo doit contenir au moins 5 caractères.';
        pseudoError.classList.remove("d-none"); // Afficher l'erreur
    } else if (!/^[a-zA-Z]/.test(pseudoValue)) {
        // Vérifier si le pseudo commence par une lettre
        pseudoError.textContent = 'Le pseudo doit commencer par une lettre.';
        pseudoError.classList.remove("d-none"); // Afficher l'erreur
    } else {
        // Si le pseudo est valide, soumettre le formulaire
        document.getElementById('joinForm').submit();
    }
});
