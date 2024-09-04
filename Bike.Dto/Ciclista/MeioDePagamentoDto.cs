namespace Bike.Dto.Ciclista
{
    public class MeioDePagamentoDto
    {
        public required string NomeTitular { get; set; }
        public required string Numero { get; set; }
        public required string Validade { get; set; }
        public required string Cvv { get; set; }
    }
}
