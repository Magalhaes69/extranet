
using FluentValidation;

namespace Application.UseCases.ItemExtensions.CreateItemExtension
{
    public class CreateItemExtensionValidator : AbstractValidator<CreateItemExtensionRequest>
    {
        public CreateItemExtensionValidator()
        {
            RuleFor(x => x.ItemCode).MaximumLength(50).NotEmpty();
            RuleFor(x => x.ItemName).MaximumLength(200);
            RuleFor(x => x.ImageUrl).MaximumLength(200);
        }
    }
}
