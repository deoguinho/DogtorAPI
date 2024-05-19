namespace DogtorAPI.ViewModel.Tutor
{
    public class PutTutorRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
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
    }
}
