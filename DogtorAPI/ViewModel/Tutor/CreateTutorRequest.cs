using DogtorAPI.ViewModel.Pet;
using System.ComponentModel.DataAnnotations;

namespace DogtorAPI.ViewModel.Tutor
{
    public class CreateTutorRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime Birth { get; set; }

        [Required]
        [StringLength(11)]
        public string CPF { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [StringLength(8)]
        public string Cep { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Complement { get; set; }

        [Required]
        public string Neighborhood { get; set; }
        public ICollection<CreatePetRequest>? createPetRequests { get; set; }
    }
}
