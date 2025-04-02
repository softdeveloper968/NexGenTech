using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById
{
    public class GetInputDocumentMessageByInputDocumentIdQuery : IRequest<Result<List<ImportDocumentMessageResponseModel>>>
    {
        public int InputDocumentId { get; set; }
        public GetInputDocumentMessageByInputDocumentIdQuery()
        {
        }
    }
    public class GetInputDocumentMessageByInputDocumentIdQueryHandler : IRequestHandler<GetInputDocumentMessageByInputDocumentIdQuery, Result<List<ImportDocumentMessageResponseModel>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetInputDocumentMessageByInputDocumentIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<ImportDocumentMessageResponseModel>>> Handle(GetInputDocumentMessageByInputDocumentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Define the mapping expression for ImportDocumentMessage to ImportDocumentMessageResponseModel
                Expression<Func<Domain.Entities.ImportDocumentMessage, ImportDocumentMessageResponseModel>> expression = e => _mapper.Map<ImportDocumentMessageResponseModel>(e);

                // Create the specification to filter by InputDocumentId
                var inputDocumentMessageSpec = new InputtDocumentMessageByInputDocumentIdSpecification(request.InputDocumentId, string.Empty);

                // Fetch ImportDocumentMessages and map to ImportDocumentMessageResponseModels
                var importDocumentMessages = await _unitOfWork.Repository<Domain.Entities.ImportDocumentMessage>()
                    .Entities
                    .Specify(inputDocumentMessageSpec)
                    .ToListAsync();

                // Initialize the response model with categorized messages
                var responseModel = new ImportDocumentMessageResponseModel
                {
                    ErroredMessages = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.Errored),
                    UnmatchedLocationMessages = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.UnmatchedLocation),
                    UnmatchedProviderMessages = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.UnmatchedProvider),
                    FileDuplicates = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.FileDuplicates),
                    UnSupplantableDuplicates = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.UnmatchedProvider)
                };

                return await Result<List<ImportDocumentMessageResponseModel>>.SuccessAsync(new List<ImportDocumentMessageResponseModel> { responseModel });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<MessageInfoViewModel> CategorizeMessages(List<Domain.Entities.ImportDocumentMessage> messages, InputDocumentMessageTypeEnum messageType)
        {
            return messages
                .Where(msg => msg.MessageType == messageType)
                .Select(msg => new MessageInfoViewModel
                {
                    MessageType = msg.MessageType,
                    Message = msg.Message,
                    ClaimStatusBatchClaimId = msg.ClaimStatusBatchClaimId,
                })
                .ToList();
        }
    }
}
