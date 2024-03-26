using System.Reflection.Metadata;

namespace DogtorAPI.Model
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public Guid TutorId { get; set; }
        protected Pet()
        {
            
        }
        public Pet(string name, string race, string color, string description, Guid tutorid)
        {
            Id = Guid.NewGuid();
            Name = name;
            Race = race;
            Color = color;
            Description = description;
            TutorId = tutorid;
        }
    }
}
