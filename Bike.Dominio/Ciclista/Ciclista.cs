namespace BikeApi.Dominio.Ciclista
{
	public class Ciclista
	{
		public required string Nome { get; set; }
		public DateTime Nascimento { get; set; }
		public required string Cpf { get; set; }
		public required Passaporte Passaporte { get; set; }
		public required string Nacionalidade { get; set; }
		public required string Email { get; set; }
		public required string UrlFotoDocumento { get; set; }
		public required string Senha { get; set; }
	}

	public class Passaporte
	{
		public required string Numero { get; set; }
		public DateTime Validade { get; set; }
		public required string Pais { get; set; }
	}
}
