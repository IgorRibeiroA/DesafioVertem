using DesafioVertem.Application.DTOs;
using DesafioVertem.Application.Interfaces;
using DesafioVertem.Domain.Entities;
using DesafioVertem.Domain.Interfaces;
using FluentValidation;

namespace DesafioVertem.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IValidator<ProdutoDTORequest> _produtoValidator;

        public ProdutoService(IProdutoRepository produtoRepository, IValidator<ProdutoDTORequest> produtoValidator)
        {
            _produtoRepository = produtoRepository;
            _produtoValidator = produtoValidator;
        }

        public async Task<IEnumerable<ProdutoDTOResponse>> GetAllProdutosAsync()
        {
            var produtos = await _produtoRepository.GetAllProdutosAsync();
            return produtos.Select(produtos => new ProdutoDTOResponse
            {
                Id = produtos.Id,
                Nome = produtos.Nome,
                Descricao = produtos.Descricao,
                Preco = produtos.Preco,
                DataDeCriacao = produtos.DataDeCriacao,
                StatusResponse = 200,
                StatusDescricao = "Lista de produtos"
            }).ToList();
        }

        public async Task<ProdutoDTOResponse> GetProdutoIdAsync(int id)
        {
            var produto = await _produtoRepository.GetProdutoByIdAsync(id);
            if (produto == null) return null;
            return new ProdutoDTOResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                DataDeCriacao = produto.DataDeCriacao,
                StatusResponse = 200,
                StatusDescricao = $"Produto {produto.Id} encontrado com sucesso!"
            };
        }
        public async Task<ProdutoDTOResponse> CreateProdutoAsync(ProdutoDTORequest produtoDto)
        {
            var validationResult = await _produtoValidator.ValidateAsync(produtoDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await ValidateNomeDuplicadoAsync(produtoDto);

            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                DataDeCriacao = DateTime.Now
            };

            var createdProduto = await _produtoRepository.CreateProdutoAsync(produto);

            return new ProdutoDTOResponse
            {
                Id = createdProduto.Id,
                Nome = createdProduto.Nome,
                Descricao = createdProduto.Descricao,
                Preco = createdProduto.Preco,
                DataDeCriacao = createdProduto.DataDeCriacao,
                StatusResponse = 200,
                StatusDescricao = "Produto criado com sucesso!"
            };
        }
        public async Task<ProdutoDTOResponse> UpdateProdutoAsync(ProdutoDTORequest produtoDto)
        {
            var validationResult = await _produtoValidator.ValidateAsync(produtoDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await ValidateNomeDuplicadoAsync(produtoDto);

            var produto = new Produto
            {
                Id = produtoDto.Id,
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                DataDeCriacao = produtoDto.DataDeCriacao
            };

            var updateProduto = await _produtoRepository.UpdateProdutoAsync(produto);
           
            return new ProdutoDTOResponse
            {
                Id = updateProduto.Id,
                Nome = updateProduto.Nome,
                Descricao = updateProduto.Descricao,
                Preco = updateProduto.Preco,
                DataDeCriacao = updateProduto.DataDeCriacao,
                StatusResponse = updateProduto != null ? 200 : 400,
                StatusDescricao = updateProduto != null ? "Produto alterado com sucesso!" : "Não foi possivel alterar o produto"
            };
        }
        public async Task<bool> DeleteProdutoAsync(int id)
        {
            return await _produtoRepository.DeleteProdutoAsync(id);
        }

        private async Task ValidateNomeDuplicadoAsync(ProdutoDTORequest produtoDto)
        {
            var produtosExistentes = await _produtoRepository.GetAllProdutosAsync();

            if (produtosExistentes.Any(p => p.Nome.Equals(produtoDto.Nome, StringComparison.OrdinalIgnoreCase) && p.Id != produtoDto.Id))
            {
                throw new ValidationException("Já existe um produto com o mesmo nome.");
            }
        }
    }
}
