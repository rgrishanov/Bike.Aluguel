using Bike.Dto.Equipamento;

namespace BikeApi.Aplicacao.AluguelServico
{
	public class IntegracaoEquipamentoServico : IIntegracaoEquipamentoServico
	{
		public BicicletaDto ObterBicicletaNaTranca(int idTranca)
		{
			// temp até integrar

			return new BicicletaDto
			{
				Id = 30,
				Ano = "2021",
				Marca = "Caloi",
				Modelo = "Mountain 9000",
				Numero = 123,
				Status = "status"
			};
		}

		public BicicletaDto ObterBicicletaPorId(int idBicicleta)
		{
			// temp até integrar
			return new BicicletaDto
			{
				Id = 30,
				Ano = "2021",
				Marca = "Caloi",
				Modelo = "Mountain 9000",
				Numero = 123,
				Status = "status"
			};
		}

		public void AlterarStatusBicicleta(int idBicicleta, string novoStatus)
		{
			// temp até integrar
		}

		public void AlterarStatusTranca(int idTranca, string novoStatus)
		{
			// temp até integrar
		}

		public bool DestrancarTranca(int idTranca)
		{
			// temp até integrar

			return true;
		}
	}
}
