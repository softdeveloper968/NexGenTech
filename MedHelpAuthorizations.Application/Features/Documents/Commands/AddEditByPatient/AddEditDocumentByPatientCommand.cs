using AutoMapper;
using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Features.Documents.Commands.AddEditByPatient
{
    public class AddEditDocumentByPatientCommand : AddEditDocumentCommand
    {
        public int PatientId { get; set; }
        public int AuthorizationId { get; set; }
    }

    public class AddEditDocumentCommandHandler : IRequestHandler<AddEditDocumentByPatientCommand, Result<int>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public AddEditDocumentCommandHandler(IMediator mediator, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditDocumentByPatientCommand command, CancellationToken cancellationToken)
        {
            if (command.PatientId == 0 && command.AuthorizationId == 0)
            {
                return Result<int>.Fail("Either Patient (or) Authorization needed for document");
            }

            command.Title = $"{command.Title}_{DateTime.UtcNow.ToString("yyyyMMddss")}";

            if(command.UploadRequest != null)
            {
                command.UploadRequest.FileName = Path.Combine(command.PatientId.ToString(), command.Title);
            }
            var uploadDoc = _mapper.Map<AddEditDocumentCommand>(command);
            var docResult = await _mediator.Send(uploadDoc);
            if (docResult.Succeeded && docResult.Data > 0)
            {
                if (command.PatientId == 0)
                    command.PatientId = (await _unitOfWork.Repository<Authorization>().GetByIdAsync(command.AuthorizationId)).PatientId;
                var doc = await _unitOfWork.Repository<Document>().Entities
                    .FirstOrDefaultAsync(x => x.Id == docResult.Data);
                var patient = await _unitOfWork.Repository<Patient>().Entities
                    .Include(x => x.Documents)
                    .Include(x => x.Authorizations)
                    .FirstOrDefaultAsync(x => x.Id == command.PatientId);
                patient.Documents.Add(doc);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(patient.Id, "Document updated for the patient");
            }
            else
            {
                return docResult;
            }
        }
    }
}
