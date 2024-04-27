namespace DogtorAPI.ViewModel.Pet
{
    public class CreatePetRequest
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public Guid TutorID { get; set; }
    }
}
