using DesafioVertem.Application.DTOs;

namespace DesafioVertem.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDTOResponse>> GetAllProdutosAsync();
        Task<ProdutoDTOResponse> GetProdutoIdAsync(int id);
        Task<ProdutoDTOResponse> CreateProdutoAsync(ProdutoDTORequest produtoDTO);
        Task<ProdutoDTOResponse> UpdateProdutoAsync(ProdutoDTORequest produtoDto);
        Task<bool> DeleteProdutoAsync(int id);
    }
}
