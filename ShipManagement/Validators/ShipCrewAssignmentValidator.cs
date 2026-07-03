using FluentValidation;
using ShipManagement.DTOs;

namespace ShipManagement.Validators;

public class CreateShipCrewAssignmentValidator : AbstractValidator<CreateShipCrewAssignmentDto>
{
    public CreateShipCrewAssignmentValidator()
    {
        RuleFor(x => x.ShipId).GreaterThan(0).WithMessage("Geçerli bir gemi seçiniz.");
        RuleFor(x => x.CrewId).GreaterThan(0).WithMessage("Geçerli bir mürettebat seçiniz.");
        RuleFor(x => x.AssignmentDate).NotEmpty().WithMessage("Atama tarihi zorunludur.");
    }
}

public class UpdateShipCrewAssignmentValidator : AbstractValidator<UpdateShipCrewAssignmentDto>
{
    public UpdateShipCrewAssignmentValidator()
    {
        RuleFor(x => x.ShipId).GreaterThan(0).WithMessage("Geçerli bir gemi seçiniz.");
        RuleFor(x => x.CrewId).GreaterThan(0).WithMessage("Geçerli bir mürettebat seçiniz.");
        RuleFor(x => x.AssignmentDate).NotEmpty().WithMessage("Atama tarihi zorunludur.");
    }
}