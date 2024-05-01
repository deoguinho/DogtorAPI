namespace DogtorAPI.Model
{
    public class Consulta
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid TutorID { get; set; }
        public Guid VeterinarioID { get; set; }

        public Consulta(string date, string hour, string description, string status, Guid tutorID, Guid veterinarioID)
        {
            Date = date;
            Hour = hour;
            Description = description;
            Status = status;
            TutorID = tutorID;
            VeterinarioID = veterinarioID;
        }
    }
}
