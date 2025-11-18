namespace SaveursInedites_Jalon2.Domain.BO
// Namespace contenant les objets métiers du domaine.
{
    public class Ingredient
    // Classe représentant un ingrédient dans le modèle métier.
    {
        // Identifiant unique (SERIAL en base)
        public int Id { get; set; }
        // Clé primaire générée automatiquement en base de données.

        // Nom de l'ingredient (VARCHAR(100) NOT NULL)
        public string Nom { get; set; } = string.Empty;
        // Intitulé de l’ingrédient. Toujours non nul grâce à une valeur par défaut.
    }
}
