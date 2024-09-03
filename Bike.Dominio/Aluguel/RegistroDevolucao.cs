using System.Text;

namespace Bike.Dominio.Aluguel
{
	public class RegistroDevolucao
	{
		public DateTime DataHoraDevolucao { get; set; }
		public DateTime DataHoraCobranca { get; set; }
		public float ValorExtraAluguel { get; set; }
		public string CartaoCobranca { get; set; }
		public int NumeroBicicleta { get; set; }
		public int NumeroTranca { get; set; }

        public RegistroAluguel RegistroAluguel { get; set; }

        public RegistroDevolucao(DateTime dataHoraDevolucao, DateTime dataHoraCobranca, float valorExtraAluguel,
			string cartaoCobranca, int numeroBicicleta, int numeroTranca, RegistroAluguel registroAluguel)
		{
			this.DataHoraDevolucao = dataHoraDevolucao;
			this.DataHoraCobranca = dataHoraCobranca;
			this.ValorExtraAluguel = valorExtraAluguel;
			this.CartaoCobranca = cartaoCobranca;
			this.NumeroBicicleta = numeroBicicleta;
			this.NumeroTranca = numeroTranca;
			this.RegistroAluguel = registroAluguel;
		}

		public string FormatarParaEmail()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Detalhes da Cobrança:");
			sb.AppendLine($"Número da Bicicleta: {this.NumeroBicicleta}");
			sb.AppendLine($"Número da Tranca: {this.NumeroTranca}");
			sb.AppendLine($"Data/Hora da Devolução: {this.DataHoraDevolucao:HH:mm de dd/MM/yyyy}");
			sb.AppendLine($"Data/Hora da Cobrança: {this.DataHoraCobranca:HH:mm de dd/MM/yyyy}");
			sb.AppendLine($"Valor Extra do Aluguel: {this.ValorExtraAluguel:C}");
			sb.AppendLine($"Cartão usado para Cobrança: {this.CartaoCobranca}");
			return sb.ToString();
		}
	}
}