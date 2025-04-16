using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByPatientId;
using MedHelpAuthorizations.Application.Features.Reports.GetCurrentAuthorizations;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Application.Specifications.ReportSpecifications;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class AuthorizationRepository : RepositoryAsync<Authorization, int>, IAuthorizationRepository
    {
        private readonly IRepositoryAsync<Authorization, int> _repository;
        //private readonly AdminDbContext _idContext;
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        //public AuthorizationRepository(ApplicationContext dbContext, AdminDbContext idContext, IMapper mapper) : base(dbContext)
        public AuthorizationRepository(ApplicationContext dbContext, AdminDbContext idContext, IMapper mapper) : base(dbContext)
        {
            //_idContext = idContext;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetAllWithProperName(GetAllAuthorizationsQuery request)
        {
            return await _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.Documents)
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Specify(new AuthorizationFilterSpecification(request))                
                .Select(x => new GetAllPagedAuthorizationsResponse()
                {
                    Id = x.Id,
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthTypeId = x.AuthTypeId,
                    AuthTypeName = x.AuthType.Name,
                    ClientId = x.ClientId,
                   //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    CreateUserId = x.CreatedBy,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                })
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>> GetByCriteriaWithProperName(GetByCriteriaPagedAuthorizationsQuery request)
        {
            var linq = _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.Documents)                
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Specify(new AuthorizationFilterCriteriaSpecification(request))
                .Select(x => new GetByCriteriaPagedAuthorizationsResponse()
                {
                    Id = x.Id,
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthTypeId = x.AuthTypeId,
                    AuthTypeName = x.AuthType.Name,
                    ClientId = x.ClientId,
                    //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    CreateUserId = x.CreatedBy,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                });                           

            return await linq.ToPaginatedListAsync(request.PageNumber, request.PageSize);
            
        }

        public async Task<GetAuthorizationByIdResponse> GetByIdWithProperName(GetAuthorizationByIdQuery request)
        {
            return await _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)                
                .Select(x => new GetAuthorizationByIdResponse()
                {
                    Id = x.Id,
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthorizationClientCptCodes = x.AuthorizationClientCptCodes.ToHashSet(),
                    AuthTypeId = x.AuthTypeId,                    
                    ClientId = x.ClientId,
                    //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,                    
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                })
                .FirstOrDefaultAsync(x => x.Id == request.Id);               
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetByPatientIdWithProperName(GetAuthorizationByPatientIdQuery request)
        {
            return await _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.Documents)
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Specify(new AuthorizationFilterByPatientSpecification(request.PatientId))
                .Select(x => new GetAllPagedAuthorizationsResponse()
                {
                    Id = x.Id,
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthTypeId = x.AuthTypeId,
                    ClientId = x.ClientId,
                    //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                })
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetExpiringWithProperNamePaginated(GetPagedExpiringAuthorizationsQuery request)
        {
            IQueryable<GetAllPagedAuthorizationsResponse> query = await GetExpiringWithProperNameQueryable(request);

            return await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<IQueryable<GetAllPagedAuthorizationsResponse>> GetExpiringWithProperNameAllClientQueryable(int expiringDays = 30)
        {
            IQueryable<GetAllPagedAuthorizationsResponse> query = _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.Documents)
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.AuthType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Specify(new ExpiringAuthorizationFilterSpecification(new GetPagedExpiringAuthorizationsQuery() { ClientId = 0, ExpiringDays = expiringDays }))
                .Select(x => new GetAllPagedAuthorizationsResponse()
                {
                    Id = x.Id,
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthTypeId = x.AuthTypeId,
                    AuthTypeName = x.AuthType.Name,
                    ClientId = x.ClientId,
                    //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    CreateUserId = x.CreatedBy,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                });

            return query;
        }

        public async Task<IQueryable<GetAllPagedAuthorizationsResponse>> GetExpiringWithProperNameQueryable(GetPagedExpiringAuthorizationsQuery request)
        {
            IQueryable<GetAllPagedAuthorizationsResponse> query = _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.Documents)
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.AuthType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Specify(new ExpiringAuthorizationFilterSpecification(request))
                .Select(x => new GetAllPagedAuthorizationsResponse()
                {
                    Id = x.Id,
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthTypeId = x.AuthTypeId,
                    AuthTypeName = x.AuthType.Name,
                    ClientId = x.ClientId,
                    //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    CreateUserId = x.CreatedBy,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                });

            return query;
        }

        public async Task<Authorization> GetExpiringForPatientIdAndAuthType(int patientId, int authTypeId, int clientId, DateTime? createdOn = null)
        {
            return await _dbContext.Authorizations
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.Documents)
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Specify(new ExpiringAuthorizationConcurrencyFilterSpecification(patientId, authTypeId, clientId))               
               .FirstOrDefaultAsync();
        }

        public async Task<IQueryable<GetCurrentAuthorizationsResponse>> GetCurrentWithProperNameAllClientQueryable()
        {
            IQueryable<GetCurrentAuthorizationsResponse> query = _dbContext.Authorizations
                            .Include(x => x.Patient)
                                //.ThenInclude(y => y.ClientInsurance)
                            .Include(x => x.Patient)
                                .ThenInclude(y => y.Person)
                            .Include(x => x.AuthType)
                            .Include(x => x.Documents)
                                .ThenInclude(y => y.DocumentType)
                            .Include(x => x.AuthType)
                            .Include(x => x.SucceededAuthorizations)
                                .ThenInclude(y => y.InitialAuthorization)
                                    .ThenInclude(z => z.Documents)
                                        .ThenInclude(zz => zz.DocumentType)
                            .Include(x => x.InitialAuthorizations)
                                .ThenInclude(y => y.SucceededAuthorization)
                                    .ThenInclude(z => z.Documents)
                                        .ThenInclude(zz => zz.DocumentType)
                            .Include(x => x.Notes)
                            .Specify(new CurrentAuthorizationsFilterSpecification(new GetPagedCurrentAuthorizationsQuery() { ClientId = 0 }))
                            .Select(x => new GetCurrentAuthorizationsResponse()
                            {
                                Id = x.Id,
                                //InsuranceName = x.Patient.ClientInsurance != null ? x.Patient.ClientInsurance.Name : "Unknown",
                                InsurancePolicyNumber = x.Patient.InsurancePolicyNumber,
                                //InsurancePhoneNumber = x.Patient.ClientInsurance != null ? x.Patient.ClientInsurance.PhoneNumber : null,
                                //InsuranceFaxNumber = x.Patient.ClientInsurance != null ? x.Patient.ClientInsurance.FaxNumber : null,
                                Notes = string.Join(Environment.NewLine, x.Notes.Select(n => string.Format("{0}: {1}", n.CreatedOn.ToShortDateString(), n.NoteContent))),
                                AuthNumber = x.AuthNumber,
                                AuthorizationStatusId = x.AuthorizationStatusId,
                                AuthTypeId = x.AuthTypeId,
                                AuthTypeName = x.AuthType.Name,
                                ClientId = x.ClientId,
                                //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                                CompleteDate = x.CompleteDate,
                                CreatedOn = x.CreatedOn,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                                DischargedOn = x.DischargedOn,
                                CreateUserId = x.CreatedBy,
                                PatientName = x.Patient.Person.LastCommaFirstName,
                                AccountNumber = x.Patient.AccountNumber,
                                PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                                PatientId = x.PatientId,
                                Units = x.Units,
                                Documents = x.Documents.ToList(),
                                NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                            });

            return query;
        }

        public async Task<IQueryable<GetCurrentAuthorizationsResponse>> GetCurrentWithProperNameQueryable(GetPagedCurrentAuthorizationsQuery request)
        {
            IQueryable<GetCurrentAuthorizationsResponse> query = _dbContext.Authorizations
                .Include(x => x.Patient)
                    //.ThenInclude(y => y.ClientInsurance)
                .Include(x => x.Patient)
                    .ThenInclude(y => y.Person)
                .Include(x => x.AuthType)
                .Include(x => x.AuthorizationClientCptCodes)
                    .ThenInclude(y => y.ClientCptCode)
                .Include(x => x.Documents)
                    .ThenInclude(y => y.DocumentType)
                .Include(x => x.AuthType)
                .Include(x => x.SucceededAuthorizations)
                    .ThenInclude(y => y.InitialAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.InitialAuthorizations)
                    .ThenInclude(y => y.SucceededAuthorization)
                        .ThenInclude(z => z.Documents)
                            .ThenInclude(zz => zz.DocumentType)
                .Include(x => x.Notes)
                .Specify(new CurrentAuthorizationsFilterSpecification(request))
                .Select(x => new GetCurrentAuthorizationsResponse()
                {
                    Id = x.Id,
                    InsurancePolicyNumber = x.Patient.InsurancePolicyNumber,
                    //InsurancePhoneNumber = x.Patient.ClientInsurance != null ? x.Patient.ClientInsurance.PhoneNumber : null,
                    //InsuranceFaxNumber = x.Patient.ClientInsurance != null ? x.Patient.ClientInsurance.FaxNumber : null,
                    Notes = string.Join(Environment.NewLine, x.Notes.Select(n => string.Format("{0}: {1}", n.CreatedOn.ToShortDateString(), n.NoteContent))),
                    AuthNumber = x.AuthNumber,
                    AuthorizationStatusId = x.AuthorizationStatusId,
                    AuthTypeId = x.AuthTypeId,
                    AuthTypeName = x.AuthType.Name,
                    ClientId = x.ClientId,
                    //Completeby = _idContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    CompleteDate = x.CompleteDate,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //DischargedBy = _idContext.Users.Where(z => z.Id == x.DischargedBy).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
                    DischargedOn = x.DischargedOn,
                    CreateUserId = x.CreatedBy,
                    PatientName = x.Patient.Person.LastCommaFirstName,
                    AccountNumber = x.Patient.AccountNumber,
                    PatientDateOfBirth = x.Patient.Person.DateOfBirth,
                    PatientId = x.PatientId,
                    Units = x.Units,
                    Documents = x.Documents.ToList(),
                    NeededDocumentTypes = x.GetNeededDocumentTypes(x)
                });

            return query;
        }

        public async Task<PaginatedResult<GetCurrentAuthorizationsResponse>> GetCurrentWithProperNamePaginated(GetPagedCurrentAuthorizationsQuery request)
        {
            IQueryable<GetCurrentAuthorizationsResponse> query = await GetCurrentWithProperNameQueryable(request);

            return await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
