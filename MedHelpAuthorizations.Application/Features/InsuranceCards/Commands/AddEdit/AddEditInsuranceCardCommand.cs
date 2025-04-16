using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Commands.AddEdit
{
    public partial class AddEditInsuranceCardCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int CardholderId { get; set; }
        public int PatientId { get; set; }
        public int ClientInsuranceId { get; set; }
        public decimal CopayAmount { get; set; } = 0m;
        public string GroupNumber { get; set; }
        public RelationShipTypeEnum? CardholderRelationshipToPatient { get; set; }
        public string MemberNumber { get; set; }
        public byte? InsuranceCoverageTypes { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool Verified { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? InsuranceCardOrder { get; set; } = 0;
        public int ClientId { get; set; } = 0;
    }
    
    public class AddEditInsuranceCardCommandHandler : IRequestHandler<AddEditInsuranceCardCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<AddEditInsuranceCardCommandHandler> _localizer;
        private int _clientId => _currentUserService.ClientId;

        public AddEditInsuranceCardCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<AddEditInsuranceCardCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditInsuranceCardCommand command, CancellationToken cancellationToken)
        {
            // if relationshiptoPatient is self.. look up patient by ID
            // Lookup cardholder by cardholderID
            if(command.CardholderId == 0)
            {
                command.ClientId = _clientId;

                if (command.CardholderRelationshipToPatient == RelationShipTypeEnum.Self || command.CardholderRelationshipToPatient == null)
                {
                    command.CardholderRelationshipToPatient = command.CardholderRelationshipToPatient == null ? RelationShipTypeEnum.Self : command.CardholderRelationshipToPatient;

                    // Get The patient so we can grab the personID and make a new cardholder and assign it to the insurance Card
                    var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(command.PatientId);
                    if (patient != null)
                    {
                        var cardholder = new Cardholder()
                        {
                            PersonId = (int)patient.PersonId,
                            SignatureOnFile = DateTime.Now,
                        };
                        await _unitOfWork.Repository<Cardholder>().AddAsync(cardholder);
                        await _unitOfWork.Commit(cancellationToken);
                        command.CardholderId = cardholder.Id;
                    }                       
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Cardholder is null and not SELF!"]);
                }
            }         
            

            if (command.Id == 0)
            {
                var insuranceCard = _mapper.Map<InsuranceCard>(command);
                await _unitOfWork.Repository<InsuranceCard>().AddAsync(insuranceCard);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(insuranceCard.Id, _localizer["Insurance Card Saved"]);
            }
            else
            {
                var insuranceCard = await _unitOfWork.Repository<InsuranceCard>().GetByIdAsync(command.Id);
                if (insuranceCard != null)
                {
                    insuranceCard.ClientId = command.ClientId;
                    insuranceCard.Active = command.Active;
                    insuranceCard.CardholderId = (int)command.CardholderId;
                    insuranceCard.ClientInsuranceId = command.ClientInsuranceId;
                    insuranceCard.CopayAmount = command.CopayAmount;
                    insuranceCard.EffectiveEndDate = command.EffectiveEndDate;
                    insuranceCard.EffectiveStartDate = command.EffectiveStartDate;
                    insuranceCard.GroupNumber = command.GroupNumber;
                    insuranceCard.MemberNumber = command.MemberNumber;
                    insuranceCard.InsuranceCardOrder = command.InsuranceCardOrder;
                    insuranceCard.InsuranceCoverageTypes = null; // TODO: Add insurance type support. Look into using bitwise operations here.                    

                    await _unitOfWork.Repository<InsuranceCard>().UpdateAsync(insuranceCard);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(insuranceCard.Id, _localizer["Insurance Card Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Insurance Card Not Found!"]);
                }
            }
        }
    }
}
