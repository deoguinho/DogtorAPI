using DogtorAPI.ViewModel.Consulta;
using DogtorAPI.ViewModel.Pet;

namespace DogtorAPI.Model
{
    public class Consulta
    {
        public Guid ConsultaId { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string? Observacoes { get; set; }

        // Chaves estrangeiras
        public Guid VeterinarioId { get; set; }
        public Guid TutorId { get; set; }
        public Guid PetId { get; set; }

        // Propriedades de navegação
        public Veterinario? Veterinario { get; set; }
        public Tutor? Tutor { get; set; }
        public Pet? Pet { get; set; }

        public Consulta(DateTime data, string status, string? observacoes, Guid veterinarioId, Guid tutorId, Guid petId)
        {
            ConsultaId = Guid.NewGuid();
            Data = data;
            Status = status;
            Observacoes = observacoes;
            VeterinarioId = veterinarioId;
            TutorId = tutorId;
            PetId = petId;
        }
        public static Consulta CreateConsultaFromConsultaRequest(CreateConsultaRequest consulta)
        {
            return new(consulta.Data, "PENDENTE", consulta.Observacoes, consulta.VeterinarioId, consulta.TutorId, consulta.PetId);
        }
    }
}
