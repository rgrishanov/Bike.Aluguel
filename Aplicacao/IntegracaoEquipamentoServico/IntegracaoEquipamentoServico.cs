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

		public bool TrancarTranca(int idTranca)
		{
			// temp até integrar

			return true;
		}

		public TrancaDto ObterTrancaPorId(int idBicicleta)
		{
			return new TrancaDto()
			{
				AnoDeFabricacao = "2022",
				Id = 30,
				Localizacao = "bla",
				Modelo = "UltraLock 9000",
				Status = "LIVRE",
				Bicicleta = idBicicleta,
				Numero = 123
			};
		}
	}
}
