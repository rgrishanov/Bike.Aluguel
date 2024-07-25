using Bike.Dto.Ciclista;
using BikeApi.Aplicacao.AluguelServico;
using Microsoft.AspNetCore.Mvc;

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
	public class AluguelController(IAluguelServico aluguelServico) : ControllerBase
	{
		private readonly IAluguelServico _aluguelServico = aluguelServico;

		/// <summary>
		/// Efetua cadastro de um Ciclista
		/// </summary>
		/// <returns>Http status 201(Created)</returns>
		/// <response code="201">Ciclista cadastrado com Sucesso</response> 
		/// <response code="404">Requisição mal formada</response> 
		/// <response code="422">Dados para cadastro de Ciclista Inválidos</response> 
		[HttpPost("ciclista")]
		[ProducesResponseType(201, Type = typeof(ObterCiclistaDto))]
		public CreatedAtActionResult CadastrarCiclista(CadastroInicialDto dto)
		{
			var ciclistaCriado = _aluguelServico.CadastrarCiclista(dto);

			return CreatedAtAction(nameof(ObterCiclista), new { idCiclista = ciclistaCriado.Id }, ciclistaCriado);
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
		/// <response code="200">Meio de Pagamento alterado com Sucesso</response> 
		/// <response code="404">Ciclista com este ID não existe</response> 
		/// <response code="422">Dados para Alteração de Meio de Pagamento Inválidos</response> 
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
		/// <response code="404">Ciclista ou Bicicleta com este ID não existe</response> 
		/// <response code="422">Dados Inválidos</response> 
		[HttpPost("aluguel")]
		[ProducesResponseType(200)]
		public IActionResult AlterarMeioDePagamento(int ciclista, int trancaInicio)
		{
			_aluguelServico.Alugar(ciclista, trancaInicio);

			return StatusCode(200);
		}
	}
}
