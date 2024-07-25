﻿using Bike.Dominio;
using Bike.Dominio.Aluguel;
using Bike.Dto.Ciclista;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.MeioDePagamento;
using BikeApi.Persistencia;

namespace BikeApi.Aplicacao.AluguelServico
{
	public class AluguelServico(IIntegracaoExternoServico integracaoExterna, IIntegracaoEquipamentoServico equipamento) : IAluguelServico
	{
		private readonly IIntegracaoExternoServico _integracaoExterna = integracaoExterna;
		private readonly IIntegracaoEquipamentoServico _equipamento = equipamento;

		#region Ciclista

		public ObterCiclistaDto CadastrarCiclista(CadastroInicialDto dto)
		{
			if (dto.Ciclista == null)
				throw new ArgumentException("É obrigatório informar os dados do Ciclista.");
			if (dto.MeioDePagamento == null)
				throw new ArgumentException("É obrigatório informar os dados de Meio do Pagamento.");

			if (Database.EmailJaEstaEmUso(dto.Ciclista.Email!))
				throw new ArgumentException("O e-mail informado já está em uso.");

			var ciclista = new Ciclista(dto.Ciclista);

			var meioPagamento = this.CriarMeioDePagamentoValidado(dto.MeioDePagamento);

			Database.ArmazenarCiclista(ciclista);

			meioPagamento.SetarIdCiclista(ciclista.Id);

			Database.ArmazenarMeioDePagamento(meioPagamento);

			if (!this._integracaoExterna.EnviarEmail(ciclista.Email, "Confirmação do seu Cadastro - Bikeria SCB",
				$"Seu cadastro foi efetuado com sucesso! Clique no Link a seguir para confirmar o seu email: https://www.scb.com.br/api/ciclista/{ciclista.Id}/ativar"))

				throw new ArgumentException("Não foi possível enviar o e-mail de confirmação do cadastro.");

			return this.MapearCiclistaParaDto(ciclista);
		}

		public ObterCiclistaDto AtivarCiclista(int idCiclista)
		{
			var ciclista = this.ObterCiclistaDominio(idCiclista);
			ciclista.AtivarCadastro();

			// aqui teria algo sobre salvar no banco, mas sem banco já está tudo salvo na memória.

			return this.MapearCiclistaParaDto(ciclista);
		}

		public ObterCiclistaDto ObterCiclista(int idCiclista)
		{
			return this.MapearCiclistaParaDto(this.ObterCiclistaDominio(idCiclista));
		}

		public ObterCiclistaDto AlterarCiclista(int idCiclista, CiclistaDto dto)
		{
			var ciclista = this.ObterCiclistaDominio(idCiclista);

			ciclista.Alterar(dto);

			// aqui teria algo sobre salvar no banco, mas sem banco já está tudo salvo na memória.

			if (!this._integracaoExterna.EnviarEmail(ciclista.Email, "Alteração de dados cadastrais - Bikeria SCB",
				$"Seu cadastro foi alterado com sucesso!"))

				throw new ArgumentException("Não foi possível enviar o e-mail de confirmação das Alterações do cadastro.");

			return this.MapearCiclistaParaDto(this.ObterCiclistaDominio(idCiclista));
		}

		public void AlterarMeioDePagamento(int idCiclista, MeioDePagamentoDto dto)
		{
			var ciclista = this.ObterCiclistaDominio(idCiclista);
			var meioPagamento = this.CriarMeioDePagamentoValidado(dto);

			meioPagamento.SetarIdCiclista(ciclista.Id);

			Database.ExcluirMeioDePagamentoDoCiclista(ciclista.Id);
			Database.ArmazenarMeioDePagamento(meioPagamento);

			if (!this._integracaoExterna.EnviarEmail(ciclista.Email, "Alteração do Meio de Pagamento - Bikeria SCB",
				$"Seu Meio de Pagamento foi alterado com sucesso!"))

				throw new ArgumentException("Não foi possível enviar o e-mail de confirmação da alteração do Meio de Pagamento.");
		}

		#endregion

		public void Alugar(int idCiclista, int idTranca)
		{
			var ciclista = this.ObterCiclistaDominio(idCiclista);
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
			_integracaoExterna.EfetuarCobranca(idCiclista, 10.00f);

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

		// status bike ['DISPONIVEL','EM_USO', 'NOVA', 'APOSENTADA', 'REPARO_SOLICITADO', 'EM_REPARO' ]

		#region Métodos privados

		private Ciclista ObterCiclistaDominio(int idCiclista) =>
			Database.ObterCiclistaPorId(idCiclista) ?? throw new EntidadeInexistenteException($"Ciclista com id {idCiclista} não existe.");

		private ObterCiclistaDto MapearCiclistaParaDto(Ciclista ciclista)
		{
			ObterCiclistaDto dto = new()
			{
				Id = ciclista.Id,
				Status = ciclista.Status,
				Nome = ciclista.Nome,
				Nascimento = ciclista.Nascimento,
				Cpf = ciclista.Cpf,
				Nacionalidade = ciclista.Nacionalidade,
				Email = ciclista.Email,
				UrlFotoDocumento = ciclista.UrlFotoDocumento
			};

			if (ciclista.Passaporte != null)
			{
				dto.Passaporte = new()
				{
					Numero = ciclista.Passaporte.Numero,
					Validade = ciclista.Passaporte.Validade,
					Pais = ciclista.Passaporte.Pais
				};
			}

			return dto;
		}
		private MeioDePagamento CriarMeioDePagamentoValidado(MeioDePagamentoDto dto)
		{
			var meioPagamentoDominio = new MeioDePagamento(dto);

			if (!this._integracaoExterna.MeioPagamnentoValido(meioPagamentoDominio))
				throw new ArgumentException("Meio de pagamento informado não pôde ser validado junto a operadora. Favor fornecer um outro meio de pagamento.");

			return meioPagamentoDominio;
		}

		#endregion
	}
}
