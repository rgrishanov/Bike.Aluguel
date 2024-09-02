using Bike.Dominio;
using Bike.Dominio.Aluguel;
using Bike.Dto.Ciclista;
using Bike.Dto.Equipamento;
using BikeApi.Aplicacao.AluguelServico;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.MeioDePagamento;
using BikeApi.Persistencia;
using FluentAssertions;
using Moq;

namespace Bike.Testes.Unidade.Aplicacao
{
	public class AluguelServicoTeste
	{
		private readonly AluguelServico _servico;
		private readonly Mock<IIntegracaoExternoServico> _integracaoExterna;
		private readonly Mock<IIntegracaoEquipamentoServico> _equipamento;

		public AluguelServicoTeste()
		{
			this._integracaoExterna = new Mock<IIntegracaoExternoServico>();
			this._equipamento = new Mock<IIntegracaoEquipamentoServico>();

			this._servico = new AluguelServico(this._integracaoExterna.Object, this._equipamento.Object);
		}

		[Fact(DisplayName = "Operações Básicas do Ciclista")]
		public void TesteCriacao()
		{
			CadastroCiclistaInicialDto dto = new CadastroCiclistaInicialDto();
			Assert.Equal("É obrigatório informar os dados do Ciclista.", Assert.Throws<ArgumentException>(() => _servico.CadastrarCiclista(dto)).Message);

			dto.Ciclista = new CiclistaDto()
			{
				Cpf = "79412268041",
				Nome = "Ciclista Teste",
				Email = "ciclista@email.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
				Senha = "123456",
				ConfirmacaoSenha = "123456",
				UrlFotoDocumento = "http://url.com/foto"
			};
			Assert.Equal("É obrigatório informar os dados de Meio do Pagamento.", Assert.Throws<ArgumentException>(() => _servico.CadastrarCiclista(dto)).Message);

			dto.MeioDePagamento = new MeioDePagamentoDto()
			{
				NomeTitular = "Nome Titular",
				Numero = "1234567890123456",
				Cvv = "123",
				Validade = DateTime.Now.AddYears(1),
			};
			var ciclista = new Ciclista(dto.Ciclista);
			Database.ArmazenarCiclista(ciclista); // só pra dar "email ja em uso"
			Assert.Equal("O e-mail informado já está em uso.", Assert.Throws<ArgumentException>(() => _servico.CadastrarCiclista(dto)).Message);
			Database.ExcluirCiclista(ciclista.Id);

			// CriarMeioDePagamentoValidado
			this._integracaoExterna.SetupSequence(x => x.MeioPagamnentoValido(It.IsAny<MeioDePagamento>())).Returns(false).Returns(true).Returns(true);
			Assert.Equal("Meio de pagamento informado não pôde ser validado junto a operadora. Favor fornecer um outro meio de pagamento.", Assert.Throws<ArgumentException>(() => _servico.CadastrarCiclista(dto)).Message);

			this._integracaoExterna.SetupSequence(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false).Returns(true);
			Assert.Equal("Não foi possível enviar o e-mail de confirmação do cadastro.", Assert.Throws<ArgumentException>(() => _servico.CadastrarCiclista(dto)).Message);

			Database.ExcluirCiclista(ciclista.Id);
			var retornoSucesso = _servico.CadastrarCiclista(dto);

			retornoSucesso.Should().NotBeNull();
			Assert.Equal(dto.Ciclista.Cpf, retornoSucesso.Cpf);
			Assert.Equal(dto.Ciclista.Nome, retornoSucesso.Nome);
			Assert.Equal(dto.Ciclista.Email, retornoSucesso.Email);
			Assert.Equal(dto.Ciclista.Nacionalidade, retornoSucesso.Nacionalidade);
			Assert.Equal(dto.Ciclista.Nascimento, retornoSucesso.Nascimento);
			retornoSucesso.Passaporte.Should().BeNull();
			Assert.Equal(dto.Ciclista.UrlFotoDocumento, retornoSucesso.UrlFotoDocumento);
			Assert.Equal("AGUARDANDO_CONFIRMACAO", retornoSucesso.Status);
			retornoSucesso.Id.Should().BeGreaterThan(0);



			/////////////////////////////////////////////////
			// ativação
			/////////////////////////////////////////////////


			Assert.Equal("Ciclista com id 123 não existe.", Assert.Throws<EntidadeInexistenteException>(() => _servico.AtivarCiclista(123)).Message);

			var retornoSucessoAtiv = _servico.AtivarCiclista(ciclista.Id);

			retornoSucessoAtiv.Should().NotBeNull();
			Assert.Equal(dto.Ciclista.Cpf, retornoSucessoAtiv.Cpf);
			Assert.Equal(dto.Ciclista.Nome, retornoSucessoAtiv.Nome);
			Assert.Equal(dto.Ciclista.Email, retornoSucessoAtiv.Email);
			Assert.Equal(dto.Ciclista.Nacionalidade, retornoSucessoAtiv.Nacionalidade);
			Assert.Equal(dto.Ciclista.Nascimento, retornoSucessoAtiv.Nascimento);
			retornoSucessoAtiv.Passaporte.Should().BeNull();
			Assert.Equal(dto.Ciclista.UrlFotoDocumento, retornoSucessoAtiv.UrlFotoDocumento);
			Assert.Equal("ATIVO", retornoSucessoAtiv.Status);
			retornoSucessoAtiv.Id.Should().BeGreaterThan(0);



			/////////////////////////////////////////////////
			// Obter Ciclista
			/////////////////////////////////////////////////

			var retornoSucessoObter = _servico.ObterCiclista(ciclista.Id);

			retornoSucessoObter.Should().NotBeNull();
			Assert.Equal(dto.Ciclista.Cpf, retornoSucessoObter.Cpf);
			Assert.Equal(dto.Ciclista.Nome, retornoSucessoObter.Nome);
			Assert.Equal(dto.Ciclista.Email, retornoSucessoObter.Email);
			Assert.Equal(dto.Ciclista.Nacionalidade, retornoSucessoObter.Nacionalidade);
			Assert.Equal(dto.Ciclista.Nascimento, retornoSucessoObter.Nascimento);
			retornoSucessoObter.Passaporte.Should().BeNull();
			Assert.Equal(dto.Ciclista.UrlFotoDocumento, retornoSucessoObter.UrlFotoDocumento);
			Assert.Equal("ATIVO", retornoSucessoObter.Status);
			retornoSucessoObter.Id.Should().BeGreaterThan(0);



			/////////////////////////////////////////////////
			// Alterar Ciclista
			/////////////////////////////////////////////////

			var dtoAlteracao = new CiclistaDto()
			{
				Cpf = "70560606095",
				Nome = "Ciclista Teste Alterado",
				Email = "ciclista@email.alterado.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(1992, 6, 6, 0, 0, 0, DateTimeKind.Utc),
				Senha = "123456!@#",
				ConfirmacaoSenha = "123456!@#",
				UrlFotoDocumento = "http://url.com/foto/Alterada.jpg"
			};

			this._integracaoExterna.SetupSequence(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false).Returns(true);
			Assert.Equal("Não foi possível enviar o e-mail de confirmação das Alterações do cadastro.", Assert.Throws<ArgumentException>(() => _servico.AlterarCiclista(ciclista.Id, dtoAlteracao)).Message);

			var retornoSucessoAlterar = _servico.ObterCiclista(ciclista.Id);

			retornoSucessoAlterar.Should().NotBeNull();
			Assert.Equal(dtoAlteracao.Cpf, retornoSucessoAlterar.Cpf);
			Assert.Equal(dtoAlteracao.Nome, retornoSucessoAlterar.Nome);
			Assert.Equal(dtoAlteracao.Email, retornoSucessoAlterar.Email);
			Assert.Equal(dtoAlteracao.Nacionalidade, retornoSucessoAlterar.Nacionalidade);
			Assert.Equal(dtoAlteracao.Nascimento, retornoSucessoAlterar.Nascimento);
			retornoSucessoAlterar.Passaporte.Should().BeNull();
			Assert.Equal(dtoAlteracao.UrlFotoDocumento, retornoSucessoAlterar.UrlFotoDocumento);
			Assert.Equal("ATIVO", retornoSucessoAlterar.Status);
			retornoSucessoAlterar.Id.Should().BeGreaterThan(0);



			/////////////////////////////////////////////////
			// Alterar Pagamento
			/////////////////////////////////////////////////

			var dtoMeioPagamentoNovo = new MeioDePagamentoDto()
			{
				NomeTitular = "Nome Titular NOVO",
				Numero = "098765432109876",
				Cvv = "998",
				Validade = DateTime.Now.AddYears(5),
			};

			this._integracaoExterna.Setup(x => x.MeioPagamnentoValido(It.IsAny<MeioDePagamento>())).Returns(true);

			this._integracaoExterna.SetupSequence(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false).Returns(true);
			Assert.Equal("Não foi possível enviar o e-mail de confirmação da alteração do Meio de Pagamento.", Assert.Throws<ArgumentException>(() => _servico.AlterarMeioDePagamentoCiclista(ciclista.Id, dtoMeioPagamentoNovo)).Message);

			_servico.AlterarMeioDePagamentoCiclista(ciclista.Id, dtoMeioPagamentoNovo);

			this._integracaoExterna.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(5));
		}

