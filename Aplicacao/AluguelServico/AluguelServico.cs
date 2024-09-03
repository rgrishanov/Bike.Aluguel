using Bike.Dominio;
using Bike.Dominio.Aluguel;
using Bike.Dto.Ciclista;
using Bike.Dto.Equipamento;
using Bike.Dto.Funcionario;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.Funcionario;
using BikeApi.Dominio.MeioDePagamento;
using BikeApi.Persistencia;

namespace BikeApi.Aplicacao.AluguelServico
{
	public class AluguelServico(IIntegracaoExternoServico integracaoExterna, IIntegracaoEquipamentoServico equipamento) : IAluguelServico
	{
		private readonly IIntegracaoExternoServico _integracaoExterna = integracaoExterna;
		private readonly IIntegracaoEquipamentoServico _equipamento = equipamento;

		#region Ciclista

		public ObterCiclistaDto CadastrarCiclista(CadastroCiclistaInicialDto dto)
		{
			if (dto.Ciclista == null)
				throw new ArgumentException("É obrigatório informar os dados do Ciclista.");
			if (dto.MeioDePagamento == null)
				throw new ArgumentException("É obrigatório informar os dados de Meio do Pagamento.");

			if (this.EmailJaEstaEmUso(dto.Ciclista.Email!))
				throw new ArgumentException("O e-mail informado já está em uso.");

			var ciclista = new Ciclista(dto.Ciclista);

			var meioPagamento = this.CriarMeioDePagamentoValidado(dto.MeioDePagamento);

			Database.ArmazenarCiclista(ciclista);

			meioPagamento.SetarIdCiclista(ciclista.Id);

			Database.ArmazenarMeioDePagamento(meioPagamento);

			if (!this._integracaoExterna.EnviarEmail(ciclista.Email, "Confirmação do seu Cadastro - Bikeria SCB",
				$"Seu cadastro foi efetuado com sucesso! Clique no Link a seguir para confirmar o seu email: https://www.scb.com.br/api/ciclista/{ciclista.Id}/ativar"))

				throw new ArgumentException("Não foi possível enviar o e-mail de confirmação do cadastro.");

			return ciclista.MapearParaDto();
		}

		public ObterCiclistaDto AtivarCiclista(int idCiclista)
		{
			var ciclista = ObterCiclistaDominio(idCiclista);
			ciclista.AtivarCadastro();

			// aqui teria algo sobre salvar no banco, mas sem banco já está tudo salvo na memória.

			return ciclista.MapearParaDto();
		}

		public ObterCiclistaDto ObterCiclista(int idCiclista)
		{
			return ObterCiclistaDominio(idCiclista).MapearParaDto();
		}

		public ObterCiclistaDto AlterarCiclista(int idCiclista, CiclistaDto dto)
		{
			var ciclista = ObterCiclistaDominio(idCiclista);

			ciclista.Alterar(dto);

			// aqui teria algo sobre salvar no banco, mas sem banco já está tudo salvo na memória.

			if (!this._integracaoExterna.EnviarEmail(ciclista.Email, "Alteração de dados cadastrais - Bikeria SCB",
				$"Seu cadastro foi alterado com sucesso!"))

				throw new ArgumentException("Não foi possível enviar o e-mail de confirmação das Alterações do cadastro.");

			return ciclista.MapearParaDto();
		}

		public MeioDePagamentoDto ObterMeioDePagamentoCiclista(int idCiclista)
		{
			// só pra ver se existe, se nao isso já dá exception
			ObterCiclistaDominio(idCiclista);

			return Database.ObterMeioDePagamentoPorIdCiclista(idCiclista).MapearParaDto();
		}

		public void AlterarMeioDePagamentoCiclista(int idCiclista, MeioDePagamentoDto dto)
		{
			var ciclista = ObterCiclistaDominio(idCiclista);
			var meioPagamento = this.CriarMeioDePagamentoValidado(dto);

			meioPagamento.SetarIdCiclista(ciclista.Id);

			Database.ExcluirMeioDePagamentoDoCiclista(ciclista.Id);
			Database.ArmazenarMeioDePagamento(meioPagamento);

			if (!this._integracaoExterna.EnviarEmail(ciclista.Email, "Alteração do Meio de Pagamento - Bikeria SCB",
				$"Seu Meio de Pagamento foi alterado com sucesso!"))

				throw new ArgumentException("Não foi possível enviar o e-mail de confirmação da alteração do Meio de Pagamento.");
		}

