using FluentValidation;
using ShipManagement.DTOs;

namespace ShipManagement.Validators;

public class CreateShipVisitValidator : AbstractValidator<CreateShipVisitDto>
{
    public CreateShipVisitValidator()
    {
        RuleFor(x => x.ShipId).GreaterThan(0).WithMessage("Geçerli bir gemi seçiniz.");
        RuleFor(x => x.PortId).GreaterThan(0).WithMessage("Geçerli bir liman seçiniz.");
        RuleFor(x => x.Purpose).NotEmpty().WithMessage("Ziyaret amacı zorunludur.");
        RuleFor(x => x.ArrivalDate).NotEmpty().WithMessage("Geliş tarihi zorunludur.");
        RuleFor(x => x.DepartureDate)
            .GreaterThanOrEqualTo(x => x.ArrivalDate)
            .When(x => x.DepartureDate.HasValue)
            .WithMessage("Ayrılış tarihi geliş tarihinden önce olamaz.");
    }
}

public class UpdateShipVisitValidator : AbstractValidator<UpdateShipVisitDto>
{
    public UpdateShipVisitValidator()
    {
        RuleFor(x => x.ShipId).GreaterThan(0).WithMessage("Geçerli bir gemi seçiniz.");
        RuleFor(x => x.PortId).GreaterThan(0).WithMessage("Geçerli bir liman seçiniz.");
        RuleFor(x => x.Purpose).NotEmpty().WithMessage("Ziyaret amacı zorunludur.");
        RuleFor(x => x.ArrivalDate).NotEmpty().WithMessage("Geliş tarihi zorunludur.");
        RuleFor(x => x.DepartureDate)
            .GreaterThanOrEqualTo(x => x.ArrivalDate)
            .When(x => x.DepartureDate.HasValue)
            .WithMessage("Ayrılış tarihi geliş tarihinden önce olamaz.");
    }
}