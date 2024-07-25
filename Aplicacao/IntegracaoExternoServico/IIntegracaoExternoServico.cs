using BikeApi.Dominio.MeioDePagamento;

namespace BikeApi.Aplicacao.AluguelServico
{
	public interface IIntegracaoExternoServico
	{
		public bool MeioPagamnentoValido(MeioDePagamento meioDePagamento);
		public bool EnviarEmail(string email, string assunto, string mensagem);
		public bool EfetuarCobranca(int idCiclista, float valor);
	}
}
