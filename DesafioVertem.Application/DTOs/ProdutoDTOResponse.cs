namespace DesafioVertem.Application.DTOs
{
    public class ProdutoDTOResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public int StatusResponse { get; set; }
        public string StatusDescricao { get; set; }
    }
}
