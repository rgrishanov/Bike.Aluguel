using System.Text;

namespace Bike.Dominio.Aluguel
{
	public class RegistroAluguel
	{
		public required int IdCiclista { get; set; }
		public required int IdBicicleta { get; set; }
		public required int IdTranca { get; set; }
		public required string MeioPagamento { get; set; }
		public DateTime DataHoraRetirada { get; private set; }

        public RegistroAluguel()
        {
			this.DataHoraRetirada = DateTime.Now;
        }

		public string FormatarParaEmail()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Dados do Aluguel:");
			sb.AppendLine($"Ciclista: {this.IdCiclista}");
			sb.AppendLine($"Bicicleta: {this.IdBicicleta}");
			sb.AppendLine($"Tranca: {this.IdTranca}");
			sb.AppendLine($"Meio de Pagamento: {this.MeioPagamento}");
			sb.AppendLine($"Data Hora Retirada: {this.DataHoraRetirada.ToString("HH:mm de dd/MM/yyyy")}");
			return sb.ToString();
		}
    }
}