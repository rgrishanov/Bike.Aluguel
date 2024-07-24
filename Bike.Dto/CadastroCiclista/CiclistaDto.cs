namespace Bike.Dto.CadastroCiclista
{
    public class CiclistaDto
    {
        public string? Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public string? Cpf { get; set; }
        public PassaporteDto? Passaporte { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Email { get; set; }
        public string? UrlFotoDocumento { get; set; }
        public string? Senha { get; set; }
        public string? SenhaConfirmacao { get; set; }
    }
}
