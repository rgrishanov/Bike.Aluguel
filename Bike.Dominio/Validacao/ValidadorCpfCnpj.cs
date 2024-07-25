using System.Text.RegularExpressions;

namespace Bike.Dominio.Validacao
{
	public static partial class Validar
	{
		public static bool Cpf(string cpf)
		{
			if (cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" ||
				cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" ||
				cpf == "88888888888" || cpf == "99999999999" || string.IsNullOrEmpty(cpf))
				return false;

			int[] d = new int[14];
			int[] v = new int[2];
			int j, i, soma;
			string soNumero;

			soNumero = Regex.Replace(cpf, "[^0-9]", string.Empty);

			if (soNumero.Length == 11)
			{
				for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(soNumero.Substring(i, 1));
				for (i = 0; i <= 1; i++)
				{
					soma = 0;
					for (j = 0; j <= 8 + i; j++) soma += d[j] * (10 + i - j);

					v[i] = (soma * 10) % 11;
					if (v[i] == 10) v[i] = 0;
				}
				return v[0] == d[9] && v[1] == d[10];
			}
			else
				return false;
		}
	}
}