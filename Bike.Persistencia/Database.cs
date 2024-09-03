using Bike.Dominio.Aluguel;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.Funcionario;
using BikeApi.Dominio.MeioDePagamento;

namespace BikeApi.Persistencia
{
	public static class Database
	{
		private static List<Ciclista> tabelaCiclista = new();
		private static List<MeioDePagamento> tabelaMeioDePagamento = new();
		private static List<RegistroAluguel> tabelaRegistroAluguel = new();
		private static List<RegistroDevolucao> tabelaRegistroDevolucao = new();
		private static List<Funcionario> tabelaFuncionario = new();

		public static void Purge()
		{
			tabelaCiclista.Clear();
			tabelaFuncionario.Clear();
			tabelaMeioDePagamento.Clear();
			tabelaRegistroAluguel.Clear();
			tabelaRegistroDevolucao.Clear();
		}

		public static void ArmazenarCiclista(Ciclista ciclista)
		{
			// pegamos o maior ID que tem na tabela, somamos 1 e atribuímos ao Id do novo Ciclista
			ciclista.SetarIdInicial(tabelaCiclista.Any() ? tabelaCiclista.Max(c => c.Id) + 1 : 1);
			tabelaCiclista.Add(ciclista);
		}

		public static void ArmazenarFuncionario(Funcionario funcionario)
		{
			// pegamos o maior ID que tem na tabela, somamos 1 e atribuímos ao Id do novo Funcionario
			funcionario.SetarIdInicial(tabelaFuncionario.Any() ? tabelaFuncionario.Max(c => c.Id) + 1 : 1);
			tabelaFuncionario.Add(funcionario);
		}

		public static void ArmazenarMeioDePagamento(MeioDePagamento meio)
		{
			// pegamos o maior ID que tem na tabela, somamos 1 e atribuímos ao Id do novo Meio de Pagamento
			meio.SetarIdInicial(tabelaMeioDePagamento.Any() ? tabelaMeioDePagamento.Max(c => c.Id) + 1 : 1);
			tabelaMeioDePagamento.Add(meio);
		}

		public static void ArmazenarRegistroDevolucao(RegistroDevolucao registro)
		{
			tabelaRegistroDevolucao.Add(registro);
		}

		public static void ArmazenarRegistroAluguel(RegistroAluguel registro)
		{
			tabelaRegistroAluguel.Add(registro);
		}

		public static void ExcluirMeioDePagamentoDoCiclista(int idCiclista) => tabelaMeioDePagamento.RemoveAll(m => m.IdCiclista == idCiclista);

		public static void ExcluirRegistroAluguel(int idCiclista) => tabelaRegistroAluguel.RemoveAll(r => r.IdCiclista == idCiclista);

		public static bool EmailJaEstaEmUso(string email) => tabelaCiclista.Exists(c => c.Email == email);

		public static void ExcluirCiclista(int idCiclista)
		{
			tabelaCiclista.RemoveAll(c => c.Id == idCiclista);
			tabelaMeioDePagamento.RemoveAll(m => m.IdCiclista == idCiclista);
		}

		public static Ciclista ObterCiclistaPorId(int id) => tabelaCiclista.Find(c => c.Id == id)!;

		public static Funcionario ObterFuncionarioPorId(int id) => tabelaFuncionario.Find(c => c.Id == id)!;

		public static void ExcluirFuncionario(int idFuncionario)
		{
			tabelaFuncionario.RemoveAll(c => c.Id == idFuncionario);
		}

		public static IEnumerable<Funcionario> ObterFuncionarios() => tabelaFuncionario;

		public static MeioDePagamento ObterMeioDePagamentoPorIdCiclista(int idCiclista) => tabelaMeioDePagamento.Find(c => c.IdCiclista == idCiclista)!;

		public static RegistroAluguel ObterAluguelAtivo(int idCiclista)
		{
			return tabelaRegistroAluguel.Find(r => r.IdCiclista == idCiclista && r.RegistroDevolucao == null)!;
		}

		public static RegistroAluguel ObterAluguelAtivoPorBicicleta(int idBicicleta)
		{
			return tabelaRegistroAluguel.Find(r => r.IdBicicleta == idBicicleta && r.RegistroDevolucao == null)!;
		}
	}
}
