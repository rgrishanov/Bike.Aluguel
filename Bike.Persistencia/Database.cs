using Bike.Dominio.Aluguel;
using BikeApi.Dominio.Ciclista;
using BikeApi.Dominio.MeioDePagamento;

namespace BikeApi.Persistencia
{
	public static class Database
	{
		private static List<Ciclista> tabelaCiclista = new();
		private static List<MeioDePagamento> tabelaMeioDePagamento = new();
		private static List<RegistroAluguel> tabelaRegistroAluguel = new();

		public static void ArmazenarCiclista(Ciclista ciclista)
		{
			// pegamos o maior ID que tem na tabela, somamos 1 e atribuímos ao Id do novo Ciclista
			ciclista.SetarIdInicial(tabelaCiclista.Any() ? tabelaCiclista.Max(c => c.Id) + 1 : 1);
			tabelaCiclista.Add(ciclista);
		}

		public static void ArmazenarMeioDePagamento(MeioDePagamento meio)
		{
			// pegamos o maior ID que tem na tabela, somamos 1 e atribuímos ao Id do novo Meio de Pagamento
			meio.SetarIdInicial(tabelaMeioDePagamento.Any() ? tabelaMeioDePagamento.Max(c => c.Id) + 1 : 1);
			tabelaMeioDePagamento.Add(meio);
		}

		public static void ArmazenarRegistroAluguel(RegistroAluguel registro)
		{
			tabelaRegistroAluguel.Add(registro);
		}

		public static void ExcluirMeioDePagamentoDoCiclista(int idCiclista) => tabelaMeioDePagamento.RemoveAll(m => m.IdCiclista == idCiclista);

		public static void ExcluirRegistroAluguel(int idCiclista) => tabelaRegistroAluguel.RemoveAll(r => r.IdCiclista == idCiclista);

		public static bool EmailJaEstaEmUso(string email) => tabelaCiclista.Any(c => c.Email == email);

		public static void ExcluirCiclista(int idCiclista)
		{
			tabelaCiclista.RemoveAll(c => c.Id == idCiclista);
			tabelaMeioDePagamento.RemoveAll(m => m.IdCiclista == idCiclista);
		}

		public static Ciclista ObterCiclistaPorId(int id) => tabelaCiclista.FirstOrDefault(c => c.Id == id)!;

		public static MeioDePagamento ObterMeioDePagamentoPorIdCiclista(int idCiclista) => tabelaMeioDePagamento.FirstOrDefault(c => c.IdCiclista == idCiclista)!;

		public static RegistroAluguel ObterAluguelAtivo(int idCiclista)
		{
			return tabelaRegistroAluguel.FirstOrDefault(r => r.IdCiclista == idCiclista)!;
		}
	}
}
