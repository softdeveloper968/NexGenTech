using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.AddEdit
{
    public class AddEditDocumentTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }

    public class AddEditDocumentTypeHandler : IRequestHandler<AddEditDocumentTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly int _clientId;

        public AddEditDocumentTypeHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _clientId = currentUserService.ClientId;
        }

        public async Task<Result<int>> Handle(AddEditDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<DocumentType>(request);            
            if (request.Id == 0)
            {
                await _unitOfWork.Repository<DocumentType>().AddAsync(mapped);
                (await _unitOfWork.Repository<Domain.Entities.Client>().Entities
                    .Include(x => x.DocumentTypes)
                    .FirstOrDefaultAsync(x =>x.Id == _clientId))?.DocumentTypes.Add(mapped);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(mapped.Id, "DocumentType Added");
            }
            else
            {
                await _unitOfWork.Repository<DocumentType>().UpdateAsync(mapped);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(mapped.Id, "DocumentType Updated");
            }            
        }
    }
}
