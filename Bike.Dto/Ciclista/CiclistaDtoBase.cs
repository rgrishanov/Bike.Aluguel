namespace Bike.Dto.Ciclista
{
    public abstract class CiclistaDtoBase
    {
        public string? Nome { get; set; }
        public string? Nascimento { get; set; }
        public string? Cpf { get; set; }
        public PassaporteDto? Passaporte { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Email { get; set; }
        public string? UrlFotoDocumento { get; set; }
    }
}
