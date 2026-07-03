using FluentValidation;
using ShipManagement.DTOs;

namespace ShipManagement.Validators;

public class CreatePortValidator : AbstractValidator<CreatePortDto>
{
    public CreatePortValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Liman adı zorunludur.").MaximumLength(100);
        RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke zorunludur.");
        RuleFor(x => x.City).NotEmpty().WithMessage("Şehir zorunludur.");
    }
}

public class UpdatePortValidator : AbstractValidator<UpdatePortDto>
{
    public UpdatePortValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Liman adı zorunludur.").MaximumLength(100);
        RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke zorunludur.");
        RuleFor(x => x.City).NotEmpty().WithMessage("Şehir zorunludur.");
    }
}