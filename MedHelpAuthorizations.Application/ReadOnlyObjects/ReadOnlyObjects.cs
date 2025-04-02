using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MedHelpAuthorizations.Domain.Entities.Enums;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;


namespace MedHelpAuthorizations.Application.ReadOnlyObjects
{
    public static class ReadOnlyObjects
    {

        private static readonly IList<RpaTypeEnum> _uiPathRpaTypes = new List<RpaTypeEnum>() { RpaTypeEnum.IcaNotes, RpaTypeEnum.ECW };

        private static readonly IList<TrendsPresetDateRangesEnum> _dailyTrendsPresetDateRanges = new List<TrendsPresetDateRangesEnum>() { TrendsPresetDateRangesEnum.Last30Days };
        private static readonly IList<TrendsPresetDateRangesEnum> _weeklyTrendsPresetDateRanges = new List<TrendsPresetDateRangesEnum>() { TrendsPresetDateRangesEnum.Last12Weeks };
        private static readonly IList<TrendsPresetDateRangesEnum> _monthlyTrendsPresetDateRanges = new List<TrendsPresetDateRangesEnum>() { TrendsPresetDateRangesEnum.LastTwoMonths, TrendsPresetDateRangesEnum.LastTwoMonthsToDate, TrendsPresetDateRangesEnum.Last12Months };
        private static readonly IList<TrendsPresetDateRangesEnum> _quarterlyTrendsPresetDateRanges = new List<TrendsPresetDateRangesEnum>() { TrendsPresetDateRangesEnum.LastEightQuarters };
        private static readonly IList<TrendsPresetDateRangesEnum> _yearlyTrendsPresetDateRanges = new List<TrendsPresetDateRangesEnum>() { TrendsPresetDateRangesEnum.LastFourYears };

