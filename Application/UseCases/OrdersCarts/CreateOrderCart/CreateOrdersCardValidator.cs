

using FluentValidation;

namespace Application.UseCases.OrdersCarts.CreateOrderCart
{
    public sealed class CreateOrdersCardValidator : AbstractValidator<CreateOrdersCardRequest>
    {
        public CreateOrdersCardValidator()
        {
            RuleFor(x => x.CardCode).NotEmpty();
            RuleFor(x => x.CardName).NotEmpty();
        }
    }
}
