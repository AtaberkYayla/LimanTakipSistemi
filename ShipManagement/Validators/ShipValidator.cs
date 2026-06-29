using FluentValidation;
using ShipManagement.DTOs;

namespace ShipManagement.Validators;

public class CreateShipValidator : AbstractValidator<CreateShipDto>
{
    public CreateShipValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Gemi adı zorunludur.").MaximumLength(100);
        RuleFor(x => x.IMO).NotEmpty().WithMessage("IMO numarası zorunludur.").Length(7, 10).WithMessage("IMO 7-10 karakter olmalıdır.");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Gemi tipi zorunludur.");
        RuleFor(x => x.Flag).NotEmpty().WithMessage("Bayrak zorunludur.");
        RuleFor(x => x.YearBuilt).InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"İnşa yılı 1900 ile {DateTime.Now.Year} arasında olmalıdır.");
    }
}

public class UpdateShipValidator : AbstractValidator<UpdateShipDto>
{
    public UpdateShipValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Gemi adı zorunludur.").MaximumLength(100);
        RuleFor(x => x.IMO).NotEmpty().WithMessage("IMO numarası zorunludur.").Length(7, 10).WithMessage("IMO 7-10 karakter olmalıdır.");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Gemi tipi zorunludur.");
        RuleFor(x => x.Flag).NotEmpty().WithMessage("Bayrak zorunludur.");
        RuleFor(x => x.YearBuilt).InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"İnşa yılı 1900 ile {DateTime.Now.Year} arasında olmalıdır.");
    }
}