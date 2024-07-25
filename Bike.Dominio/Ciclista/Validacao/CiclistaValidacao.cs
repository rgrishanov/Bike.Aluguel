using FluentValidation;
using Bike.Dto.Ciclista;
using Bike.Dominio.Validacao;

namespace Bike.Dominio.Ciclista.Validacao
{
	public class CiclistaValidacao : AbstractValidator<CiclistaDto>
	{
		public CiclistaValidacao()
		{
			this.RuleFor(x => x.Nome)
				.NotEmpty().WithMessage("Nome do Ciclista não pode ser vazio");

			this.RuleFor(x => x.Nascimento)
				.LessThan(DateTime.Now)
				.WithMessage("Data de Nascimento do Ciclista deve ser anterior a hoje").Unless(x => x.Nascimento < new DateTime(1900, 1, 1))

				.GreaterThan(new DateTime(1900, 1, 1))
				.WithMessage("Data de Nascimento do Ciclista deve ser no século 20 ou 21");

			// só valida CPF pra Brasileiros
			this.When(x => !string.IsNullOrEmpty(x.Nacionalidade) && x.Nacionalidade.ToUpperInvariant().Equals("BRASILEIRO"), () => {
				this.RuleFor(x => x.Cpf).Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("CPF do Ciclista não pode ser vazio")

				.Must(c => Validar.Cpf(c!))
				.WithMessage("CPF do Ciclista é inválido");
			});

			// só valida Passaporte pra Estrangeiros
			this.When(x => !string.IsNullOrEmpty(x.Nacionalidade) && x.Nacionalidade.ToUpperInvariant().Equals("ESTRANGEIRO"), () => {
				this.RuleFor(x => x.Passaporte)
				.NotEmpty()
				.WithMessage("Dados do Passaporte do Ciclista Estrangeiro devem ser informados");
			});

			this.RuleFor(x => x.Nacionalidade).Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("Nacionalidade do Ciclista não pode ser vazia")

				.Must(x => x!.ToUpperInvariant().Equals("BRASILEIRO") || x.ToUpperInvariant().Equals("ESTRANGEIRO"))
				.WithMessage("Nacionalidade do Ciclista deve ser 'BRASILEIRO' ou 'ESTRANGEIRO'");

			this.RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("Email do Ciclista não pode ser vazio");

			this.RuleFor(x => x.Email)
				.Must(x => Validar.Email(x!)).Unless(x => string.IsNullOrEmpty(x.Email))
				.WithMessage("Email do Ciclista é inválido");

			this.RuleFor(x => x.UrlFotoDocumento)
				.NotEmpty().WithMessage("Url da Foto do Documento do Ciclista não pode ser vazio");

			this.RuleFor(x => x.Senha).Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("Senha não pode ser vazia")

				.Equal(x => x.SenhaConfirmacao)
				.WithMessage("Senha e Confirmação de senha são diferentes");
		}
	}

}
