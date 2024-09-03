using Bike.Dominio.Aluguel;
using Bike.Dto.Ciclista;
using Bike.Dto.Equipamento;
using Bike.Dto.Funcionario;
using BikeApi.Aplicacao.AluguelServico;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.Funcionario;
using BikeApi.Dominio.MeioDePagamento;
using BikeApi.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BikeApi.Controllers
{
	/// <summary>
	/// Controller de Aluguel
	/// </summary>
	/// <remarks>
	/// Microsserviço de Aluguel de Bicicletas
	/// </remarks>
	/// <param name="aluguelServico"></param>
	[ApiController]
	[ExcludeFromCodeCoverage] // chamadas ultra simples, e maior parte do código é inserção de dados iniciais que nem faz parte da aplicação
	public partial class AluguelController(IAluguelServico aluguelServico) : ControllerBase
	{
		private readonly IAluguelServico _aluguelServico = aluguelServico;

		/// <summary>
		/// Restaura os Dados básicas para execução de testes
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Dados restauradas com Sucesso</response> 
		[HttpGet("restaurarDados")]
		[ProducesResponseType(200)]
		public IActionResult RestaurarDados()
		{
			// primeiro limpa o banco
			Database.Purge();

			// começa ciclista 1

			var ciclista1dto = new CiclistaDto()
			{
				Cpf = "78804034009",
				Nome = "Fulano Beltrano",
				Email = "user@example.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
				Senha = "ABC123",
				ConfirmacaoSenha = "ABC123",
				UrlFotoDocumento = "http://url.com/foto"
			};
			var meio1dto = new MeioDePagamentoDto()
			{
				NomeTitular = "Fulano Beltrano",
				Numero = "4012001037141112",
				Cvv = "132",
				Validade = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc)
			};

			var ciclista1 = new Ciclista(ciclista1dto);
			ciclista1.AtivarCadastro();
			Database.ArmazenarCiclista(ciclista1);

			var meio1 = new MeioDePagamento(meio1dto);
			Database.ArmazenarMeioDePagamento(meio1);
			meio1.SetarIdCiclista(ciclista1.Id);

			// termina ciclista 1

			// começa ciclista 2

			var ciclista2dto = new CiclistaDto()
			{
				Cpf = "43943488039",
				Nome = "Fulano Beltrano",
				Email = "user2@example.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
				Senha = "ABC123",
				ConfirmacaoSenha = "ABC123",
				UrlFotoDocumento = "http://url.com/foto"
			};

			var meio2dto = new MeioDePagamentoDto()
			{
				NomeTitular = "Fulano Beltrano",
				Numero = "4012001037141112",
				Cvv = "132",
				Validade = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc)
			};

			var ciclista2 = new Ciclista(ciclista2dto);
			Database.ArmazenarCiclista(ciclista2);

			var meio2 = new MeioDePagamento(meio2dto);
			Database.ArmazenarMeioDePagamento(meio2);
			meio2.SetarIdCiclista(ciclista2.Id);

			// termina ciclista 2

			// começa ciclista 3

			var ciclista3dto = new CiclistaDto()
			{
				Cpf = "10243164084",
				Nome = "Fulano Beltrano",
				Email = "user3@example.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
				Senha = "ABC123",
				ConfirmacaoSenha = "ABC123",
				UrlFotoDocumento = "http://url.com/foto"
			};

			var meio3dto = new MeioDePagamentoDto()
			{
				NomeTitular = "Fulano Beltrano",
				Numero = "4012001037141112",
				Cvv = "132",
				Validade = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc)
			};

			var ciclista3 = new Ciclista(ciclista3dto);
			ciclista3.AtivarCadastro();
			Database.ArmazenarCiclista(ciclista3);

			var meio3 = new MeioDePagamento(meio3dto);
			Database.ArmazenarMeioDePagamento(meio3);
			meio3.SetarIdCiclista(ciclista3.Id);

			// termina ciclista 3

			// começa ciclista 4

			var ciclista4dto = new CiclistaDto()
			{
				Cpf = "30880150017",
				Nome = "Fulano Beltrano",
				Email = "user4@example.com",
				Nacionalidade = "BRASILEIRO",
				Nascimento = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
				Senha = "ABC123",
				ConfirmacaoSenha = "ABC123",
				UrlFotoDocumento = "http://url.com/foto"
			};

			var meio4dto = new MeioDePagamentoDto()
			{
				NomeTitular = "Fulano Beltrano",
				Numero = "4012001037141112",
				Cvv = "132",
				Validade = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc)
			};

			var ciclista4 = new Ciclista(ciclista4dto);
			ciclista4.AtivarCadastro();
			Database.ArmazenarCiclista(ciclista4);

			var meio4 = new MeioDePagamento(meio4dto);
			Database.ArmazenarMeioDePagamento(meio4);
			meio4.SetarIdCiclista(ciclista4.Id);

			// termina ciclista 4

			// começa Funcionário 1

			var dtoFunc = new FuncionarioBaseDto()
			{
				Cpf = "99999999999",
				Nome = "Beltrano",
				Email = "employee@example.com",
				Senha = "123",
				ConfirmacaoSenha = "123",
				Funcao = "REPARADOR",
				Idade = 25
			};

			var dominioFunc = new Funcionario(dtoFunc, false);
			dominioFunc.ForcarMatricula("12345");

			Database.ArmazenarFuncionario(dominioFunc);

			// termina Funcionário 1



			// começa aluguel 1

			var reg1 = new RegistroAluguel()
			{
				IdBicicleta = 3,
				IdCiclista = 3,
				IdTranca = 2,
				MeioPagamento = meio1.Numero
			};
			Database.ArmazenarRegistroAluguel(reg1);

			// termina aluguel 1


			// começa aluguel 2

			var reg2 = new RegistroAluguel()
			{
				IdBicicleta = 5,
				IdCiclista = 4,
				IdTranca = 4,
				MeioPagamento = meio2.Numero
			};
			reg2.ForcarDataRetirada(DateTime.Now.AddHours(-2));
			Database.ArmazenarRegistroAluguel(reg2);

			// termina aluguel 2



			// começa aluguel 3

			var reg3 = new RegistroAluguel()
			{
				IdBicicleta = 1,
				IdCiclista = 3,
				IdTranca = 1,
				MeioPagamento = meio3.Numero
			};
			reg2.ForcarDataRetirada(DateTime.Now.AddHours(-2));
			Database.ArmazenarRegistroAluguel(reg3);

			var regDev3 = new RegistroDevolucao(DateTime.Now, DateTime.Now, 5, meio3.Numero, 1, 2, reg3);
			Database.ArmazenarRegistroDevolucao(regDev3);

			// termina aluguel 3


			return StatusCode(200);
		}

		/// <summary>
		/// Efetua cadastro de um Ciclista
		/// </summary>
		/// <returns>Http status 201(Created)</returns>
		/// <response code="201">Ciclista cadastrado com Sucesso</response> 
		/// <response code="404">Requisição mal formada</response> 
		/// <response code="422">Dados para cadastro de Ciclista Inválidos</response> 
		[HttpPost("ciclista")]
		[ProducesResponseType(201, Type = typeof(ObterCiclistaDto))]
		public CreatedAtActionResult CadastrarCiclista(CadastroCiclistaInicialDto dto)
		{
			var ciclistaCriado = _aluguelServico.CadastrarCiclista(dto);

			return CreatedAtAction(nameof(CadastrarCiclista), new { idCiclista = ciclistaCriado.Id }, ciclistaCriado);
		}

		/// <summary>
		/// Obtém um Ciclista por Id
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Cadastro Ativado com Sucesso</response> 
		/// <response code="404">Ciclista Inexistente</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpGet("ciclista/{idCiclista}")]
		[ProducesResponseType(200, Type = typeof(ObterCiclistaDto))]
		public OkObjectResult ObterCiclista([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			return new OkObjectResult(_aluguelServico.ObterCiclista(idCiclista));
		}

		/// <summary>
		/// Efetua Ativação do Cadastro novo de um Ciclista
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Cadastro Ativado com Sucesso</response> 
		/// <response code="404">Ciclista Inexistente</response> 
		/// <response code="422">Dados para ativação do cadastro Inválidos</response> 
		[HttpPost("ciclista/{idCiclista}/ativar")]
		[ProducesResponseType(200, Type = typeof(ObterCiclistaDto))]
		public OkObjectResult AtivarCadastroCiclista([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			return new OkObjectResult(_aluguelServico.AtivarCiclista(idCiclista));
		}

		/// <summary>
		/// Verifica se o ciclista pode alugar uma bicicleta, já que só pode alugar uma por vez.
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Verificado com Sucesso</response> 
		/// <response code="404">Ciclista Inexistente</response>
		[HttpGet("ciclista/{idCiclista}/permiteAluguel")]
		[ProducesResponseType(200, Type = typeof(bool))]
		public OkObjectResult PermiteAluguel([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			return new OkObjectResult(_aluguelServico.CiclistaPodeAlugar(idCiclista));
		}

		/// <summary>
		/// Obtém bicicleta alugada por um ciclista (ou vazio caso contrário)
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Bicicleta obtida com Sucesso</response> 
		/// <response code="404">Ciclista Inexistente</response>
		[HttpGet("ciclista/{idCiclista}/bicicletaAlugada")]
		[ProducesResponseType(200, Type = typeof(BicicletaDto))]
		public OkObjectResult ObterBicicletaAlugada([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			var retorno = _aluguelServico.ObterBicicletaAlugada(idCiclista);

			if (retorno == null)
				return new OkObjectResult("");

			return new OkObjectResult(retorno);
		}

		/// <summary>
		/// Verifica se o e-mail já foi utilizado por algum ciclista.
		/// </summary>
		/// <param name="email"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Email verificado com Sucesso</response> 
		/// <response code="400">Email não enviado como parâmetro</response>
		/// <response code="422">Dados Inválidos</response>
		[HttpGet("ciclista/existeEmail/{email}")]
		[ProducesResponseType(200, Type = typeof(bool))]
		public OkObjectResult VerificarEmailJaEmUso([FromRoute(Name = "email")] string email)
		{
			return new OkObjectResult(_aluguelServico.EmailJaEstaEmUso(email));
		}

		/// <summary>
		/// Obtém todos os Funcionarios
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Funcionarios obtidos com Sucesso</response> 
		[HttpGet("funcionario")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public OkObjectResult ObterFuncionarios()
		{
			return new OkObjectResult(_aluguelServico.ObterFuncionarios());
		}

		/// <summary>
		/// Efetua cadastro de um Funcionario
		/// </summary>
		/// <returns>Http status 200(Created)</returns>
		/// <response code="200">Funcionario cadastrado com Sucesso</response> 
		/// <response code="422">Dados para cadastro de Funcionario Inválidos</response> 
		[HttpPost("funcionario")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public OkObjectResult CadastrarFuncionario(FuncionarioBaseDto dto)
		{
			return new OkObjectResult(_aluguelServico.CadastrarFuncionario(dto));
		}

		/// <summary>
		/// Obtém um Funcionario por Id
		/// </summary>
		/// <param name="idFuncionario"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Funcionario Obtido com Sucesso</response> 
		/// <response code="404">Funcionario Inexistente</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpGet("funcionario/{idFuncionario}")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public OkObjectResult ObterFuncionario([FromRoute(Name = "idFuncionario")] int idFuncionario)
		{
			return new OkObjectResult(_aluguelServico.ObterFuncionario(idFuncionario));
		}

		/// <summary>
		/// Efetua Alteração de um Funcionario
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Funcionario alterado com Sucesso</response> 
		/// <response code="404">Funcionario com este ID não existe</response> 
		/// <response code="422">Dados para cadastro de Funcionario Inválidos</response> 
		[HttpPut("funcionario/{idFuncionario}")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public OkObjectResult AlterarFuncionario([FromRoute(Name = "idFuncionario")] int idFuncionario, FuncionarioBaseDto dto)
		{
			return new OkObjectResult(_aluguelServico.AlterarFuncionario(idFuncionario, dto));
		}

		/// <summary>
		/// Exclui um Funcionario
		/// </summary>
		/// <param name="idFuncionario"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Funcionario excluido com Sucesso</response> 
		/// <response code="404">Funcionario Inexistente</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpDelete("funcionario/{idFuncionario}")]
		[ProducesResponseType(200)]
		public IActionResult ExcluirFuncionario([FromRoute(Name = "idFuncionario")] int idFuncionario)
		{
			_aluguelServico.ExcluirFuncionario(idFuncionario);
			return StatusCode(200);
		}

		/// <summary>
		/// Efetua Alteração de um Ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Ciclista alterado com Sucesso</response> 
		/// <response code="404">Ciclista com este ID não existe</response> 
		/// <response code="422">Dados para cadastro de Ciclista Inválidos</response> 
		[HttpPut("ciclista/{idCiclista}")]
		[ProducesResponseType(200, Type = typeof(ObterCiclistaDto))]
		public IActionResult AlterarCiclista([FromRoute(Name = "idCiclista")] int idCiclista, CiclistaDto dto)
		{
			return new OkObjectResult(_aluguelServico.AlterarCiclista(idCiclista, dto));
		}

		/// <summary>
		/// Efetua Alteração do Meio de Pagamento de um Ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Dados de pagamento obtidos com Sucesso</response> 
		/// <response code="404">Ciclista com este ID não existe</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpGet("cartaoDeCredito/{idCiclista}")]
		[ProducesResponseType(200, Type = typeof(MeioDePagamentoDto))]
		public OkObjectResult ObterMeioDePagamentoCiclista([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			return new OkObjectResult(_aluguelServico.ObterMeioDePagamentoCiclista(idCiclista));
		}

		/// <summary>
		/// Recupera dados de cartão de crédito de um ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Meio de Pagamento alterado com Sucesso</response> 
		/// <response code="404">Ciclista com este ID não existe</response> 
		/// <response code="422">Dados para Alteração de Meio de Pagamento Inválidos</response> 
		[HttpPut("cartaoDeCredito/{idCiclista}")]
		[ProducesResponseType(200)]
		public IActionResult AlterarMeioDePagamentoCiclista([FromRoute(Name = "idCiclista")] int idCiclista, MeioDePagamentoDto dto)
		{
			_aluguelServico.AlterarMeioDePagamentoCiclista(idCiclista, dto);

			return StatusCode(200);
		}

		/// <summary>
		/// Realiza Aluguel de uma Bicicleta pelo Ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Aluguel realizado com Sucesso</response> 
		/// <response code="404">Ciclista ou Bicicleta com este ID não existe</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpPost("aluguel")]
		[ProducesResponseType(200)]
		public IActionResult AlugarBicicleta(int ciclista, int trancaInicio)
		{
			_aluguelServico.Alugar(ciclista, trancaInicio);

			return StatusCode(200);
		}

		/// <summary>
		/// Realiza Devolução de uma Bicicleta Alugada pelo Ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Aluguel realizado com Sucesso</response> 
		/// <response code="404">Ciclista ou Bicicleta com este ID não existe</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpPost("devolucao")]
		[ProducesResponseType(200)]
		public IActionResult DevolverBicicleta(int idTranca, int idBicicleta)
		{
			_aluguelServico.Devolver(idTranca, idBicicleta);

			return StatusCode(200);
		}
	}
}
