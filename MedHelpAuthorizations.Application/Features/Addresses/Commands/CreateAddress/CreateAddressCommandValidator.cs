using MedHelpAuthorizations.Application.Interfaces.Repositories;
using FluentValidation;
using MedHelpAuthorizations.Application.BaseFeatures.Addresses;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommandValidator : AddressCommandValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator(IAddressRepositoryAsync addressRepository) : base(addressRepository)
        {

        }
    }
}
