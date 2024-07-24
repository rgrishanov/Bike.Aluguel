using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
