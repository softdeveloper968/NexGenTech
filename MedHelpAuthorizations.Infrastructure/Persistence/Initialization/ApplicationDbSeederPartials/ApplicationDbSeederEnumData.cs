using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using MedHelpAuthorizations.Shared.Extensions;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Domain.CustomAttributes;
using System.Reflection;
using MudBlazor.Extensions;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    public partial class ApplicationDbSeeder
    {
        private async Task SeedAuthorizationStatuses()
        {
            List<AuthorizationStatus> authorizationStatuses = new List<AuthorizationStatus>()
            {
                new AuthorizationStatus(AuthorizationStatusEnum.ClientRequestAdded, "ClientRequestAdded", "Auth requested"),
                new AuthorizationStatus(AuthorizationStatusEnum.InformationNeeded, "InformationNeeded", "Questionnaire not complete or other info required"),
                new AuthorizationStatus(AuthorizationStatusEnum.NurseReview, "NurseReview", "Nurse to review questionnaire for completeness"),
                new AuthorizationStatus(AuthorizationStatusEnum.RFR, "RFR", "Request ready for robot"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthApproved, "AuthApproved", "Auth approved on insurance website"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthPended, "AuthPended", "Auth pending by insurance company"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthDischarged, "AuthDischarged", "Auth has been discharged in Insurance website"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthDenied, "AuthDenied", "Auth denied by insurance company"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthExpiring, "AuthExpiring", "Auth end date in the next 30 days or less"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthConcurrent, "AuthConcurrent", "New auth request for this service type"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthExpired, "AuthExpired", "Auth end date is past the current date"),
                new AuthorizationStatus(AuthorizationStatusEnum.AuthInProcess, "AuthInProcess", "In-Process"),
            };
            await Task.Run(async () =>
            {
                foreach (var authstatus in authorizationStatuses)
                {
                    if (!_dbContext.AuthorizationStatuses.Any(a => a.Id == authstatus.Id))
                    {
                        _dbContext.AuthorizationStatuses.Add(authstatus);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded AuthorizationStatuses");
        }

        private async Task SeedAdministrativeGenders()
        {
            List<AdministrativeGender> administrativeGenders = new List<AdministrativeGender>()
            {
                new AdministrativeGender(AdministrativeGenderEnum.Unknown, "Unknown"),
                new AdministrativeGender(AdministrativeGenderEnum.Male, "Male"),
                new AdministrativeGender(AdministrativeGenderEnum.Female, "Female"),
                new AdministrativeGender(AdministrativeGenderEnum.Undifferentiated, "Undifferentiated"),
            };
            await Task.Run(async () =>
            {
                foreach (var gender in administrativeGenders)
                {
                    if (!_dbContext.AdministrativeGenders.Any(a => a.Id == gender.Id))
                    {
                        _dbContext.AdministrativeGenders.Add(gender);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded AdministrativeGenders");
        }

        private async Task SeedDbOperationEnums()
        {
            List<DbOperation> dbOperations = new List<DbOperation>()
            {
                new DbOperation(DbOperationEnum.Insert, "Insert"),
                new DbOperation(DbOperationEnum.Update, "Update"),
            };
            await Task.Run(async () =>
            {
                foreach (var dbOp in dbOperations)
                {
                    if (!_dbContext.DbOperations.Any(o => o.Id == dbOp.Id))
                    {
                        _dbContext.DbOperations.Add(dbOp);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded DbOperations");
        }

        private async Task SeedStates()
        {
            List<State> states = new List<State>()
            {
                new State(StateEnum.UNK, "UNK", "Unknown"),
                new State(StateEnum.AL, "AL", "Alabama"),
                new State(StateEnum.AK, "AK", "Alaska"),
                new State(StateEnum.AZ, "AZ", "Arizona"),
                new State(StateEnum.AR, "AR", "Arkansas"),
                new State(StateEnum.CA, "CA", "California"),
                new State(StateEnum.CO, "CO", "Colorado"),
                new State(StateEnum.CT, "CT", "Connecticut"),
                new State(StateEnum.DE, "DE", "Delaware"),
                new State(StateEnum.FL, "FL", "Florida"),
                new State(StateEnum.GA, "GA", "Georgia"),
                new State(StateEnum.HI, "HI", "Hawaii"),
                new State(StateEnum.IA, "IA", "Iowa"),
                new State(StateEnum.ID, "ID", "Idaho"),
                new State(StateEnum.IL, "IL", "Illinois"),
                new State(StateEnum.IN, "IN", "Indiana"),
                new State(StateEnum.KS, "KS", "Kansas"),
                new State(StateEnum.KY, "KY", "Kentucky"),
                new State(StateEnum.LA, "LA", "Louisiana"),
                new State(StateEnum.MA, "MA", "Massachusetts"),
                new State(StateEnum.MD, "MD", "Maryland"),
                new State(StateEnum.ME, "ME", "Maine"),
                new State(StateEnum.MI, "MI", "Michigan"),
                new State(StateEnum.MN, "MN", "Minnesota"),
                new State(StateEnum.MO, "MO", "Missouri"),
                new State(StateEnum.MS, "MS", "Mississippi"),
                new State(StateEnum.MT, "MT", "Montana"),
                new State(StateEnum.NC, "NC", "North Carolina"),
                new State(StateEnum.ND, "ND", "North Dakota"),
                new State(StateEnum.NE, "NE", "Nebraska"),
                new State(StateEnum.NH, "NH", "New Hampshire"),
                new State(StateEnum.NJ, "NJ", "New Jersey"),
                new State(StateEnum.NM, "NM", "New Mexico"),
                new State(StateEnum.NV, "NV", "Nevada"),
                new State(StateEnum.NY, "NY", "New York"),
                new State(StateEnum.OK, "OK", "Oklahoma"),
                new State(StateEnum.OH, "OH", "Ohio"),
                new State(StateEnum.OR, "OR", "Oregon"),
                new State(StateEnum.PA, "PA", "Pennsylvania"),
                new State(StateEnum.RI, "RI", "Rhode Island"),
                new State(StateEnum.SC, "SC", "South Carolina"),
                new State(StateEnum.SD, "SD", "South Dakota"),
                new State(StateEnum.TN, "TN", "Tennessee"),
                new State(StateEnum.TX, "TX", "Texas"),
                new State(StateEnum.UT, "UT", "Utah"),
                new State(StateEnum.VA, "VA", "Virginia"),
                new State(StateEnum.VT, "VT", "Vermont"),
                new State(StateEnum.WA, "WA", "Washington"),
                new State(StateEnum.WI, "WI", "Wisconsin"),
                new State(StateEnum.WV, "WV", "West Virginia"),
                new State(StateEnum.WY, "WY", "Wyoming"),
                new State(StateEnum.DC, "DC", "D.C"),
            };
            await Task.Run(async () =>
            {
                foreach (var state in states)
                {
                    if (!_dbContext.States.Any(a => a.Id == state.Id))
                    {
                        _dbContext.States.Add(state);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded States");
        }

        private async Task SeedTypesOfService()
        {
            List<TypeOfService> typesOfService = new List<TypeOfService>()
            {
                //new TypeOfService(TypeOfServiceEnum.WholeBlood, "WholeBlood", "Whole Blood", "0"),
                // TODO: Add all the rest

            };
            await Task.Run(async () =>
            {
                foreach (var tos in typesOfService)
                {
                    if (!_dbContext.TypesOfService.Any(a => a.Id == tos.Id))
                    {
                        _dbContext.TypesOfService.Add(tos);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded TypesOfService");
        }

        private async Task SeedAddressTypes()
        {
            List<AddressType> addressTypes = new List<AddressType>()
            {
                new AddressType(AddressTypeEnum.Residential, "Residential"),
                new AddressType(AddressTypeEnum.Commercial, "Commercial"),
            };
            await Task.Run(async () =>
            {
                foreach (var at in addressTypes)
                {
                    if (!_dbContext.AddressTypes.Any(a => a.Id == at.Id))
                    {
                        _dbContext.AddressTypes.Add(at);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded AddressTypes");
        }


        private async Task SeedPlaceOfServiceCodes()
        {
            try
            {
                List<PlaceOfServiceCode> placeOfServiceCodes = new List<PlaceOfServiceCode>()
                {
                    new PlaceOfServiceCode(PlaceOfServiceCodeEnum.Pharmacy, "Pharmacy", "Pharmacy"),
                    // TODO: Add all the rest
               
                };
                await Task.Run(async () =>
                {
                    //foreach (var pos in placeOfServiceCodes)
                    //{
                    //    if (!_dbContext.PlaceOfServiceCodes.Any(a => a.Id == pos.Id))
                    //    {
                    //        _dbContext.PlaceOfServiceCodes.Add(pos);
                    //    }
                    //}
                    foreach (var enumValue in Enum.GetValues<PlaceOfServiceCodeEnum>())
                    {
                        //If already exist
                        //if get PlaceOfServiceCode by Id (enum)==null then add
                        var posCodeExists = _dbContext.PlaceOfServiceCodes.FirstOrDefault(z => z.Id == enumValue) ?? null;
                        if (posCodeExists is null)
                        {
                            _dbContext.PlaceOfServiceCodes.Add(new PlaceOfServiceCode
                            {
                                Id = enumValue,
                                Name = enumValue.ToString(),
                                LookupName = enumValue.ToString(),
                            });
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                //Squelch
            }

            _logger.LogInformation("Seeded PlaceOfServiceCodes");
        }
         
        private async Task SeedEmployeeRoleClaimStatusExceptionReasonCategories()
        {
            Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<EmployeeRoleEnum>())
                {
                    var roleAssociatedExceptionReasonCategories = GetClaimStatusExceptionReasonCategoryEnumsByRoleId(enumValue);
                    foreach (var erc in roleAssociatedExceptionReasonCategories)
                    {
                        var EmployeeRoleClaimStatusExceptionReasonCategoryExist = _dbContext.EmployeeRoleClaimStatusExceptionReasonCategories.FirstOrDefault(z => z.EmployeeRoleId == enumValue && z.ClaimStatusExceptionReasonCategoryId == erc) ?? null;
                        if (EmployeeRoleClaimStatusExceptionReasonCategoryExist is null)
                        {
                            _dbContext.EmployeeRoleClaimStatusExceptionReasonCategories.Add(new EmployeeRoleClaimStatusExceptionReasonCategory
                            {
                                EmployeeRoleId = enumValue,
                                ClaimStatusExceptionReasonCategoryId = erc
                            });
                        }

                    }

                }

            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default ReportCategories");
        }

        //AA-298 For seeding TypeOfService
        private async Task SeedTypeOfServiceEnums()
        {
            Task.Run(async () =>
            {
                var typeOfServices = new List<TypeOfService>();
                foreach (TypeOfServiceEnum serviceEnum in Enum.GetValues(typeof(TypeOfServiceEnum)))
                {
                    // Get the ServiceTypeAttribute associated with the enum value
                    var memberInfo = typeof(TypeOfServiceEnum).GetMember(serviceEnum.ToString()).FirstOrDefault();
                    var serviceTypeAttribute = memberInfo?.GetCustomAttribute<ServiceTypeAttribute>();

                    if (serviceTypeAttribute != null)
                    {
                        // Check if a TypeOfService entity with the same enum value exists in the database
                        var typeOfServiceExist = _dbContext.TypesOfService.FirstOrDefault(z => z.Id == (TypeOfServiceEnum)(int)serviceEnum) ?? null;

                        if (typeOfServiceExist is null)
                        {
                            // Create a new TypeOfService entity based on the enum and ServiceTypeAttribute
                            var typeOfService = new TypeOfService()
                            {
                                Id = (TypeOfServiceEnum)(int)serviceEnum,
                                Code = serviceTypeAttribute.Code,
                                Name = serviceTypeAttribute.Name,
                                Description = serviceTypeAttribute.Description,
                                // Parse the StartDate from the attribute, or set it to null if it's not a valid date
                                StartDate = DateTime.TryParse(serviceTypeAttribute.StartDate, out var startDate) ? startDate : (DateTime?)null,
                            };
                            _dbContext.TypesOfService.Add(typeOfService);
                        }
                        else
                        {
                            // Update the existing TypeOfService entity based on the enum and ServiceTypeAttribute
                            typeOfServiceExist.Code = serviceTypeAttribute.Code;
                            typeOfServiceExist.Name = serviceTypeAttribute.Name;
                            typeOfServiceExist.Description = serviceTypeAttribute.Description;
                            // Parse the StartDate from the attribute, or set it to null if it's not a valid date
                            typeOfServiceExist.StartDate = DateTime.TryParse(serviceTypeAttribute.StartDate, out var startDate) ? startDate : (DateTime?)null;
                        }
                    }
                }
            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default TypeOfServiceEnum");
        }

        //AA-231
        //private async Task SeedWriteOffTypeEnums()
        //{
        //    Task.Run(async () =>
        //    {
        //        foreach (var enumValue in Enum.GetValues<WriteOffTypeEnum>())
        //        {
        //            var existingWriteOffType = _dbContext.WriteOffTypes.FirstOrDefault(z => z.Id == enumValue) ?? null;
        //            if (existingWriteOffType is null)
        //            {
        //                _dbContext.WriteOffTypes.Add(new WriteOffType
        //                {
        //                    Id = enumValue,
        //                    Name = enumValue.ToString(),
        //                    Description = enumValue.GetDescription(),
        //                });
        //            }
        //        }
        //    }).GetAwaiter().GetResult();
        //    _logger.LogInformation("Seeded default WriteOffTypeEnums");
        //}

        private async Task SeedAlphaSplitEnums()
        {
            Task.Run(async () =>
            {
                if (!await _dbContext.AlphaSplits.AnyAsync())
                {
                    foreach (var enumValue in Enum.GetValues<AlphaSplitEnum>())
                    {
                        if (enumValue == AlphaSplitEnum.CustomRange)
                        {
                            _dbContext.AlphaSplits.Add(new AlphaSplit
                            {
                                Id = enumValue,
                                Name = enumValue.ToString(),
                                Description = enumValue.GetDescription(),
                                Code = enumValue.ToString(),
                                BeginAlpha = null,
                                EndAlpha = null
                            });
                        }
                        if (enumValue == AlphaSplitEnum.AtoG)
                        {
                            _dbContext.AlphaSplits.Add(new AlphaSplit
                            {
                                Id = enumValue,
                                Name = enumValue.ToString(),
                                Description = enumValue.GetDescription(),
                                Code = enumValue.ToString(),
                                BeginAlpha = "A",
                                EndAlpha = "Z"
                            });
                        }
                        if (enumValue == AlphaSplitEnum.HtoL)
                        {
                            _dbContext.AlphaSplits.Add(new AlphaSplit
                            {
                                Id = enumValue,
                                Name = enumValue.ToString(),
                                Description = enumValue.GetDescription(),
                                Code = enumValue.ToString(),
                                BeginAlpha = "H",
                                EndAlpha = "L"
                            });
                        }
                        if (enumValue == AlphaSplitEnum.MtoR)
                        {
                            _dbContext.AlphaSplits.Add(new AlphaSplit
                            {
                                Id = enumValue,
                                Name = enumValue.ToString(),
                                Description = enumValue.GetDescription(),
                                Code = enumValue.ToString(),
                                BeginAlpha = "M",
                                EndAlpha = "R"
                            });
                        }
                        if (enumValue == AlphaSplitEnum.StoZ)
                        {
                            _dbContext.AlphaSplits.Add(new AlphaSplit
                            {
                                Id = enumValue,
                                Name = enumValue.ToString(),
                                Description = enumValue.GetDescription(),
                                Code = enumValue.ToString(),
                                BeginAlpha = "S",
                                EndAlpha = "Z"
                            });
                        }
                    }
                }

            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default EmployeeRoles");
        }

        private async Task SeedEmployeeRoleDepartments()
        {
            Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<DepartmentEnum>())
                {
                    var departmentAssociatedRoles = GetRolesByDepartment(enumValue);
                    foreach (var dr in departmentAssociatedRoles)
                    {
                        var EmployeeRoleDepartmentExist = _dbContext.EmployeeRoleDepartments.FirstOrDefault(z => z.DepartmentId == enumValue && z.EmployeeRoleId == dr) ?? null;
                        if (EmployeeRoleDepartmentExist is null)
                        {
                            _dbContext.EmployeeRoleDepartments.Add(new EmployeeRoleDepartment
                            {
                                EmployeeRoleId = dr,
                                DepartmentId = enumValue
                            });
                        }
                    }
                }
            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default EmployeeRoleDepartments");
        }


        private async Task SeedDefaultReportsEnum()
        {
            Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<ReportsEnum>())
                {
                    //If already exist
                    //if get reports by Id (enum)==null then add
                    var reportExist = _dbContext.Report.FirstOrDefault(z => z.Id == enumValue) ?? null;
                    if (reportExist is null)
                    {
                        _dbContext.Report.Add(new Report
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription(),
                            Code = enumValue.GetDescription(),
                            ReportCategoryId = GetReportCategory(enumValue)
                        });
                    }
                }
            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default Reports");
        }

        private async Task SeedDefaultReportCategoryEnum()
        {
            Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<ReportCategoryEnum>())
                {
                    var reportcategoryExist = _dbContext.ReportCategories.FirstOrDefault(z => z.Id == enumValue) ?? null;
                    if (reportcategoryExist is null)
                    {
                        _dbContext.ReportCategories.Add(new ReportCategories
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription(),
                            Code = enumValue.GetDescription(),
                        });
                    }
                }

            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default ReportCategories");
        }

        private async Task SeedDefaultEmployeeRoleEnum()
        {
            Task.Run(async () =>
            {
                if (!await _dbContext.EmployeeRoles.AnyAsync())
                {
                    foreach (var enumValue in Enum.GetValues<EmployeeRoleEnum>())
                    {
                        _dbContext.EmployeeRoles.Add(new EmployeeRole
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription()
                        });
                    }
                }

            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default EmployeeRoles");
        }

        private async Task SeedClaimLineItemStatuses()
        {
            List<ClaimLineItemStatus> claimLineItemStatuses = new List<ClaimLineItemStatus>()
            {
                    // Param order : (Id, Code, Description, DaysWait, MaxPipeline, MinResolution, MaxResolution, Rank)
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Unknown, "Unknown", "Unknown", 1, 10, 30, 100, 1, ClaimStatusTypeEnum.OtherOpenClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Paid, "Paid", "Paid", 0, 0, 0, 0, 23, ClaimStatusTypeEnum.PaidClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Approved, "Approved", "Approved", 1, 14, 4, 14, 18, ClaimStatusTypeEnum.PaidClaimStatusType), //initial Wait of 6
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Rejected, "Rejected", "Rejected", 0, 0, 0, 0, 9, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Voided, "Voided", "Voided", 0, 0, 0, 0, 10, ClaimStatusTypeEnum.OtherOpenClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Received, "Received", "Received", 1, 14, 4, 14, 15, ClaimStatusTypeEnum.OpenClaimStatusType), // Has not been used yet. Candidate for replacement. 
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.NotAdjudicated, "NotAdjudicated", "Not-Adjudicated", 1, 20, 4, 20, 11, ClaimStatusTypeEnum.OpenClaimStatusType), //initial Wait of 2
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Denied, "Denied", "Denied", 10, 60, 3, 6, 17, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Pended, "Pended", "Pended", 2, 20, 4, 20, 14, ClaimStatusTypeEnum.OpenClaimStatusType), //initial Wait of 5
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.UnMatchedProcedureCode, "UnMatchedProcedureCode", "UnMatched-ProcedureCode", 10, 14, 2, 100, 8, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Error, "Error", "Error / Exception", 0, 10, 4, 21, 3, null),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Unavailable, "Unavailable", "Unavailable For Review", 1, 20, 4, 20, 7, ClaimStatusTypeEnum.OpenClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.MemberNotFound, "MemberNotFound", "Member Not Found", 5, 16, 2, 100, 5, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Ignored, "Ignored", "Ignored", 0, 0, 0, 0, 4, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.ZeroPay, "ZeroPay", "Zero Pay", 0, 0, 0, 0, 24, ClaimStatusTypeEnum.PaidClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.BundledFqhc, "BundledFqhc", "Bundled Fqhc", 0, 0, 0, 0, 21, ClaimStatusTypeEnum.PaidClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.NeedsReview, "NeedsReview", "Needs Review", 0, 0, 0, 0, 12, ClaimStatusTypeEnum.OpenClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.TransientError, "TransientError", "Transient Error", 0, 99, 10, 99, 2, null),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.CallPayer, "CallPayer", "Call Payer", 0, 0, 0, 0, 13, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Returned, "Returned", "Returned", 0, 0, 0, 0, 19, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Writeoff, "Writeoff", "Write-off", 0, 0, 0, 0, 20, ClaimStatusTypeEnum.OtherAdjudicatedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.Rebilled, "Rebilled", "Rebilled", 1, 14, 4, 14, 4, ClaimStatusTypeEnum.OpenClaimStatusType), //Does not appear to be used. A candidate for replacement. 
				new ClaimLineItemStatus(ClaimLineItemStatusEnum.Contractual, "Contractual", "Contractual", 0, 0, 0, 0, 25, ClaimStatusTypeEnum.OtherAdjudicatedClaimStatusType), //EN-97,
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.NotOnFile, "NotOnFile", "Not On File", 0, 0, 0, 0, 22, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.MemberNotEligible, "MemberNotEligible", "Member Not Eligible", 0, 0, 0, 0, 16, ClaimStatusTypeEnum.DeniedClaimStatusType),
                new ClaimLineItemStatus(ClaimLineItemStatusEnum.RetryMemberNotFound, "RetryMemberNotFound", "Retry Member Not Found", 0, 16, 0, 0, 6, ClaimStatusTypeEnum.OpenClaimStatusType),
            };
            await Task.Run(async () =>
            {
                foreach (var status in claimLineItemStatuses)
                {
                    var cs = await _dbContext.ClaimLineItemStatuses.FirstOrDefaultAsync(a => a.Id == status.Id);
                    if (cs == null)
                    {
                        _dbContext.ClaimLineItemStatuses.Add(status);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        cs.Description = status.Description;
                        cs.Code = status.Code;
                        cs.Rank = status.Rank;
                        cs.DaysWaitBetweenAttempts = status.DaysWaitBetweenAttempts;
                        cs.MaximumPipelineDays = status.MaximumPipelineDays;
                        cs.MinimumResolutionAttempts = status.MinimumResolutionAttempts;
                        cs.MaximumResolutionAttempts = status.MaximumResolutionAttempts;
                        cs.ClaimStatusTypeId = status.ClaimStatusTypeId;
                        // = _mapper.Map(status, cs); //Throws tracking error
                        _dbContext.ClaimLineItemStatuses.Update(cs);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            });
            _logger.LogInformation("Seeded ClaimLineItemStatuses");
        }

        private async Task SeedClaimStatuses()
        {
            List<ClaimStatus> claimStatuses = new List<ClaimStatus>()
            {
                new ClaimStatus(ClaimStatusEnum.None, "None", "None"),
                new ClaimStatus(ClaimStatusEnum.Completed, "Completed", "Completed"),
                new ClaimStatus(ClaimStatusEnum.Rejected, "Rejected", "Rejected"),
                new ClaimStatus(ClaimStatusEnum.Voided, "Voided", "Voided"),
                new ClaimStatus(ClaimStatusEnum.InProcess, "InProcess", "In-Process"),
                new ClaimStatus(ClaimStatusEnum.Received, "Received", "Received"),
                new ClaimStatus(ClaimStatusEnum.NotAdjudicated, "NotAdjudicated", "Not-Adjudicated"),
                new ClaimStatus(ClaimStatusEnum.Acknowledged, "Acknowledged", "Acknowledged"),
            };
            await Task.Run(async () =>
            {
                foreach (var status in claimStatuses)
                {
                    if (!_dbContext.ClaimStatuses.Any(a => a.Id == status.Id))
                    {
                        _dbContext.ClaimStatuses.Add(status);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded ClaimStatuses");
        }

        private async Task SeedX12ClaimCodeTypes()
        {
            List<X12ClaimCodeType> claimCodeTypes = new List<X12ClaimCodeType>()
            {
                new X12ClaimCodeType(X12ClaimCodeTypeEnum.RARC, "RARC", "RARC", "Remittance Advice Remark Code"),
                new X12ClaimCodeType(X12ClaimCodeTypeEnum.CARC, "CARC", "CARC", "Claim Adjustment Reason Code"),
                new X12ClaimCodeType(X12ClaimCodeTypeEnum.REMARK, "REMARK", "REMARK", "Remark Code"),
                new X12ClaimCodeType(X12ClaimCodeTypeEnum.ICES, "ICES", "ICES", "Optum Claims Editing System"),

            };
            await Task.Run(async () =>
            {
                foreach (var type in claimCodeTypes)
                {
                    if (!_dbContext.X12ClaimCodeTypes.Any(a => a.Id == type.Id))
                    {
                        _dbContext.X12ClaimCodeTypes.Add(type);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            });
            _logger.LogInformation("Seeded X12ClaimCodeTypes");
        }


        private async Task SeedX12ClaimCategories()
        {
            List<X12ClaimCategory> claimCategories = new List<X12ClaimCategory>()
            {
                new X12ClaimCategory(X12ClaimCategoryEnum.Acknowledgment, "Acknowledgment", "Acknowledgment"),
                new X12ClaimCategory(X12ClaimCategoryEnum.DataReportingAcknowledgment, "DataReportingAcknowledgment", "Data Reporting Acknowledgment"),
                new X12ClaimCategory(X12ClaimCategoryEnum.Pending, "Pending", "Pending"),
                new X12ClaimCategory(X12ClaimCategoryEnum.Finalized, "Finalized", "Finalized"),
                new X12ClaimCategory(X12ClaimCategoryEnum.RequestsForAdditionalInfo, "RequestsForAdditionalInfo", "Requests For Additional Info"),
                new X12ClaimCategory(X12ClaimCategoryEnum.General, "General", "General"),
                new X12ClaimCategory(X12ClaimCategoryEnum.Error, "Error", "Error"),
                new X12ClaimCategory(X12ClaimCategoryEnum.Searches, "Searches", "Searches"),
            };
            await Task.Run(async () =>
            {
                foreach (var category in claimCategories)
                {
                    if (!_dbContext.X12ClaimCategories.Any(a => a.Id == category.Id))
                    {
                        _dbContext.X12ClaimCategories.Add(category);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded X12ClaimCategories");
        }

        private async Task SeedAdjustmentTypes()
        {
            List<AdjustmentType> adjustmentTypes = new List<AdjustmentType>()
            {
                new AdjustmentType(){ Id = AdjustmentTypeEnum.Credit, Name = "Credit" },
                new AdjustmentType(){ Id = AdjustmentTypeEnum.Debit, Name = "Debit" },
            };
            await Task.Run(async () =>
            {
                foreach (var type in adjustmentTypes)
                {
                    if (!_dbContext.AdjustmentTypes.Any(a => a.Id == type.Id))
                    {
                        _dbContext.AdjustmentTypes.Add(type);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            });
            _logger.LogInformation("Seeded AdjustmentTypes");
        }

        private async Task SeedTransactionTypes()
        {
            List<TransactionType> transactionTypes = new List<TransactionType>()
            {
                new TransactionType(TransactionTypeEnum.Unknown, "Unknown", "Unknown"),
                new TransactionType(TransactionTypeEnum.ClaimStatus, "ClaimStatus", "Claim Status"),
                new TransactionType(TransactionTypeEnum.ChargeEntry, "ChargeEntry", "Charge Entry"),
            };
            await Task.Run(async () =>
            {
                foreach (var type in transactionTypes)
                {
                    if (!_dbContext.TransactionTypes.Any(t => t.Id == type.Id))
                    {
                        _dbContext.TransactionTypes.Add(type);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded TransactionTypes");
        }

        private async Task SeedInputDocumentTypes()
        {
            List<InputDocumentType> inputDocumentTypes = new List<InputDocumentType>()
            {
                new InputDocumentType(InputDocumentTypeEnum.ClaimStatusInput, "ClaimStatusInput", "Claim Status Input File"),
            };

            await Task.Run(async () =>
            {
                foreach (var type in inputDocumentTypes)
                {
                    if (!_dbContext.InputDocumentTypes.Any(t => t.Id == type.Id))
                    {
                        _dbContext.InputDocumentTypes.Add(type);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded InputDocumentTypes");
        }

        private async Task SeedApplicationFeatures()
        {
            List<ApplicationFeature> applicationFeatures = new List<ApplicationFeature>()
            {
                new ApplicationFeature(ApplicationFeatureEnum.Authorizations, "Authorizations"),
                new ApplicationFeature(ApplicationFeatureEnum.ClaimStatus, "ClaimStatus"),
                new ApplicationFeature(ApplicationFeatureEnum.ChargeEntry, "ChargeEntry"),
                new ApplicationFeature(ApplicationFeatureEnum.DataPipe, "DataPipe")
            };

            await Task.Run(async () =>
            {
                foreach (var af in applicationFeatures)
                {
                    if (!_dbContext.ApplicationFeatures.Any(a => a.Id == af.Id))
                    {
                        _dbContext.ApplicationFeatures.Add(af);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded ApplicationFeatures");
        }

        private async Task SeedRpaTypes()
        {
            List<RpaType> rpaTypes = new List<RpaType>()
            {
                new RpaType(RpaTypeEnum.IcaNotes, "IcaNotes", "ICANotes", "ea3eb734-734a-4d29-88dc-36c7de4f203c"),
            };

            await Task.Run(async () =>
            {
                foreach (var type in rpaTypes)
                {
                    if (!_dbContext.RpaTypes.Any(t => t.Id == type.Id))
                    {
                        _dbContext.RpaTypes.Add(type);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded RpaTypes");
        }

        private async Task SeedClaimStatusExceptionReasonCategories()
        {
            List<ClaimStatusExceptionReasonCategory> claimStatusExceptionReasonCategories = new List<ClaimStatusExceptionReasonCategory>()
            {
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.Other, "Other", "Other"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.COB, "COB", "COB"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "CodingIssue", "Coding Issue"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.Coverage, "Coverage", "Coverage"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "Credentialing", "Credentialing"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "InsuranceTermed", "InsuranceTermed"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.InvalidInsurance, "InvalidInsurance", "Invalid Insurance"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.MRNeeded, "MRNeeded", "MR Needed"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "NoClaimMissingProcedure", "No Claim/CPT"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.PolicyNumber, "PolicyNumber", "Policy Number"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.ProviderType, "ProviderType", "Provider Type"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.Review, "Review", "Internal Review"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "WrongPayer", "Wrong Payer"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.AuthorizationDenial, "AuthorizationDenial", "Authorization Denial"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.Contractual, "Contractual", "Contractual"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "Duplicate", "Duplicate"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.SecondaryMissingEob, "SecondaryMissingEob", "Secondary Missing Eob"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.MedicareAdvCoverage, "MedicareAdvCoverage", "Medicare Advantage Coverage"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.TimelyFiling, "TimelyFiling", "Timely Filing"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.MedicalNecessity, "MedicalNecessity", "Medical Necessity"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.ClaimIssue, "ClaimIssue", "Claim Issue"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue, "DemographicsIssue", "Demographics Issue"),
                new ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum.MaxBenefits, "MaxBenefits", "Max Benefits"),

            };

            await Task.Run(async () =>
            {
                foreach (var cat in claimStatusExceptionReasonCategories)
                {
                    if (!_dbContext.ClaimStatusExceptionReasonCategories.Any(c => c.Id == cat.Id))
                    {
                        _dbContext.ClaimStatusExceptionReasonCategories.Add(cat);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded ClaimStatusExceptionReasonCategories");
        }

        private async Task SeedApplicationReports()
        {
            List<ApplicationReport> applicationReports = new List<ApplicationReport>()
            {
                new ApplicationReport(ApplicationReportEnum.DailyClaimStatusReport, "DailyClaimStatusReport", ApplicationFeatureEnum.ClaimStatus)
            };

            await Task.Run(async () =>
            {
                foreach (var ar in applicationReports)
                {
                    if (!_dbContext.ApplicationReports.Any(a => a.Id == ar.Id))
                    {
                        _dbContext.ApplicationReports.Add(ar);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded ApplicationReports");
        }

        private async Task SeedApiIntegrationTypes()
        {
            List<ApiIntegrationType> apiIntegrationTypes = new List<ApiIntegrationType>()
            {
                new ApiIntegrationType(ApiIntegrationTypeEnum.Internal, "Internal", "AIT Internal", "AIT Internal API"),
                new ApiIntegrationType(ApiIntegrationTypeEnum.Claims, "Claims", "Claims", "Claims API"),
                new ApiIntegrationType(ApiIntegrationTypeEnum.Eligibility, "Eligibility", "Eligibility", "Eligibility API"),
                new ApiIntegrationType(ApiIntegrationTypeEnum.EncounterNote, "EncounterNote", "EncounterNote", "EncounterNote API"),
            };

            await Task.Run(async () =>
            {
                foreach (var type in apiIntegrationTypes)
                {
                    if (!_dbContext.ApiIntegrationTypes.Any(i => i.Id == type.Id))
                    {
                        _dbContext.ApiIntegrationTypes.Add(type);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded ApiIntegrationTypes");
        }

        private async Task SeedApiIntegrations()
        {
            List<ApiIntegration> apiIntegrations = new List<ApiIntegration>()
            {
                new ApiIntegration(ApiIntegrationEnum.SelfPayEligibility, "SelfPayEligibility", "Self Pay Eligibility", "Self Pay Eligibility", false, false, true, true, false, ApiIntegrationTypeEnum.Internal),
                new ApiIntegration(ApiIntegrationEnum.UhcClaims, "UhcClaims", "UHC Claims Api", "United Healthcare Claims API", true, true,true, true, false, ApiIntegrationTypeEnum.Claims),
                new ApiIntegration(ApiIntegrationEnum.UhcEligibility, "UhcEligibility", "UHC Eligibility Api", "United Healthcare Eligibility API", false, false, true, true, false, ApiIntegrationTypeEnum.Eligibility),
                new ApiIntegration(ApiIntegrationEnum.AIEncounterNoteAnalytics, "AIEncounterNoteAnalytics", "AIEncounter Note Analytics", "Encounter Note", false, false, true, true, false, ApiIntegrationTypeEnum.EncounterNote),
            };

            await Task.Run(async () =>
            {
                foreach (var integration in apiIntegrations)
                {
                    var api = await _dbContext.ApiIntegrations.FirstOrDefaultAsync(x => x.Id == integration.Id);

                    //if (!_dbContext.ApiIntegrations.Any(i => i.Id == integration.Id))
                    if (api == null)
                    {
                        _dbContext.ApiIntegrations.Add(integration);
                    }
                    else
                    {
                        api.ApiIntegrationTypeId = integration.ApiIntegrationTypeId;
                        api.RequirePolicyNumber = integration.RequirePolicyNumber;
                        api.RequirePayerIdentifier = integration.RequirePayerIdentifier;
                        api.RequireTaxId = integration.RequireTaxId;
                        api.RequireDateOfBirth = integration.RequireDateOfBirth;

                        _dbContext.ApiIntegrations.Update(api);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded ApiIntegrations");
        }

        private async Task SeedSpecialties()
        {
            await Task.Run(async () =>
            {
                var specialties = Enum.GetValues<SpecialtyEnum>().ToList();

                foreach (var enumValue in specialties)
                {
                    var memberInfo = typeof(SpecialtyEnum).GetMember(enumValue.ToString()).FirstOrDefault();
                    var serviceTypeAttribute = memberInfo?.GetCustomAttribute<ServiceTypeAttribute>();
                    bool isSpecialtyServiceAttribute = ((int)enumValue > (int)SpecialtyEnum.UnknownPhysicianSpecialty) && serviceTypeAttribute is not null;

                    var existingEnumDetail = _dbContext.Specialties.FirstOrDefault(s => s.Id == enumValue) ?? null;
                    if (existingEnumDetail is null)
                    {
                        var specialty = new Specialty()
                        {
                            Id = enumValue,
                            Code = isSpecialtyServiceAttribute ? serviceTypeAttribute.Code : Convert.ToInt32(enumValue).ToString(),
                            Name = isSpecialtyServiceAttribute ? serviceTypeAttribute.Name : enumValue.ToString(),
                            Description = isSpecialtyServiceAttribute ? serviceTypeAttribute.Description : enumValue.GetDescription()
                        };

                        _dbContext.Specialties.Add(specialty);
                    }
                    else
                    {
                        // Update the existing Specialty entity based on the enum description and ServiceTypeAttribute
                        existingEnumDetail.Code = isSpecialtyServiceAttribute ? serviceTypeAttribute.Code : Convert.ToInt32(enumValue).ToString();
                        existingEnumDetail.Name = isSpecialtyServiceAttribute ? serviceTypeAttribute.Name : enumValue.ToString();
                        existingEnumDetail.Description = isSpecialtyServiceAttribute ? serviceTypeAttribute.Description : enumValue.GetDescription();

                        _dbContext.Specialties.Update(existingEnumDetail);
                    }
                }
                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded Specialties");
        }

        private async Task SeedDefaultDepartments()
        {
            await Task.Run(async () =>
            {
                if (!await _dbContext.Departments.AnyAsync())
                {
                    foreach (var enumValue in Enum.GetValues<DepartmentEnum>())
                    {
                        _dbContext.Departments.Add(new Department
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription()
                        });
                    }
                    await _dbContext.SaveChangesAsync();
                }
            });
            _logger.LogInformation("Seeded default Department");
        }

        //private async Task SeedDefaultProviderLevels()
        //{
        //	await Task.Run(async () =>
        //	{
        //		if (!await _dbContext.ProviderLevels.AnyAsync())
        //		{
        //			foreach (var enumValue in Enum.GetValues<ProviderLevelEnum>())
        //			{
        //				_dbContext.ProviderLevels.Add(new ProviderLevel()
        //				{
        //					Id = enumValue,
        //					Name = enumValue.ToString(),
        //					Description = enumValue.GetDescription()
        //				});
        //			}
        //			await _dbContext.SaveChangesAsync();
        //		}
        //	});
        //	_logger.LogInformation("Seeded default ProviderLevels.");
        //}

        private ReportCategoryEnum GetReportCategory(ReportsEnum enumValue)
        {
            var arManageMentReports = new List<ReportsEnum> { ReportsEnum.AR_Aging_Summary, ReportsEnum.AR_Aging_Summary_With_Payment_info, ReportsEnum.Activity_Summary, ReportsEnum.Activity_Summary_By_Charge_Status };
            var dailyClaimReports = ReportsEnum.Daily_Claim_Report;
            if (arManageMentReports.Contains(enumValue))
            {
                return ReportCategoryEnum.AR_Management_Report;
            }
            else if (enumValue == dailyClaimReports)
            {
                return ReportCategoryEnum.Daily_Monthly_Reports;
            }
            return 0;
        }

        private static List<ClaimStatusExceptionReasonCategoryEnum> GetClaimStatusExceptionReasonCategoryEnumsByRoleId(EmployeeRoleEnum role)
        {
            switch (role)
            {
                case EmployeeRoleEnum.RegistrationManager:
                    return ReadOnlyObjects.RegistrationManagerExceptionReasonCategorEnums.ToList();
                case EmployeeRoleEnum.BillingManager:
                    return ReadOnlyObjects.BillingManagerExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.Registor:
                    return ReadOnlyObjects.RegistorExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.MedicalAssistance:
                    return ReadOnlyObjects.MedicalAssistanceExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.CEO:
                    return ReadOnlyObjects.CEOExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.CFO:
                    return ReadOnlyObjects.CFOExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.COO:
                    return ReadOnlyObjects.COOExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.CIO:
                    return ReadOnlyObjects.CIOExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.DirectorOfPatientFinancialServices:
                    return ReadOnlyObjects.DOPFSExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.VicePresident:
                    return ReadOnlyObjects.VicePresidentExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.MedicalDirector:
                    return ReadOnlyObjects.MedicalDirectorExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.BillingSupervisor:
                    return ReadOnlyObjects.BillingSupervisorExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.CashPostingManager:
                    return ReadOnlyObjects.CashPostingManagerExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.Biller:
                    return ReadOnlyObjects.BillerExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.CashPoster:
                    return ReadOnlyObjects.CashPosterExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.ChargeEnrty:
                    return ReadOnlyObjects.ChargeEnrtyExceptionReasonCategoryEnum.ToList();
                case EmployeeRoleEnum.InsuranceContractor:
                    return ReadOnlyObjects.InsuranceContractorExceptionReasonCategoryEnum.ToList();
                default:
                    return new List<ClaimStatusExceptionReasonCategoryEnum>();
            }
        }

        private static List<EmployeeRoleEnum> GetRolesByDepartment(DepartmentEnum dept)
        {
            switch (dept)
            {
                case DepartmentEnum.Registor:
                    return ReadOnlyObjects.RegistrationEmployeeRoles.ToList();
                case DepartmentEnum.Medical:
                    return ReadOnlyObjects.MedicalEmployeeRoles.ToList();
                case DepartmentEnum.Billing:
                    return ReadOnlyObjects.BillingEmployeeRoles.ToList();
                case DepartmentEnum.Credentialing:
                    return ReadOnlyObjects.CredentialingEmployeeRoles.ToList();
                case DepartmentEnum.ChargeEntry:
                    return ReadOnlyObjects.ChargeEntryEmployeeRoles.ToList();
                case DepartmentEnum.CashPosting:
                    return ReadOnlyObjects.CashPostingEmployeeRoles.ToList();
                default:
                    return new List<EmployeeRoleEnum>();
            }
        }

        private async Task SeedSourceSystemsEnums()
        {
            Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<SourceSystemEnum>())
                {
                    var reportcategoryExist = _dbContext.SourceSystems.FirstOrDefault(z => z.Id == enumValue) ?? null;
                    if (reportcategoryExist is null)
                    {
                        _dbContext.SourceSystems.Add(new SourceSystem
                        {
                            Id = enumValue,
                            Name = enumValue.GetDescription(),
                            Code = enumValue.ToString(),
                        });
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default SourceSystem");
        }

        private async Task SeedDefaultProviderLevels()
        {
            Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<ProviderLevelEnum>())
                {
                    var providerLevelExist = _dbContext.ProviderLevels.FirstOrDefault(z => z.Id == enumValue) ?? null;

                    if (providerLevelExist is null)
                    {
                        _dbContext.ProviderLevels.Add(new ProviderLevel()
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription()
                        });
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default ProviderLevels.");
        }

        private async Task SeedDefaultClaimStatusTypes()
        {
            await Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<ClaimStatusTypeEnum>())
                {
                    var claimStatusType = _dbContext.ClaimStatusTypes.FirstOrDefault(z => z.Id == (ClaimStatusTypeEnum)(int)enumValue) ?? null;
                    if (claimStatusType is null)
                    {
                        _dbContext.ClaimStatusTypes.Add(new ClaimStatusType
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription()
                        });
                    }
                }
                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded default ClaimStatusTypes");
        }

        private async Task SeedHolidays()
        {
            await Task.Run(async () =>
            {
                foreach (var enumValue in Enum.GetValues<HolidaysEnum>())
                {
                    var holiday = _dbContext.Holidays.FirstOrDefault(z => z.Id == (HolidaysEnum)(int)enumValue) ?? null;
                    if (holiday is null)
                    {
                        var memberInfo = typeof(HolidaysEnum).GetMember(enumValue.ToString()).FirstOrDefault();
                        var tes = memberInfo.GetHolidayMonth();
                        _dbContext.Holidays.Add(new Holiday
                        {
                            Id = enumValue,
                            Code = enumValue.ToString(),
                            Description = enumValue.GetDescription(),
                            Month = memberInfo.GetHolidayMonth()
                        });
                    }
                }
                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded Holidays");
        }

        private async Task SeedApiClaimsMessageClaimLineitemStatusMap()
        {
            List<ApiClaimsMessageClaimLineitemStatusMap> apiClaimsMessageClaimLineitemStatusMaps = new List<ApiClaimsMessageClaimLineitemStatusMap>()
            {
                // Param order : (Code, Message, ClaimLineItemStatusId)
                 new ApiClaimsMessageClaimLineitemStatusMap
                 {
                    Code = "LCLM_M_141",
                    Message = "Member ID is incorrect, please re-enter the member ID number.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                 },
                 new ApiClaimsMessageClaimLineitemStatusMap
                 {
                    Code = "LCLM_M_301",
                    Message = "No claim found with the requested search criteria: DOS Mismatch",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                 },
                 new ApiClaimsMessageClaimLineitemStatusMap
                 {
                    Code = "LCLM_M_302",
                    Message = "Functional Error: System Failure, while fetching the response.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                 },
                 new ApiClaimsMessageClaimLineitemStatusMap
                 {
                     Code = "LCLM_M_302",
                     Message = "Functional Error: System Failure, while fetching the response.",
                     ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                 },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_301",
                    Message = "Functional Error: No details found with the information given.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_102",
                    Message = "Mandatory Element Missing in the Request: claimNumber",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_103",
                    Message = "Mandatory Element Missing in the Request: payerId",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_109",
                    Message = "System is unable to find this claim using this search type, please try a different search type",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_121",
                    Message = "Invalid claimNumber: length cannot be less than 8 characters.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_122",
                    Message = "Claim number is not in the valid format.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_301",
                    Message = "Functional Error: No claim found with the information given.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_303",
                    Message = "Multiple claims exist with the given claim number, please provide patient first name and last name in order to return claim details.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_201",
                    Message = "Access Denied: Resource Forbidden Exception",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_202",
                    Message = "Authorization Error: Payer ID not Allowed",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_302",
                    Message = "Functional Error: System Failure, while fetching the response",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_500",
                    Message = "An invalid response was received from the upstream server",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_503",
                    Message = "Service Unavailable",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_D_504",
                    Message = "The upstream server is timing out",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_101",
                    Message = "Mandatory Element Missing in the Request: tin",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_102",
                    Message = "Mandatory Element Missing in the Request: claimNumber",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_103",
                    Message = "Mandatory Element Missing in the Request: payerId",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_105",
                    Message = "Claim found with given search criteria but provider details not matching",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_106",
                    Message = "We are unable to find this claim number using this search option. Either the number is invalid or this search cannot find the claim at this time. Please check the number or try a different search option.", ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_107",
                    Message = "Check claim status for Behavior Health claims",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_108",
                    Message = "Only Delegated Encounters Found",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_109",
                    Message = "System is unable to find this claim using this search type, please try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_110",
                    Message = "Claim is in pre-adjudicated status",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_111",
                    Message = "UHG Employee data - no access allowed to UHG internal/Super users",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_112",
                    Message = "Invalid claimNumber: length cannot be less than 8 characters.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_113",
                    Message = "Claim number is not in the valid format",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_301",
                    Message = "Functional Error: No details found with the information given.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_401",
                    Message = "Data Error: Tin Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_202",
                    Message = "Authorization Error: Payer ID not Allowed",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_S_302",
                    Message = "Functional Error: System Failure, while fetching the response",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_101",
                    Message = "Mandatory Element Missing in the Request: payerId.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_102",
                    Message = "Mandatory Element Missing in the Request: firstSrvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_103",
                    Message = "Incorrect Date Format in the Request: firstSrvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_104",
                    Message = "Mandatory Element Missing in the Request: lastSrvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_105",
                    Message = "Incorrect Date Format in the Request: lastSrvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_106",
                    Message = "Incorrect Date Format in the Request: patientDob.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_107",
                    Message = "Incorrect Data: lastSrvcDt should be greater than firstSrvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_108",
                    Message = "Incorrect Data: Search valid only for 30 days.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_109",
                    Message = "Missing incorrect combination element for search.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_110",
                    Message = "Invalid search combination for the requested payer Id.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_112",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member ID, Payer Name/Payer ID, and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_113",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member ID, Payer Name/Payer ID, and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_114",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member ID, Payer Name/Payer ID, and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                { Code = "LCLM_M_115",
                    Message = "Member was found, but there is no active coverage for the entered Dates of Service. Please enter new Dates of Service and resubmit.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_116",
                    Message = "UHG Employee data - no access allowed to UHG internal user.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                { Code = "LCLM_M_117",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member ID, Payer Name/Payer ID, and try again. Your search returned no results. Please review your search criteria, including the Payer Name/Payer ID, and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_119",
                    Message = "Required data element is missing or invalid, please review your entries and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_120",
                    Message = "Authorization Error: Invalid TIN information.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_121",
                    Message = "Authorization Error: Invalid MPIN/TIN Combination.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_122",
                    Message = "Claim found with given search criteria but date of service is not matching.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_123",
                    Message = "Claim found with given search criteria but provider details not matching.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_124",
                    Message = "Check claim status for Behavior Health claims.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_126",
                    Message = "Required data element is missing or invalid, please review your entries and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_127",
                    Message = "Authorization Error: Invalid TIN information.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_128",
                    Message = "Authorization Error: Invalid MPIN/TIN Combination.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_129",
                    Message = "DOS mismatch with the given request.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_130",
                    Message = "Claim found with given search criteria but date of service is not matching.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_131",
                    Message = "Claim found with given search criteria but provider details not matching.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_132",
                    Message = "Check claim status for Behavior Health claims.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_134",
                    Message = "Required data element is missing or invalid, please review your entries and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_135",
                    Message = "Authorization Error: Invalid TIN information.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_136",
                    Message = "Authorization Error: Invalid MPIN/TIN Combination.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_137",
                    Message = "DOB mismatch with the given request.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_138",
                    Message = "Claim found with given search criteria but date of service is not matching.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_139",
                    Message = "Claim found with given search criteria but provider details not matching.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_140",
                    Message = "Date of birth is incorrect, please re-enter the patient date of birth.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_142",
                    Message = "Claim cannot be found. The member is not showing active coverage for this date of service.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_143",
                    Message = "Member not found, please check the data entered or try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_144",
                    Message = "Claim cannot be found. The member is not showing active medical coverage for this date of service.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_145",
                    Message = "UHG Employee data - no access allowed to UHG internal/Super users.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_146",
                    Message = "First Name mismatch with the given request.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_147",
                    Message = "ID mismatch with the given request.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_148",
                    Message = "Claim cannot be found. The member is not showing active coverage for this date of service.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_149",
                    Message = "Member not found, please check the data entered or try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_150",
                    Message = "Claim cannot be found. The member is not showing active medical coverage for this date of service.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_151",
                    Message = "UHG Employee data - no access allowed to UHG internal/Super users.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_152",
                    Message = "Claim cannot be found. The member is not showing active coverage for this date of service.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_153",
                    Message = "Member not found, please check the data entered or try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_154",
                    Message = "Claim cannot be found. The member is not showing active medical coverage for this date of service.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotEligible
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_155",
                    Message = "UHG Employee data - no access allowed to UHG internal/Super users.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_156",
                    Message = "System is unable to find this claim using this search type, please try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_157",
                    Message = "System is unable to find this claim using this search type, please try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_158",
                    Message = "System is unable to find this claim using this search type, please try a different search type.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_160",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member ID, Date of Birth, Payer Name/Payer ID and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                { Code = "LCLM_M_162",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member ID, Member Name and Payer Name/Payer ID and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_164",
                    Message = "The member cannot be found in our records. Please review your search criteria, including the Member Name, Date of Birth, Payer Name/Payer ID and try again.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_301",
                    Message = "Functional Error: No details found with the information given.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unavailable
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_303",
                    Message = "Functional Error: Unable to identify the member information uniquely.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_304",
                    Message = "No claim found with the requested search criteria: DOS Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_305",
                    Message = "First Name Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_306",
                    Message = "Last Name Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_307",
                    Message = "ID Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_308",
                    Message = "DOB Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_309",
                    Message = "Policy Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_310",
                    Message = "No Platform found.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_311",
                    Message = "No Coverage Found - No Active, Pre & Post Coverages Found.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_312",
                    Message = "Only Delegated Encounters Found.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_313",
                    Message = "Bad Request. Required/Conditional data element missing in the request.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_314",
                    Message = "No claim found with the requested search criteria: TIN mismatch/NPI mismatch/MPIN mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_315",
                    Message = "No claim found with the requested search criteria: Platform MisMatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_316",
                    Message = "No claim found with the requested search criteria: DOB mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_317",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberFirstName, memberLastName, memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_318",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberDOB, memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_319",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberId, memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_320",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_401",
                    Message = "Data Error: XXXXXXXX.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_201",
                    Message = "Access Denied: Resource Forbidden Exception.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_M_202",
                    Message = "Authorization Error: Payer ID not Allowed.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_101",
                    Message = "Mandatory Element Missing in the Request: payerId",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_102",
                    Message = "Mandatory Element Missing in the Request: srvcDt",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_103",
                    Message = "Incorrect Date Format in the Request: firstSrvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_104",
                    Message = "Mandatory Element Missing in the Request: srvcDt.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_105",
                    Message = "Missing incorrect combination element for search.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_106",
                    Message = "Invalid search combination for the requested payer Id.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_301",
                    Message = "Functional Error: No details found with the information given.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_303",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberFirstName, memberLastName, memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_304",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberDOB, memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_305",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberId, memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_306",
                    Message = "Functional Error: Unable to identify the member information uniquely. Member may be having multiple memberships. Please verify the member information in the criteria or pass additional member information like: memberPolicy.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_307",
                    Message = "System is unable to find this claim details using the requested information: as its a pre-adjudicated claim.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_308",
                    Message = "System is unable to find the member using the requested information: memberId Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_309",
                    Message = "System is unable to find the member using the requested information: patientDob Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_310",
                    Message = "System is unable to find the member using the requested information: patientFn Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_311",
                    Message = "System is unable to find the member using the requested information: patientLn Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_312",
                    Message = "System is unable to find the member using the requested information: srvcDt Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_313",
                    Message = "System is unable to find the member using the requested information: policy Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_314",
                    Message = "System is unable to find the member platform using the requested information.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_315",
                    Message = "System is unable to find the claim using the requested information.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_316",
                    Message = "System is unable to find the claim using the requested information: as only delegated encounters found.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_317",
                    Message = "System is unable to find the claim using the requested information: patientDob Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_318",
                    Message = "System is unable to find the claim using the requested information: srvcDt Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_319",
                    Message = "System is unable to find the claim using the requested information: tin Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_320",
                    Message = "System is unable to find the claim using the requested information: platform Mismatch.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_201",
                    Message = "Access Denied: Resource Forbidden Exception.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_202",
                    Message = "Authorization Error: Payer ID not Allowed.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                },
                new ApiClaimsMessageClaimLineitemStatusMap
                {
                    Code = "LCLM_DM_302",
                    Message = "Functional Error: System Failure, while fetching the response.",
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error
                }
            };

            await Task.Run(async () =>
            {
                foreach (var map in apiClaimsMessageClaimLineitemStatusMaps)
                {
                    var existingMap = await _dbContext.ApiClaimsMessageClaimLineitemStatusMaps
                        .FirstOrDefaultAsync(a => a.Code == map.Code);

                    if (existingMap == null)
                    {
                        // Add new record if code does not exist
                        _dbContext.ApiClaimsMessageClaimLineitemStatusMaps.Add(map);
                    }
                    else
                    {
                        // Update existing record's message and status if code exists
                        existingMap.Message = map.Message;
                        existingMap.ClaimLineItemStatusId = map.ClaimLineItemStatusId;
                        _dbContext.ApiClaimsMessageClaimLineitemStatusMaps.Update(existingMap);
                    }
                }
                await _dbContext.SaveChangesAsync();
            });

            _logger.LogInformation("Seeded ApiClaimsMessageClaimLineitemStatusMap");
        }

    }
}

