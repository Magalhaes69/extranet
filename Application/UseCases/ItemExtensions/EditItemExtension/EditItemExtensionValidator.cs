

using FluentValidation;

namespace Application.UseCases.ItemExtensions.EditItemExtension
{
    public class EditItemExtensionValidator : AbstractValidator<EditItemExtensionRequest>
    {
        public EditItemExtensionValidator()
        {
            RuleFor(x => x.ItemCode).MaximumLength(50).NotEmpty();
            RuleFor(x => x.ItemName).MaximumLength(200);
            RuleFor(x => x.ImageUrl).MaximumLength(200);
        }
    }
}
