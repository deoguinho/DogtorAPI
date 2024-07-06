namespace DogtorAPI.ViewModel.Veterinario
{
    public class VeterinarioDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        // Outras propriedades do veterinário...
        public double MediaAvaliacoes { get; set; } // Propriedade para armazenar a média das avaliações
    }
}
