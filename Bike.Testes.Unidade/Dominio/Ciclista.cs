using Bike.Dto.Ciclista;
using BikeApi.Dominio.Ciclista;
using FluentAssertions;

namespace Bike.Testes.Unidade.Dominio
{
	public class CiclistaDominioTeste
	{
		[Fact(DisplayName = "Criação Básica e alteração do Ciclista")]
		public void TesteCriacaoAlteracao()
		{
			var dto = new CiclistaDto()
			{
				Cpf = "79412268041",
				Nome = "Ciclista Teste",
				Email = "ciclista@email.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
				Senha = "123456",
				SenhaConfirmacao = "123456",
				UrlFotoDocumento = "http://url.com/foto"
			};

			var dominioBr = new Ciclista(dto);

			Assert.Equal(dto.Cpf, dominioBr.Cpf);
			Assert.Equal(dto.Nome, dominioBr.Nome);
			Assert.Equal(dto.Email, dominioBr.Email);
			Assert.Equal(dto.Nacionalidade, dominioBr.Nacionalidade);
			Assert.Equal(dto.Nascimento, dominioBr.Nascimento);
			dominioBr.Passaporte.Should().BeNull();
			Assert.Equal(dto.Senha, dominioBr.Senha);
			Assert.Equal(dto.UrlFotoDocumento, dominioBr.UrlFotoDocumento);
			Assert.Equal("AGUARDANDO_CONFIRMACAO", dominioBr.Status);
			dominioBr.AtivoDesde.Should().Be(DateTime.MinValue);

			// converter dto pra estrangeiro
			dto.Passaporte = new PassaporteDto()
			{
				Numero = "1234567890123456",
				Pais = "Brasil",
				Validade = DateTime.Now.AddYears(1),
			};
			dto.Nacionalidade = "ESTRANGEIRO";
			dto.Cpf = null;

			var dominioEs = new Ciclista(dto);

			dominioEs.Cpf.Should().BeNull();
			Assert.Equal(dto.Nome, dominioEs.Nome);
			Assert.Equal(dto.Email, dominioEs.Email);
			Assert.Equal(dto.Nacionalidade, dominioEs.Nacionalidade);
			Assert.Equal(dto.Nascimento, dominioEs.Nascimento);
			Assert.Equal(dto.Senha, dominioEs.Senha);
			Assert.Equal(dto.UrlFotoDocumento, dominioEs.UrlFotoDocumento);
			Assert.Equal("AGUARDANDO_CONFIRMACAO", dominioEs.Status);
			dominioEs.AtivoDesde.Should().Be(DateTime.MinValue);

			dominioEs.Passaporte.Should().NotBeNull();
			dominioEs.Passaporte!.Numero.Should().Be(dto.Passaporte!.Numero);
			dominioEs.Passaporte!.Pais.Should().Be(dto.Passaporte!.Pais);
			dominioEs.Passaporte!.Validade.Should().Be(dto.Passaporte!.Validade);

			Assert.Equal(0, dominioBr.Id);
			Assert.Equal(0, dominioEs.Id);

			dominioBr.SetarIdInicial(5);
			Assert.Equal(5, dominioBr.Id);

			var exception1 = Assert.Throws<ArgumentException>(() => dominioBr.SetarIdInicial(60));
			Assert.Equal("Não é possível alterar o Id do Ciclista.", exception1.Message);

			// ativar cadastro

			dominioBr.AtivarCadastro();
			Assert.Equal("ATIVO", dominioBr.Status);
			dominioBr.AtivoDesde.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 2));


			/////////// altearação ////////////////

			var dtoAlteracao = new CiclistaDto()
			{
				Cpf = "70560606095",
				Nome = "Ciclista Teste Alterado",
				Email = "ciclista@email.alterado.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(1992, 6, 6, 0, 0, 0, DateTimeKind.Utc),
				Senha = "123456!@#",
				SenhaConfirmacao = "123456!@#",
				UrlFotoDocumento = "http://url.com/foto/Alterada.jpg"
			};

			dominioBr.Alterar(dtoAlteracao);

			dominioBr.Should().NotBeNull();
			Assert.Equal(dtoAlteracao.Cpf, dominioBr.Cpf);
			Assert.Equal(dtoAlteracao.Nome, dominioBr.Nome);
			Assert.Equal(dtoAlteracao.Email, dominioBr.Email);
			Assert.Equal(dtoAlteracao.Nacionalidade, dominioBr.Nacionalidade);
			Assert.Equal(dtoAlteracao.Nascimento, dominioBr.Nascimento);
			dominioBr.Passaporte.Should().BeNull();
			Assert.Equal(dtoAlteracao.UrlFotoDocumento, dominioBr.UrlFotoDocumento);
			Assert.Equal("ATIVO", dominioBr.Status);
			dominioBr.Id.Should().BeGreaterThan(0);
		}

		private static PassaporteDto passaporte = new PassaporteDto()
		{
			Numero = "1234567890123456",
			Pais = "Brasil",
			Validade = DateTime.Now.AddYears(1),
		};

		public static IEnumerable<object[]> DadosPraTestesException =>
			new List<object[]>
			{
				new object[] { string.Empty, "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "CPF do Ciclista não pode ser vazio" },
				new object[] { "0987098708", "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "CPF do Ciclista é inválido" },
				new object[] { "79412268041", string.Empty, "ciclista@email.com", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Nome do Ciclista não pode ser vazio" },
				new object[] { "79412268041", "Ciclista Teste", null!, "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Email do Ciclista não pode ser vazio" },
				new object[] { "79412268041", "Ciclista Teste", "email_falso", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Email do Ciclista é inválido" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Nacionalidade do Ciclista não pode ser vazia" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "Gringo", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Nacionalidade do Ciclista deve ser 'BRASILEIRO' ou 'ESTRANGEIRO'" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", DateTime.Now.AddDays(1),
					"123456", "123456", "http://url.com/foto", null!, "Data de Nascimento do Ciclista deve ser anterior a hoje" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", new DateTime(1890, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Data de Nascimento do Ciclista deve ser no século 20 ou 21" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"", "123456", "http://url.com/foto", null!, "Senha não pode ser vazia" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "1234567", "http://url.com/foto", null!, "Senha e Confirmação de senha são diferentes" },
				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "BRASILEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", string.Empty, "http://url.com/foto", null!, "Senha e Confirmação de senha são diferentes" },

				new object[] { "79412268041", "Ciclista Teste", "ciclista@email.com", "ESTRANGEIRO", new DateTime(1990, 11, 11, 0, 0, 0, DateTimeKind.Utc),
					"123456", "123456", "http://url.com/foto", null!, "Dados do Passaporte do Ciclista Estrangeiro devem ser informados" }
			};

		[Theory(DisplayName = "Obtém Exceptions ao Criar Ciclista com valores Errados, Nulos ou Vazios")]
		[MemberData(nameof(DadosPraTestesException))]
		public void ObterExceptionAoCriarCiclistaComValoresNulosOuVazios(string cpf, string nome, string email, string nacionalidade,
			 DateTime nascimento, string senha, string senhaConfirmacao, string urlFotoDocumento, PassaporteDto passaporte, string erro)
		{
			var dto = new CiclistaDto() { 
				Cpf = cpf, Nome = nome, Email = email, Nacionalidade = nacionalidade, Nascimento = nascimento, Senha = senha,
				SenhaConfirmacao = senhaConfirmacao, UrlFotoDocumento = urlFotoDocumento, Passaporte = passaporte
			};

			var exception = Assert.Throws<ArgumentException>(() => new Ciclista(dto));
			Assert.Equal(erro, exception.Message);
		}
	}
}