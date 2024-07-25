using Bike.Dto.Ciclista;
using BikeApi.Dominio.MeioDePagamento;

namespace Bike.Testes.Unidade.Dominio
{
	public class MeioDePagamentoDominioTeste
	{
		[Fact(DisplayName = "Criação Básica do Meio de Pagamento e possíveis erros de operação")]
		public void TesteCriacao()
		{
			var dto = new MeioDePagamentoDto()
			{
				NomeTitular = "Nome Titular",
				Numero = "1234567890123456",
				Cvv = "123",
				Validade = DateTime.Now.AddYears(1),
			};

			var dominio = new MeioDePagamento(dto);

			Assert.Equal(dto.NomeTitular, dominio.NomeTitular);
			Assert.Equal(dto.Numero, dominio.Numero);
			Assert.Equal(dto.Cvv, dominio.Cvv);
			Assert.Equal(dto.Validade, dominio.Validade);

			Assert.Equal(0, dominio.Id);
			Assert.Equal(0, dominio.IdCiclista);

			dominio.SetarIdInicial(5);
			Assert.Equal(5, dominio.Id);

			var exception1 = Assert.Throws<ArgumentException>(() => dominio.SetarIdInicial(60));
			Assert.Equal("Não é possível alterar o Id do Meio de Pagamento.", exception1.Message);

			dominio.SetarIdCiclista(10);
			Assert.Equal(10, dominio.IdCiclista);

			var exception2 = Assert.Throws<ArgumentException>(() => dominio.SetarIdCiclista(100));
			Assert.Equal("Não é possível alterar o IdCiclista do Meio de Pagamento.", exception2.Message);
		}

		public static IEnumerable<object[]> DadosPraTestesException =>
			new List<object[]>
			{
				new object[] { string.Empty, "1234567890123456", "123", DateTime.Now.AddYears(1), "Nome do Titular do Meio de Pagamento não pode ser vazio" },
				new object[] { "Nome Titular", "", "123", DateTime.Now.AddYears(1), "Numero do Meio de Pagamento não pode ser vazio" },
				new object[] { "Nome Titular", "1234567890123456", "", DateTime.Now.AddYears(1), "CVV do Meio de Pagamento não pode ser vazio" },
				new object[] { "Nome Titular", "1234567890123456", "123", DateTime.Now.AddDays(-1), "O Meio de Pagamento já está vencido e não pode ser utilizado para este cadastro" }
			};

		[Theory(DisplayName = "Obtém Exceptions ao Criar Meio de Pagamento com valores Errados, Nulos ou Vazios")]
		[MemberData(nameof(DadosPraTestesException))]
		public void ObterExceptionAoCriarMeioDePagamentoComValoresNulosOuVazios(string nome, string numero, string cvv, DateTime validade, string erro)
		{
			var dto = new MeioDePagamentoDto() { NomeTitular = nome, Numero = numero, Cvv = cvv, Validade = validade, };

			var exception = Assert.Throws<ArgumentException>(() => new MeioDePagamento(dto));
			Assert.Equal(erro, exception.Message);

		}
	}
}