using System.ComponentModel.DataAnnotations;

namespace DogtorAPI.ViewModel.Pet
{
    public class CreatePetResponse
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Race { get; set; }
        [Required]
        public string Color { get; set; }
        public string Description { get; set; }
        public Guid TutorID { get; set; }
    }
}
