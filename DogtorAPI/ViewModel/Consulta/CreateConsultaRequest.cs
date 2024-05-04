namespace DogtorAPI.ViewModel.Consulta
{
    public class CreateConsultaRequest
    {
        public Guid ConsultaId { get; set; }
        public DateTime Data { get; set; }
        public string? Observacoes { get; set; }

        // Chaves estrangeiras
        public Guid VeterinarioId { get; set; }
        public Guid TutorId { get; set; }
        public Guid PetId { get; set; }
    }
}
