using Bike.Dominio.Ciclista.Validacao;
using Bike.Dominio.Validacao;
using Bike.Dto.Ciclista;

namespace BikeApi.Dominio.Ciclista
{
	public class Passaporte
	{
		public string Numero { get; set; }
		public DateTime Validade { get; set; }
		public string Pais { get; set; }

        public Passaporte(PassaporteDto dto)
        {
			Validador.Validar(dto, new PassaporteValidacao());

			Numero = dto.Numero!;
			Validade = dto.Validade!;
			Pais = dto.Pais!;
		}
    }
}
