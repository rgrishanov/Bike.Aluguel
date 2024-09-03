using Bike.Dominio.Funcionario.Validacao;
using Bike.Dominio.Validacao;
using Bike.Dto.Funcionario;

namespace BikeApi.Dominio.Funcionario
{
	public class Funcionario
	{
		public int Id { get; private set; }
		public string Nome { get; private set; }
		public string Email { get; private set; }
		public string Cpf { get; private set; }
		public string Matricula { get; private set; }
		public int Idade { get; private set; }
		public string Funcao { get; private set; }
		public string Senha { get; private set; }

		public Funcionario(FuncionarioBaseDto dto, bool validar = true)
		{
			if (validar)
				Validador.Validar(dto, new FuncionarioValidacao());

			this.PreencherCamposBasicos(dto);

			this.GerarMatricula();
		}

		public void GerarMatricula()
		{
			this.Matricula = DateTime.Today.Year.ToString() + new Random().Next(1000000, 9999999);
		}

		public void ForcarMatricula(string matricula)
		{
			this.Matricula = matricula;
		}

		public void SetarIdInicial(int id)
		{
			if (this.Id == 0)
				this.Id = id;
			else
				throw new ArgumentException("Não é possível alterar o Id do Funcionario.");
		}

		public void Alterar(FuncionarioBaseDto dto)
		{
			Validador.Validar(dto, new FuncionarioValidacao());

			this.PreencherCamposBasicos(dto);
		}

		private void PreencherCamposBasicos(FuncionarioBaseDto dto)
		{
			this.Nome = dto.Nome!;
			this.Email = dto.Email!;
			this.Cpf = dto.Cpf!;
			this.Idade = dto.Idade!;
			this.Funcao = dto.Funcao!;
			this.Senha = dto.Senha!;
		}

		public FuncionarioDto MapearParaDto()
		{
			FuncionarioDto dto = new()
			{
				Nome = this.Nome,
				Email = this.Email,
				Cpf = this.Cpf,
				Idade = this.Idade,
				Matricula = this.Matricula,
				Senha = this.Senha,
				ConfirmacaoSenha = this.Senha,
				Funcao = this.Funcao,
				Id = this.Id
			};

			return dto;
		}
	}
}
