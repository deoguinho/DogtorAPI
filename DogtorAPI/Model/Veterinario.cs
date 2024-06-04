using System.Collections.Generic;

namespace DogtorAPI.Model
{
    public class Veterinario
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
        public string Foto_CRMV { get; set; }
        public string CPF { get; set; }
        public bool Status { get; set; }
        public List<Especialidade>? Especialidade { get; set; }
        public List<VeterinarioFotos>? VeterinarioFotos { get; set; }
        public ICollection<Consulta>? Consultas { get; set; }
        public ICollection<Avaliacoes>? Avaliacoes { get; set; }

        protected Veterinario()
        {
        }

        public Veterinario(Guid id, string name, string email, string birth, string phone, string cep, 
            string street, int number, string city, string complement, string neighborhood, string uf, string crmv, string foto_CRMV, string cpf)
        {
            Id = id;
            Name = name;
            Email = email;
            Birth = birth;
            Phone = phone;
            Cep = cep;
            Street = street;
            Number = number;
            City = city;
            Complement = complement;
            Neighborhood = neighborhood;

            UF = uf;
            CRMV = crmv;
            Foto_CRMV = foto_CRMV;
            CPF = cpf;
            Status = false;
        }
    }
}
