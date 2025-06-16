using FakeStore.Communication.Requests;
using FakeStore.Exception;
using FluentValidation;

namespace FakeStore.Application.UseCases.Products.Update
{
    public class UpdateProductValidator : AbstractValidator<RequestUpdateProductJson>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(ResourceErrorMessages.TITULO_NECESSARIO)
                .MinimumLength(3).WithMessage(ResourceErrorMessages.TITULO_PEQUENO)
                .MaximumLength(100).WithMessage(ResourceErrorMessages.TITULO_GRANDE)
                .When(x => x.Title != null);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(ResourceErrorMessages.PRICE_INVALIDO)
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ResourceErrorMessages.DESCRICAO_NECESSARIA)
                .MaximumLength(500).WithMessage(ResourceErrorMessages.DESCRICAO_GRANDE)
                .MinimumLength(5).WithMessage(ResourceErrorMessages.DESCRICAO_PEQUENA)
                .When(x => x.Description != null);

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage(ResourceErrorMessages.CATEGORIA_NECESSAIRA)
                .MaximumLength(50).WithMessage(ResourceErrorMessages.CATEGORIA_PEQUENA)
                .When(x => x.Category != null);
        }
    }
}
