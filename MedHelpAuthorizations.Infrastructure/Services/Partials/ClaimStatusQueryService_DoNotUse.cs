using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.QueryableModels;
using MedHelpAuthorizations.Client.Shared.Models.ChartObjects.Trends;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Shared.Models.IntegratedServices.ChartObjects.Trends;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static MudBlazor.CategoryTypes;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ClaimStatusQueryService
    {
        //    private IQueryable<ClaimStatusBatchClaim> GetInitialUploadedClaimsTotalsQueryable(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        var initialUploadedTotals = _claimStatusBatchClaimsRepository.ClaimStatusBatchClaims
        //            .Include(c => c.ClaimStatusBatch)
        //            .ThenInclude(b => b.AuthType)
        //            .Include(c => c.ClaimStatusBatch.ClientInsurance)
        //            .OrderBy(c => c.CreatedOn)
        //            .DistinctBy(c => c.EntryMd5Hash).ToList().AsQueryable()
        //            .GroupBy(x => x.EntryMd5Hash)
        //            .Select(g => g.First())
        //            .Specify<ClaimStatusBatchClaim>(new ClaimStatusBatchClaimDashboardInitialFilterSpecification(filters, _currentUserService.ClientId));

        //        return initialUploadedTotals;
        //    }

        //    private IQueryable<IGrouping<GroupInsLookupNameProcedureCode, ClaimStatusBatchClaim>> GetInitialUploadedClaimsTotalsGroupQueryable(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        var initialUploadedTotals = GetInitialUploadedClaimsTotalsQueryable(filters)
        //            .GroupBy(g => new GroupInsLookupNameProcedureCode()
        //            {
        //                LookupName = g.ClaimStatusBatch.ClientInsurance.LookupName,
        //                ProcedureCode = g.ProcedureCode
        //            });

        //            return initialUploadedTotals;
        //    }

        //    private IQueryable<ClaimStatusTransaction> GetInitialTransactionsQueryable(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        try
        //        {
        //            var initalBatchClaimsQuery = GetInitialUploadedClaimsTotalsQueryable(filters);

        //            var initialClaimStatusTransactions = _unitOfWork.Repository<ClaimStatusTransaction>().Entities
        //                .Specify(new ClaimStatusTransactionDashboardInitialFilterSpecification(filters,
        //                    _currentUserService.ClientId))
        //                .Join(initalBatchClaimsQuery, trn => trn, clm => clm.ClaimStatusTransaction,
        //                    (transaction, claim) => new ClaimStatusTransaction());

        //            return initialClaimStatusTransactions;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }

        //    private IQueryable<IGrouping<GroupInsLookupNameProcedureCodeLineItemStatus, ClaimStatusTransaction>> GetInitialTransactionsTotalsGroupQueryable(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        try
        //        {
        //            var initialClaimStatusTransactionTotals = GetInitialTransactionsQueryable(filters)
        //                .GroupBy(t => new GroupInsLookupNameProcedureCodeLineItemStatus()
        //                {
        //                    LookupName = t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance.LookupName,
        //                    ClaimLineItemStatusId = t.ClaimLineItemStatusId,
        //                    ProcedureCode = t.ClaimStatusBatchClaim.ProcedureCode
        //                });

        //            return initialClaimStatusTransactionTotals;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }

        //    private IQueryable<ClaimStatusTransaction> GetInitialDenialQueryable(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        try
        //        {
        //            var initalBatchClaimsQuery = GetInitialUploadedClaimsTotalsQueryable(filters);

        //            var initialClaimStatusTransactions = _unitOfWork.Repository<ClaimStatusTransaction>().Entities
        //               .Specify(new ClaimStatusTransactionDashboardInitialFilterSpecification(filters,
        //                    _currentUserService.ClientId))
        //                .Specify(new ClaimStatusDenialsFilterSpecification())
        //                .Join(initalBatchClaimsQuery, trn => trn, clm => clm.ClaimStatusTransaction,
        //                    (transaction, claim) => new ClaimStatusTransaction());

        //            return initialClaimStatusTransactions;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }

        //    public IQueryable<IGrouping<GroupInsLookupNameProcedureCodeLineItemStatusDenialCategory, ClaimStatusTransaction>> GetInitialDenialTotalsGroupQueryable(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        try
        //        {
        //            var initialClaimStatusDenialTotals = GetInitialDenialQueryable(filters)
        //                .GroupBy(t => new GroupInsLookupNameProcedureCodeLineItemStatusDenialCategory()
        //                {
        //                    LookupName = t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance.LookupName,
        //                    ClaimLineItemStatusId = t.ClaimLineItemStatusId,
        //                    ProcedureCode = t.ClaimStatusBatchClaim.ProcedureCode,
        //                    ClaimStatusExceptionReasonCategory = t.ClaimStatusExceptionReasonCategory.Description
        //                });

        //            return initialClaimStatusDenialTotals;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }


        //    public Task<List<ClaimStatusTotal>> GetInitialUploadedTotals(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        var claimsUploadedTotals = GetInitialUploadedClaimsTotalsGroupQueryable(filters)
        //            .Select(c => new ClaimStatusTotal()
        //            {
        //                ClientInsuranceName = c.Key.LookupName,
        //                ClaimLineItemStatus = "Received/Upload",
        //                ProcedureCode = c.Key.ProcedureCode,
        //                Quantity = c.Count(),
        //                ChargedSum = c.Sum(cs => cs.BilledAmount ?? 0.00m),
        //                AllowedAmountSum = 0.00m,
        //                NonAllowedAmountSum = 0.00m
        //            })
        //            .ToList();

        //        return Task.FromResult<List<ClaimStatusTotal>>(claimsUploadedTotals);
        //    }

        //    public Task<List<ClaimStatusTotal>> GetInitialTransactionTotals(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        var transactionTotals = GetInitialTransactionsTotalsGroupQueryable(filters)
        //            .Select(t => new ClaimStatusTotal()
        //            {
        //                ClientInsuranceName = t.Key.LookupName,
        //                ProcedureCode = t.Key.ProcedureCode,
        //                Quantity = t.Count(),
        //                ChargedSum = t.Sum(cs => cs.ClaimStatusBatchClaim.BilledAmount ?? 0.00m),
        //                PaidAmountSum = t.Sum(ps => ps.LineItemPaidAmount ?? 0.00m),
        //                AllowedAmountSum = t.Sum(a => a.TotalAllowedAmount ?? 0.00m),
        //                NonAllowedAmountSum = t.Sum(a => a.TotalNonAllowedAmount ?? 0.00m)
        //            })
        //            .ToList();

        //        return Task.FromResult(transactionTotals);
        //    }

        //    public Task<List<ClaimStatusTotal>> GetInitialDenialTotals(IClaimStatusDashboardDataInitialQuery filters)
        //    {
        //        var denialReasonTotals = GetInitialDenialTotalsGroupQueryable(filters)
        //            .Select(t => new ClaimStatusTotal()
        //            {
        //                ClientInsuranceName = t.Key.LookupName,
        //                ClaimStatusExceptionReasonCategory = t.Key.ClaimStatusExceptionReasonCategory,
        //                ProcedureCode = t.Key.ProcedureCode,
        //                Quantity = t.Count(),
        //                ChargedSum = t.Sum(cs => cs.ClaimStatusBatchClaim.BilledAmount ?? 0.00m),
        //                PaidAmountSum = t.Sum(ps => ps.LineItemPaidAmount ?? 0.00m),
        //                AllowedAmountSum = t.Sum(a => a.TotalAllowedAmount ?? 0.00m),
        //                NonAllowedAmountSum = t.Sum(a => a.TotalNonAllowedAmount ?? 0.00m)
        //            }).ToList();

        //        return Task.FromResult(denialReasonTotals);
        //    }
        //}
    }

}