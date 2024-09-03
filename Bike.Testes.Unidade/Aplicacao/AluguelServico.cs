using Bike.Dominio;
using Bike.Dominio.Aluguel;
using Bike.Dto.Ciclista;
using Bike.Dto.Equipamento;
using Bike.Dto.Funcionario;
using BikeApi.Aplicacao.AluguelServico;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.Funcionario;
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
		public void TesteCiclista()
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

			Assert.False(_servico.EmailJaEstaEmUso(dto.Ciclista.Email));
			Assert.Equal("Informe o e-mail que gostaria de verificar.", Assert.Throws<ArgumentException>(() => _servico.EmailJaEstaEmUso("")).Message);

			dto.MeioDePagamento = new MeioDePagamentoDto()
			{
				NomeTitular = "Nome Titular",
				Numero = "1234567890123456",
				Cvv = "123",
				Validade = DateTime.Now.AddYears(1),
			};
			var ciclista = new Ciclista(dto.Ciclista);
			Database.ArmazenarCiclista(ciclista);

			Assert.True(_servico.EmailJaEstaEmUso(dto.Ciclista.Email));

			// só pra dar "email ja em uso"
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


			var meioPagamentoDto = _servico.ObterMeioDePagamentoCiclista(retornoSucesso.Id);
			meioPagamentoDto.Should().NotBeNull();
			meioPagamentoDto.NomeTitular.Should().Be(dto.MeioDePagamento.NomeTitular);
			meioPagamentoDto.Cvv.Should().Be(dto.MeioDePagamento.Cvv);
			meioPagamentoDto.Numero.Should().Be(dto.MeioDePagamento.Numero);


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

			var retornoSucessoAlterar = _servico.AlterarCiclista(ciclista.Id, dtoAlteracao);

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

			this._integracaoExterna.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(6));
		}

		[Fact(DisplayName = "Operações Básicas do Funcionário")]
		public void TesteFuncionario()
		{
			Assert.Equal("Funcionario com id 123 não existe.", Assert.Throws<EntidadeInexistenteException>(() => _servico.ExcluirFuncionario(123)).Message);
			Assert.Equal("É obrigatório informar os dados do Funcionário.", Assert.Throws<ArgumentException>(() => _servico.CadastrarFuncionario(null!)).Message);

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

			var retornoSucesso = _servico.CadastrarFuncionario(dto);

			retornoSucesso.Should().NotBeNull();
			retornoSucesso.Email.Should().Be(dto.Email);
			retornoSucesso.Senha.Should().Be(dto.Senha);
			retornoSucesso.Funcao.Should().Be(dto.Funcao);
			retornoSucesso.Idade.Should().Be(dto.Idade);
			retornoSucesso.Cpf.Should().Be(dto.Cpf);
			retornoSucesso.Nome.Should().Be(dto.Nome);
			retornoSucesso.Matricula.Length.Should().Be(11);
			retornoSucesso.Id.Should().Be(1);


			/////////////////////////////////////////////////
			// Obter Funcionario
			/////////////////////////////////////////////////

			var retornoSucessoObter = _servico.ObterFuncionario(retornoSucesso.Id);

			retornoSucessoObter.Should().NotBeNull();
			retornoSucessoObter.Should().NotBeNull();
			retornoSucessoObter.Email.Should().Be(dto.Email);
			retornoSucessoObter.Senha.Should().Be(dto.Senha);
			retornoSucessoObter.Funcao.Should().Be(dto.Funcao);
			retornoSucessoObter.Idade.Should().Be(dto.Idade);
			retornoSucessoObter.Cpf.Should().Be(dto.Cpf);
			retornoSucessoObter.Nome.Should().Be(dto.Nome);
			retornoSucessoObter.Matricula.Length.Should().Be(11);
			retornoSucessoObter.Id.Should().Be(1);



			/////////////////////////////////////////////////
			// Alterar Funcionario
			/////////////////////////////////////////////////

			var dtoAlteracao = new FuncionarioBaseDto()
			{
				Cpf = "05470701794",
				Nome = "Funcionario Teste Alterado",
				Email = "funcionario@email.Alterado.com",
				Senha = "123456Alterado",
				ConfirmacaoSenha = "123456Alterado",
				Funcao = "REPARADOR",
				Idade = 39
			};

			var retornoSucessoAlterar = _servico.AlterarFuncionario(retornoSucesso.Id, dtoAlteracao);

			retornoSucessoAlterar.Should().NotBeNull();
			retornoSucessoAlterar.Email.Should().Be(dtoAlteracao.Email);
			retornoSucessoAlterar.Senha.Should().Be(dtoAlteracao.Senha);
			retornoSucessoAlterar.Funcao.Should().Be(dtoAlteracao.Funcao);
			retornoSucessoAlterar.Idade.Should().Be(dtoAlteracao.Idade);
			retornoSucessoAlterar.Cpf.Should().Be(dtoAlteracao.Cpf);
			retornoSucessoAlterar.Nome.Should().Be(dtoAlteracao.Nome);
			retornoSucessoAlterar.Matricula.Length.Should().Be(11);
			retornoSucessoAlterar.Id.Should().Be(1);

			// e por fim exclusão
			_servico.ExcluirFuncionario(retornoSucesso.Id);
			Assert.Equal($"Funcionario com id {retornoSucesso.Id} não existe.", Assert.Throws<EntidadeInexistenteException>(() => _servico.ExcluirFuncionario(retornoSucesso.Id)).Message);



			// teste de obter lista:

			var dto1 = new FuncionarioBaseDto()
			{
				Cpf = "79412268041",
				Nome = "Funcionario1 Teste",
				Email = "funcionario1@email.com",
				Senha = "1123456",
				ConfirmacaoSenha = "1123456",
				Funcao = "ADMINISTRATIVO",
				Idade = 22
			};

			var retornoSucesso1 = _servico.CadastrarFuncionario(dto1);

			var dto2 = new FuncionarioBaseDto()
			{
				Cpf = "79412268041",
				Nome = "Funcionario2 Teste",
				Email = "funcionario2@email.com",
				Senha = "2123456",
				ConfirmacaoSenha = "2123456",
				Funcao = "REPARADOR",
				Idade = 33
			};

			var retornoSucesso2 = _servico.CadastrarFuncionario(dto2);

			var dto3 = new FuncionarioBaseDto()
			{
				Cpf = "79412268041",
				Nome = "Funcionario3 Teste",
				Email = "funcionario3@email.com",
				Senha = "3123456",
				ConfirmacaoSenha = "3123456",
				Funcao = "ADMINISTRATIVO",
				Idade = 55
			};

			var retornoSucesso3 = _servico.CadastrarFuncionario(dto3);

			/////////////////////////////////////////////////
			// Obter Funcionarios
			/////////////////////////////////////////////////

			var retorno = _servico.ObterFuncionarios();

			retorno.Should().NotBeNull();
			retorno.Count().Should().Be(3);

			retorno.Count(f => f.Funcao == "REPARADOR").Should().Be(1);
			retorno.Count(f => f.Funcao == "ADMINISTRATIVO").Should().Be(2);
			retorno.Count(f => f.Senha == "3123456").Should().Be(1);
			retorno.Count(f => f.Email == "funcionario1@email.com").Should().Be(1);
			retorno.Sum(f => f.Idade).Should().Be(110);
		}

		[Fact(DisplayName = "Teste de Alugar e Devolver")]
		public void TesteAlugarDevolver()
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

			// não pode alugar se nao ativado
			Assert.False(_servico.CiclistaPodeAlugar(idCiclista));

			var retornoSucessoAtiv = _servico.AtivarCiclista(idCiclista);

			Assert.True(_servico.CiclistaPodeAlugar(idCiclista));
			Assert.Null(_servico.ObterBicicletaAlugada(idCiclista));

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

			// não pode alugar pq ja alugou
			Assert.False(_servico.CiclistaPodeAlugar(idCiclista));

			var dtoBikeAlugada = new BicicletaDto
			{
				Id = 30,
				Ano = "2021",
				Marca = "Caloi",
				Modelo = "Mountain 9000",
				Numero = 123,
				Status = "EM_USO"
			};
			this._equipamento.Setup(x => x.ObterBicicletaPorId(It.IsAny<int>())).Returns(dtoBikeAlugada);
			var bikeAlugada = _servico.ObterBicicletaAlugada(idCiclista);

			bikeAlugada.Should().NotBeNull();
			bikeAlugada.Status.Should().Be("EM_USO");
			bikeAlugada.Marca.Should().Be(bicicletaNaTrancaDto.Marca);
			bikeAlugada.Modelo.Should().Be(bicicletaNaTrancaDto.Modelo);

			this._equipamento.Verify(x => x.AlterarStatusBicicleta(It.IsAny<int>(), "EM_USO"), Times.Exactly(2));
			this._integracaoExterna.Verify(x => x.EfetuarCobranca(It.IsAny<int>(), It.IsAny<float>()), Times.Exactly(2));

			this._equipamento.Verify(x => x.AlterarStatusTranca(It.IsAny<int>(), "DESTRANCAR"), Times.Exactly(1));


			//////////////////////////////////////////////////
			////   agora devolução    ////////////////////////
			//////////////////////////////////////////////////

			// primeiro erros

			var trancaDto = new TrancaDto() { AnoDeFabricacao = "2024", Localizacao = "Local", Modelo = "Modelo", Status = "teste" };
			this._equipamento.Setup(x => x.ObterTrancaPorId(It.IsAny<int>())).Returns(trancaDto);
			Assert.Equal("Esta tranca não está disponível para devolver a bicicleta, escolha outra tranca.", Assert.Throws<ArgumentException>(() => _servico.Devolver(1,1)).Message);


			BicicletaDto bikeNull = null!;

			trancaDto.Status = "LIVRE";
			this._equipamento.Setup(x => x.ObterTrancaPorId(It.IsAny<int>())).Returns(trancaDto);
			this._equipamento.Setup(x => x.ObterBicicletaPorId(It.IsAny<int>())).Returns(bikeNull!);
			Assert.Equal("Bicicleta com identificador Inválido. Favor entrar em contato com Suporte.", Assert.Throws<ArgumentException>(() => _servico.Devolver(1, bikeAlugada.Id)).Message);
		

			var bikeDto = new BicicletaDto() { Ano = "2022", Id = 1, Marca = "xina", Modelo = "Li Ming", Numero = 123, Status = "DISPONIVEL" };
			this._equipamento.Setup(x => x.ObterTrancaPorId(It.IsAny<int>())).Returns(trancaDto);
			this._equipamento.Setup(x => x.ObterBicicletaPorId(It.IsAny<int>())).Returns(bikeDto);
			Assert.Equal("Esta bicicleta não está em uso. Favor entrar em contato com Suporte.", Assert.Throws<ArgumentException>(() => _servico.Devolver(1, 1)).Message);


			bikeDto.Status = "EM_USO";
			this._equipamento.Setup(x => x.ObterTrancaPorId(It.IsAny<int>())).Returns(trancaDto);
			this._equipamento.Setup(x => x.ObterBicicletaPorId(It.IsAny<int>())).Returns(bikeDto);
			Assert.Equal("Não foi possivel localizar o registro do aluguel", Assert.Throws<ArgumentException>(() => _servico.Devolver(15, 15)).Message);


			// agora fluxo feliz completo 

			this._equipamento.Setup(x => x.ObterTrancaPorId(It.IsAny<int>())).Returns(trancaDto);
			this._equipamento.Setup(x => x.ObterBicicletaPorId(It.IsAny<int>())).Returns(bikeDto);

			var registro = Database.ObterAluguelAtivoPorBicicleta(bicicletaNaTrancaDto.Id);
			registro.ForcarDataRetirada(DateTime.Now.AddHours(3)); // pra poder testar, se nao não vai gerar cobrança nova



			this._integracaoExterna.Setup(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			_servico.Devolver(20, bicicletaNaTrancaDto.Id);


			this._integracaoExterna.Verify(x => x.EfetuarCobranca(idCiclista, 10), Times.Exactly(2));

			this._equipamento.Verify(x => x.AlterarStatusBicicleta(bicicletaNaTrancaDto.Id, "DISPONIVEL"), Times.Exactly(1));
			this._equipamento.Verify(x => x.TrancarTranca(20), Times.Exactly(1));

		}
	}
}