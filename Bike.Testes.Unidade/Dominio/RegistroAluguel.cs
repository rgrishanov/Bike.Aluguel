using Bike.Dominio.Aluguel;
using FluentAssertions;

namespace Bike.Testes.Unidade.Dominio
{
	public class RegistroAluguelDominioTeste
	{
		[Fact(DisplayName = "Criação Básica do RegistroAluguel")]
		public void TesteCriacao()
		{
			var dominio = new RegistroAluguel()
			{
				IdBicicleta = 1,
				IdCiclista = 2,
				IdTranca = 3,
				MeioPagamento = "9276927364976234"
			};

			Assert.Equal(1, dominio.IdBicicleta);
			Assert.Equal(2, dominio.IdCiclista);
			Assert.Equal(3, dominio.IdTranca);
			Assert.Equal("9276927364976234", dominio.MeioPagamento);
			dominio.DataHoraRetirada.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 2));
		}
	}
}