		public bool CiclistaPodeAlugar(int idCiclista)
		{
			// só pra ver se existe, se nao isso já dá exception
			var ciclista = ObterCiclistaDominio(idCiclista);

			// se não está ativo - não pode alugar
			if (ciclista.Status != "ATIVO")
				return false;

			return Database.ObterAluguelAtivo(idCiclista) == null;
		}

		public BicicletaDto ObterBicicletaAlugada(int idCiclista)
		{
			// só pra ver se existe, se nao isso já dá exception
			ObterCiclistaDominio(idCiclista);

			var aluguelAtivo = Database.ObterAluguelAtivo(idCiclista);

			if (aluguelAtivo == null)
				return null!;

			return _equipamento.ObterBicicletaPorId(aluguelAtivo.IdBicicleta);
		}

		public bool EmailJaEstaEmUso(string email)
		{
			if (string.IsNullOrEmpty(email))
				throw new ArgumentException("Informe o e-mail que gostaria de verificar.");

			return Database.EmailJaEstaEmUso(email);
		}

		#endregion

		#region Funcionario

		public FuncionarioDto CadastrarFuncionario(FuncionarioBaseDto dto)
		{
			if (dto == null)
				throw new ArgumentException("É obrigatório informar os dados do Funcionário.");

			var funcionario = new Funcionario(dto);

			Database.ArmazenarFuncionario(funcionario);

			return funcionario.MapearParaDto();
		}

		public FuncionarioDto ObterFuncionario(int idFuncionario) => ObterFuncionarioDominio(idFuncionario).MapearParaDto();

		public void ExcluirFuncionario(int idFuncionario)
		{
			// só pra ver se existe
			ObterFuncionarioDominio(idFuncionario);

			Database.ExcluirFuncionario(idFuncionario);
		}

		public FuncionarioDto AlterarFuncionario(int idFuncionario, FuncionarioBaseDto dto)
		{
			var funcionario = ObterFuncionarioDominio(idFuncionario);

			funcionario.Alterar(dto);

			// aqui teria algo sobre salvar no banco, mas sem banco já está tudo salvo na memória.

			return funcionario.MapearParaDto();
		}

		public IEnumerable<FuncionarioDto> ObterFuncionarios() => Database.ObterFuncionarios().Select(f => f.MapearParaDto()).ToList();

		#endregion

		public void Alugar(int idCiclista, int idTranca)
		{
			var ciclista = ObterCiclistaDominio(idCiclista);
			var bikeNaTranca = _equipamento.ObterBicicletaNaTranca(idTranca);

			if (ciclista.Status != "ATIVO")
				throw new ArgumentException("Ciclista não está ativo e não pode efetuar aluguel.");

			var aluguelAtivo = Database.ObterAluguelAtivo(idCiclista);
			if (aluguelAtivo != null)
			{
				this._integracaoExterna.EnviarEmail(ciclista.Email, "Dados do Aluguel Ativo - Bikeria SCB",
				$"Você possui alugel de uma bicicleta em andamento. " + Environment.NewLine + aluguelAtivo.FormatarParaEmail());

				throw new ArgumentException("Ciclista não pode alugar bicicleta pois tem aluguel ativo em andamento.");
			}

			if (bikeNaTranca.Status == "REPARO_SOLICITADO" || bikeNaTranca.Status == "EM_REPARO")
				throw new ArgumentException("Esta bicicleta está precisando de reparo, então ela não pode ser alugada. Por gentileza, escolha uma outra.");

			if (bikeNaTranca.Status != "DISPONIVEL")
				throw new ArgumentException("Bicicleta não está disponível para aluguel.");

			// validações terminaram, agora finalmente processar aluguel

			// não tem erro. mesmo com erro a cobrança é armazenada para mais tarde.
			// R2 – Deverá ser feita uma cobrança de R$ 10,00 pelas duas primeiras horas de uso.
			this._integracaoExterna.EfetuarCobranca(idCiclista, 10.00f);

			var registroDesteAluguel = new RegistroAluguel()
			{
				IdBicicleta = bikeNaTranca.Id,
				IdCiclista = idCiclista,
				MeioPagamento = Database.ObterMeioDePagamentoPorIdCiclista(idCiclista).Numero,
				IdTranca = idTranca
			};
			Database.ArmazenarRegistroAluguel(registroDesteAluguel);

			_equipamento.AlterarStatusBicicleta(bikeNaTranca.Id, "EM_USO");

			if (_equipamento.DestrancarTranca(idTranca))
				_equipamento.AlterarStatusTranca(idTranca, "DESTRANCAR");
			else
			{
				Database.ExcluirRegistroAluguel(idCiclista); // se não, fica preso registro de aluguel sem ter destrancado
				throw new ArgumentException("Não foi possível destrancar a tranca da bicicleta escolhida. Escolha outra bicicleta.");
			}
			this._integracaoExterna.EnviarEmail(ciclista.Email, "Aluguel efetuado com Sucesso - Bikeria SCB", registroDesteAluguel.FormatarParaEmail());
		}

