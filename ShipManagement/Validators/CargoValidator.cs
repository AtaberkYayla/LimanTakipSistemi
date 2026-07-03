using FluentValidation;
using ShipManagement.DTOs;

namespace ShipManagement.Validators;

public class CreateCargoValidator : AbstractValidator<CreateCargoDto>
{
    public CreateCargoValidator()
    {
        RuleFor(x => x.ShipId).GreaterThan(0).WithMessage("Geçerli bir gemi seçiniz.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Yük açıklaması zorunludur.");
        RuleFor(x => x.WeightTon).GreaterThan(0).WithMessage("Ağırlık 0'dan büyük olmalıdır.");
        RuleFor(x => x.CargoType).NotEmpty().WithMessage("Yük tipi zorunludur.");
    }
}

public class UpdateCargoValidator : AbstractValidator<UpdateCargoDto>
{
    public UpdateCargoValidator()
    {
        RuleFor(x => x.ShipId).GreaterThan(0).WithMessage("Geçerli bir gemi seçiniz.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Yük açıklaması zorunludur.");
        RuleFor(x => x.WeightTon).GreaterThan(0).WithMessage("Ağırlık 0'dan büyük olmalıdır.");
        RuleFor(x => x.CargoType).NotEmpty().WithMessage("Yük tipi zorunludur.");
    }
}