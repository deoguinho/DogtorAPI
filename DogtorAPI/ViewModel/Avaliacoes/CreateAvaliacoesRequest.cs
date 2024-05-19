namespace DogtorAPI.ViewModel.Avaliacoes
{
    public class CreateAvaliacoesRequest
    {
        public string Comentario { get; set; }
        public int Nota { get; set; }
        public Guid VeterinarioID { get; set; }
        public Guid TutorID { get; set; }

    }
}
