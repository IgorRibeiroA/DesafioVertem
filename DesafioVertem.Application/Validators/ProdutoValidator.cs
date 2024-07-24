using DesafioVertem.Application.DTOs;
using FluentValidation;

namespace DesafioVertem.Application.Validators
{
    public class ProdutoValidator: AbstractValidator<ProdutoDTORequest>
    {
        public ProdutoValidator() 
        {
            RuleFor(produto => produto.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .Length(3, 100).WithMessage("O nome do produto deve ter entre 3 e 100 caracteres.");

            RuleFor(produto => produto.Preco)
                .NotEmpty().WithMessage("O preço do produto é obrigatório.")
                .GreaterThan(0).WithMessage("O preço do produto deve ser um valor positivo.");
        }
    }
}
