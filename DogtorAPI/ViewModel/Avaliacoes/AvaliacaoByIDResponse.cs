namespace DogtorAPI.ViewModel.Avaliacoes
{
    public class AvaliacaoByIDResponse
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public  string Comentario { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid TutorID { get; set; }
        public string TutorName { get; set;}
        public string TutorPhoto { get; set; }
    }
}
