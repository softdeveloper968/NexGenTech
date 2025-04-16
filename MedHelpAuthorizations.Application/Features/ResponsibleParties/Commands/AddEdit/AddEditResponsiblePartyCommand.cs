using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.Commands.AddEdit
{
    public class AddEditResponsiblePartyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public AddressTypeEnum AddressTypeId { get; set; }
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string City { get; set; }
        public StateEnum StateId { get; set; } = StateEnum.UNK;
        public string PostalCode { get; set; }
        public bool Normalized { get; set; }
        public int DeliveryPointBarcode { get; set; }
        public GenderIdentityEnum? GenderIdentityId { get; set; }
        public AdministrativeGenderEnum? AdministrativeGenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string SocialSecurityNumber { get; set; }

        public int ResponsiblePartyId { get; set; }

        public string AccountNumber { get; set; }
    }

    public class AddEditResponsiblePartyCommandHandler : IRequestHandler<AddEditResponsiblePartyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditResponsiblePartyCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditResponsiblePartyCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var cmd = _mapper.Map<ResponsibleParty>(command);
                await _unitOfWork.Repository<ResponsibleParty>().AddAsync(cmd);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(cmd.Id, "ResponsibleParty Saved");
            }
            else
            {
                var cmd = await _unitOfWork.Repository<ResponsibleParty>().GetByIdAsync(command.Id);
                if (cmd != null)
                {
                    _mapper.Map(command, cmd);
                    await _unitOfWork.Repository<ResponsibleParty>().UpdateAsync(cmd);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(cmd.Id, "ResponsibleParty Updated");
                }
                else
                {
                    return await Result<int>.FailAsync("ResponsibleParty Not Found!");
                }
            }
        }
    }
}
