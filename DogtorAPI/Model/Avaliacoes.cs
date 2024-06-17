using DogtorAPI.ViewModel.Avaliacoes;
using DogtorAPI.ViewModel.Consulta;

namespace DogtorAPI.Model
{
    public class Avaliacoes
    {
        public Guid Id { get; set; }
        public string Comentario { get; set; }
        public int Nota { get; set; }
        public Guid VeterinarioID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //Resposta
        public string Resposta { get; set; }
        public DateTime? DataResposta { get; set; } // Data opcional para a resposta

        //Referencia do Tutor
        public Guid TutorID { get; set; }
        public Tutor? Tutor { get; set; }
        public Veterinario? Veterinario { get; set; }

        protected Avaliacoes()
        {

        }

        public Avaliacoes(string comentario, int nota, Guid veterinarioID, Guid tutorID)
        {
            Id = Guid.NewGuid();
            Comentario = comentario;
            Nota = nota;
            VeterinarioID = veterinarioID;
            TutorID = tutorID;
        }

        public static Avaliacoes CreateAvaliacoesFromConsultaRequest(CreateAvaliacoesRequest avaliacao)
        {
            return new(avaliacao.Comentario, avaliacao.Nota, avaliacao.VeterinarioID, avaliacao.TutorID);
        }

    }
}
