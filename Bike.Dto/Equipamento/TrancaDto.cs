namespace Bike.Dto.Equipamento
{
	public class TrancaDto
	{
		public int Id { get; set; }
		public int Bicicleta { get; set; }
		public int Numero { get; set; }
		public required string Localizacao { get; set; }
		public required string AnoDeFabricacao { get; set; }
		public required string Modelo { get; set; }
		public required string Status { get; set; }
	}
}
