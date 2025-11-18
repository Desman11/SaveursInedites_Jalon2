namespace SaveursInedites_Jalon2.Domain.DTO.DTOOut
// Namespace regroupant les DTO envoyés depuis l’API vers le client.
{
    public class IngredientDTO
    // Représentation simplifiée d’un ingrédient destinée à la sortie (réponse API).
    {
        public int Id { get; set; }
        // Identifiant unique de l’ingrédient.

        public string Nom { get; set; }
        // Nom de l’ingrédient (ex. "Tomate", "Sel", "Pâtes", etc.).
    }
}
