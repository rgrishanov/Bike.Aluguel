using Bike.Dto.Ciclista;
using FluentValidation;

namespace Bike.Dominio.Ciclista.Validacao
{
	public class PassaporteValidacao : AbstractValidator<PassaporteDto>
	{
		public PassaporteValidacao()
		{
			this.RuleFor(p => p.Numero)
				.NotEmpty().WithMessage("Numero do Passaporte não pode ser vazio");

			this.RuleFor(p => p.Validade).Cascade(CascadeMode.Stop)

				.GreaterThan(new DateTime(1980, 1, 1))
				.WithMessage("Data de Validade do Passaporte deve ser preenchida")

				.GreaterThan(DateTime.Now)
				.WithMessage("O Passaporte já está vencido e não pode ser utilizado para cadastro");

			this.RuleFor(p => p.Pais)
				.NotEmpty().WithMessage("Pais do Passaporte não pode ser vazio");
		}
	}
}
