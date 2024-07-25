using Bike.Dto.Ciclista;
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
	public class AluguelController(IAluguelServico aluguelServico) : ControllerBase
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
		public CreatedAtActionResult CadastrarCiclista(CadastroInicialDto dto)
		{
			var ciclistaCriado = _aluguelServico.CadastrarCiclista(dto);

			return CreatedAtAction(nameof(ObterCiclista), new { idCiclista = ciclistaCriado.Id }, ciclistaCriado);
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
		/// <response code="200">Meio de Pagamento alterado com Sucesso</response> 
		/// <response code="404">Ciclista com este ID n�o existe</response> 
		/// <response code="422">Dados para Altera��o de Meio de Pagamento Inv�lidos</response> 
		[HttpPut("cartaoDeCredito/{idCiclista}")]
		[ProducesResponseType(200)]
		public IActionResult AlterarMeioDePagamento([FromRoute(Name = "idCiclista")] int idCiclista, MeioDePagamentoDto dto)
		{
			_aluguelServico.AlterarMeioDePagamento(idCiclista, dto);

			return StatusCode(200);
		}

		/// <summary>
		/// Realiza Aluguem de uma Bicicleta pelo Ciclista
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
