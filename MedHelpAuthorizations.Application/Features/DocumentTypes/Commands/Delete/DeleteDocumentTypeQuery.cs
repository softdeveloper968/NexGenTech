using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.Delete
{
    public class DeleteDocumentTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteDocumentTypeHandler : IRequestHandler<DeleteDocumentTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly int _client;

        public DeleteDocumentTypeHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _client = currentUserService.ClientId;
        }

        public async Task<Result<int>> Handle(DeleteDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var type = await _unitOfWork.Repository<DocumentType>().GetByIdAsync(request.Id);           
            await _unitOfWork.Repository<DocumentType>().DeleteAsync(type);

            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(request.Id, "Deleted Successfully");
        }
    }
}