        private static readonly IList<ClaimLineItemStatusEnum> _deniedClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.Denied,
                ClaimLineItemStatusEnum.MemberNotFound,
                ClaimLineItemStatusEnum.NotOnFile,
                ClaimLineItemStatusEnum.UnMatchedProcedureCode,
                ClaimLineItemStatusEnum.Rejected,
                ClaimLineItemStatusEnum.Ignored,
                ClaimLineItemStatusEnum.Returned,
                ClaimLineItemStatusEnum.CallPayer,
                ClaimLineItemStatusEnum.MemberNotEligible
            };
        private static readonly IList<ClaimLineItemStatusEnum> _paidApprovedClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.Paid,
                ClaimLineItemStatusEnum.Approved,
            };
        private static readonly IList<ClaimLineItemStatusEnum> _zeroPayBundledClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.BundledFqhc,
                ClaimLineItemStatusEnum.ZeroPay
            };
        private static readonly IList<ClaimLineItemStatusEnum> _allPaidClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.Paid,
                ClaimLineItemStatusEnum.Approved,
                ClaimLineItemStatusEnum.BundledFqhc,
                ClaimLineItemStatusEnum.ZeroPay
            };
        private static readonly IList<ClaimLineItemStatusEnum> _errorClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
        {
                ClaimLineItemStatusEnum.Error,
                ClaimLineItemStatusEnum.TransientError
        };
        private static IList<ClaimLineItemStatusEnum> _adjudicatedClaimLineItemStatuses
        {
            get
            {
                var adjudicatedStatuses = _allPaidClaimLineItemStatuses.ToList();
                adjudicatedStatuses.AddRange(_deniedClaimLineItemStatuses);
                adjudicatedStatuses.Add(ClaimLineItemStatusEnum.Writeoff);
                adjudicatedStatuses.Add(ClaimLineItemStatusEnum.Contractual);

                //TODO: Any more considered Adjudicated? 

                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(adjudicatedStatuses);
            }
        }


        private static readonly IList<ClaimLineItemStatusEnum> _openClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.NotAdjudicated,
                ClaimLineItemStatusEnum.Pended,
                ClaimLineItemStatusEnum.Unavailable,
                ClaimLineItemStatusEnum.Received,
                ClaimLineItemStatusEnum.NeedsReview,
                ClaimLineItemStatusEnum.Rebilled,
                ClaimLineItemStatusEnum.RetryMemberNotFound,
            };

        private static readonly IList<ClaimLineItemStatusEnum> _allClaimLineItemStatuses = Enum.GetValues(typeof(ClaimLineItemStatusEnum)).Cast<ClaimLineItemStatusEnum>().ToList();


        private static IList<ClaimLineItemStatusEnum> _otherClaimLineItemStatuses
        {
            // So far other will naturally contain at least voided, error, unknown, writeoff, contractual claims.
            // Any more that we do not properly add to AllPaid, Denied, or open lists will automatically be in OTHER
            get
            {
                var otherClaimLineItemStatuses = AllClaimLineItemStatuses.ToList();
                otherClaimLineItemStatuses.Except(AllPaidClaimLineItemStatuses);
                otherClaimLineItemStatuses.Except(AdjudicatedClaimLineItemStatuses);
                otherClaimLineItemStatuses.Except(OpenClaimLineItemStatuses);

                return otherClaimLineItemStatuses;
            }
        }

        private static readonly IList<ClaimLineItemStatusEnum> _initiallyReviewedByPayerLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.Unknown,
				//ClaimLineItemStatusEnum.Error,
				ClaimLineItemStatusEnum.Received,
                ClaimLineItemStatusEnum.NotAdjudicated,
                ClaimLineItemStatusEnum.Voided,
                ClaimLineItemStatusEnum.MemberNotFound,
                ClaimLineItemStatusEnum.Unavailable,
                ClaimLineItemStatusEnum.TransientError,
                ClaimLineItemStatusEnum.Ignored
            }
            .ToList();

        /// <summary>
        /// ARRaging Report excluding _allARExcludedClaimLineItemStatuses  TAPI-126.
        /// </summary>
        private static readonly IList<ClaimLineItemStatusEnum> _allARExcludedClaimLineItemStatuses = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.Paid,
                ClaimLineItemStatusEnum.Approved,
                ClaimLineItemStatusEnum.ZeroPay,
                ClaimLineItemStatusEnum.BundledFqhc,
                ClaimLineItemStatusEnum.Writeoff,
                ClaimLineItemStatusEnum.Contractual
            };

        //EN-81
        private static IList<ClaimLineItemStatusEnum> _contractualClaimLineItemStatuses
        {
            get
            {
                var contractualStatuses = _allPaidClaimLineItemStatuses.ToList();
                contractualStatuses.Add(ClaimLineItemStatusEnum.Contractual);

                return contractualStatuses;
            }
        }


        public static IReadOnlyCollection<RpaTypeEnum> UiPathRpaTypes
        {
            get
            {
                return new ReadOnlyCollection<RpaTypeEnum>(_uiPathRpaTypes);
            }
        }

        public static IReadOnlyCollection<TrendsPresetDateRangesEnum> DailyTrendsPresetDateRanges
        {
            get
            {
                return new ReadOnlyCollection<TrendsPresetDateRangesEnum>(_dailyTrendsPresetDateRanges);
            }
        }

        public static IReadOnlyCollection<TrendsPresetDateRangesEnum> WeeklyTrendsPresetDateRanges
        {
            get
            {
                return new ReadOnlyCollection<TrendsPresetDateRangesEnum>(_weeklyTrendsPresetDateRanges);
            }
        }

        public static IReadOnlyCollection<TrendsPresetDateRangesEnum> MonthlyTrendsPresetDateRanges
        {
            get
            {
                return new ReadOnlyCollection<TrendsPresetDateRangesEnum>(_monthlyTrendsPresetDateRanges);
            }
        }

        public static IReadOnlyCollection<TrendsPresetDateRangesEnum> QuarterlyTrendsPresetDateRanges
        {
            get
            {
                return new ReadOnlyCollection<TrendsPresetDateRangesEnum>(_quarterlyTrendsPresetDateRanges);
            }
        }

        public static IReadOnlyCollection<TrendsPresetDateRangesEnum> YearlyTrendsPresetDateRanges
        {
            get
            {
                return new ReadOnlyCollection<TrendsPresetDateRangesEnum>(_yearlyTrendsPresetDateRanges);
            }
        }

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> AllClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_allClaimLineItemStatuses);
            }
        }

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> DeniedClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_deniedClaimLineItemStatuses);
            }
        }
        public static IReadOnlyCollection<ClaimLineItemStatusEnum> PaidApprovedClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_paidApprovedClaimLineItemStatuses);
            }
        }
        public static IReadOnlyCollection<ClaimLineItemStatusEnum> AllPaidClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_allPaidClaimLineItemStatuses);
            }
        }
        public static IReadOnlyCollection<ClaimLineItemStatusEnum> ZeroPayBundledClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_zeroPayBundledClaimLineItemStatuses);
            }
        }
        public static IReadOnlyCollection<ClaimLineItemStatusEnum> OpenClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_openClaimLineItemStatuses);
            }
        }

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> AdjudicatedClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_adjudicatedClaimLineItemStatuses);
            }
        }

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> OtherClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_otherClaimLineItemStatuses);
            }
        }

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> InitiallyReviewedByPayerClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_initiallyReviewedByPayerLineItemStatuses);
            }
        }
        public static IReadOnlyCollection<ClaimLineItemStatusEnum> ErrorClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_errorClaimLineItemStatuses);
            }
        }
        public static IReadOnlyCollection<ClaimLineItemStatusEnum> AllARExcludedClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_allARExcludedClaimLineItemStatuses);
            }
        }

        //established procedure codes
        private static readonly IList<string> _establishedProcedureCodes = new List<string>()
        {
            "99211", "99212", "99213", "99214", "99215"
        };

        //established procedure codes
        private static readonly IList<string> _newProcedureCodes = new List<string>()
        {
            "99201", "99202", "99203", "99204", "99205"
        };

        public static IReadOnlyCollection<string> EstablishedProcedureCodes
        {
            get
            {
                return new ReadOnlyCollection<string>(_establishedProcedureCodes);
            }
        }
        
        public static IReadOnlyCollection<string> NewProcedureCodes
        {
            get
            {
                return new ReadOnlyCollection<string>(_newProcedureCodes);
            }
        }

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> ContractualClaimLineItemStatuses
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_contractualClaimLineItemStatuses);
            }
        }



        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> GetAllClaimStatusExceptionReasonCategoryEnum()
        {
            var AllClaimStatusExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>();
            foreach (var enumValue in Enum.GetValues<ClaimStatusExceptionReasonCategoryEnum>())
            {
                AllClaimStatusExceptionReasonCategoryEnum.Add(enumValue);
            }
            return AllClaimStatusExceptionReasonCategoryEnum;
        }

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> RegistrationManagerExceptionReasonCategorEnums = new List<ClaimStatusExceptionReasonCategoryEnum>()
        {
            ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue,
            ClaimStatusExceptionReasonCategoryEnum.WrongPayer,
            ClaimStatusExceptionReasonCategoryEnum.Duplicate,
            ClaimStatusExceptionReasonCategoryEnum.COB,
            ClaimStatusExceptionReasonCategoryEnum.PolicyNumber,
            ClaimStatusExceptionReasonCategoryEnum.TimelyFiling
        };

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> BillingManagerExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> RegistorExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>()
        {
            ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue,
            ClaimStatusExceptionReasonCategoryEnum.WrongPayer,
            ClaimStatusExceptionReasonCategoryEnum.Duplicate,
            ClaimStatusExceptionReasonCategoryEnum.COB,
            ClaimStatusExceptionReasonCategoryEnum.PolicyNumber,
            ClaimStatusExceptionReasonCategoryEnum.TimelyFiling,
            ClaimStatusExceptionReasonCategoryEnum.MedicareAdvCoverage,
            ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue
        };

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> MedicalAssistanceExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>()
        {
            ClaimStatusExceptionReasonCategoryEnum.CodingIssue,
            ClaimStatusExceptionReasonCategoryEnum.MRNeeded,
            ClaimStatusExceptionReasonCategoryEnum.Coverage
        };

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> CEOExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> CFOExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> COOExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> CIOExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> DOPFSExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();

        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> VicePresidentExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>()
        { ClaimStatusExceptionReasonCategoryEnum.CodingIssue};
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> MedicalDirectorExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>()
        {
            ClaimStatusExceptionReasonCategoryEnum.CodingIssue,
            ClaimStatusExceptionReasonCategoryEnum.MRNeeded
        };
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> BillingSupervisorExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> CashPostingManagerExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>() { };
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> BillerExceptionReasonCategoryEnum = GetAllClaimStatusExceptionReasonCategoryEnum();
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> CashPosterExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>() { };
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> ChargeEnrtyExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>() { };
        public static IReadOnlyCollection<ClaimStatusExceptionReasonCategoryEnum> InsuranceContractorExceptionReasonCategoryEnum = new List<ClaimStatusExceptionReasonCategoryEnum>()
        {
            ClaimStatusExceptionReasonCategoryEnum.Credentialing,
            ClaimStatusExceptionReasonCategoryEnum.ProviderType
        };

        public static IReadOnlyCollection<EmployeeRoleEnum> RegistrationEmployeeRoles = new List<EmployeeRoleEnum>()
        {
            EmployeeRoleEnum.CEO,
            EmployeeRoleEnum.CFO,
            EmployeeRoleEnum.COO,
            EmployeeRoleEnum.CIO,
            EmployeeRoleEnum.DirectorOfPatientFinancialServices,
            EmployeeRoleEnum.VicePresident,
            EmployeeRoleEnum.MedicalDirector,
            EmployeeRoleEnum.RegistrationManager,
            EmployeeRoleEnum.Registor
        };

        public static IReadOnlyCollection<EmployeeRoleEnum> ChargeEntryEmployeeRoles = new List<EmployeeRoleEnum>()
        {
            EmployeeRoleEnum.CEO,
            EmployeeRoleEnum.CFO,
            EmployeeRoleEnum.COO,
            EmployeeRoleEnum.CIO,
            EmployeeRoleEnum.DirectorOfPatientFinancialServices,
            EmployeeRoleEnum.VicePresident,
            EmployeeRoleEnum.ChargeEnrty,
            EmployeeRoleEnum.BillingManager
        };

        public static IReadOnlyCollection<EmployeeRoleEnum> CredentialingEmployeeRoles = new List<EmployeeRoleEnum>()
        {
            EmployeeRoleEnum.CEO,
            EmployeeRoleEnum.CFO,
            EmployeeRoleEnum.COO,
            EmployeeRoleEnum.CIO,
            EmployeeRoleEnum.DirectorOfPatientFinancialServices,
            EmployeeRoleEnum.VicePresident,
            EmployeeRoleEnum.BillingManager,
            EmployeeRoleEnum.InsuranceContractor
        };

        public static IReadOnlyCollection<EmployeeRoleEnum> MedicalEmployeeRoles = new List<EmployeeRoleEnum>() {
             EmployeeRoleEnum.CEO,
             EmployeeRoleEnum.CFO,
             EmployeeRoleEnum.COO,
             EmployeeRoleEnum.CIO,
             EmployeeRoleEnum.DirectorOfPatientFinancialServices,
             EmployeeRoleEnum.VicePresident,
             EmployeeRoleEnum.MedicalDirector,
            EmployeeRoleEnum.MedicalAssistance
        };

        public static IReadOnlyCollection<EmployeeRoleEnum> BillingEmployeeRoles = new List<EmployeeRoleEnum>() {
             EmployeeRoleEnum.CEO,
             EmployeeRoleEnum.CFO,
             EmployeeRoleEnum.COO,
             EmployeeRoleEnum.CIO,
             EmployeeRoleEnum.DirectorOfPatientFinancialServices,
             EmployeeRoleEnum.VicePresident,
             EmployeeRoleEnum.BillingManager,
             EmployeeRoleEnum.BillingSupervisor,
             EmployeeRoleEnum.Biller
        };

        public static IReadOnlyCollection<EmployeeRoleEnum> CashPostingEmployeeRoles = new List<EmployeeRoleEnum>()
        {
             EmployeeRoleEnum.CEO,
             EmployeeRoleEnum.CFO,
             EmployeeRoleEnum.COO,
             EmployeeRoleEnum.CIO,
             EmployeeRoleEnum.DirectorOfPatientFinancialServices,
             EmployeeRoleEnum.VicePresident,
             EmployeeRoleEnum.BillingManager,
             EmployeeRoleEnum.CashPostingManager,
             EmployeeRoleEnum.CashPoster
        };


        private static readonly IList<ClaimLineItemStatusEnum> _unavailableOrNotOnFile = new List<ClaimLineItemStatusEnum>()
            {
                ClaimLineItemStatusEnum.Unavailable,
                ClaimLineItemStatusEnum.NotOnFile
            };

        public static IReadOnlyCollection<ClaimLineItemStatusEnum> UnavailableOrNotOnFile
        {
            get
            {
                return new ReadOnlyCollection<ClaimLineItemStatusEnum>(_unavailableOrNotOnFile);
            }
        }

        public static string GetDelimitedLineItemStatusesFromFlattenedName(string flattenedLineItemStatus)
        {

            //TODO: Match the refactored ReadOnly lists better
            switch (flattenedLineItemStatus)
            {
                case "Other":
                    return string.Join(",", ReadOnlyObjects.OtherClaimLineItemStatuses.Cast<int>());

                case "Paid/Approved":
                    return string.Join(",", ReadOnlyObjects.PaidApprovedClaimLineItemStatuses.Cast<int>()); //1,2,15

                case "Paid/Approved/ZeroPay":
                    return string.Join(",", ReadOnlyObjects.AllPaidClaimLineItemStatuses.Cast<int>());

                case "Not-Adjudicated":
                    return string.Join(",", ReadOnlyObjects.OpenClaimLineItemStatuses.Cast<int>());

                case "Denied":
                    return string.Join(",", ReadOnlyObjects.DeniedClaimLineItemStatuses.Cast<int>());

                case "WriteOff":
                    return ((int)ClaimLineItemStatusEnum.Writeoff).ToString();
                    
                case "Contractual":
                    return ((int)ClaimLineItemStatusEnum.Contractual).ToString();

                //case "Zero Pay":
                //    return $"{(int)ClaimLineItemStatusEnum.ZeroPay}";

                case "In-Process":
                    //Batch Claims without a transaction (ClaimstatusTransactionId is null) OR the claim has a transaction but the Status is error or transient error
                    return string.Join(",", ReadOnlyObjects.ErrorClaimLineItemStatuses.Cast<int>());

                default:
                    return null;
            }
        }

    }
}
