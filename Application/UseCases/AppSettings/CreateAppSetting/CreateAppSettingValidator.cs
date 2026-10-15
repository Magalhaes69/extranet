using FluentValidation;

namespace Application.UseCases.AppSettings.CreateAppSetting
{
    public class CreateAppSettingValidator : AbstractValidator<CreateAppSettingRequest>
    {
        
        public CreateAppSettingValidator()
        {
            RuleFor(x => x.Key).MaximumLength(25).NotEmpty();
            RuleFor(x => x.Value).MaximumLength(100);
            RuleFor(x => x.Category).MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(100);
        }

    }
}
