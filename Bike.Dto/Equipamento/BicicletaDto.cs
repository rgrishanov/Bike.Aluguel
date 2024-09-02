namespace Bike.Dto.Equipamento
{
	public class BicicletaDto
	{ 
		public required int Id { get; set; }
		public required string Marca { get; set; }
		public required string Modelo { get; set; }
		public required string Ano { get; set; }
		public required int Numero { get; set; }
		public required string Status { get; set; }
	}
}
