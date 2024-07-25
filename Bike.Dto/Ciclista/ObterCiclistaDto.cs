namespace Bike.Dto.Ciclista
{
    public class ObterCiclistaDto : CiclistaDtoBase
	{
        public int Id { get; set; }

        public required string Status { get; set; }
    }
}
