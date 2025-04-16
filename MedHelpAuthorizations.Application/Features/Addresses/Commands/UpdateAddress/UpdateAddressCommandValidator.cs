using MedHelpAuthorizations.Application.BaseFeatures.Addresses;
using MedHelpAuthorizations.Application.Interfaces.Repositories;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator : AddressCommandValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator(IAddressRepositoryAsync addressRepository) : base(addressRepository)
        {

        }
    }
}
