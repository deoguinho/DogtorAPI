namespace DogtorAPI.ViewModel.Consulta
{
    public class RelatorioConsultaResponse
    {
        public int TotalConsultas { get; set; }
        public int TotalConsultasPendentes { get; set; }
        public int TotalConsultasAceitas { get; set; }
        public int TotalConsultasNegadas { get; set; }
        public int TotalConsultasConcluidas { get; set; }
        public int TotalConsultasCanceladas { get; set; }

        // Adicione outros campos conforme necessário
    }
}
