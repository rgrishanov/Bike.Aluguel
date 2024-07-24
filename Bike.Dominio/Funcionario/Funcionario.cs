namespace BikeApi.Dominio.Funcionario
{
	public class Funcionario
	{
		public required string Matricula { get; set; }
		public required string Senha { get; set; }
		public required string Email { get; set; }
		public required string Nome { get; set; }
		public int Idade { get; set; }
		public required string Funcao { get; set; }
		public required string Cpf { get; set; }
	}
}
