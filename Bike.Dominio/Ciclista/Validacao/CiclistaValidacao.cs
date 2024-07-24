using FluentValidation;
using Bike.Dto.CadastroCiclista;
using Bike.Dominio.Validacao;

namespace Bike.Dominio.Ciclista.Validacao
{
	public class CiclistaValidacao : AbstractValidator<CiclistaDto>
	{
		public CiclistaValidacao()
		{
			this.RuleFor(p => p.Nome)
				.NotEmpty().WithMessage("Nome do Ciclista não pode ser vazio.");

			this.RuleFor(x => x.Nascimento)
				.LessThan(DateTime.Now)
				.WithMessage("Data de Nascimento deve ser anterior a hoje.").Unless(x => x.Nascimento < new DateTime(1900, 1, 1))
				
				.GreaterThan(new DateTime(1900, 1, 1))
				.WithMessage("Data de Nascimento deve ser no século 20 ou 21.");

			this.RuleFor(x => x.Cpf)
				.NotEmpty().WithMessage("CPF do Ciclista não pode ser vazio.")
				.Must(c => Validar.CpfCnpj(c))
				.WithMessage("CPF/CNPJ do Ciclista é inválido.");

			this.RuleFor(p => p.Nacionalidade)
				.MaximumLength(128).WithMessage("Nacionalidade do Ciclista não pode ser vazia.");

			this.RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("Email do Ciclista não pode ser vazio.")

				.Must(x => Validar.Email(x)).Unless(x => string.IsNullOrEmpty(x.Email))
				.WithMessage("Email de Usuário é inválido.");

			this.RuleFor(p => p.UrlFotoDocumento)
				.NotEmpty().WithMessage("Url da Foto do Documento do Ciclista não pode ser vazio.");

			this.RuleFor(p => p.Senha)
				.NotEmpty()
				.WithMessage("Senha não pode ser vazio.")

				.Equal(p => p.SenhaConfirmacao)
				.WithMessage("Senha e Confirmação de senha são diferentes.");
		}
	}

}
