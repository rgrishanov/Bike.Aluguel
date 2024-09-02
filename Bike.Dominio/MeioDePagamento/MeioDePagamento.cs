using Bike.Dominio.Ciclista.Validacao;
using Bike.Dominio.Validacao;
using Bike.Dto.Ciclista;

namespace BikeApi.Dominio.MeioDePagamento
{
	public class MeioDePagamento
	{
		public int Id { get; private set; }
		public int IdCiclista { get; private set; }
		public string NomeTitular { get; private set; }
		public string Numero { get; private set; }
		public DateTime Validade { get; private set; }
		public string Cvv { get; private set; }

		public MeioDePagamento(MeioDePagamentoDto dto)
		{
			Validador.Validar(dto, new MeioDePagamentoValidacao());

			NomeTitular = dto.NomeTitular;
			Numero = dto.Numero;
			Validade = dto.Validade;
			Cvv = dto.Cvv;
		}

		public void SetarIdInicial(int id)
		{
			if (this.Id == 0)
				this.Id = id;
			else
				throw new ArgumentException("Não é possível alterar o Id do Meio de Pagamento.");
		}

		public void SetarIdCiclista(int id)
		{
			if (this.IdCiclista == 0)
				this.IdCiclista = id;
			else
				throw new ArgumentException("Não é possível alterar o IdCiclista do Meio de Pagamento.");
		}

		public ObterMeioDePagamentoDto MapearParaDto()
		{
			return new ObterMeioDePagamentoDto()
			{
				Id = this.Id,
				NomeTitular = this.NomeTitular,
				Numero = this.Numero,
				Validade = this.Validade,
				Cvv = this.Cvv
			};
		}
	}
}
