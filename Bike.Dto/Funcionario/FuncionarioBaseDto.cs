namespace Bike.Dto.Funcionario
{
	public class FuncionarioBaseDto
	{
		public string? Nome { get; set; }
		public string? Email { get; set; }
		public string? Cpf { get; set; }
		public int Idade { get; set; }
		public string? Funcao { get; set; }
		public string? Senha { get; set; }
		public string? ConfirmacaoSenha { get; set; }
	}
}
