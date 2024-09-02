using Bike.Dto.Ciclista;
using Bike.Dto.Equipamento;
using Bike.Dto.Funcionario;

namespace BikeApi.Aplicacao.AluguelServico
{
	public interface IAluguelServico
	{
		public ObterCiclistaDto CadastrarCiclista(CadastroCiclistaInicialDto dto);

		public ObterCiclistaDto AtivarCiclista(int idCiclista);

		public ObterCiclistaDto ObterCiclista(int idCiclista);

		public ObterCiclistaDto AlterarCiclista(int idCiclista, CiclistaDto dto);

		public bool CiclistaPodeAlugar(int idCiclista);

		public BicicletaDto ObterBicicletaAlugada(int idCiclista);

		public bool EmailJaEstaEmUso(string email);

		public MeioDePagamentoDto ObterMeioDePagamentoCiclista(int idCiclista);

		public void AlterarMeioDePagamentoCiclista(int idCiclista, MeioDePagamentoDto dto);

		public void Alugar(int idCiclista, int idTranca);

		public FuncionarioDto CadastrarFuncionario(FuncionarioBaseDto dto);


		public FuncionarioDto ObterFuncionario(int idFuncionario);
		public void ExcluirFuncionario(int idFuncionario);
		public FuncionarioDto AlterarFuncionario(int idFuncionario, FuncionarioBaseDto dto);
		public IEnumerable<FuncionarioDto> ObterFuncionarios();
	}
}
