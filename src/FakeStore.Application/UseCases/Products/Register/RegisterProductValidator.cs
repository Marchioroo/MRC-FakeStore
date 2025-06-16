using FakeStore.Communication.Requests;
using FluentValidation;
using FakeStore.Exception;
    
namespace FakeStore.Application.UseCases.Products.Register;
public class RegisterProductValidator : AbstractValidator<RequestRegisterProductJson>
{

    public RegisterProductValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ResourceErrorMessages.TITULO_NECESSARIO)
            .MinimumLength(3).WithMessage(ResourceErrorMessages.TITULO_PEQUENO)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.TITULO_GRANDE);
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(ResourceErrorMessages.PRICE_INVALIDO);
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ResourceErrorMessages.DESCRICAO_NECESSARIA)
            .MinimumLength(5).WithMessage(ResourceErrorMessages.DESCRICAO_PEQUENA)
            .MaximumLength(500).WithMessage(ResourceErrorMessages.DESCRICAO_GRANDE);
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage(ResourceErrorMessages.CATEGORIA_NECESSAIRA)
            .MaximumLength(50).WithMessage(ResourceErrorMessages.CATEGORIA_PEQUENA).When(x => x.Category != null);
    }
}
