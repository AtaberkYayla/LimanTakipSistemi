using FluentValidation;
using ShipManagement.DTOs;

namespace ShipManagement.Validators;

public class CreateCrewMemberValidator : AbstractValidator<CreateCrewMemberDto>
{
    public CreateCrewMemberValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad zorunludur.").MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad zorunludur.").MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta zorunludur.").EmailAddress().WithMessage("Geçersiz e-posta formatı.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon zorunludur.")
            .Matches(@"^\+90\s5\d{2}\s\d{3}\s\d{2}\s\d{2}$").WithMessage("Telefon formatı: +90 5XX XXX XX XX");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Görev zorunludur.");
    }
}

public class UpdateCrewMemberValidator : AbstractValidator<UpdateCrewMemberDto>
{
    public UpdateCrewMemberValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad zorunludur.").MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad zorunludur.").MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta zorunludur.").EmailAddress().WithMessage("Geçersiz e-posta formatı.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon zorunludur.")
            .Matches(@"^\+90\s5\d{2}\s\d{3}\s\d{2}\s\d{2}$").WithMessage("Telefon formatı: +90 5XX XXX XX XX");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Görev zorunludur.");
    }
}