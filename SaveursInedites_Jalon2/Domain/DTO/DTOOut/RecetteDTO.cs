namespace SaveursInedites_Jalon2.Domain.DTO.DTOOut
{
    public class RecetteDTO
    {
        public int Id { get; set; }

        public string Nom { get; set; } 

        public TimeSpan TempsPreparation { get; set; }

        public TimeSpan TempsCuisson { get; set; } 

        public int Difficulte { get; set; }

        public string? Photo { get; set; }

        public int Createur { get; set; }

        public int Role{ get; set; }
    }
}
