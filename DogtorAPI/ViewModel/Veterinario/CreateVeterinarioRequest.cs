using DogtorAPI.Model;
using System.Collections.Generic;

namespace DogtorAPI.ViewModel.Veterinario
{
    public class CreateVeterinarioRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
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
        public string Foto_CRMV { get; set; }
        public string CPF { get; set; }
        public string[] Especialidade { get; set; }
        public List<string> Link { get; set; } = new List<string>();

    }
}
