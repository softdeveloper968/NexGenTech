using System;
using System.Collections.Generic;
using System.Globalization;
using MedHelpAuthorizations.Shared.Helpers;

namespace MedHelpAuthorizations.Domain.CustomAttributes.CustomReport
{
    public class ClaimReportTypePreviewModel
    {
        private DateTime _myDateTime = DateTime.MinValue;

        // _claimStatusBatchClaim properties
        public string PatientLastName_ClaimStatusBatchClaim { get; set; }
        public string PatientFirstName_ClaimStatusBatchClaim { get; set; }
        public string PolicyNumber_ClaimStatusBatchClaim { get; set; }
        public DateTime DateOfServiceFrom_ClaimStatusBatchClaim
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public DateTime DateOfServiceTo_ClaimStatusBatchClaim
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public DateTime ClaimBilledOn_ClaimStatusBatchClaim
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public string ProcedureCode_ClaimStatusBatchClaim { get; set; }
        public string Modifiers_ClaimStatusBatchClaim { get; set; }
        public decimal BilledAmount_ClaimStatusBatchClaim { get; set; }
        public string ClaimNumber_ClaimStatusBatchClaim { get; set; }
        public DateTime CreatedOn_ClaimStatusBatchClaim
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }

        // _claimStatusTransaction properties
        public string LineItemControlNumber_ClaimStatusTransaction { get; set; }
        public string ClaimLineItemStatusValue_ClaimStatusTransaction { get; set; }
        public int ClaimStatusExceptionReasonCategoryId_ClaimStatusTransaction { get; set; }
        public string ExceptionReason_ClaimStatusTransaction { get; set; }
        public string ExceptionRemark_ClaimStatusTransaction { get; set; }
        public string RemarkCode_ClaimStatusTransaction { get; set; }
        public string ReasonCode_ClaimStatusTransaction { get; set; }
        public string ReasonDescription_ClaimStatusTransaction { get; set; }
        public string RemarkDescription_ClaimStatusTransaction { get; set; }
        public decimal TotalNonAllowedAmount_ClaimStatusTransaction { get; set; }
        public decimal TotalAllowedAmount_ClaimStatusTransaction { get; set; }
        public decimal DeductibleAmount_ClaimStatusTransaction { get; set; }
        public decimal CopayAmount_ClaimStatusTransaction { get; set; }
        public decimal CoinsuranceAmount_ClaimStatusTransaction { get; set; }
        public decimal CobAmount_ClaimStatusTransaction { get; set; }
        public decimal PenalityAmount_ClaimStatusTransaction { get; set; }
        public decimal LineItemPaidAmount_ClaimStatusTransaction { get; set; }
        public string CheckNumber_ClaimStatusTransaction { get; set; }
        public DateTime CheckDate_ClaimStatusTransaction
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public decimal CheckPaidAmount_ClaimStatusTransaction { get; set; }
        public string EligibilityInsurance_ClaimStatusTransaction { get; set; }
        public string EligibilityPolicyNumber_ClaimStatusTransaction { get; set; }
        public DateTime EligibilityFromDate_ClaimStatusTransaction
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public DateTime LastActiveEligibleDateRange_ClaimStatusTransaction
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public DateTime CobLastVerified_ClaimStatusTransaction
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
        public string PrimaryPayer_ClaimStatusTransaction { get; set; }
        public string PrimaryPolicyNumber_ClaimStatusTransaction { get; set; }
        public string EligibilityStatus_ClaimStatusTransaction { get; set; }
        public decimal WriteoffAmount_ClaimStatusTransaction { get; set; }
        public DateTime CreatedOn_ClaimStatusTransaction
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }

        // _claimStatusBatch properties
        public string BatchNumber_ClaimStatusBatch { get; set; }
        public DateTime CreatedOn_ClaimStatusBatch
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }

        // _clientInsurance properties
        public string LookupName_ClientInsurance { get; set; }
        public DateTime CreatedOn_ClientInsurance
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }

        // _provider properties
        public string Npi_Provider { get; set; }
        public string License_Provider { get; set; }
        public DateTime CreatedOn_Provider
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }

        // _clientLocation properties
        public string Name_ClientLocation { get; set; }
        public DateTime CreatedOn_ClientLocation
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }

        // _patient properties
        public DateTime CreatedOn_Patient
        {
            get { return _myDateTime; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString(ARAgingReporthelper._dateFormat)))
                {
                    _myDateTime = DateTime.MinValue;
                }
                else
                {
                    _myDateTime = DateTime.ParseExact(value.ToString(ARAgingReporthelper._dateFormat), ARAgingReporthelper._dateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
    }

    public class ColumnsToolTip
    {
        public string ColumnKey { get; set; }
        public string ColumnValue { get; set; }
    }
    public class UpdatedClaimReportTypePreviewModel
    {
        public List<Dictionary<string, object>> PreviewReportDetails { get; set; } = new();
        //public Dictionary<string, object> ColumnsWithToolTip { get; set; } = new();
        public List<ColumnsToolTip> ColumnWithToolTip { get; set; } = new();
        public string PreviewReportSQLQuery { get; set; } = string.Empty;
        public string ColumnsForSQLQuery { get; set; } = string.Empty;


    }
    public class PreviewReportResponseWithSQLQuery : UpdatedClaimReportTypePreviewModel
    {
        public PreviewReportResponseWithSQLQuery() { }
        public string PreviewReportSQLQuery { get; set; }
    }
}
