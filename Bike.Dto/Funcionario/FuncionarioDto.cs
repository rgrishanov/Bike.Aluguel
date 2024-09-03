namespace Bike.Dto.Funcionario
{
	public class FuncionarioDto : FuncionarioBaseDto
	{
		public required string Matricula { get; set; }
		public int Id { get; set; }
	}
}
