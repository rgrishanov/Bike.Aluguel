using Bike.Dto.Ciclista;

namespace BikeApi.Aplicacao.AluguelServico
{
    public interface IAluguelServico
	{
		public ObterCiclistaDto CadastrarCiclista(CadastroInicialDto dto);

		public ObterCiclistaDto AtivarCiclista(int idCiclista);

		public ObterCiclistaDto ObterCiclista(int idCiclista);

		public ObterCiclistaDto AlterarCiclista(int idCiclista, CiclistaDto dto);
		
		public void AlterarMeioDePagamento(int idCiclista, MeioDePagamentoDto dto);

		public void Alugar(int idCiclista, int idTranca);
	}
}
