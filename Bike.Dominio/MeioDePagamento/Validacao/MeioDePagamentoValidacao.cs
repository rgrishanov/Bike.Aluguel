using Bike.Dto.Ciclista;
using FluentValidation;

namespace Bike.Dominio.Ciclista.Validacao
{
	public class MeioDePagamentoValidacao : AbstractValidator<MeioDePagamentoDto>
	{
		public MeioDePagamentoValidacao()
		{
			this.RuleFor(p => p.NomeTitular)
				.NotEmpty().WithMessage("Nome do Titular do Meio de Pagamento não pode ser vazio");
			
			this.RuleFor(p => p.Numero)
				.NotEmpty().WithMessage("Numero do Meio de Pagamento não pode ser vazio");

			this.RuleFor(p => p.Validade)
				.GreaterThan(DateTime.Now)
				.WithMessage("O Meio de Pagamento já está vencido e não pode ser utilizado para este cadastro");

			this.RuleFor(p => p.Cvv)
				.NotEmpty().WithMessage("CVV do Meio de Pagamento não pode ser vazio");
		}
	}
}
