using FluentValidation;
using System.Text;

namespace Bike.Dominio.Validacao
{
	public static class Validador
	{
		public static void Validar<T>(T objeto, AbstractValidator<T> validador)
		{

			var validacaoResult = validador.Validate(objeto);

			if (!validacaoResult.IsValid)
			{
				StringBuilder builder = new StringBuilder();

				foreach (var item in validacaoResult.Errors.Select(x => x.ErrorMessage))
					builder.Append($"{item}, ");

				builder.Remove(builder.Length - 2, 2);

				throw new ArgumentException(builder.ToString());
			}
		}
	}
}
