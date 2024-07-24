using Bike.Dominio.Ciclista.Validacao;
using Bike.Dominio.Validacao;
using Bike.Dto.CadastroCiclista;

namespace BikeApi.Dominio.Ciclista
{
	public class Ciclista
	{
		public required string Nome { get; set; }
		public DateTime Nascimento { get; set; }
		public required string Cpf { get; set; }
		public required Passaporte Passaporte { get; set; }
		public required string Nacionalidade { get; set; }
		public required string Email { get; set; }
		public required string UrlFotoDocumento { get; set; }
		public required string Senha { get; set; }

		public Ciclista(CiclistaDto dto)
		{
			Validador.Validar(dto, new CiclistaValidacao());

			Nome = dto.Nome!;
			Nascimento = dto.Nascimento!.Value;
			Cpf = dto.Cpf!;
			//Passaporte = new Passaporte
			//{
			//	Numero = dto.Passaporte!.Numero!,
			//	Validade = dto.Passaporte.Validade,
			//	Pais = dto.Passaporte.Pais!
			//};
			Nacionalidade = dto.Nacionalidade!;
			Email = dto.Email!;
			UrlFotoDocumento = dto.UrlFotoDocumento!;
			Senha = dto.Senha!;

			if (dto.Senha == null)
				throw new System.Exception("Senha não pode ser nula");
		}
    }

	public class Passaporte
	{
		public required string Numero { get; set; }
		public DateTime Validade { get; set; }
		public required string Pais { get; set; }
	}
}
