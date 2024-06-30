namespace Bike.Dominio.Ciclista
{
	public class Ciclista
	{
		public string Nome { get; set; }
		public DateTime Nascimento { get; set; }
		public string Cpf { get; set; }
		public Passaporte Passaporte { get; set; }
		public string Nacionalidade { get; set; }
		public string Email { get; set; }
		public string UrlFotoDocumento { get; set; }
		public string Senha { get; set; }
	}

	public class Passaporte
	{
		public string Numero { get; set; }
		public DateTime Validade { get; set; }
		public string Pais { get; set; }
	}
}
