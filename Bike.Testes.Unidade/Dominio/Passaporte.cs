using Bike.Dto.Ciclista;
using BikeApi.Dominio.Ciclista;

namespace Bike.Testes.Unidade.Dominio
{
	public class PassaporteDominioTeste
	{
		[Fact(DisplayName = "Criação Básica do Passaporte")]
		public void TesteCriacao()
		{
			var dto = new PassaporteDto()
			{
				Numero = "1234567890123456",
				Pais = "Brasil",
				Validade = DateTime.Now.AddYears(1),
			};

			var dominio = new Passaporte(dto);

			Assert.Equal(dominio.Numero, dto.Numero);
			Assert.Equal(dominio.Pais, dto.Pais);
			Assert.Equal(dominio.Validade, dto.Validade);
		}

		public static IEnumerable<object[]> DadosPraTestesException =>
			new List<object[]>
			{
				new object[] { "", "Brasil", DateTime.Now.AddYears(1), "Numero do Passaporte não pode ser vazio" },
				new object[] { "12343214", string.Empty, DateTime.Now.AddYears(1), "Pais do Passaporte não pode ser vazio" },
				new object[] { "12343214", "Brasil", DateTime.MinValue, "Data de Validade do Passaporte deve ser preenchida" },
				new object[] { "12343214", "Brasil", DateTime.Now.AddYears(-1), "O Passaporte já está vencido e não pode ser utilizado para cadastro" },
			};

		[Theory(DisplayName = "Obtém Exceptions ao Criar Passaporte com valores Errados, Nulos ou Vazios")]
		[MemberData(nameof(DadosPraTestesException))]
		public void ObterExceptionAoCriarPassaporteComValoresNulosOuVazios(string numero, string pais, DateTime validade, string erro)
		{
			var dto = new PassaporteDto() { Numero = numero, Validade = validade, Pais = pais };

			var exception = Assert.Throws<ArgumentException>(() => new Passaporte(dto));
			Assert.Equal(erro, exception.Message);
		}
	}
}