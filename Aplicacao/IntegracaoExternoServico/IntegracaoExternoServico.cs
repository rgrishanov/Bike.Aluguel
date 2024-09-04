using BikeApi.Dominio.MeioDePagamento;
using System.Text;
using System.Text.Json;

namespace BikeApi.Aplicacao.AluguelServico
{
	public class IntegracaoExternoServico : IIntegracaoExternoServico
	{
		private static string urlServicoExterno = "http://ec2-54-226-108-79.compute-1.amazonaws.com/";

		public bool MeioPagamnentoValido(MeioDePagamento meioDePagamento)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string jsonString = string.Format(@"{{
								""nomeTitular"": ""{0}"",
								""numero"": ""{1}"",
								""validade"": ""{2}"",
								""cvv"": ""{3}""
							}}", meioDePagamento.NomeTitular, meioDePagamento.Numero, meioDePagamento.Validade.ToString("yyyy-MM-dd"), meioDePagamento.Cvv);

					StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

					var result = client.PostAsync(urlServicoExterno + "validaCartaoDeCredito", content).GetAwaiter().GetResult();

					return result.IsSuccessStatusCode;
				}
				catch
				{
					return false;
				}
			}
		}

		public bool EnviarEmail(string email, string assunto, string mensagem)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					StringContent content = new StringContent(JsonSerializer.Serialize(new { email, assunto, mensagem }), Encoding.UTF8, "application/json");

					return client.PostAsync(urlServicoExterno + "enviarEmail", content).GetAwaiter().GetResult().IsSuccessStatusCode;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}

		public bool EfetuarCobranca(int idCiclista, float valor)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					StringContent content = new StringContent(JsonSerializer.Serialize(new { valor, ciclista = idCiclista }), Encoding.UTF8, "application/json");

					return client.PostAsync(urlServicoExterno + "cobranca", content).GetAwaiter().GetResult().IsSuccessStatusCode;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}
	}
}
