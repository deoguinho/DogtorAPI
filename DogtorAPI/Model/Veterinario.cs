namespace DogtorAPI.Model
{
    public class Veterinario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }
        public string CRMV { get; set; }
        public string Phone { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }

        protected Veterinario()
        {
        }

        public Veterinario(Guid id, string name, string email, DateTime birth, string crmv, string phone, string cep, string street, int number, string city, string complement, string neighborhood)
        {
            Id = id;
            Name = name;
            Email = email;
            Birth = birth;
            CRMV = crmv;
            Phone = phone;
            Cep = cep;
            Street = street;
            Number = number;
            City = city;
            Complement = complement;
            Neighborhood = neighborhood;
        }
    }
}
