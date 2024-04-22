namespace DogtorAPI.Model
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public Guid TutorID { get; set; }
        protected Pet()
        {
            
        }
        public Pet(string name, string race, string color, string description, Guid TutorID)
        {
            Id = Guid.NewGuid();
            Name = name;
            Race = race;
            Color = color;
            Description = description;
        }
    }
}
