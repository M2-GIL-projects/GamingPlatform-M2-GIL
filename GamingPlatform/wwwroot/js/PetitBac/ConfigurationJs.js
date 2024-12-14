document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("configureGameForm");

    if (!form) {
        console.error("Formulaire introuvable. Vérifiez l'ID.");
        return;
    }

    form.addEventListener("submit", function (event) {
        let hasError = false;

        // Validation des lettres sélectionnées
        const selectedLetters = document.querySelectorAll("input[name='SelectedLetters']:checked");
        const lettersError = document.getElementById("lettersError");

        if (!lettersError) {
            console.error("Élément lettersError introuvable dans le DOM.");
        } else if (selectedLetters.length === 0) {
            lettersError.classList.remove("d-none");
            lettersError.style.display = "block";
            console.log("Aucune lettre sélectionnée.");
            hasError = true;
        } else {
            lettersError.classList.add("d-none");
            lettersError.style.display = "none";
            console.log("Validation des lettres réussie.");
        }

        // Validation des catégories sélectionnées
        const selectedCategories = document.querySelectorAll("input[name='SelectedCategories']:checked");
        const categoryError = document.getElementById("categoryError");

        if (!categoryError) {
            console.error("Élément categoryError introuvable dans le DOM.");
        } else if (selectedCategories.length < 3) {
            categoryError.classList.remove("d-none");
            categoryError.style.display = "block";
            console.log("Moins de trois catégories sélectionnées.");
            hasError = true;
        } else {
            categoryError.classList.add("d-none");
            categoryError.style.display = "none";
            console.log("Validation des catégories réussie.");
        }

        // Empêche la soumission si une erreur est détectée
        if (hasError) {
            event.preventDefault();
            console.log("Formulaire non soumis en raison des erreurs.");
        }
    });

    // Gestion des événements sur les lettres
    document.querySelectorAll("input[name='SelectedLetters']").forEach(input => {
        input.addEventListener("change", function () {
            const lettersError = document.getElementById("lettersError");
            if (lettersError && document.querySelectorAll("input[name='SelectedLetters']:checked").length > 0) {
                lettersError.classList.add("d-none");
                lettersError.style.display = "none";
                console.log("Erreur de lettres masquée.");
            }
        });
    });

    // Gestion des événements sur les catégories
    document.querySelectorAll("input[name='SelectedCategories']").forEach(input => {
        input.addEventListener("change", function () {
            const categoryError = document.getElementById("categoryError");
            if (categoryError && document.querySelectorAll("input[name='SelectedCategories']:checked").length >= 3) {
                categoryError.classList.add("d-none");
                categoryError.style.display = "none";
                console.log("Erreur de catégories masquée.");
            }
        });
    });
});
