namespace SaveursInedites_Jalon2.Domain.BO
{
    public class Ingredient
    {
        // Identifiant unique (SERIAL en base)
        public int Id { get; set; }

        // Nom de la recette (VARCHAR(100) NOT NULL)
        public string Nom { get; set; } = string.Empty;
    }
}
