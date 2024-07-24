namespace Bike.Dto.CadastroCiclista
{
    public class MeioDePagamento
    {
        public required string NomeTitular { get; set; }
        public required string Numero { get; set; }
        public DateTime Validade { get; set; }
        public required string Cvv { get; set; }
    }
}
