

using FluentValidation;

namespace Application.UseCases.User.UpdateUser
{
    public sealed class UpdateUserValidator :AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(120);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(60);
            RuleFor(x => x.Nuit)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(x => x.CardCode)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(x => x.Theme)
               .NotEmpty();
            RuleFor(x => x.Language)
              .NotEmpty();
        }
    }
}
