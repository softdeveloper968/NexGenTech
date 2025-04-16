using MedHelpAuthorizations.Application.BaseFeatures.Addresses;
using MedHelpAuthorizations.Application.Interfaces.Repositories;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.UpsertAddresses
{
    public class UpsertAddressCommandValidator : AddressCommandValidator<UpsertAddressCommand>
    {
        public UpsertAddressCommandValidator(IAddressRepositoryAsync addressRepository) :base(addressRepository)
        {

        }
    }
}
