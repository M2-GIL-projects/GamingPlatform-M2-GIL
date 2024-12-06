document.getElementById('copyButton').addEventListener('click', function () {
    const linkField = document.getElementById('playerLink');
    linkField.select();
    linkField.setSelectionRange(0, 99999); // Pour compatibilité mobile

    // Récupérer le conteneur de l'alerte
    const copyAlert = document.getElementById('copyAlert');

    // Essayer de copier le texte dans le presse-papier
    navigator.clipboard.writeText(linkField.value).then(function () {
        // Afficher le message d'alerte personnalisé
        copyAlert.textContent = 'Lien copié dans le presse-papier !';
        copyAlert.style.display = 'block'; // Afficher l'alerte

        // Optionnel : cacher l'alerte après un délai
        setTimeout(function() {
            copyAlert.style.display = 'none';
        }, 3000); // Masquer après 3 secondes
    }).catch(function (err) {
        // En cas d'erreur, afficher un message d'erreur
        copyAlert.textContent = 'Erreur lors de la copie : ' + err;
        copyAlert.style.display = 'block'; // Afficher l'alerte

        // Optionnel : cacher l'alerte après un délai
        setTimeout(function() {
            copyAlert.style.display = 'none';
        }, 3000); // Masquer après 3 secondes
    });
});