		[Fact(DisplayName = "Teste de Alugar")]
		public void TesteAlugar()
		{
			CadastroCiclistaInicialDto dto = new CadastroCiclistaInicialDto();
			dto.Ciclista = new CiclistaDto()
			{
				Cpf = "79412268041",
				Nome = "Ciclista Teste",
				Email = "ciclista@email.existente.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
				Senha = "123456",
				ConfirmacaoSenha = "123456",
				UrlFotoDocumento = "http://url.com/foto"
			};
			dto.MeioDePagamento = new MeioDePagamentoDto()
			{
				NomeTitular = "Nome Titular",
				Numero = "1234567890123456",
				Cvv = "123",
				Validade = DateTime.Now.AddYears(1),
			};

			this._integracaoExterna.SetupSequence(x => x.MeioPagamnentoValido(It.IsAny<MeioDePagamento>())).Returns(true);
			this._integracaoExterna.SetupSequence(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			var retornoSucesso = _servico.CadastrarCiclista(dto);
			int idCiclista = retornoSucesso.Id;

			Assert.Equal("Ciclista não está ativo e não pode efetuar aluguel.", Assert.Throws<ArgumentException>(() => _servico.Alugar(idCiclista, 123)).Message);

			var retornoSucessoAtiv = _servico.AtivarCiclista(idCiclista);

			// ciclista criado e ativado, vamos alugar pra ele

			BicicletaDto bicicletaNaTrancaDto = new BicicletaDto()
			{
				Ano = "2020",
				Id = 1,
				Marca = "Caloi",
				Modelo = "Mountain 9000",
				Numero = 1,
				Status = "DISPONIVEL"
			};

			this._equipamento.Setup(x => x.ObterBicicletaNaTranca(It.IsAny<int>())).Returns(bicicletaNaTrancaDto);

			var registroAluguelTemp = new RegistroAluguel() { IdBicicleta = 123, IdCiclista = idCiclista, MeioPagamento = "238475692874", IdTranca = 123 };
			Database.ArmazenarRegistroAluguel(registroAluguelTemp);

			this._integracaoExterna.Setup(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
			Assert.Equal("Ciclista não pode alugar bicicleta pois tem aluguel ativo em andamento.", Assert.Throws<ArgumentException>(() => _servico.Alugar(idCiclista, 123)).Message);
			Database.ExcluirRegistroAluguel(idCiclista);

			bicicletaNaTrancaDto.Status = "REPARO_SOLICITADO";
			this._equipamento.Setup(x => x.ObterBicicletaNaTranca(It.IsAny<int>())).Returns(bicicletaNaTrancaDto);
			Assert.Equal("Esta bicicleta está precisando de reparo, então ela não pode ser alugada. Por gentileza, escolha uma outra.", Assert.Throws<ArgumentException>(() => _servico.Alugar(idCiclista, 123)).Message);

			bicicletaNaTrancaDto.Status = "EM_REPARO";
			this._equipamento.Setup(x => x.ObterBicicletaNaTranca(It.IsAny<int>())).Returns(bicicletaNaTrancaDto);
			Assert.Equal("Esta bicicleta está precisando de reparo, então ela não pode ser alugada. Por gentileza, escolha uma outra.", Assert.Throws<ArgumentException>(() => _servico.Alugar(idCiclista, 123)).Message);

			bicicletaNaTrancaDto.Status = "APOSENTADA";
			this._equipamento.Setup(x => x.ObterBicicletaNaTranca(It.IsAny<int>())).Returns(bicicletaNaTrancaDto);
			Assert.Equal("Bicicleta não está disponível para aluguel.", Assert.Throws<ArgumentException>(() => _servico.Alugar(idCiclista, 123)).Message);

			bicicletaNaTrancaDto.Status = "DISPONIVEL";
			this._equipamento.SetupSequence(x => x.ObterBicicletaNaTranca(It.IsAny<int>())).Returns(bicicletaNaTrancaDto).Returns(bicicletaNaTrancaDto);


			this._equipamento.SetupSequence(x => x.DestrancarTranca(It.IsAny<int>())).Returns(false).Returns(true);
			Assert.Equal("Não foi possível destrancar a tranca da bicicleta escolhida. Escolha outra bicicleta.", Assert.Throws<ArgumentException>(() => _servico.Alugar(idCiclista, 123)).Message);

			this._integracaoExterna.Setup(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			_servico.Alugar(idCiclista, 123);

			this._equipamento.Verify(x => x.AlterarStatusBicicleta(It.IsAny<int>(), "EM_USO"), Times.Exactly(2));
			this._integracaoExterna.Verify(x => x.EfetuarCobranca(It.IsAny<int>(), It.IsAny<float>()), Times.Exactly(2));

			this._equipamento.Verify(x => x.AlterarStatusTranca(It.IsAny<int>(), "DESTRANCAR"), Times.Exactly(1));
		}
	}
}