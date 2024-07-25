using Bike.Dominio;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace Bike.Api.ControleErros
{
	/// <summary>
	/// Filtra exceções pra retornar um json com a mensagem de erro amigável
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ExceptionFilter : IExceptionFilter
	{
		/// <summary>
		/// Filtra exceções pra retornar um json com a mensagem de erro amigável
		/// </summary>
		/// <param name="context"></param>
		public void OnException(ExceptionContext context)
		{
			var exceptionType = context.Exception.GetType();
			var mensagem = context.Exception.Message;

			HttpStatusCode codigo;

			if (exceptionType == typeof(ArgumentException) || exceptionType == typeof(ArgumentNullException))
				codigo = HttpStatusCode.UnprocessableEntity;
			else if (exceptionType == typeof(EntidadeInexistenteException))
				codigo = HttpStatusCode.NotFound;
			else
			{
				mensagem = "Requisição mal formada";
				codigo = HttpStatusCode.BadRequest;
			}

			context.ExceptionHandled = true;
			HttpResponse response = context.HttpContext.Response;
			response.StatusCode = (int)codigo;
			response.ContentType = "application/json";

			string jsonErro = JsonSerializer.Serialize(new { codigo, mensagem });

			response.WriteAsync(jsonErro);
		}
	}
}