using DesafioVertem.Infrastructure.Data;
using DesafioVertem.Domain.Entities;
using Dapper;
using DesafioVertem.Domain.Interfaces;

namespace DesafioVertem.Infrastructure.Repositorys
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DapperContext _dapperContext;
        public ProdutoRepository(DapperContext dapperContext) 
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
        {
            using var connection = _dapperContext.CreateConnection();
            var query = "select * from Produtos";
            var produtos = await connection.QueryAsync<Produto>(query);
            return produtos;
        }
        public async Task<Produto> GetProdutoByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = "select * from Produtos where Id = @Id";
            var produto = await connection.QueryFirstOrDefaultAsync<Produto>(query, new { Id = id });
            if (produto == null) return null;
            return produto;
        }
        public async Task<Produto> CreateProdutoAsync(Produto produto)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = "insert into Produtos (Nome, Descricao, Preco, DataDeCriacao) values (@Nome, @Descricao, @Preco, @DataDeCriacao)";
            var rowsAffected = await connection.ExecuteAsync(query, produto);
            if (rowsAffected > 0) return produto;

            return null;
        }
        public async Task<Produto> UpdateProdutoAsync(Produto produto)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = "update Produtos set Nome = @Nome, Descricao = @Descricao, Preco = @Preco, DataDeCriacao = @DataDeCriacao where Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(query, produto);
            if (rowsAffected > 0) return produto;
        
            return null;
        }

        public async Task<bool> DeleteProdutoAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = "delete from Produtos where Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
