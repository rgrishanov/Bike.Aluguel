namespace Bike.Dominio.MeioDePagamento
{
	public class MeioDePagamento
	{
		public string NomeTitular { get; set; }
		public string Numero { get; set; }
		public DateTime Validade { get; set; }
		public string Cvv { get; set; }
	}
}
