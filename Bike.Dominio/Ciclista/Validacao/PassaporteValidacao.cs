using Bike.Dto.Ciclista;
using FluentValidation;
using System.Globalization;

namespace Bike.Dominio.Ciclista.Validacao
{
	public class PassaporteValidacao : AbstractValidator<PassaporteDto>
	{
		public PassaporteValidacao()
		{
			this.RuleFor(p => p.Numero)
				.NotEmpty().WithMessage("Numero do Passaporte não pode ser vazio");

			this.RuleFor(p => DateTime.ParseExact(p.Validade!, "yyyy-MM-dd", CultureInfo.GetCultureInfo("pt-BR"))).Cascade(CascadeMode.Stop)

				.GreaterThan(new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc))
				.WithMessage("Data de Validade do Passaporte deve ser preenchida")

				.GreaterThan(DateTime.Now)
				.WithMessage("O Passaporte já está vencido e não pode ser utilizado para cadastro");

			this.RuleFor(p => p.Pais)
				.NotEmpty().WithMessage("Pais do Passaporte não pode ser vazio");
		}
	}
}
