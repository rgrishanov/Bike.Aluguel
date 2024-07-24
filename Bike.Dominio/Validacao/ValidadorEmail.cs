using System.Text.RegularExpressions;

namespace Bike.Dominio.Validacao
{
	public static partial class Validar
	{
		public static bool Email(string email)
		{
			string regexEmail = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
			return (!string.IsNullOrEmpty(email)) && Regex.IsMatch(email, regexEmail);
		}
	}
}
