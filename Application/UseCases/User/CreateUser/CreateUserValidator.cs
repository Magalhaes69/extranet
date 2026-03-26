using FluentValidation;

namespace Application.UseCases.User.CreateUser
{
    public sealed class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
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
        }
    }
}
