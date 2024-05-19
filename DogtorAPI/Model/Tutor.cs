namespace DogtorAPI.Model
{
    public class Tutor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Birth { get; set; }
        public string CPF { get; set; }
        public string Phone { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string? Photo { get; set; }
        public ICollection<Pet>? Pets { get; set; }
        public ICollection<Consulta>? Consultas { get; set; }
        public ICollection<Avaliacoes>? Avaliacoes { get; set; }


        protected Tutor()
        {
            
        }
        public Tutor(Guid id, string name, string email, string birth, string cpf, string phone, string cep, string street, int number, string city, string complement, string neighborhood, string photo)
        {
            Id = id;
            Name = name;
            Email = email;
            Birth = birth;
            CPF = cpf;
            Phone = phone;
            Cep = cep;
            Street = street;
            Number = number;
            City = city;
            Complement = complement;
            Neighborhood = neighborhood;
            Photo = photo;
        }
    }
}
