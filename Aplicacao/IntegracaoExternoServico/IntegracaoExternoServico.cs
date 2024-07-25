using BikeApi.Dominio.MeioDePagamento;

namespace BikeApi.Aplicacao.AluguelServico
{
	public class IntegracaoExternoServico : IIntegracaoExternoServico
	{
		public bool MeioPagamnentoValido(MeioDePagamento meioDePagamento)
		{
			// temp até integrar

			return true;
		}

		public bool EnviarEmail(string email, string assunto, string mensagem)
		{
			// temp até integrar

			return true;
		}

		public bool EfetuarCobranca(int idCiclista, float valor)
		{
			// temp até integrar

			return true;
		}
	}
}
