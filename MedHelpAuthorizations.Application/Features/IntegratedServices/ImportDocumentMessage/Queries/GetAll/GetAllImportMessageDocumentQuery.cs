using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetAll
{
    public class GetAllImportMessageDocumentQuery : IRequest<PaginatedResult<ImportDocumentMessageResponse>>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int InputDocumentId { get; set; }
		public int MessageType { get; set; }

		public GetAllImportMessageDocumentQuery(int pageNumber, int pageSize, int inputDocumentId, int messageType)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			InputDocumentId = inputDocumentId;
			MessageType = messageType;
		}
	}
	public class GetAllImportMessageDocumentQueryHandler : IRequestHandler<GetAllImportMessageDocumentQuery, PaginatedResult<ImportDocumentMessageResponse>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllImportMessageDocumentQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<ImportDocumentMessageResponse>> Handle(GetAllImportMessageDocumentQuery request, CancellationToken cancellationToken)
		{
			var data = await _unitOfWork.Repository<Domain.Entities.ImportDocumentMessage>()
			.Entities
				.Specify(new GetInputDocumentMessageByMessageTypeSpecification(request.InputDocumentId, request.MessageType))
				.Select(x => _mapper.Map<ImportDocumentMessageResponse>(x))
				.ToPaginatedListAsync(request.PageNumber, request.PageSize);

			return data;
		}
	}
}
