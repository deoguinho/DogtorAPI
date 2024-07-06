namespace DogtorAPI.Model
{
    public class VeterinarioFotos
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public Guid VeterinarioID { get; set; }

        protected VeterinarioFotos()
        {
        }

        public VeterinarioFotos(string link, Guid veterinarioID)
        {
            Id = Guid.NewGuid();
            Link = link;
            VeterinarioID = veterinarioID;
        }
    }

   
    
}
