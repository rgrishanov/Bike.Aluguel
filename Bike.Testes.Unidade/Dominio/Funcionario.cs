using Bike.Dto.Funcionario;
using BikeApi.Dominio.Funcionario;
using FluentAssertions;

namespace Bike.Testes.Unidade.Dominio
{
	public class FuncionarioDominioTeste
	{
		[Fact(DisplayName = "Criação Básica e alteração do Funcionario")]
		public void TesteCriacaoAlteracao()
		{
			var dto = new FuncionarioBaseDto()
			{
				Cpf = "79412268041",
				Nome = "Funcionario Teste",
				Email = "funcionario@email.com",
				Senha = "123456",
				ConfirmacaoSenha = "123456",
				Funcao = "ADMINISTRATIVO",
				Idade = 33
			};

			//'ADMINISTRATIVO' ou 'REPARADOR'");

			var dominio = new Funcionario(dto);

			Assert.Equal(dto.Cpf, dominio.Cpf);
			Assert.Equal(dto.Nome, dominio.Nome);
			Assert.Equal(dto.Email, dominio.Email);
			Assert.Equal(dto.Senha, dominio.Senha);
			Assert.Equal(dto.Funcao, dominio.Funcao);
			Assert.Equal(dto.Idade, dominio.Idade);
			Assert.True(dominio.Matricula.StartsWith("2024") && dominio.Matricula.Length == 11);

			Assert.Equal(0, dominio.Id);

			dominio.SetarIdInicial(5);
			Assert.Equal(5, dominio.Id);

			var exception1 = Assert.Throws<ArgumentException>(() => dominio.SetarIdInicial(60));
			Assert.Equal("Não é possível alterar o Id do Funcionario.", exception1.Message);


			/////////// altearação ////////////////

			var dtoAlteracao = new FuncionarioBaseDto()
			{
				Cpf = "79412268041",
				Nome = "Funcionario Teste ALTERADO",
				Email = "funcionario@alt.email.com",
				Senha = "123456alt",
				ConfirmacaoSenha = "123456alt",
				Funcao = "REPARADOR",
				Idade = 37
			};

			dominio.Alterar(dtoAlteracao);

			dominio.Should().NotBeNull();

			Assert.Equal(dtoAlteracao.Cpf, dominio.Cpf);
			Assert.Equal(dtoAlteracao.Nome, dominio.Nome);
			Assert.Equal(dtoAlteracao.Email, dominio.Email);
			Assert.Equal(dtoAlteracao.Senha, dominio.Senha);
			Assert.Equal(dtoAlteracao.Funcao, dominio.Funcao);
			Assert.Equal(dtoAlteracao.Idade, dominio.Idade);
			Assert.True(dominio.Matricula.StartsWith("2024") && dominio.Matricula.Length == 11);

			dominio.Id.Should().BeGreaterThan(0);

			dominio.ForcarMatricula("12345");
			dominio.Matricula.Should().Be("12345");

			var dtoRetorno = dominio.MapearParaDto();
			dtoRetorno.Cpf.Should().Be(dtoAlteracao.Cpf);
			dtoRetorno.Email.Should().Be(dtoAlteracao.Email);
			dtoRetorno.Senha.Should().Be(dtoAlteracao.Senha);
			dtoRetorno.Nome.Should().Be(dtoAlteracao.Nome);
			dtoRetorno.Funcao.Should().Be(dtoRetorno.Funcao);
			dtoRetorno.Idade.Should().Be(dtoRetorno.Idade);
		}
	}
}