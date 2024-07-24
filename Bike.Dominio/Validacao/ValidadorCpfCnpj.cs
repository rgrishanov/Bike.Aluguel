using System.Text.RegularExpressions;

namespace Bike.Dominio.Validacao
{
	public static partial class Validar
	{
		public static bool CpfCnpj(string cpfcnpj)
		{
			if (cpfcnpj == "00000000000" || cpfcnpj == "11111111111" || cpfcnpj == "22222222222" || cpfcnpj == "33333333333" ||
				cpfcnpj == "44444444444" || cpfcnpj == "55555555555" || cpfcnpj == "66666666666" || cpfcnpj == "77777777777" ||
				cpfcnpj == "88888888888" || cpfcnpj == "99999999999" || string.IsNullOrEmpty(cpfcnpj))
				return false;

			int[] d = new int[14];
			int[] v = new int[2];
			int j, i, soma;
			string Sequencia, SoNumero;

			SoNumero = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

			// se a quantidade de dígitos numérios for igual a 11
			// iremos verificar como CPF
			if (SoNumero.Length == 11)
			{
				for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
				for (i = 0; i <= 1; i++)
				{
					soma = 0;
					for (j = 0; j <= 8 + i; j++) soma += d[j] * (10 + i - j);

					v[i] = (soma * 10) % 11;
					if (v[i] == 10) v[i] = 0;
				}
				return v[0] == d[9] && v[1] == d[10];
			}
			// se a quantidade de dígitos numérios for igual a 14
			// iremos verificar como CNPJ
			else if (SoNumero.Length == 14)
			{
				Sequencia = "6543298765432";
				for (i = 0; i <= 13; i++)
					d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));

				for (i = 0; i <= 1; i++)
				{
					soma = 0;
					for (j = 0; j <= 11 + i; j++)
						soma += d[j] * Convert.ToInt32(Sequencia.Substring(j + 1 - i, 1));

					v[i] = (soma * 10) % 11;
					if (v[i] == 10) v[i] = 0;
				}
				return (v[0] == d[12] && v[1] == d[13]);
			}
			// CPF ou CNPJ inválido se  a quantidade de dígitos numérios for diferente de 11 e 14
			else
				return false;
		}
	}
}