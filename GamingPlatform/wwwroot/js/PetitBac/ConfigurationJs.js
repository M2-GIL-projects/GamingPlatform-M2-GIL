
document.addEventListener("DOMContentLoaded", function () {
const form = document.getElementById("configureGameForm");
const pseudoInput = document.getElementById("CreatorPseudo");
const timeLimitInput = document.getElementById("TimeLimit");
const endConditionDropdown = document.getElementById("EndCondition");
const categoriesContainer = document.getElementById("categoriesContainer");
const categoryCheckboxes = document.querySelectorAll(".category-checkbox");

const pseudoError = document.getElementById("pseudoError");
const timeLimitError = document.getElementById("timeLimitError");
const categoryError = document.getElementById("categoryError");

// Fonction pour afficher/masquer le champ Temps Limite
const toggleTimeLimitSection = () => {
const timeLimitSection = document.getElementById("TimeLimitSection");
if (endConditionDropdown.value === "TimeLimit") {
    timeLimitSection.style.display = "block";
} else {
    timeLimitSection.style.display = "none";
}
};

endConditionDropdown.addEventListener("change", toggleTimeLimitSection);
toggleTimeLimitSection();

form.addEventListener("submit", function (e) {
let isValid = true;

// Validation du pseudo
const pseudoValue = pseudoInput.value.trim();
if (pseudoValue === "") {
    pseudoError.classList.remove("d-none");
    pseudoError.textContent = "Le pseudo est obligatoire.";
    isValid = false;
} else if (pseudoValue.length < 5) {
    pseudoError.classList.remove("d-none");
    pseudoError.textContent = "Le pseudo doit contenir au moins 5 caractères.";
    isValid = false;
  } else if (!/^[a-zA-Z]+$/.test(pseudoValue)) {
// Vérifier si le pseudo contient uniquement des lettres
    pseudoError.textContent = 'Le pseudo ne peut contenir que des lettres de l\'alphabet.';
    pseudoError.classList.remove("d-none"); // Afficher l'erreur
    isValid = false;
} else {
    pseudoError.classList.add("d-none");
}

// Validation du temps limite
if (endConditionDropdown.value === "TimeLimit") {
    const timeLimitValue = parseInt(timeLimitInput.value);
    if (isNaN(timeLimitValue) || timeLimitValue < 1 || timeLimitValue > 30) {
        timeLimitError.classList.remove("d-none");
        timeLimitError.textContent = "Le temps limite doit être entre 1 et 30 minutes.";
        isValid = false;
    } else {
        timeLimitError.classList.add("d-none");
    }
}

// Validation des catégories
const selectedCategories = Array.from(categoryCheckboxes).filter(checkbox => checkbox.checked);
if (selectedCategories.length < 3) {
    categoryError.classList.remove("d-none");
    isValid = false;
} else {
    categoryError.classList.add("d-none");
}

// Empêche l'envoi si le formulaire n'est pas valide
if (!isValid) {
    e.preventDefault();
}
});
});

