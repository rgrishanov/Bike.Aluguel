using BikeApi.Dominio.MeioDePagamento;
using System.Text.Json;
using System.Text;
using System;

namespace BikeApi.Aplicacao.AluguelServico
{
	public class IntegracaoExternoServico : IIntegracaoExternoServico
	{
		private static string urlServicoExterno = "https://ec2-54-226-108-79.compute-1.amazonaws.com/";

		public bool MeioPagamnentoValido(MeioDePagamento meioDePagamento)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					StringContent content = new StringContent(JsonSerializer.Serialize(meioDePagamento), Encoding.UTF8, "application/json");

					return client.PostAsync(urlServicoExterno + "validaCartaoDeCredito", content).GetAwaiter().GetResult().IsSuccessStatusCode;
				}
				catch (Exception ex)
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
