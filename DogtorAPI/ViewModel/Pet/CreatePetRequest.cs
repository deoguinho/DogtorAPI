using DogtorAPI.ViewModel.Tutor;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace DogtorAPI.ViewModel.Pet
{
    public class CreatePetRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Race { get; set; }

        [Required]
        public string Color { get; set; }

        public string Description { get; set; }


    }
}
