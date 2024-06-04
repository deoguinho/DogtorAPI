namespace DogtorAPI.ViewModel.Tutor
{
    public class GetAllTutorResponse
    {
        public class PetDto
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
        }

        public class ConsultaDto
        {
            public Guid Id { get; set; }
            public DateTime Data { get; set; }
        }

        public class TutorDto
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public ICollection<PetDto> Pets { get; set; }
            public ICollection<ConsultaDto> Consultas { get; set; }
        }
    }
}
