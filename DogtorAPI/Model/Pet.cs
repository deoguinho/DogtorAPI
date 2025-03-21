﻿using DogtorAPI.ViewModel.Pet;

namespace DogtorAPI.Model
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TypeAnimal { get; set; }
        public string Race { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string? Photo { get; set; } 
        public Guid TutorID { get; set; }
        public ICollection<Consulta>? Consultas { get; set; }

        protected Pet()
        {
            
        }
        public Pet(string name, string typeAnimal, string race, string color, string description, string photo, Guid tutorId)
        {
            Id = Guid.NewGuid();
            Name = name;
            TypeAnimal = typeAnimal;
            Race = race;
            Color = color;
            Description = description;
            Photo = photo;
            TutorID = tutorId;
        }
        public static Pet CreatePetFromPetRequest(CreatePetRequest pet)
        {
            return new(pet.Name, pet.TypeAnimal, pet.Race, pet.Color, pet.Description, pet.Photo, pet.TutorID);
        }
    }
}
