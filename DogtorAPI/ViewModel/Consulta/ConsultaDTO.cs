namespace DogtorAPI.ViewModel.Consulta
{
    public class ConsultaDTO
    {
        public Guid ConsultaId { get; set; }
        public DateTime Data { get; set; }
        public string Observacoes { get; set; }
        public  string Status { get; set; }
        public Guid TutorID { get; set; }
        public string TutorNome { get; set; }
        public Guid VeterinarioID { get; set; }
        public string VeterinarioNome { get; set; }
        public Guid PetID { get; set; }
        public string PetNome { get; set; }
        // Adicione outras propriedades conforme necessário
    }
}
