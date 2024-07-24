namespace Bike.Dto.CadastroCiclista
{
    public class CadastroCiclistaDto
    {
        public required CiclistaDto Ciclista { get; set; }
		public MeioDePagamento? MeioDePagamento { get; set; }
	}
}
