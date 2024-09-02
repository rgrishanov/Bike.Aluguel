using Bike.Dto.Ciclista;
using Bike.Dto.Equipamento;
using Bike.Dto.Funcionario;
using BikeApi.Aplicacao.AluguelServico;
using Microsoft.AspNetCore.Mvc;

namespace BikeApi.Controllers
{
	/// <summary>
	/// Controller de Aluguel
	/// </summary>
	/// <remarks>
	/// Microsservi�o de Aluguel de Bicicletas
	/// </remarks>
	/// <param name="aluguelServico"></param>
	[ApiController]
	public partial class AluguelController(IAluguelServico aluguelServico) : ControllerBase
	{
		private readonly IAluguelServico _aluguelServico = aluguelServico;

		/// <summary>
		/// Efetua cadastro de um Ciclista
		/// </summary>
		/// <returns>Http status 201(Created)</returns>
		/// <response code="201">Ciclista cadastrado com Sucesso</response> 
		/// <response code="404">Requisi��o mal formada</response> 
		/// <response code="422">Dados para cadastro de Ciclista Inv�lidos</response> 
		[HttpPost("ciclista")]
		[ProducesResponseType(201, Type = typeof(ObterCiclistaDto))]
		public CreatedAtActionResult CadastrarCiclista(CadastroCiclistaInicialDto dto)
		{
			var ciclistaCriado = _aluguelServico.CadastrarCiclista(dto);

			return CreatedAtAction(nameof(CadastrarCiclista), new { idCiclista = ciclistaCriado.Id }, ciclistaCriado);
		}

		/// <summary>
		/// Obt�m um Ciclista por Id
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Cadastro Ativado com Sucesso</response> 
		/// <response code="404">Ciclista Inexistente</response> 
		/// <response code="422">Dados Inv�lidos</response> 
		[HttpGet("ciclista/{idCiclista}")]
		[ProducesResponseType(200, Type = typeof(ObterCiclistaDto))]
		public OkObjectResult ObterCiclista([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			return new OkObjectResult(_aluguelServico.ObterCiclista(idCiclista));
		}

		/// <summary>
		/// Efetua Ativa��o do Cadastro novo de um Ciclista
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Cadastro Ativado com Sucesso</response> 
		/// <response code="404">Ciclista Inexistente</response> 
		/// <response code="422">Dados para ativa��o do cadastro Inv�lidos</response> 
		[HttpPost("ciclista/{idCiclista}/ativar")]
		[ProducesResponseType(200, Type = typeof(ObterCiclistaDto))]
		public OkObjectResult AtivarCadastroCiclista([FromRoute(Name = "idCiclista")] int idCiclista)
		{

			return new OkObjectResult(_aluguelServico.AtivarCiclista(idCiclista));
		}

		/// <summary>
		/// Verifica se o ciclista pode alugar uma bicicleta, j� que s� pode alugar uma por vez.
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
		/// Obt�m bicicleta alugada por um ciclista (ou vazio caso contr�rio)
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
		/// Verifica se o e-mail j� foi utilizado por algum ciclista.
		/// </summary>
		/// <param name="idCiclista"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Email verificado com Sucesso</response> 
		/// <response code="400">Email n�o enviado como par�metro</response>
		/// <response code="422">Dados Inv�lidos</response>
		[HttpGet("ciclista/existeEmail/{email}")]
		[ProducesResponseType(200, Type = typeof(bool))]
		public OkObjectResult VerificarEmailJaEmUso([FromRoute(Name = "email")] string email)
		{
			return new OkObjectResult(_aluguelServico.EmailJaEstaEmUso(email));
		}

		/// <summary>
		/// Obt�m todos os Funcionarios
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
		/// <response code="422">Dados para cadastro de Funcionario Inv�lidos</response> 
		[HttpPost("funcionario")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public OkObjectResult CadastrarFuncionario(FuncionarioBaseDto dto)
		{
			return new OkObjectResult(_aluguelServico.CadastrarFuncionario(dto));
		}

		/// <summary>
		/// Obt�m um Funcionario por Id
		/// </summary>
		/// <param name="idFuncionario"></param>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Funcionario Obtido com Sucesso</response> 
		/// <response code="404">Funcionario Inexistente</response> 
		/// <response code="422">Dados Inv�lidos</response> 
		[HttpGet("funcionario/{idFuncionario}")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public OkObjectResult ObterFuncionario([FromRoute(Name = "idFuncionario")] int idFuncionario)
		{
			return new OkObjectResult(_aluguelServico.ObterFuncionario(idFuncionario));
		}

		/// <summary>
		/// Efetua Altera��o de um Funcionario
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Funcionario alterado com Sucesso</response> 
		/// <response code="404">Funcionario com este ID n�o existe</response> 
		/// <response code="422">Dados para cadastro de Funcionario Inv�lidos</response> 
		[HttpPut("funcionario/{idFuncionario}")]
		[ProducesResponseType(200, Type = typeof(FuncionarioDto))]
		public IActionResult AlterarFuncionario([FromRoute(Name = "idFuncionario")] int idFuncionario, FuncionarioBaseDto dto)
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
		/// <response code="422">Dados Inv�lidos</response> 
		[HttpDelete("funcionario/{idFuncionario}")]
		[ProducesResponseType(200)]
		public IActionResult ExcluirFuncionario([FromRoute(Name = "idFuncionario")] int idFuncionario)
		{
			_aluguelServico.ExcluirFuncionario(idFuncionario);
			return StatusCode(200);
		}

		/// <summary>
		/// Efetua Altera��o de um Ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Ciclista alterado com Sucesso</response> 
		/// <response code="404">Ciclista com este ID n�o existe</response> 
		/// <response code="422">Dados para cadastro de Ciclista Inv�lidos</response> 
		[HttpPut("ciclista/{idCiclista}")]
		[ProducesResponseType(200, Type = typeof(ObterCiclistaDto))]
		public IActionResult AlterarCiclista([FromRoute(Name = "idCiclista")] int idCiclista, CiclistaDto dto)
		{
			return new OkObjectResult(_aluguelServico.AlterarCiclista(idCiclista, dto));
		}

		/// <summary>
		/// Efetua Altera��o do Meio de Pagamento de um Ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Dados de pagamento obtidos com Sucesso</response> 
		/// <response code="404">Ciclista com este ID n�o existe</response> 
		/// <response code="422">Dados Inv�lidos</response> 
		[HttpGet("cartaoDeCredito/{idCiclista}")]
		[ProducesResponseType(200, Type = typeof(MeioDePagamentoDto))]
		public OkObjectResult ObterMeioDePagamentoCiclista([FromRoute(Name = "idCiclista")] int idCiclista)
		{
			return new OkObjectResult(_aluguelServico.ObterMeioDePagamentoCiclista(idCiclista));
		}

		/// <summary>
		/// Recupera dados de cart�o de cr�dito de um ciclista
		/// </summary>
		/// <returns>Http status 200(OK)</returns>
		/// <response code="200">Meio de Pagamento alterado com Sucesso</response> 
		/// <response code="404">Ciclista com este ID n�o existe</response> 
		/// <response code="422">Dados para Altera��o de Meio de Pagamento Inv�lidos</response> 
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
		/// <response code="404">Ciclista ou Bicicleta com este ID n�o existe</response> 
		/// <response code="422">Dados Inv�lidos</response> 
		[HttpPost("aluguel")]
		[ProducesResponseType(200)]
		public IActionResult AlterarMeioDePagamento(int ciclista, int trancaInicio)
		{
			_aluguelServico.Alugar(ciclista, trancaInicio);

			return StatusCode(200);
		}
	}
}
