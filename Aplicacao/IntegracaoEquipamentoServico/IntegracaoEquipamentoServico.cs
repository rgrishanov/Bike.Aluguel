using Bike.Dto.Equipamento;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BikeApi.Aplicacao.AluguelServico
{
	[ExcludeFromCodeCoverage] // Excluindo pois não há como fazer testes de integração na falta de ambiente de testes estável.
	public class IntegracaoEquipamentoServico : IIntegracaoEquipamentoServico
	{
		private readonly static string urlEquipamento = "http://34.95.254.151:3000/api/";
		private readonly JsonSerializerOptions options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		public BicicletaDto ObterBicicletaNaTranca(int idTranca)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					var url = urlEquipamento + $"tranca/{idTranca}/bicicleta";

					HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();

					var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

					return JsonSerializer.Deserialize<BicicletaDto>(content, options)!;
				}
				catch (Exception ex)
				{
					return null!;
				}
			}

			//return new BicicletaDto
			//{
			//	Id = 30,
			//	Ano = "2021",
			//	Marca = "Caloi",
			//	Modelo = "Mountain 9000",
			//	Numero = 123,
			//	Status = "status"
			//};
		}

		public BicicletaDto ObterBicicletaPorId(int idBicicleta)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = client.GetAsync(urlEquipamento + $"bicicleta/{idBicicleta}").GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();

					return JsonSerializer.Deserialize<BicicletaDto>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(), options)!;
				}
				catch (Exception ex)
				{
					return null!;
				}
			}


			//return new BicicletaDto
			//{
			//	Id = 30,
			//	Ano = "2021",
			//	Marca = "Caloi",
			//	Modelo = "Mountain 9000",
			//	Numero = 123,
			//	Status = "status"
			//};
		}

		public BicicletaDto AlterarStatusBicicleta(int idBicicleta, string novoStatus)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = client.PostAsync(urlEquipamento + $"bicicleta/{idBicicleta}/status/{novoStatus}", null).GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();

					return JsonSerializer.Deserialize<BicicletaDto>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(), options)!;
				}
				catch (Exception ex)
				{
					return null!;
				}
			}
		}

		public void AlterarStatusTranca(int idTranca, string novoStatus)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = client.PostAsync(urlEquipamento + $"tranca/{idTranca}/status/{novoStatus}", null).GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{

				}
			}
		}

		public bool DestrancarTranca(int idTranca)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string url = urlEquipamento + $"tranca/{idTranca}/destrancar";

					HttpResponseMessage response = client.PostAsync(url, null).GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();

					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}

		public bool TrancarTranca(int idTranca)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = client.PostAsync(urlEquipamento + $"tranca/{idTranca}/trancar", null).GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();

					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}

		public TrancaDto ObterTrancaPorId(int idTranca)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = client.GetAsync(urlEquipamento + $"tranca/{idTranca}").GetAwaiter().GetResult();
					response.EnsureSuccessStatusCode();

					return JsonSerializer.Deserialize<TrancaDto>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(), options)!;
				}
				catch (Exception ex)
				{
					return null!;
				}
			}

			//return new TrancaDto()
			//{
			//	AnoDeFabricacao = "2022",
			//	Id = 30,
			//	Localizacao = "bla",
			//	Modelo = "UltraLock 9000",
			//	Status = "LIVRE",
			//	Bicicleta = idBicicleta,
			//	Numero = 123
			//};
		}
	}
}
