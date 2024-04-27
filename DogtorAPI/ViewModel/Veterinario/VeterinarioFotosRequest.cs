namespace DogtorAPI.ViewModel.Veterinario
{
    public class VeterinarioFotosRequest
    {
        public List<string> Link { get; set; } = new List<string>();
        public Guid VeterinarioID { get; set; }
    }
}
