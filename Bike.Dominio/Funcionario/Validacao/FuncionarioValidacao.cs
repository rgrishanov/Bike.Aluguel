using FluentValidation;
using Bike.Dto.Funcionario;
using Bike.Dominio.Validacao;

namespace Bike.Dominio.Funcionario.Validacao
{
	public class FuncionarioValidacao : AbstractValidator<FuncionarioBaseDto>
	{
		public FuncionarioValidacao()
		{
			this.RuleFor(x => x.Nome)
				.NotEmpty().WithMessage("Nome do Funcionario não pode ser vazio");

			this.RuleFor(x => x.Idade)
				.GreaterThan(17)
				.WithMessage("Funcionário deve ser maior de idade.");

			this.RuleFor(x => x.Cpf).Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("CPF do Funcionario não pode ser vazio")

				.Must(c => Validar.Cpf(c!))
				.WithMessage("CPF do Funcionario é inválido");

			this.RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("Email do Funcionario não pode ser vazio");

			this.RuleFor(x => x.Email)
				.Must(x => Validar.Email(x!)).Unless(x => string.IsNullOrEmpty(x.Email))
				.WithMessage("Email do Funcionario é inválido");

			this.RuleFor(x => x.Funcao).Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("Funcao do Funcionario não pode ser vazia")

				.Must(x => x!.ToUpperInvariant().Equals("ADMINISTRATIVO") || x.ToUpperInvariant().Equals("REPARADOR"))
				.WithMessage("Funcao do Funcionario deve ser 'ADMINISTRATIVO' ou 'REPARADOR'");

			this.RuleFor(x => x.Senha).Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("Senha não pode ser vazia")

				.Equal(x => x.ConfirmacaoSenha).Unless(x => string.IsNullOrEmpty(x.ConfirmacaoSenha))
				.WithMessage("Senha e Confirmação de senha são diferentes");
		}
	}

}
