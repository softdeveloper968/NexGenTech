using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Requests;
using MedHelpAuthorizations.Shared.Wrapper;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Command.AddEdit
{
    public class AddEditClientDocumentCommand : IRequest<Result<int>> //EN-791
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Comments { get; set; }
        public DateTime? DocumentDate { get; set; } = DateTime.UtcNow;
        public UploadRequest? UploadRequest { get; set; }
        public string URL { get; set; }
        public bool IsOnlyView { get; set; }
    }

    internal class AddEditClientDocumentCommandHandler : IRequestHandler<AddEditClientDocumentCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientDocumentCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IClientDocumentRepository _clientDocumentRepository;
        private readonly IClientRepository _clientRepository;

        private string _userId => _currentUserService.UserId;
        private int _clientId => _currentUserService.ClientId;

        public AddEditClientDocumentCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IStringLocalizer<AddEditClientDocumentCommandHandler> localizer,
            ICurrentUserService currentUserService, IClientDocumentRepository clientDocumentRepository, 
            IBlobStorageService blobStorageService, IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _clientDocumentRepository = clientDocumentRepository;
            _blobStorageService = blobStorageService;
            _clientRepository = clientRepository;
        }

        public async Task<Result<int>> Handle(AddEditClientDocumentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.Id == 0)
                {
                    var clientDocument = _mapper.Map<Domain.Entities.ClientDocument>(command);
                    clientDocument.ClientId = _clientId;
                    clientDocument.CreatedBy = _userId;

                    if (command.UploadRequest != null)
                    {
                        clientDocument.FileName = $"{command.UploadRequest.FileName}";
                    }
                    var existingInputDocument = await _clientDocumentRepository.GetByFileNameAndClientIdAsync(command.UploadRequest.FileName);
                    if (existingInputDocument != null && existingInputDocument.ByteLength == command.UploadRequest.Data.Length)
                    {
                        return await Result<int>.FailAsync(_localizer[$"We already have the same file in our system , Please upload a different file!"]);
                    }
                    var client = await _clientRepository.GetById(_clientId); // fetch client to get client code
                    
                    if (command.UploadRequest != null)
                    {
                        clientDocument.URL = await _blobStorageService.UploadToBlobStorageAsync(command.UploadRequest,client.ClientCode).ConfigureAwait(false);
                        clientDocument.ByteLength = command.UploadRequest.Data.Length;
                    }

                    await _unitOfWork.Repository<Domain.Entities.ClientDocument>().AddAsync(clientDocument);
                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(clientDocument.Id, _localizer["ClientDocument Saved"]);
                }
                else
                {
                    // Edit existing document
                    var existingDocument = await _unitOfWork.Repository<Domain.Entities.ClientDocument>().GetByIdAsync(command.Id);
                    if (existingDocument == null)
                    {
                        return await Result<int>.FailAsync(_localizer["ClientDocument Not Found!"]);
                    }

                    // Update title, comments, and created date
                    existingDocument.Title = command.Title;
                    existingDocument.Comments = command.Comments;
                    existingDocument.DocumentDate = command.DocumentDate;
                                       

                    await _unitOfWork.Repository< Domain.Entities.ClientDocument> ().UpdateAsync(existingDocument);
                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(existingDocument.Id, _localizer["ClientDocument Updated"]);
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"Error Saving or Updating ClientDocument: {ex.Message}"]);
            }
        }
    }
}
