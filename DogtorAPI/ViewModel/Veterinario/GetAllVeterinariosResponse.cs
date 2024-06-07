using DogtorAPI.Model;

namespace DogtorAPI.ViewModel.Veterinario
{
    public class GetAllVeterinariosResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Birth { get; set; }
        public string Phone { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string UF { get; set; }
        public string CRMV { get; set; }
        public string CPF { get; set; }
        public int QuantidadeAvaliacoes { get; set; }
        public double MediaAvaliacoes { get; set; }
        public int Status { get; set; }
        public List<Especialidade> Especialidades { get; set; }
        public List<VeterinarioFotos> VeterinarioFotos { get; set; }
    }
}
