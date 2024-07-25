namespace Bike.Dominio
{
	/// <summary>
	/// Exception customizada para diferenciar quando alguma entidade não foi encontrada (geralmente na busca por ID)
	/// </summary>
	public class EntidadeInexistenteException : Exception
	{
		public EntidadeInexistenteException(string? message) : base(message) { }
	}
}
