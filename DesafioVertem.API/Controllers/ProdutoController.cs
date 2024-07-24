using DesafioVertem.Application.DTOs;
using DesafioVertem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioVertem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : Controller
    {

        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTOResponse>>> GetProduto()
        {
            try
            {
                var produtos = await _produtoService.GetAllProdutosAsync();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTOResponse>> GetProdutoById(int id)
        {
            try
            {
                var produto = await _produtoService.GetProdutoIdAsync(id);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTOResponse>> CreateProduto([FromBody] ProdutoDTORequest produto)
        {
            try
            {
                var createdProduto = await _produtoService.CreateProdutoAsync(produto);

                if (createdProduto.StatusResponse == 400) return BadRequest(createdProduto);

                return CreatedAtAction(nameof(GetProdutoById), new { id = createdProduto.Id }, createdProduto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] ProdutoDTORequest produto)
        {
            try
            {
                if (id != produto.Id) return BadRequest("O Id não é compativel");

                var updateProduto = await _produtoService.UpdateProdutoAsync(produto);

                if(updateProduto.StatusResponse == 400) return BadRequest(updateProduto);
                
                return Ok(updateProduto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                var delete = await _produtoService.DeleteProdutoAsync(id);

                if (!delete) return BadRequest($"Id não encontrado");

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex) 
            {
                return BadRequest($"Ocorreu um erro: {ex.Message}");
            }
        }
    }
}
