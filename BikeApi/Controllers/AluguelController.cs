using Bike.Dto.CadastroCiclista;
using BikeApi.Aplicacao.AluguelServico;
using Microsoft.AspNetCore.Mvc;

namespace BikeApi.Controllers
{
	[ApiController]
	//[Route("aluguel")]
	public class AluguelController : ControllerBase
	{
		private readonly IAluguelServico _aluguelServico;
		public AluguelController(IAluguelServico aluguelServico)
		{
			this._aluguelServico = aluguelServico;
		}

		/// <summary>
		/// Efetua cadastro de um Ciclista
		/// </summary>
		/// <returns>Http status 201(Created)</returns>
		/// <response code="201">Ciclista cadastrados com Sucesso</response> 
		/// <response code="400">Erro ao cadastrar o Ciclista</response> 
		/// <response code="500">Erro inesperado</response> 
		[HttpPost("ciclista")]
		[ProducesResponseType(201)]
		public OkObjectResult CadastrarCiclista(CadastroCiclistaDto dto)
		{

			return Ok(_aluguelServico.MetodoTeste());
		}

	}
}
