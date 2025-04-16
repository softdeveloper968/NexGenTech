using AutoMapper;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.Entities;
using MedHelpAuthorizations.Infrastructure.DataPipe.Helpers;
using MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces;
using MedHelpAuthorizations.Infrastructure.DataPipe.Models;
using MedHelpAuthorizations.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Services
{
    public class DfStagingService : IDfStagingService
    {
        private IMediator _mediator;
        private readonly ITenantCryptographyService _tenantCryptographyService;
        private readonly IMapper _mapper;
        private readonly IDfStagingUnitOfWork _dfStagingUnitOfWork;
        protected readonly string _tenantClientString;
        protected ILogger<IDfStagingService> _logger { get; }
        protected readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public DfStagingService(ITenantCryptographyService tenantCryptographyService, IDfStagingUnitOfWork dfStagingUnitOfWork, ITenantRepositoryFactory tenantRepositoryFactory, ILogger<IDfStagingService> logger, IMapper mapper, IMediator mediator)
        {
            _tenantCryptographyService = tenantCryptographyService;
            _dfStagingUnitOfWork = dfStagingUnitOfWork;
            _mapper = mapper;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<List<T>> GetUnprocessedDfRecordsByTenantClient<T>(string tenantClientString) where T : DfStagingAuditableEntity
        {
            var unprocessedRecords = await _dfStagingUnitOfWork.Repository<T>().GetUnprocessedByTenantClientAsync(tenantClientString);
            return unprocessedRecords;
        }

        public async Task UpdateDfProcessedSuccessfully<T>(int id, CancellationToken cancellationToken = new CancellationToken()) where T : DfStagingAuditableEntity
        {
            var stgRecord = await _dfStagingUnitOfWork.Repository<T>().GetByIdAsync(id);
            if (stgRecord == null)
                throw new Exception($"Could not find record Id: {id} in for Entity Type: {typeof(T)}");
            else
            {
                await _dfStagingUnitOfWork.Repository<T>().UpdateProcessedSuccessfullyAsync(stgRecord);
                await _dfStagingUnitOfWork.Commit(cancellationToken);
            }
        }

        public async Task UpdateDfProcessedSuccessfully<T>(T dfStagingEntity, CancellationToken cancellationToken = new CancellationToken()) where T : DfStagingAuditableEntity
        {
            if (dfStagingEntity == null)
                throw new Exception($"Entity Type: {typeof(T)}  is null");
            else
            {
                await _dfStagingUnitOfWork.Repository<T>().UpdateProcessedSuccessfullyAsync(dfStagingEntity);
                await _dfStagingUnitOfWork.Commit(cancellationToken);
            }
        }

        public async Task UpdateDfProcessedError<T>(int id, string errorMessage, CancellationToken cancellationToken = new CancellationToken()) where T : DfStagingAuditableEntity
        {
            var stgRecord = await _dfStagingUnitOfWork.Repository<T>().GetByIdAsync(id);
            if (stgRecord == null)
                throw new Exception($"Could not find record Id: {id} in for Entity Type: {typeof(T)}");
            else
            {
                await _dfStagingUnitOfWork.Repository<T>().UpdateErroredAsync(stgRecord, errorMessage);
                await _dfStagingUnitOfWork.Commit(cancellationToken);
            }
        }

        public async Task UpdateDfProcessedError<T>(T dfStagingEntity, string errorMessage, CancellationToken cancellationToken = new CancellationToken()) where T : DfStagingAuditableEntity
        {
            if (dfStagingEntity == null)
                throw new Exception($"Entity Type: {typeof(T)}  is null");
            else
            {
                await _dfStagingUnitOfWork.Repository<T>().UpdateErroredAsync(dfStagingEntity, errorMessage);
                await _dfStagingUnitOfWork.Commit(cancellationToken);
            }

        }

        public async Task TransformTblAdjustmentCodes(List<TblAdjustmentCode> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.Id == null)
                        throw new Exception("TblAdjustmentCode record has a null Id");

                    ClientAdjustmentCode adjustmentCode =
                        await unitOfWork.Repository<ClientAdjustmentCode>()
                                .Entities
                                .Specify(new GenericByClientIdSpecification<ClientAdjustmentCode>(clientId))
                                .Specify(new GenericByDfExternalIdSpecification<ClientAdjustmentCode>(record.Id.ToString()))
                                .FirstOrDefaultAsync(); //No null check.We want it to throw an exception if record.Id is null. 

                    //If not found, create new
                    if (adjustmentCode == null)
                    {
                        adjustmentCode = new ClientAdjustmentCode(clientId, record.Name, record.Name, record.Name, record.GetAdjustmentTypeId(), record.Id.ToString(), record.StgCreatedOn, record.StgLastModifiedOn);
                        await unitOfWork.Repository<ClientAdjustmentCode>().AddAsync(adjustmentCode);
                    }
                    else
                    {
                        //update found adjustment code
                        adjustmentCode.AdjustmentTypeId = record.GetAdjustmentTypeId();
                        adjustmentCode.ClientId = clientId;
                        adjustmentCode.Code = record.Name;
                        //adjustmentCode.DfCreatedOn = record.StgCreatedOn;
                        //adjustmentCode.DfLastModifiedOn = record.StgLastModifiedOn;
                        adjustmentCode.Description = record.Name;
                        await unitOfWork.Repository<ClientAdjustmentCode>().UpdateAsync(adjustmentCode);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblAdjustmentCode>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblAdjustmentCode>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblCardholders(List<TblCardHolder> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.CardHolderId == null)
                        throw new Exception("TblCardHolder record has a null CardholderId");

                    //If cardholder externalId record exists
                    var cardholder =
                        await unitOfWork.Repository<Cardholder>()
                                .Entities
                                .Specify(new GenericByClientIdSpecification<Cardholder>(clientId))
                                .Specify(new GenericByDfExternalIdSpecification<Cardholder>(record.CardHolderId.ToString()))
                                .FirstOrDefaultAsync();

                    if (cardholder != null)
                    {
                        await UpdateDfProcessedSuccessfully<TblCardHolder>(record);
                        continue;
                    }

                    var personId = await UpsertPersonWithAddress(record.FirstName,
                                                                record.LastName,
                                                                DateHelpers.ConvertStringToNullableDate(record.DateOfBirth),
                                                                record.Address,
                                                                null,//record.Address2 TODO: Updae StagingTable with this info
                                                                record.City,
                                                                record.State,
                                                                record.PostalCode,
                                                                personRepository,
                                                                addressRepository,
                                                                clientId);

                    //Add new Cardholder
                    cardholder = new Cardholder()
                    {
                        DfExternalId = record.CardHolderId.ToString(),
                        ClientId = clientId,
                        CreatedBy = "DfStagingService",
                        //DfCreatedOn = record.StgCreatedOn,
                        //DfLastModifiedOn = record.StgLastModifiedOn,
                        PersonId = personId,

                    };
                    await unitOfWork.Repository<Cardholder>().AddAsync(cardholder);
                    await unitOfWork.Commit(new CancellationToken());

                    await UpdateDfProcessedSuccessfully<TblCardHolder>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblCardHolder>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblCharges(List<TblCharge> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(record?.ChargeId) || string.IsNullOrWhiteSpace(record?.PatientId))
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References:", 255);

                        if (string.IsNullOrWhiteSpace(record?.ChargeId))
                            errorMessage.Append($"{Environment.NewLine}ChargeId is null or empty.");

                        if (string.IsNullOrWhiteSpace(record?.PatientId))
                            errorMessage.Append($"{Environment.NewLine}PatientId is null or empty.");

                        // Assuming UpdateDfProcessedError is an asynchronous method and needs to be awaited
                        await UpdateDfProcessedError<TblCharge>(record, errorMessage.ToString());

                        throw new Exception(errorMessage.ToString());
                    }


                    PatientLedgerCharge patientLedgerCharge = await unitOfWork.Repository<PatientLedgerCharge>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<PatientLedgerCharge>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<PatientLedgerCharge>(record.ChargeId))
                                              .FirstOrDefaultAsync();

                    Patient patient = await unitOfWork.Repository<Patient>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<Patient>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<Patient>(record.PatientId.ToString()))
                                              .FirstOrDefaultAsync();

                    ResponsibleParty responsibleParty = string.IsNullOrWhiteSpace(record?.ResponsiblePartyId)
                                    ? null
                                    : await unitOfWork.Repository<ResponsibleParty>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<ResponsibleParty>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<ResponsibleParty>(record.ResponsiblePartyId))
                                              .FirstOrDefaultAsync();

                    ClientCptCode clientCptCode = string.IsNullOrWhiteSpace(record?.ProcedureCode) ? await GetOrCreateDefaultClientCptCode(unitOfWork, clientId, "CptMissing", "CptMissing", "CptMissing") : await GetOrCreateDefaultClientCptCode(unitOfWork, clientId, record.ProcedureCode, record.Description, record.Description);

                    //string.IsNullOrWhiteSpace(record.ProcedureCode)
                    //   ? null
                    //   : await unitOfWork.Repository<ClientCptCode>()
                    //   .Entities
                    //   .Specify(new GenericByClientIdSpecification<ClientCptCode>(clientId))
                    //   .FirstOrDefaultAsync(x => x.Code == record.ProcedureCode);

                    ClientPlaceOfService clientPlaceOfService = string.IsNullOrWhiteSpace(record.PlaceOfServiceId)
                                                   ? null
                                                   : await unitOfWork.Repository<ClientPlaceOfService>()
                                                   .Entities
                                                   .Specify(new GenericByClientIdSpecification<ClientPlaceOfService>(clientId))
                                                   .Specify(new GenericByDfExternalIdSpecification<ClientPlaceOfService>(record.PlaceOfServiceId))
                                                   .FirstOrDefaultAsync();

                    ClientProvider clientProvider = string.IsNullOrWhiteSpace(record.RenderingProviderId)
                                                   ? null
                                                   : await unitOfWork.Repository<ClientProvider>()
                                                   .Entities
                                                   .Specify(new GenericByClientIdSpecification<ClientProvider>(clientId))
                                                   .Specify(new GenericByDfExternalIdSpecification<ClientProvider>(record.RenderingProviderId))
                                                   .FirstOrDefaultAsync();

                    ClientLocation? clientLocation = string.IsNullOrWhiteSpace(record?.LocationId.ToString())
                                                  ? null
                                                  : await unitOfWork.Repository<ClientLocation>()
                                                            .Entities
                                                            .Specify(new GenericByClientIdSpecification<ClientLocation>(clientId))
                                                            .Specify(new GenericByDfExternalIdSpecification<ClientLocation>(record.LocationId.ToString()))
                                                            .FirstOrDefaultAsync();

                    ClientInsurance? clientInsurance = (int.TryParse(record.BilledToInsuranceId, out int number))
                                                  ? await unitOfWork.Repository<ClientInsurance>()
                                                            .Entities
                                                            .Specify(new GenericByClientIdSpecification<ClientInsurance>(clientId))
                                                            .Specify(new GenericByDfExternalIdSpecification<ClientInsurance>(number.ToString()))
                                                            .FirstOrDefaultAsync() ?? null : null;

                    // Cardholder, Patient, insurance references should have been processed before these records and should exist. 
                    if (patient == null || responsibleParty == null || clientCptCode == null)
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References to: ", 255);
                        if (responsibleParty == null)
                            errorMessage.Append($"{Environment.NewLine}ResponsiblePartyId = {record.ResponsiblePartyId}");
                        if (patient == null)
                            errorMessage.Append($"{Environment.NewLine}PatientId = {record.PatientId}");
                        if (clientCptCode == null)
                            errorMessage.Append($"{Environment.NewLine}ProcedureCode = {record.ProcedureCode}");

                        await UpdateDfProcessedError<TblCharge>(record, errorMessage.ToString());

                        continue;
                    }

                    if (!double.TryParse(record.Quantity, out double quantityDouble))
                    {
                        quantityDouble = 0; // Default value if parsing fails
                    }
                    int quantity = (int)quantityDouble; // Convert to integer (truncating any decimal places)

                    //If not found, create new
                    if (patientLedgerCharge == null)
                    {
                        patientLedgerCharge = new PatientLedgerCharge()
                        {
                            ClientId = clientId,
                            DfExternalId = record.ChargeId,
                            DfCreatedOn = DateHelpers.ConvertStringToNullableDate(record.EntryDate),
                            DfLastModifiedOn = DateHelpers.ConvertStringToNullableDate(record.ModifiedDate),
                            CreatedBy = "DfStagingService",
                            PatientId = patient.Id,
                            ClientCptCodeId = clientCptCode.Id,
                            ResponsiblePartyId = responsibleParty.Id,
                            ClientLocationId = clientLocation?.Id,
                            ClaimNumber = record.ClaimNumber,
                            ClientPlaceOfServiceId = clientPlaceOfService?.Id,
                            Quantity = quantity,
                            ChargeAmount = record.ChargeAmount ?? 0.0m,
                            Description = record.Description,
                            InsuranceCard1Id = await GetInsuranceCardIdByIdentifierAsync(unitOfWork, clientId, record.PatientInsuranceCard1Id ?? string.Empty),
                            InsuranceCard2Id = await GetInsuranceCardIdByIdentifierAsync(unitOfWork, clientId, record.PatientInsuranceCard2Id ?? string.Empty),
                            InsuranceCard3Id = await GetInsuranceCardIdByIdentifierAsync(unitOfWork, clientId, record.PatientInsuranceCard3Id ?? string.Empty),
                            //BilledToInsuranceCardId =  //ToDo
                            RenderingProviderId = clientProvider?.Id,
                            Modifier1 = record.Modifier1,
                            Modifier2 = record.Modifier2,
                            Modifier3 = record.Modifier3,
                            Modifier4 = record.Modifier4,
                            IcdCode1 = record.IcdCode1,
                            IcdCode2 = record.IcdCode2,
                            IcdCode3 = record.IcdCode3,
                            IcdCode4 = record.IcdCode4,
                            DateOfServiceFrom = DateHelpers.ConvertStringToNullableDate(record.DateOfServiceFrom),
                            DateOfServiceTo = DateHelpers.ConvertStringToNullableDate(record.DateOfServiceTo),
                            PatientFirstBillDate = record.PatientFirstBillDate,
                            PatientLastBillDate = record.PatientLastBillDate,
                            BilledToClientInsuranceId = clientInsurance?.Id,
                            InsuranceFirstBilledOn = DateHelpers.ConvertStringToNullableDate(record.InsuranceFirstBilledOn),
                            InsuranceLastBilledOn = DateHelpers.ConvertStringToNullableDate(record.InsuranceLastBilledOn)
                        };
                        await unitOfWork.Repository<PatientLedgerCharge>().AddAsync(patientLedgerCharge);
                    }
                    else
                    {
                        patientLedgerCharge.DfLastModifiedOn = DateHelpers.ConvertStringToNullableDate(record.ModifiedDate);
                        patientLedgerCharge.CreatedBy = "DfStagingService";
                        patientLedgerCharge.PatientId = patient.Id;
                        patientLedgerCharge.ClientCptCodeId = clientCptCode.Id;
                        patientLedgerCharge.ResponsiblePartyId = responsibleParty.Id;
                        patientLedgerCharge.ClientLocationId = clientLocation?.Id;
                        patientLedgerCharge.ClaimNumber = record.ClaimNumber;
                        patientLedgerCharge.ClientPlaceOfServiceId = clientPlaceOfService?.Id;
                        patientLedgerCharge.Quantity = quantity;
                        patientLedgerCharge.ChargeAmount = record.ChargeAmount ?? 0.0m;
                        patientLedgerCharge.Description = record.Description;
                        patientLedgerCharge.InsuranceCard1Id = await GetInsuranceCardIdByIdentifierAsync(unitOfWork, clientId, record.PatientInsuranceCard1Id);
                        patientLedgerCharge.InsuranceCard2Id = await GetInsuranceCardIdByIdentifierAsync(unitOfWork, clientId, record.PatientInsuranceCard2Id);
                        patientLedgerCharge.InsuranceCard3Id = await GetInsuranceCardIdByIdentifierAsync(unitOfWork, clientId, record.PatientInsuranceCard3Id);
                        //patientLedgerCharge.BilledToInsuranceCardId = To Do
                        patientLedgerCharge.RenderingProviderId = clientProvider?.Id;
                        patientLedgerCharge.Modifier1 = record.Modifier1;
                        patientLedgerCharge.Modifier2 = record.Modifier2;
                        patientLedgerCharge.Modifier3 = record.Modifier3;
                        patientLedgerCharge.Modifier4 = record.Modifier4;
                        patientLedgerCharge.IcdCode1 = record.IcdCode1;
                        patientLedgerCharge.IcdCode2 = record.IcdCode2;
                        patientLedgerCharge.IcdCode3 = record.IcdCode3;
                        patientLedgerCharge.IcdCode4 = record.IcdCode4;
                        patientLedgerCharge.DateOfServiceFrom = DateHelpers.ConvertStringToNullableDate(record.DateOfServiceFrom);
                        patientLedgerCharge.DateOfServiceTo = DateHelpers.ConvertStringToNullableDate(record.DateOfServiceTo);
                        patientLedgerCharge.PatientFirstBillDate = record.PatientFirstBillDate;
                        patientLedgerCharge.PatientLastBillDate = record.PatientLastBillDate;
                        patientLedgerCharge.BilledToClientInsuranceId = clientInsurance?.Id;

                        await unitOfWork.Repository<PatientLedgerCharge>().UpdateAsync(patientLedgerCharge);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblCharge>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblCharge>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblClaimAdjustments(List<TblClaimAdjustment> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.ClaimAdjustmentsId == null)
                        throw new Exception("TblClaimAdjustment record has a null ClaimAdjustmentsId");

                    PatientLedgerAdjustment patientLedgerAdjustment = await unitOfWork.Repository<PatientLedgerAdjustment>()
                                                                        .Entities
                                                                        .Specify(new GenericByClientIdSpecification<PatientLedgerAdjustment>(clientId))
                                                                        .Specify(new GenericByDfExternalIdSpecification<PatientLedgerAdjustment>(record.ClaimAdjustmentsId.ToString()))
                                                                        .FirstOrDefaultAsync();

                    PatientLedgerCharge patientLedgerCharge = string.IsNullOrWhiteSpace(record?.ClaimChargeId)
                                                       ? null
                                                       : await unitOfWork.Repository<PatientLedgerCharge>()
                                                                 .Entities
                                                                 .Specify(new GenericByClientIdSpecification<PatientLedgerCharge>(clientId))
                                                                 .Specify(new GenericByDfExternalIdSpecification<PatientLedgerCharge>(record.ClaimChargeId))
                                                                    .FirstOrDefaultAsync();

                    ClientRemittance clientRemittance = string.IsNullOrWhiteSpace(record?.RemittenceId)
                                                       ? null
                                                       : await unitOfWork.Repository<ClientRemittance>()
                                                                 .Entities
                                                                 .Specify(new GenericByClientIdSpecification<ClientRemittance>(clientId))
                                                                 .Specify(new GenericByDfExternalIdSpecification<ClientRemittance>(record.RemittenceId))
                                                                    .FirstOrDefaultAsync();

                    ClientAdjustmentCode clientAdjustmentCode = string.IsNullOrWhiteSpace(record?.AdjustmentCodeId)
                                                       ? null
                                                       : await unitOfWork.Repository<ClientAdjustmentCode>()
                                                                 .Entities
                                                                 .Specify(new GenericByClientIdSpecification<ClientAdjustmentCode>(clientId))
                                                                 .Specify(new GenericByDfExternalIdSpecification<ClientAdjustmentCode>(record.AdjustmentCodeId))
                                                                    .FirstOrDefaultAsync();

                    if (clientAdjustmentCode == null)
                    {
                        var client = await unitOfWork.Repository<Domain.Entities.Client>().GetByIdAsync(clientId);
                        switch (client.SourceSystemId)
                        {
                            case SourceSystemEnum.CyFluent:
                                if (!string.IsNullOrWhiteSpace(record?.Description) && record.Description.Contains("DIS:"))
                                {
                                    clientAdjustmentCode = await unitOfWork.Repository<ClientAdjustmentCode>().Entities
                                            .Specify(new GenericByClientIdSpecification<ClientAdjustmentCode>(clientId))
                                            .FirstOrDefaultAsync(x => x.Code == "DefaultDisallow");

                                    //Create a default adjustmentCode if one does not exist. 
                                    if (clientAdjustmentCode == null)
                                    {
                                        clientAdjustmentCode = await GetOrCreateDefaultClientAdjustmentCode(unitOfWork, clientId, "DefaultDisallow", "DefaultDisallow", "Created when no adjustment code reference provided", AdjustmentTypeEnum.Credit);
                                    }
                                }
                                break;
                        }
                    }

                    if (patientLedgerCharge == null || clientAdjustmentCode == null)
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References to: ", 255);
                        if (patientLedgerCharge == null)
                            errorMessage.Append($"{Environment.NewLine}PatientLedgerChargeId = {record.ClaimChargeId}");

                        if (clientAdjustmentCode == null)
                            errorMessage.Append($"{Environment.NewLine}AdjustmentCodeId = {record.AdjustmentCodeId}");
                        await UpdateDfProcessedError<TblClaimAdjustment>(record, errorMessage.ToString());
                        continue;
                    }

                    //If not found, create new
                    if (patientLedgerAdjustment == null)
                    {
                        patientLedgerAdjustment = new PatientLedgerAdjustment()
                        {
                            ClientId = clientId,
                            DfExternalId = record.ClaimAdjustmentsId.ToString(),
                            DfCreatedOn = DateHelpers.ConvertStringToNullableDate(record.EntryDate),
                            DfLastModifiedOn = DateHelpers.ConvertStringToNullableDate(record.ModifiedDate),
                            PatientLedgerChargeId = patientLedgerCharge.Id,
                            Amount = record.Amount ?? 0.0m,
                            ClaimNumber = record.ClaimNumber.ToString(),
                            ClientRemittanceId = clientRemittance?.Id,
                            Description = record?.Description,
                            ClientAdjustmentCodeId = clientAdjustmentCode.Id,
                        };
                        await unitOfWork.Repository<PatientLedgerAdjustment>().AddAsync(patientLedgerAdjustment);
                    }
                    else
                    {
                        patientLedgerAdjustment.DfLastModifiedOn = DateHelpers.ConvertStringToNullableDate(record.ModifiedDate);
                        patientLedgerAdjustment.PatientLedgerChargeId = patientLedgerCharge.Id;
                        patientLedgerAdjustment.Amount = record.Amount ?? 0.0m;
                        patientLedgerAdjustment.ClaimNumber = record.ClaimNumber.ToString();
                        patientLedgerAdjustment.ClientRemittanceId = clientRemittance?.Id;
                        patientLedgerAdjustment.Description = record.Description;
                        patientLedgerAdjustment.ClientAdjustmentCodeId = clientAdjustmentCode.Id;

                        await unitOfWork.Repository<PatientLedgerAdjustment>().UpdateAsync(patientLedgerAdjustment);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblClaimAdjustment>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblClaimAdjustment>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblClaimPayments(List<TblClaimPayment> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.ClaimPaymentId == null)
                        throw new Exception("TblClaimPayment record has a null ClaimPaymentId");

                    PatientLedgerPayment patientLedgerPayment = await unitOfWork.Repository<PatientLedgerPayment>()
                                                                    .Entities
                                                                    .Specify(new GenericByClientIdSpecification<PatientLedgerPayment>(clientId))
                                                                    .Specify(new GenericByDfExternalIdSpecification<PatientLedgerPayment>(record.ClaimPaymentId.ToString()))
                                                                    .FirstOrDefaultAsync();

                    PatientLedgerCharge patientLedgerCharge = string.IsNullOrWhiteSpace(record?.ClaimChargeId)
                                                       ? null
                                                       : await unitOfWork.Repository<PatientLedgerCharge>()
                                                                 .Entities
                                                                 .Specify(new GenericByClientIdSpecification<PatientLedgerCharge>(clientId))
                                                                 .Specify(new GenericByDfExternalIdSpecification<PatientLedgerCharge>(record.ClaimChargeId))
                                                                    .FirstOrDefaultAsync();

                    ClientRemittance clientRemittance = string.IsNullOrWhiteSpace(record?.RemittanceId)
                                                       ? null
                                                       : await unitOfWork.Repository<ClientRemittance>()
                                                                 .Entities
                                                                 .Specify(new GenericByClientIdSpecification<ClientRemittance>(clientId))
                                                                 .Specify(new GenericByDfExternalIdSpecification<ClientRemittance>(record.RemittanceId))
                                                                    .FirstOrDefaultAsync();

                    if (patientLedgerCharge == null || clientRemittance == null)
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References to: ", 255);
                        if (patientLedgerCharge == null)
                            errorMessage.Append($"{Environment.NewLine}PatientLedgerChargeId = {record.ClaimChargeId}");

                        if (clientRemittance == null)
                            errorMessage.Append($"{Environment.NewLine}RemittanceId = {record.RemittanceId}");

                        await UpdateDfProcessedError<TblClaimPayment>(record, errorMessage.ToString());

                        continue;
                    }

                    //If not found, create new
                    if (patientLedgerPayment == null)
                    {
                        patientLedgerPayment = new PatientLedgerPayment()
                        {
                            ClientId = clientId,
                            DfExternalId = record.ClaimPaymentId.ToString(),
                            DfCreatedOn = DateHelpers.ConvertStringToNullableDate(record.EntryDate),
                            DfLastModifiedOn = DateHelpers.ConvertStringToNullableDate(record.ModifiedDate),
                            PatientLedgerChargeId = patientLedgerCharge.Id,
                            Amount = record.Amount ?? 0.0m,
                            ClaimNumber = record.ClaimNumber.ToString(),
                            ClientRemittanceId = clientRemittance.Id,
                            Description = record.Description,
                        };
                        await unitOfWork.Repository<PatientLedgerPayment>().AddAsync(patientLedgerPayment);
                    }
                    else
                    {
                        //update found PatientLedgerPayment                   
                        patientLedgerPayment.DfLastModifiedOn = DateHelpers.ConvertStringToNullableDate(record.ModifiedDate);
                        patientLedgerPayment.PatientLedgerChargeId = patientLedgerCharge.Id;
                        patientLedgerPayment.Amount = record.Amount ?? 0.0m;
                        patientLedgerPayment.ClaimNumber = record.ClaimNumber.ToString();
                        //patientLedgerPayment.ClientRemittanceId = clientRemittance.Id;
                        patientLedgerPayment.Description = record.Description;

                        await unitOfWork.Repository<PatientLedgerPayment>().UpdateAsync(patientLedgerPayment);
                    }

                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblClaimPayment>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblClaimPayment>(record, errorMessage);
                }
            }
        }
        public async Task TransformTblInsurances(List<TblInsurance> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.InsuranceId == null)
                        throw new Exception("TblInsurances record has a null InsuranceId");

                    ClientInsurance clientInsurance =
                    await unitOfWork.Repository<ClientInsurance>()
                            .Entities
                            .Specify(new GenericByClientIdSpecification<ClientInsurance>(clientId))
                            .Specify(new GenericByDfExternalIdSpecification<ClientInsurance>(record.InsuranceId.ToString()))
                            .FirstOrDefaultAsync(); //No null check.We want it to throw an exception if record.InsuranceId is null. 

                    //If not found, create new
                    if (clientInsurance == null)
                    {
                        clientInsurance = new ClientInsurance()
                        {
                            ClientId = clientId,
                            DfExternalId = record.InsuranceId.ToString(),
                            //DfCreatedOn = record.StgCreatedOn,
                            //DfLastModifiedOn = record.StgLastModifiedOn,
                            Name = record.Name ?? "Missing Insurance Name",
                            LookupName = !string.IsNullOrWhiteSpace(record?.Name) ? record?.Name : "Missing Insurance Name",
                        };
                        await unitOfWork.Repository<ClientInsurance>().AddAsync(clientInsurance);
                    }
                    else
                    {
                        //update found client Insurance                       
                        clientInsurance.DfLastModifiedOn = record.StgLastModifiedOn;
                        clientInsurance.Name = record.Name ?? "Missing Insurance Name";
                        clientInsurance.LookupName = !string.IsNullOrWhiteSpace(record?.Name) ? record?.Name : "Missing Insurance Name";

                        await unitOfWork.Repository<ClientInsurance>().UpdateAsync(clientInsurance);
                    }

                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblInsurance>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblInsurance>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblLocations(List<TblLocation> records, int clientId, IUnitOfWork<int> unitOfWork, IAddressRepository addressRepository)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.LocationId == null)
                        throw new Exception("TblLocation record has a null LocationId");

                    ClientLocation clientLocation =
                        await unitOfWork.Repository<ClientLocation>()
                                .Entities
                                .Specify(new GenericByClientIdSpecification<ClientLocation>(clientId))
                                .Specify(new GenericByDfExternalIdSpecification<ClientLocation>(record.LocationId.ToString()))
                                .FirstOrDefaultAsync(); //No null check.We want it to throw an exception if record.LocationId is null. 

                    var addressId = await UpsertAddress(record.AddressStreetLine1, record.AddressStreetLine2, record.City, record.State, record.PostalCode, addressRepository, clientId);

                    //If not found, create new
                    if (clientLocation == null)
                    {
                        clientLocation = new ClientLocation()
                        {
                            ClientId = clientId,
                            DfExternalId = record.LocationId.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            Name = record.LocationName,
                            AddressId = addressId,
                            //Npi = record.Npi, TODO: Add to DfStaging table. 
                            //OfficePhoneNumber = record.OfficePhoneNumber, TODO: Add to DfStaging table. 
                        };
                        await unitOfWork.Repository<ClientLocation>().AddAsync(clientLocation);
                    }
                    else
                    {
                        //update found client Location                        
                        clientLocation.DfLastModifiedOn = record.StgLastModifiedOn;
                        clientLocation.Name = record.LocationName;
                        //clientLocation.Npi = record.Npi; TODO: Add to DfStaging table. 
                        //OfficePhoneNumber = record.OfficePhoneNumber, TODO: Add to DfStaging table. 
                        clientLocation.AddressId = addressId;
                        await unitOfWork.Repository<ClientLocation>().UpdateAsync(clientLocation);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblLocation>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblLocation>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblPatientInsuranceCards(List<TblPatientInsuranceCard> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.Id == null)
                        throw new Exception("TblPatientInsuranceCard record has a null Id");

                    ClientInsurance clientInsurance = string.IsNullOrWhiteSpace(record?.InsuranceId?.ToString())
                                    ? null
                                    : await unitOfWork.Repository<ClientInsurance>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<ClientInsurance>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<ClientInsurance>(record.InsuranceId.ToString()))
                                              .FirstOrDefaultAsync();
                    Patient patient = string.IsNullOrWhiteSpace(record?.PatientId?.ToString())
                                    ? null
                                    : await unitOfWork.Repository<Patient>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<Patient>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<Patient>(record.PatientId.ToString()))
                                              .FirstOrDefaultAsync();
                    Cardholder cardholder = string.IsNullOrWhiteSpace(record?.CardHolderId?.ToString())
                                    ? null
                                    : await unitOfWork.Repository<Cardholder>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<Cardholder>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<Cardholder>(record.CardHolderId.ToString()))
                                              .FirstOrDefaultAsync();
                    InsuranceCard insuranceCard =
                       await unitOfWork.Repository<InsuranceCard>()
                               .Entities
                               .Specify(new GenericByClientIdSpecification<InsuranceCard>(clientId))
                               .Specify(new GenericByDfExternalIdSpecification<InsuranceCard>(record.Id.ToString()))
                               .FirstOrDefaultAsync();

                    // Cardholder, Patient, insurance references should have been processed before these records and should exist. 
                    if (cardholder == null || patient == null || clientInsurance == null)
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References to: ", 255);
                        if (cardholder == null)
                            errorMessage.Append($"{Environment.NewLine}CardholderId = {record.CardHolderId?.ToString()}");
                        if (patient == null)
                            errorMessage.Append($"{Environment.NewLine}PatientId = {record.PatientId?.ToString()}");
                        if (cardholder == null)
                            errorMessage.Append($"{Environment.NewLine}InsuranceId = {record.InsuranceId.ToString()}");

                        await UpdateDfProcessedError<TblPatientInsuranceCard>(record, errorMessage.ToString());

                        continue;
                    }
                    //If not found, create new
                    if (insuranceCard == null)
                    {
                        insuranceCard = new InsuranceCard()
                        {
                            ClientId = clientId,
                            DfExternalId = record.Id.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            Active = DateHelpers.ConvertStringToNullableDate(record.InactiveDate) != null,
                            CopayAmount = record.CoPay ?? 0.00m,
                            CardholderId = cardholder.Id,
                            //CardholderRelationshipToPatient = record.CardHolderRelationship TODO: Add a mapping method to map relationship to Enum 
                            ClientInsuranceId = clientInsurance.Id,
                            CreatedBy = "DfStagingService",
                            InsuranceCardOrder = int.TryParse(record.InsuranceCardOrder, out int order) ? order : null,
                            PatientId = patient.Id,
                            EffectiveStartDate = DateHelpers.ConvertStringToNullableDate(record.EffectiveStartDate),
                            EffectiveEndDate = DateHelpers.ConvertStringToNullableDate(record.EffectiveEndDate),
                            GroupNumber = record.GroupId,
                            MemberNumber = record.MemberNumber
                        };
                        await unitOfWork.Repository<InsuranceCard>().AddAsync(insuranceCard);
                    }
                    else
                    {
                        //update found InsuranceCard
                        insuranceCard.DfLastModifiedOn = record.StgLastModifiedOn;
                        insuranceCard.Active = DateHelpers.ConvertStringToNullableDate(record.InactiveDate) != null;
                        insuranceCard.CopayAmount = record.CoPay ?? 0.00m;
                        insuranceCard.CardholderId = cardholder.Id;
                        //insuranceCard.CardholderRelationshipToPatient = record.CardHolderRelationship TODO: Add a mapping method to map relationship to Enum 
                        insuranceCard.ClientInsuranceId = clientInsurance.Id;
                        insuranceCard.CreatedBy = "DfStagingService";
                        insuranceCard.InsuranceCardOrder = int.TryParse(record.InsuranceCardOrder, out int order) ? order : null;
                        //insuranceCard.PatientId = patient.Id;
                        insuranceCard.EffectiveStartDate = DateHelpers.ConvertStringToNullableDate(record.EffectiveStartDate);
                        insuranceCard.EffectiveEndDate = DateHelpers.ConvertStringToNullableDate(record.EffectiveEndDate);
                        insuranceCard.GroupNumber = record.GroupId;
                        insuranceCard.MemberNumber = record.MemberNumber;

                        await unitOfWork.Repository<InsuranceCard>().UpdateAsync(insuranceCard);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblPatientInsuranceCard>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblPatientInsuranceCard>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblPatients(List<TblPatient> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            foreach (var record in records)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(record?.PatientId?.ToString()))
                        throw new Exception("TblPatient record has a null PatientId");

                    Patient patient = await unitOfWork.Repository<Patient>()
                                              .Entities
                                              .Specify(new GenericByClientIdSpecification<Patient>(clientId))
                                              .Specify(new GenericByDfExternalIdSpecification<Patient>(record.PatientId.ToString()))
                                              .FirstOrDefaultAsync();

                    ResponsibleParty responsibleParty = string.IsNullOrWhiteSpace(record?.ResponsiblePartyId?.ToString())
                                                            ? null :
                                                         await unitOfWork.Repository<ResponsibleParty>()
                                                                      .Entities
                                                                      .Specify(new GenericByClientIdSpecification<ResponsibleParty>(clientId))
                                                                      .Specify(new GenericByDfExternalIdSpecification<ResponsibleParty>(record.ResponsiblePartyId.ToString()))
                                                                      .FirstOrDefaultAsync();

                    //ReferringProvider referringProvider = string.IsNullOrWhiteSpace(record?.RenderingProviderId?.ToString())
                    //                                        ? null :
                    //                                     await unitOfWork.Repository<ReferringProvider>()
                    //                                                  .Entities
                    //                                                  .Specify(new GenericByClientIdSpecification<ReferringProvider>(clientId))
                    //                                                  .Specify(new GenericByDfExternalIdSpecification<ReferringProvider>(record.RenderingProviderId.ToString()))
                    //                                                  .FirstOrDefaultAsync();

                    var personId = await UpsertPersonWithAddress(record.FirstName,
                                                             record.LastName,
                                                             DateHelpers.ConvertStringToNullableDate(record.DateOfBirth),
                                                             record.AddressStreetLine1,
                                                             null,//record.Address2 TODO: Updae StagingTable with this info
                                                             record.City,
                                                             record.State,
                                                             record.PostalCode,
                                                             personRepository,
                                                             addressRepository,
                                                             clientId);


                    AdministrativeGenderEnum administrativeGenderEnum = await GetAdministrativeGenderId(record.Gender);

                    //If not found, create new
                    if (patient == null)
                    {
                        patient = new Patient()
                        {
                            ClientId = clientId,
                            DfExternalId = record.PatientId.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            CreatedBy = "DfStagingService",
                            ResponsiblePartyId = responsibleParty?.Id,
                            //ReferringProviderId = record.RenderingProviderId,
                            AdministrativeGenderId = administrativeGenderEnum,
                            PersonId = personId,
                            ExternalId = record.ExternalId,
                        };
                        await unitOfWork.Repository<Patient>().AddAsync(patient);
                    }
                    else
                    {
                        patient.DfLastModifiedOn = record.StgLastModifiedOn;
                        patient.PersonId = personId;
                        patient.AdministrativeGenderId = administrativeGenderEnum;
                        //patient.ReferringProviderId = record.RenderingProviderId;
                        patient.ResponsiblePartyId = responsibleParty.Id;
                        patient.CreatedBy = "DfStagingService";
                        patient.ExternalId = record.ExternalId;

                        await unitOfWork.Repository<Patient>().UpdateAsync(patient);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblPatient>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblPatient>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblPlaceOfServices(List<TblPlaceOfService> records, int clientId, IUnitOfWork<int> unitOfWork, IAddressRepository addressRepository)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.PlaceOfServiceId == null)
                        throw new Exception("TblPlaceOfService record has a null PlaceOfServiceId");

                    ClientPlaceOfService clientPlaceOfService =
                        await unitOfWork.Repository<ClientPlaceOfService>()
                                .Entities
                                .Specify(new GenericByClientIdSpecification<ClientPlaceOfService>(clientId))
                                .Specify(new GenericByDfExternalIdSpecification<ClientPlaceOfService>(record.PlaceOfServiceId.ToString()))
                                .FirstOrDefaultAsync(); //No null check.We want it to throw an exception if record.PlaceOfServiceId is null. 

                    PlaceOfServiceCodeEnum placeOfServiceCodeEnum = await GetPlaceOfServiceCodeEnum(record.PlaceOfServiceCode);
                    var addressId = await UpsertAddress(record.Address, string.Empty, record.City, record.State, record.PostalCode, addressRepository, clientId);

                    // Check if necessary references are found
                    if (placeOfServiceCodeEnum == null)
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References to: ", 255);
                        if (placeOfServiceCodeEnum == null)
                            errorMessage.Append($"{Environment.NewLine}PlaceOfServiceCode = {record.PlaceOfServiceCode?.ToString()}");

                        await UpdateDfProcessedError<TblPlaceOfService>(record, errorMessage.ToString());
                        continue;
                    }

                    //If not found, create new
                    if (clientPlaceOfService == null)
                    {
                        clientPlaceOfService = new ClientPlaceOfService()
                        {
                            ClientId = clientId,
                            DfExternalId = record.PlaceOfServiceId.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            Name = !string.IsNullOrWhiteSpace(record?.Name) ? record?.Name : "Missing PlaceOfService Name",
                            AddressId = addressId,
                            LookupName = !string.IsNullOrWhiteSpace(record?.Name) ? record?.Name : "Missing PlaceOfService Name",
                            PlaceOfServiceCodeId = placeOfServiceCodeEnum,
                            //Npi = record.Npi, TODO: Add to DfStaging table. 
                            //OfficePhoneNumber = record.OfficePhoneNumber, TODO: Add to DfStaging table. 
                        };
                        await unitOfWork.Repository<ClientPlaceOfService>().AddAsync(clientPlaceOfService);
                    }
                    else
                    {
                        //update found Client PlaceOfService                       
                        clientPlaceOfService.DfLastModifiedOn = record.StgLastModifiedOn;
                        clientPlaceOfService.Name = !string.IsNullOrWhiteSpace(record?.Name) ? record?.Name : "Missing PlaceOfService Name";
                        clientPlaceOfService.LookupName = !string.IsNullOrWhiteSpace(record?.Name) ? record?.Name : "Missing PlaceOfService Name";
                        clientPlaceOfService.PlaceOfServiceCodeId = placeOfServiceCodeEnum;
                        //clientPlaceOfService.Npi = record.Npi; TODO: Add to DfStaging table. 
                        //OfficePhoneNumber = record.OfficePhoneNumber, TODO: Add to DfStaging table. 
                        clientPlaceOfService.AddressId = addressId;
                        await unitOfWork.Repository<ClientPlaceOfService>().UpdateAsync(clientPlaceOfService);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblPlaceOfService>(record);
                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblPlaceOfService>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblProviderLocations(List<TblProviderLocation> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.ProviderLocationId == null)
                        throw new Exception("TblProviderLocation record has a null ProviderLocationId");

                    ClientProviderLocation clientProviderLocation = await unitOfWork.Repository<ClientProviderLocation>()
                                                                        .Entities
                                                                        .Include(x => x.ClientProvider)
                                                                        .Specify(new GenericByDfExternalIdSpecification<ClientProviderLocation>(record.ProviderLocationId.ToString()))
                                                                        .FirstOrDefaultAsync(x => x.ClientProvider != null && x.ClientProvider.ClientId == clientId);

                    ClientProvider clientProvider = string.IsNullOrWhiteSpace(record?.ProviderId?.ToString())
                                   ? null
                                   : await unitOfWork.Repository<ClientProvider>()
                                             .Entities
                                             .Specify(new GenericByClientIdSpecification<ClientProvider>(clientId))
                                             .Specify(new GenericByDfExternalIdSpecification<ClientProvider>(record.ProviderId.ToString()))
                                             .FirstOrDefaultAsync();

                    ClientLocation clientLocation = string.IsNullOrWhiteSpace(record?.LocationId?.ToString())
                                   ? null
                                   : await unitOfWork.Repository<ClientLocation>()
                                             .Entities
                                             .Specify(new GenericByClientIdSpecification<ClientLocation>(clientId))
                                             .Specify(new GenericByDfExternalIdSpecification<ClientLocation>(record.LocationId.ToString()))
                    .FirstOrDefaultAsync();

                    if (clientProvider == null || clientLocation == null)
                    {
                        StringBuilder errorMessage = new StringBuilder("Missing References to: ", 255);
                        if (clientProvider == null)
                            errorMessage.Append($"{Environment.NewLine}ProviderId = {record.ProviderId?.ToString()}");
                        if (clientLocation == null)
                            errorMessage.Append($"{Environment.NewLine}LocationId = {record.LocationId?.ToString()}");

                        await UpdateDfProcessedError<TblProviderLocation>(record, errorMessage.ToString());

                        continue;
                    }

                    // Check if the record already exists
                    var existingRecord = await unitOfWork.Repository<ClientProviderLocation>()
                                            .Entities
                                            .FirstOrDefaultAsync(x => x.ClientProvider != null && x.ClientProvider.ClientId == clientId && x.ClientProviderId == clientProvider.Id &&
                                            x.ClientLocation != null && x.ClientLocationId == clientLocation.Id);

                    //If not found, create new
                    if (existingRecord == null)
                    {
                        clientProviderLocation = new ClientProviderLocation()
                        {
                            DfExternalId = record.StgId.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            ClientProviderId = clientProvider.Id,
                            ClientLocationId = clientLocation.Id,

                        };
                        await unitOfWork.Repository<ClientProviderLocation>().AddAsync(clientProviderLocation);
                    }
                    else
                    {
                        //update found clientProviderLocation                       
                        existingRecord.DfLastModifiedOn = record.StgLastModifiedOn;
                        existingRecord.ClientLocationId = clientLocation.Id;
                        existingRecord.ClientProviderId = clientProvider.Id;
                        existingRecord.CreatedBy = "DfStagingService";

                        await unitOfWork.Repository<ClientProviderLocation>().UpdateAsync(existingRecord);
                    }

                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblProviderLocation>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblProviderLocation>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblProviders(List<TblProvider> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.ProviderId == null)
                    {
                        throw new Exception("TblProviders record has a null ProviderId");
                    }

                    ClientProvider clientProvider = await unitOfWork.Repository<ClientProvider>()
                                                .Entities
                                                .Specify(new GenericByClientIdSpecification<ClientProvider>(clientId))
                                                .Specify(new GenericByDfExternalIdSpecification<ClientProvider>(record.ProviderId.ToString()))
                                                .FirstOrDefaultAsync();

                    var personId = await UpsertPersonWithAddress(record.FirstName,
                                                             record.LastName,
                                                             null,
                                                             record.Address,
                                                             null,
                                                             record.City,
                                                             record.State,
                                                             record.PostalCode,
                                                             personRepository,
                                                             addressRepository,
                                                             clientId);

                    SpecialtyEnum specialityEnum = await GetSpecialtyIdAsync(record.SpecialtyName);

                    // Normalize TaxId
                    string normalizedTaxId = new string(record.TaxId.Where(char.IsDigit).ToArray());
                    if (normalizedTaxId.Length != 9)
                    {
                        normalizedTaxId = "999999999";
                    }


                    if (clientProvider == null)
                    {
                        clientProvider = new ClientProvider()
                        {
                            ClientId = clientId,
                            DfExternalId = record.ProviderId.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            Npi = record.Npi,
                            TaxId = normalizedTaxId,
                            PersonId = personId,
                            ExternalId = record.ExternalId,
                            SpecialtyId = specialityEnum
                        };
                        await unitOfWork.Repository<ClientProvider>().AddAsync(clientProvider);
                    }
                    else
                    {
                        clientProvider.DfLastModifiedOn = record.StgLastModifiedOn;
                        clientProvider.Npi = record.Npi;
                        clientProvider.TaxId = normalizedTaxId;
                        clientProvider.PersonId = personId;
                        clientProvider.ExternalId = record.ExternalId;
                        clientProvider.SpecialtyId = specialityEnum;
                        await unitOfWork.Repository<ClientProvider>().UpdateAsync(clientProvider);
                    }

                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblProvider>(record);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblProvider>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblRemittances(List<TblRemittance> records, int clientId, IUnitOfWork<int> unitOfWork)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.RemittanceId == null)
                        throw new Exception("TblRemittance record has a null RemittanceId");

                    ClientRemittance clientRemittance =
                                            await unitOfWork.Repository<ClientRemittance>()
                                                    .Entities
                                                    .Specify(new GenericByClientIdSpecification<ClientRemittance>(clientId))
                                                    .Specify(new GenericByDfExternalIdSpecification<ClientRemittance>(record.RemittanceId))
                                                    .FirstOrDefaultAsync();

                    Patient patient = string.IsNullOrWhiteSpace(record?.PatientId)
                                   ? null
                                   : await unitOfWork.Repository<Patient>()
                                             .Entities
                                             .Specify(new GenericByClientIdSpecification<Patient>(clientId))
                                             .Specify(new GenericByDfExternalIdSpecification<Patient>(record.PatientId))
                                             .FirstOrDefaultAsync();

                    ClientInsurance clientInsurance = string.IsNullOrWhiteSpace(record?.InsuranceId?.ToString())
                                  ? null
                                  : await unitOfWork.Repository<ClientInsurance>()
                                            .Entities
                                            .Specify(new GenericByClientIdSpecification<ClientInsurance>(clientId))
                                            .Specify(new GenericByDfExternalIdSpecification<ClientInsurance>(record.InsuranceId.ToString()))
                                            .FirstOrDefaultAsync();

                    ClientLocation clientLocation = string.IsNullOrWhiteSpace(record?.LocationId?.ToString())
                               ? null
                               : await unitOfWork.Repository<ClientLocation>()
                                         .Entities
                                         .Specify(new GenericByClientIdSpecification<ClientLocation>(clientId))
                                         .Specify(new GenericByDfExternalIdSpecification<ClientLocation>(record.LocationId.ToString()))
                    .FirstOrDefaultAsync();


                    if (patient == null && clientInsurance == null)
                    {
                        await UpdateDfProcessedError<TblRemittance>(record, "Either PatientId and InsuranceId must not be null.");
                        continue;
                    }

                    //If not found, create new
                    if (clientRemittance == null)
                    {
                        clientRemittance = new ClientRemittance()
                        {
                            ClientId = clientId,
                            DfExternalId = record.RemittanceId,
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            PatientId = patient?.Id,
                            ClientLocationId = clientLocation?.Id,
                            ClientInsuranceId = clientInsurance?.Id,
                            CheckNumber = record.CheckNumber,
                            UndistributedAmount = record.UndistributedAmount,
                            PaymentAmount = record.PaymentAmount,
                            RemittanceFormType = record.RemittanceFormType,
                            RemittanceSource = record.RemittanceSource,
                            CheckDate = DateHelpers.ConvertStringToNullableDate(record.CheckDate),
                        };
                        await unitOfWork.Repository<ClientRemittance>().AddAsync(clientRemittance);
                    }
                    else
                    {
                        //update found ClientRemittance                      
                        clientRemittance.DfLastModifiedOn = record.StgLastModifiedOn;
                        clientRemittance.ClientLocationId = clientLocation?.Id;
                        clientRemittance.ClientInsuranceId = clientInsurance?.Id;
                        clientRemittance.CheckNumber = record.CheckNumber;
                        clientRemittance.UndistributedAmount = record.UndistributedAmount;
                        clientRemittance.PaymentAmount = record.PaymentAmount;
                        clientRemittance.RemittanceFormType = record.RemittanceFormType;
                        clientRemittance.RemittanceSource = record.RemittanceSource;
                        clientRemittance.CheckDate = DateHelpers.ConvertStringToNullableDate(record.CheckDate);
                        clientRemittance.PatientId = patient?.Id;

                        await unitOfWork.Repository<ClientRemittance>().UpdateAsync(clientRemittance);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblRemittance>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblRemittance>(record, errorMessage);
                }
            }
        }

        public async Task TransformTblResponsibleParties(List<TblResponsibleParty> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            foreach (var record in records)
            {
                try
                {
                    if (record?.ResponsiblePartiesId == null)
                        throw new Exception("TblResponsibleParty record has a null ResponsiblePartiesId");

                    ResponsibleParty responsibleParty =
                     await unitOfWork.Repository<ResponsibleParty>()
                             .Entities
                             .Specify(new GenericByClientIdSpecification<ResponsibleParty>(clientId))
                             .Specify(new GenericByDfExternalIdSpecification<ResponsibleParty>(record.ResponsiblePartiesId.ToString()))
                             .FirstOrDefaultAsync(); //No null check.We want it to throw an exception if record.ResponsiblePartiesId is null. 

                    var personId = await UpsertPersonWithAddress(record.FirstName,
                                                               record.LastName,
                                                               null,
                                                               record.AddressStreetLine1,
                                                               record.AddressStreetLine2,
                                                               record.City,
                                                               record.State,
                                                               record.PostalCode,
                                                               personRepository,
                                                               addressRepository,
                                                               clientId);


                    //If not found, create new
                    if (responsibleParty == null)
                    {
                        responsibleParty = new ResponsibleParty()
                        {
                            ClientId = clientId,
                            DfExternalId = record.ResponsiblePartiesId.ToString(),
                            DfCreatedOn = record.StgCreatedOn,
                            DfLastModifiedOn = record.StgLastModifiedOn,
                            PersonId = personId,
                        };
                        await unitOfWork.Repository<ResponsibleParty>().AddAsync(responsibleParty);
                    }
                    else
                    {
                        //update found ResponsibleParty                        
                        responsibleParty.DfLastModifiedOn = record.StgLastModifiedOn;
                        responsibleParty.PersonId = personId;
                        await unitOfWork.Repository<ResponsibleParty>().UpdateAsync(responsibleParty);
                    }
                    await unitOfWork.Commit(new CancellationToken());
                    await UpdateDfProcessedSuccessfully<TblResponsibleParty>(record);

                    continue;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message} {Environment.NewLine}{ex.StackTrace}";
                    await UpdateDfProcessedError<TblResponsibleParty>(record, errorMessage);
                }
            }
        }

        private async Task<int> UpsertAddress(string addressLine1, string addressLine2, string city, string state, string postalCode, IAddressRepository addressRepository, int clientId)
        {
            var address = string.IsNullOrWhiteSpace(addressLine1)
                            ? await addressRepository.FindByStreetAddressLine1AndPostalCode(addressLine1, postalCode, clientId)
                            : null;
            if (address == null)
            {
                address = new Address() { ClientId = clientId, AddressStreetLine1 = addressLine1, AddressStreetLine2 = addressLine2, City = city, StateId = await GetStateId(state), PostalCode = postalCode, CreatedBy = "DfStagingService" };
                await addressRepository.AddAsync(address);
                await addressRepository.Commit(new CancellationToken());
            }

            return address.Id;
        }

        private async Task<int> UpsertPerson(string firstName, string lastName, DateTime? dateOfBirth, int? addressId, IPersonRepository personRepository, int clientId)
        {
            var person = await personRepository.GetFirstPersonByCriteriaAsync(firstName, lastName, dateOfBirth, null, clientId);
            if (person != null && addressId != null)
            {
                person.AddressId = addressId;
                await personRepository.UpdateAsync(person);
            }
            else
            {
                person = new Person()
                {
                    ClientId = clientId,
                    FirstName = firstName,
                    LastName = lastName,
                    AddressId = addressId,
                    DateOfBirth = dateOfBirth,
                    CreatedBy = "DfStagingService",
                };
                await personRepository.AddAsync(person);
            }
            await personRepository.Commit(new CancellationToken());

            return person.Id;
        }

        private async Task<int> UpsertPersonWithAddress(string firstName, string lastName, DateTime? dateOfBirth, string addressLine1, string addressLine2, string city, string state, string postalCode, IPersonRepository personRepository, IAddressRepository addressRepository, int clientId)
        {
            int? addressId = null;
            if (addressLine1 != null)
            {
                addressId = await UpsertAddress(addressLine1, addressLine2, city, state, postalCode, addressRepository, clientId);
            }

            var personId = await UpsertPerson(firstName, lastName, dateOfBirth, addressId, personRepository, clientId);

            return personId;
        }

        public async Task<StateEnum> GetStateId(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
            {
                return StateEnum.UNK;
            }

            var normalizedState = state.ToUpper().Trim();

            if (StateLookupHelper.StateLookup.TryGetValue(normalizedState, out var stateEnum))
            {
                return stateEnum;
            }

            return StateEnum.UNK;
        }

        public async Task<PlaceOfServiceCodeEnum> GetPlaceOfServiceCodeEnum(int? code)
        {
            if (!code.HasValue)
                throw new ArgumentException("Code cannot be null.", nameof(code));

            try
            {
                if (Enum.IsDefined(typeof(PlaceOfServiceCodeEnum), code.Value))
                {
                    return (PlaceOfServiceCodeEnum)code.Value;
                }
                else
                {
                    throw new ArgumentException($"Invalid code value for {nameof(PlaceOfServiceCodeEnum)}.", nameof(code));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<AdministrativeGenderEnum> GetAdministrativeGenderId(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
            {
                return AdministrativeGenderEnum.Unknown;
            }

            if (Enum.TryParse<AdministrativeGenderEnum>(gender, true, out var result))
            {
                return result;
            }

            return AdministrativeGenderEnum.Unknown;

        }

        public async Task<SpecialtyEnum> GetSpecialtyIdAsync(string specialtyName)
        {
            // Check if the input is null or empty
            if (string.IsNullOrWhiteSpace(specialtyName))
            {
                // Return the ID of SpecialtyEnum.GeneralPractice
                return SpecialtyEnum.GeneralPractice;
            }

            // Normalize input
            var normalizedSpecialtyName = specialtyName.Trim().ToUpper();

            // Loop through Enum values
            foreach (SpecialtyEnum specialty in Enum.GetValues(typeof(SpecialtyEnum)))
            {
                // Get enum name and description
                var enumName = EnumHelpers.GetEnumName(specialty)?.ToUpper();
                var enumDescription = EnumHelpers.GetEnumDescription(specialty)?.ToUpper();

                // Check if either matches the normalized input
                if (enumName == normalizedSpecialtyName || enumDescription == normalizedSpecialtyName)
                {
                    return specialty;
                }
            }

            // Return the ID of SpecialtyEnum.GeneralPractice if no match is found
            return SpecialtyEnum.GeneralPractice;
        }

        private async Task<int?> GetInsuranceCardIdByIdentifierAsync(IUnitOfWork<int> unitOfWork, int clientId, string insuranceIdentifier)
        {
            if (string.IsNullOrWhiteSpace(insuranceIdentifier))
            {
                return null;
            }
            var insuranceCard = await unitOfWork.Repository<InsuranceCard>()
                .Entities
                .Specify(new GenericByClientIdSpecification<InsuranceCard>(clientId))
                .Specify(new GenericByDfExternalIdSpecification<InsuranceCard>(insuranceIdentifier))
                .FirstOrDefaultAsync();

            return insuranceCard?.Id;
        }

        private async Task<ClientAdjustmentCode> GetOrCreateDefaultClientAdjustmentCode(IUnitOfWork<int> unitOfWork, int clientId, string code, string name, string description, AdjustmentTypeEnum adjustmentTypeId)
        {
            var adjustmentCode = await unitOfWork.Repository<ClientAdjustmentCode>().Entities
                    .Specify(new GenericByClientIdSpecification<ClientAdjustmentCode>(clientId))
                    .FirstOrDefaultAsync(x => x.Code == code);

            //Create a default adjustmentCode if one does not exist. 
            if (adjustmentCode == null)
            {
                adjustmentCode = new ClientAdjustmentCode(clientId, code, name, description, adjustmentTypeId, null, null, null);
                adjustmentCode = await unitOfWork.Repository<ClientAdjustmentCode>().AddAsync(adjustmentCode);
                await unitOfWork.Commit(new CancellationToken());
            }

            return adjustmentCode;
        }


        private async Task<ClientCptCode> GetOrCreateDefaultClientCptCode(IUnitOfWork<int> unitOfWork, int clientId, string code, string name, string description)
        {
            var cptCode = await unitOfWork.Repository<ClientCptCode>().Entities
                    .Specify(new GenericByClientIdSpecification<ClientCptCode>(clientId))
                    .FirstOrDefaultAsync(x => x.Code == code);

            //Create a default adjustmentCode if one does not exist. 
            if (cptCode == null)
            {
                cptCode = new ClientCptCode() { ClientId= clientId, Code = code, LookupName = name, Description = description, ShortDescription = description };
                cptCode = await unitOfWork.Repository<ClientCptCode>().AddAsync(cptCode);
                await unitOfWork.Commit(new CancellationToken());
            }

            return cptCode;
        }

    }
}
