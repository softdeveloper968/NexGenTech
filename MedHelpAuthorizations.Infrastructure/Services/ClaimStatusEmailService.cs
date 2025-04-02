using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Repositories;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ClaimStatusEmailService : IClaimStatusEmailService
    {
        private IClaimStatusQueryService claimStatusQueryService;
        private IMailService mailService;
        private ICurrentUserService _currentUserService;
        private IUnitOfWork<int> _unitOfWork;
        private string _clientCode;
        private IClaimStatusBatchRepository _claimStatusBatchRepository;
        public ClaimStatusEmailService(IClaimStatusQueryService _claimStatusQueryService, IMailService _mailService, ICurrentUserService currentUserService, IUnitOfWork<int> unitOfWork, IClaimStatusBatchRepository claimStatusBatchRepository)
        {
            claimStatusQueryService = _claimStatusQueryService;
            mailService = _mailService;
            this._currentUserService = currentUserService;
            this._unitOfWork = unitOfWork;
            _claimStatusBatchRepository = claimStatusBatchRepository;
        }

        public async Task<string> Htmlemailtemplate(Dictionary<string, string> dictionary, int batchId, string rpaCode)
        {
            var client = await _unitOfWork.Repository<Domain.Entities.Client>()
                .GetByIdAsync(_currentUserService.ClientId);
            _clientCode = client.ClientCode;
            string content = GetEmailBodyTemplateForCompletedBatch();
            content = content.Replace("{BatchId}", batchId.ToString());
            content = content.Replace("{Bot}", rpaCode);
            content = content.Replace("{ClientName}", _clientCode);
            int index = dictionary.Count;
            foreach (var kvp in dictionary)
            {
                content = content.Replace($"Header-{index}", kvp.Key);
                content = content.Replace($"ColValue-{index}", kvp.Value.ToString());
                index--;
            }
            return content;
        }

        private string GetEmailBodyTemplateForCompletedBatch()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\BatchCompletedEmailTemplate.template");
        }

        public async Task GetClaimCategoryCountsByBatchIdAndSendEmail(int batchId, string rpaCode,string batchCreatedDate)
        {
            var totals = await claimStatusQueryService.GetClaimsStatusTotalsAsync(new ClaimStatusDashboardDenialsDetailQuery()
            {
                ClaimStatusBatchId = batchId
            });
            int paidAmountTotal = totals.ClaimStatusTransactionTotals.Where(x => x.ClaimLineItemStatus == "Paid").Sum(x => x.Quantity);
            int approvedAmountTotal = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == "Approved").Sum(x => x.Quantity);
            int rejectedTotal = totals.ClaimStatusTransactionTotals.Where(x => x.ClaimLineItemStatus == "Rejected")
                .Sum(x => x.Quantity);
            int nonAdjudicatedTotal = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.NotAdjudicated.ToString())
                .Sum(x => x.Quantity);
            int deniedTotal = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.Denied.ToString()).Sum(x => x.Quantity);
            int unMatchedProcuedureCode = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.UnMatchedProcedureCode.ToString())
                .Sum(x => x.Quantity);
            int pended = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.Pended.ToString()).Sum(x => x.Quantity);
            int unavailable = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.Unavailable.ToString())
                .Sum(x => x.Quantity);
            int notOnFile = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.NotOnFile.ToString())
                .Sum(x => x.Quantity);
            int memberNotFound = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.MemberNotFound.ToString())
                .Sum(x => x.Quantity);
            int ignored = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.Ignored.ToString()).Sum(x => x.Quantity);
            int zeroPay = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.ZeroPay.ToString()).Sum(x => x.Quantity);
            int bundled_fqhc = totals.ClaimStatusTransactionTotals
                .Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.BundledFqhc.ToString())
                .Sum(x => x.Quantity);
            int unProcessedTotal = totals.ClaimStatusUploadedTotals.Sum(x => x.Quantity) -
                                   totals.ClaimStatusTransactionTotals.Sum(x => x.Quantity);
            int errorTotal = totals.ClaimStatusTransactionTotals.Where(x => x.ClaimLineItemStatus == ClaimLineItemStatusEnum.Error.ToString()).Sum(x => x.Quantity);

            Dictionary<string, string> dataToEmail = new System.Collections.Generic.Dictionary<string, string>()
            {
                { ClaimLineItemStatusEnum.Paid.ToString(), paidAmountTotal.ToString() },
                { ClaimLineItemStatusEnum.Approved.ToString(), approvedAmountTotal.ToString() },
                { ClaimLineItemStatusEnum.BundledFqhc.ToString(), bundled_fqhc.ToString() },
                { ClaimLineItemStatusEnum.Denied.ToString(), deniedTotal.ToString() },
                { ClaimLineItemStatusEnum.Unavailable.ToString(), unavailable.ToString() },
                { ClaimLineItemStatusEnum.NotOnFile.ToString(), notOnFile.ToString() },
                { ClaimLineItemStatusEnum.MemberNotFound.ToString(), memberNotFound.ToString() },
                { ClaimLineItemStatusEnum.ZeroPay.ToString(), zeroPay.ToString() },
                { ClaimLineItemStatusEnum.Pended.ToString(), pended.ToString() },
                { ClaimLineItemStatusEnum.Ignored.ToString(), ignored.ToString() },
                { ClaimLineItemStatusEnum.NotAdjudicated.ToString(), nonAdjudicatedTotal.ToString() },
                { ClaimLineItemStatusEnum.UnMatchedProcedureCode.ToString(), unMatchedProcuedureCode.ToString() },
                {"In-Process",unProcessedTotal.ToString()},
                {ClaimLineItemStatusEnum.Error.ToString(), errorTotal.ToString() },
                {"BatchUploadedDate",batchCreatedDate}
            };

            string emailBody = await Htmlemailtemplate(dataToEmail, batchId, rpaCode);
            MailRequest request = new MailRequest()
            {
                Body = emailBody,
                From = "no-reply@automatedintegrationtechnologies.com",
                To = "mohit@automatedintegrationtechnologies.com,jamesnichols@automatedintegrationtechnologies.com,kmccaffery@automatedintegrationtechnologies.com",
                Subject = "AIT Completed Batch Notification Details"
            };
            await mailService.SendAsync(request);
        }
    }
}