		public void Devolver(int idTranca, int idBicicleta)
		{
			var tranca = this._equipamento.ObterTrancaPorId(idTranca);
			if (tranca.Status.ToUpperInvariant() != "LIVRE")
				throw new ArgumentException("Esta tranca não está disponível para devolver a bicicleta, escolha outra tranca.");

			var bike = this._equipamento.ObterBicicletaPorId(idBicicleta);

			if (bike == null)
				throw new ArgumentException("Bicicleta com identificador Inválido. Favor entrar em contato com Suporte.");

			if (bike.Status.ToUpperInvariant() != "EM_USO")
				throw new ArgumentException("Esta bicicleta não está em uso. Favor entrar em contato com Suporte.");

			var registroAluguel = Database.ObterAluguelAtivoPorBicicleta(idBicicleta);
			if (registroAluguel == null)
				throw new ArgumentException("Não foi possivel localizar o registro do aluguel");

			var ciclista = Database.ObterCiclistaPorId(registroAluguel.IdCiclista);

			var valoraAPagar = CalcularValorDevido(registroAluguel.DataHoraRetirada);

			this._integracaoExterna.EfetuarCobranca(ciclista.Id, valoraAPagar);

			var cartao = Database.ObterMeioDePagamentoPorIdCiclista(ciclista.Id);
			var regDevolucao = new RegistroDevolucao(DateTime.Now, DateTime.Now, valoraAPagar, cartao.Numero, idBicicleta, idTranca, registroAluguel);

			registroAluguel.RegistroDevolucao = regDevolucao;
			// aqui teria atualizar Registro Aluguel no Banco se tivesse banco

			Database.ArmazenarRegistroDevolucao(regDevolucao);

			_equipamento.AlterarStatusBicicleta(bike.Id, "DISPONIVEL");

			this._equipamento.TrancarTranca(idTranca);

			this._integracaoExterna.EnviarEmail(ciclista.Email, "Devolução Efetuada com Sucesso - Bikeria SCB", regDevolucao.FormatarParaEmail());
		}


		// status bike ['DISPONIVEL','EM_USO', 'NOVA', 'APOSENTADA', 'REPARO_SOLICITADO', 'EM_REPARO' ]

		#region Métodos privados

		private static Ciclista ObterCiclistaDominio(int idCiclista) =>
			Database.ObterCiclistaPorId(idCiclista) ?? throw new EntidadeInexistenteException($"Ciclista com id {idCiclista} não existe.");

		private static Funcionario ObterFuncionarioDominio(int idFuncionario) =>
			Database.ObterFuncionarioPorId(idFuncionario) ?? throw new EntidadeInexistenteException($"Funcionario com id {idFuncionario} não existe.");


		private MeioDePagamento CriarMeioDePagamentoValidado(MeioDePagamentoDto dto)
		{
			var meioPagamentoDominio = new MeioDePagamento(dto);

			if (!this._integracaoExterna.MeioPagamnentoValido(meioPagamentoDominio))
				throw new ArgumentException("Meio de pagamento informado não pôde ser validado junto a operadora. Favor fornecer um outro meio de pagamento.");

			return meioPagamentoDominio;
		}

		private static float CalcularValorDevido(DateTime horaRetirada)
		{
			var tempoTotal = DateTime.Now - horaRetirada;

			if (tempoTotal <= TimeSpan.FromHours(2))
				return 0f;
			else
				return 5 * (int)Math.Ceiling((tempoTotal - TimeSpan.FromHours(2)).TotalMinutes / 30);
		}

		#endregion
	}
}
