using CsvHelper;
using CsvHelper.Configuration;
using MedHelpAuthorizations.Application.Models.CsvDeserialize;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static MedHelpAuthorizations.Shared.Constants.Application.ApplicationConstants;
using static MedHelpAuthorizations.Shared.Enums.CustomDashboardEnums;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    public partial class ApplicationDbSeeder
    {
        private async Task SeedRpaInsurances()
        {
            List<RpaInsurance> rpaInsurances = new List<RpaInsurance>()
            {
                new RpaInsurance("Ambetter", "Ambetter", 1, null),
                new RpaInsurance("Amerigroup", "Amerigroup", 1, null),
                new RpaInsurance("Carefirst BCBS", "CarefirstBS", 2, null),
                new RpaInsurance("CollabMD", "CollabMD", 24, null),
                new RpaInsurance("DentaQuest", "DentaQuest", 3, null),
                new RpaInsurance("Optum", "Optum", 5, null),
                new RpaInsurance("Maryland Physcians Care", "MPC", 6, null),
                new RpaInsurance("United Healthcare", "UHC", 7, ApiIntegrationEnum.UhcClaims),
                new RpaInsurance("Payspan", "Payspan", 9, null),
                new RpaInsurance("Novitas CMS", "Novitas", 8, null),
                new RpaInsurance("Maryland Medicaid", "MedicaidMD", 9, null),
                new RpaInsurance("Cigna", "Cigna", 10, null),
                new RpaInsurance("Aetna", "Aetna", 1, null),
                new RpaInsurance("Aetna Better Health", "AetnaBH", 1, null),
                new RpaInsurance("Carefirst Michigan", "CarefirstMI", 1, null),
                new RpaInsurance("MedStar Family Choice MCO", "MedStar", 11, null),
                new RpaInsurance("Aetna Medicaid", "AetnaMC", 1, null),
                new RpaInsurance("Connex Medicare Part A", "ConnexA", 12, null),
                new RpaInsurance("CarefirstMC", "Carefirst_MC", 13, null),
                new RpaInsurance("Carefirst BC", "CarefirstBC", 26, null),
                new RpaInsurance("Healthy Blue", "HealthyBlue", 27, null),
                new RpaInsurance("Priority Partners", "PriPartners", 14, null),
                new RpaInsurance("Liberty", "Liberty", 15, null),
                new RpaInsurance("Oscar", "Oscar", 16, null),
                new RpaInsurance("Carefirst Administrators","CFAdmin", 17, null),
                new RpaInsurance("Tricare", "Tricare", 18, null),
                new RpaInsurance("Meridian", "Meridian", 19, null),
                new RpaInsurance("Blue Cross Complete", "BCComplete", 20, null),
                new RpaInsurance("Amerihealth", "Amerihealth", 20, null),
                new RpaInsurance("BlueE", "BlueE", 21, null),
                new RpaInsurance("NCTracks", "NCTracks", 22, null),
                new RpaInsurance("WellCare", "WellCare", 23, null),
                new RpaInsurance("DentaQuest Michigan", "DentaQuestMI", 4, null),
                new RpaInsurance("Humana", "Humana", 1, null),
                new RpaInsurance("Molina", "Molina", 1, null),
                new RpaInsurance("Friday", "Friday", 28, null),
                new RpaInsurance("Anthem", "Anthem", 1, null),
                new RpaInsurance("Unicare", "Unicare", 1, null),
                new RpaInsurance("WVMedicaid", "WVMedicaid", 29, null),
                new RpaInsurance("BCWV", "BCWV", 20, null),
                new RpaInsurance("HumanaMilitary", "HumanaMilita", 30, null),
                new RpaInsurance("UMR", "UMR", 31, null),
                new RpaInsurance("Ambetter Sunshine Health", "AmbetterSH", 32, null),
                new RpaInsurance("Carelon", "Carelon", 1, null),
                new RpaInsurance("CarePlus", "CarePlus", 1, null),
                new RpaInsurance("Florida Medicaid", "FLMedicaid", 32, null),
                new RpaInsurance("Simply Healthcare", "SmplyHlth", 1, null),
                new RpaInsurance("Staywell", "Staywell", 1, null),
                new RpaInsurance("Availity Wellcare", "WellcareAv", 1, null),
                new RpaInsurance("Florida Blue", "FLBlue", 1, null),
                new RpaInsurance("Florida Blue Medicare", "FLBlueMC", 1, null),
                new RpaInsurance("SKYGEN", "SKYGEN", 33, null),
                new RpaInsurance("CT Medicaid", "CTMedicaid ", 34, null),
                new RpaInsurance("NY Medicaid", "NYMedicaid", 35, null),
                new RpaInsurance("GA Medicaid", "GAMedicaid", 36, null),
                new RpaInsurance("NY Medicaid ePaces", "NYePacesMA", 37, null),
                new RpaInsurance("NY Managed Long Term Care", "NYMLTC", 1, null),
                new RpaInsurance("VA Community Care Network", "VACCN", 38, null),
                new RpaInsurance("Anthem New York", "AnthemNY", 39, null),
                new RpaInsurance("Fidelis Care New York", "FidelisNY", 40, null),
                new RpaInsurance("Alterwood Advantage", "Alterwood", 41, null),
                new RpaInsurance("VNS Health Plan", "VNSHealth", 42, null),
                new RpaInsurance("Unknown", "Unknown", 43, null),
                //new RpaInsurance("XXX", "XXX", 999),
            };
            await Task.Run(async () =>
            {
                foreach (var ins in rpaInsurances)
                {
                    var rpa = await _dbContext.RpaInsurances.FirstOrDefaultAsync(ri => ri.Code == ins.Code);

                    if (rpa == null)
                    {
                        _dbContext.RpaInsurances.Add(ins);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        rpa.RpaInsuranceGroupId = ins.RpaInsuranceGroupId;
                        rpa.Name = ins.Name;
                        rpa.ApiIntegrationId = ins.ApiIntegrationId;
                        _dbContext.RpaInsurances.Update(rpa);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                await _dbContext.SaveChangesAsync();
            });
            _logger.LogInformation("Seeded RpaInsurances");
        }

        private async Task SeedDefaultClient()
        {
            await Task.Run(async () =>
            {
                //Check if default client Exists
                var defaultClient = await _dbContext.Clients.Where(c => c.ClientCode == "default").FirstOrDefaultAsync();
                if (defaultClient == null)
                {
                    ICollection<ClientAuthType> clientAuthTypes = new List<ClientAuthType>();
                    ICollection<ClientApplicationFeature> applicationFeatures = new List<ClientApplicationFeature>();
                    applicationFeatures.Add(new ClientApplicationFeature() { ApplicationFeatureId = ApplicationFeatureEnum.Authorizations });
                    applicationFeatures.Add(new ClientApplicationFeature() { ApplicationFeatureId = ApplicationFeatureEnum.ClaimStatus });
                    applicationFeatures.Add(new ClientApplicationFeature() { ApplicationFeatureId = ApplicationFeatureEnum.ChargeEntry });

                    var newClient = new Domain.Entities.Client()
                    {
                        Name = "AIT Default",
                        ClientCode = "default",
                        ClientAuthTypes = clientAuthTypes,
                        ClientApplicationFeatures = applicationFeatures
                    };
                    _dbContext.Clients.Add(newClient);
                }
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded default Client");
        }

        private async Task SeedDefaultClientProvider()
        {
            await Task.Run(async () =>
            {
                //Check if patient Exists
                var defaultProvider = await _dbContext.ClientProviders.Where(c => c.Npi == "0123456789").FirstOrDefaultAsync();
                if (defaultProvider == null)
                {
                    var newProvider = new ClientProvider()
                    {
                        Person = new Person() { FirstName = "Default", LastName = "Provider1", DateOfBirth = new DateTime(1976, 04, 21), Email = "default@provider1.com", MobilePhoneNumber = 1112223333, OfficePhoneNumber = 9998887777, ClientId = 1 },
                        ClientId = 1,
                        ExternalId = "123456789AIT",
                        Npi = "0123456789",
                        TaxId = "999887777",
                        SpecialtyId = SpecialtyEnum.InternalMedicine,
                        Upin = "A12345"
                    };
                    _dbContext.ClientProviders.Add(newProvider);
                }
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded default Provider");
        }
        private async Task SeedDefaultClientLocation()
        {
            await Task.Run(async () =>
            {
                //check if clientLocations exists
                var defaultLocation = await _dbContext.ClientLocations.Where(l => l.Name == "Default").FirstOrDefaultAsync();
                if (defaultLocation == null)
                {
                    var newLocation = new ClientLocation()
                    {
                        Name = "Default",
                        OfficePhoneNumber = 7897897897,
                        OfficeFaxNumber = 7897897897,
                        //AddressId =null,
                        ClientId = 1,
                    };
                    _dbContext.ClientLocations.Add(newLocation);
                }
            });

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Seeded default Client location");
        }

        private async Task SeedDefaultPatient()
        {
            await Task.Run(async () =>
            {
                //Check if patient Exists
                var defaultPatient = await _dbContext.Patients.FirstOrDefaultAsync(x => x.Person.FirstName.ToLower() == "kevin");
                if (defaultPatient == null)
                {
                    ICollection<ClientAuthType> clientAuthTypes = new List<ClientAuthType>();

                    var newPatient = new Domain.Entities.Patient()
                    {
                        Person = new Person()
                        {
                            FirstName = "Kevin",
                            LastName = "McInitialPerson",
                            DateOfBirth = new DateTime(1976, 04, 21),
                            GenderIdentityId = GenderIdentityEnum.Male,
                            ClientId = 1
                        },
                        ExternalId = "1000001",
                        //ClientInsurance = null,
                        InsurancePolicyNumber = "PolicyNumberForECSID001",
                        InsuranceGroupNumber = "GroupNumberFor001",
                        ClientId = 1
                    };
                    _dbContext.Patients.Add(newPatient);
                }
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded patient");
        }

        private async Task SeedUserClients()
        {
            await Task.Run(async () =>
            {
                //Check if user client Exists
                var defaultUserClient = await _dbContext.UserClients.FirstOrDefaultAsync(x => x.UserId.ToLower() == this._adminUserId);
                if (defaultUserClient == null)
                {
                    var newUserClient = new Domain.Entities.UserClient()
                    {
                        UserId = this._adminUserId,
                        ClientId = 1
                    };
                    _dbContext.UserClients.Add(newUserClient);
                }
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded default admin user client");
        }

        private async Task SeedDefaultAuthTypes()
        {
            await Task.Run(async () =>
            {
                string[] names = new[] { "PT50", "PT20", "PT54", "PRP", "OMHC", "MH-IOP", "MH-PHP", "DENTAL", "FQHC", "PHTHPY", "OPHTH", "TYPE32", "ACT", "SCMP", "SupEmp", "HmHlth", "CCS" };
                string[] descriptions = new[] { "PT50 - SUD IOP", "PT20 - SUD Doctors office", "PT54 - Residential",
                                                  "PRP - Psychiatric Rehabilitation Program", "OMHC - Outpatient Mental Health Clinic",
                                                  "Mental Health - IOP","MH-PHP Mental Health - Partial Hospitalization Program","Dental",
                                                  "Federal Qualified Health Center", "Physical Therapy", "Ophthalmology", "Opioid Treatment Center",
                                                  "Assertive Community Treatment", "Specialized Case Management Program", "Supportive Employment Vocational Services", "Home Health", "Community Care Services"};

                for (int i = 0; i < names.Length; i++)
                {
                    //add new AuthType if authtype does not exist                
                    if (!_dbContext.AuthTypes.Any(x => x.Name == names[i]))
                    {
                        var newAuth = new AuthType()
                        {
                            Name = names[i],
                            Description = descriptions[i]
                        };
                        _dbContext.AuthTypes.Add(newAuth);
                        _dbContext.ClientAuthTypes.Add(new ClientAuthType
                        {
                            ClientId = 1,
                            AuthType = newAuth
                        });
                    }
                }
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded default Auth");
        }
        private async Task SeedAuthStatuses()
        {
            await Task.Run(async () =>
            {
                if (!await _dbContext.AuthorizationStatuses.AnyAsync())
                {
                    foreach (var enumValue in Enum.GetValues<AuthorizationStatusEnum>())
                    {
                        _dbContext.AuthorizationStatuses.Add(new AuthorizationStatus
                        {
                            Id = enumValue,
                            Name = enumValue.ToString(),
                            Description = enumValue.GetDescription()
                        });
                    }
                    //foreach (var auth in _dbContext.Authorizations.Where(x => x.AuthorizationStatusId == null))
                    //{
                    //    auth.AuthorizationStatusId = AuthorizationStatusEnum.ClientRequestAdded;
                    //}
                }
            });
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedDocumentTypeRelated()
        {
            await Task.Run(async () =>
            {
                var defaultclient = await _dbContext.Clients.Include(x => x.DocumentTypes).FirstOrDefaultAsync(x => x.ClientCode == "default");
                if (!_dbContext.DocumentTypes.Any())
                {
                    //add the default types
                    var defaultTypes = DocumentTypeConstants.GetDefaults();
                    foreach (var type in defaultTypes)
                    {
                        var doctype = new DocumentType() { Name = type.Item1, Description = type.Item2, IsDefault = true };
                        _dbContext.DocumentTypes.Add(doctype);
                    }
                }
                if (defaultclient.DocumentTypes.Count == 0)
                {
                    var defaultTypes = DocumentTypeConstants.GetDefaults();
                    var docs = _dbContext.DocumentTypes.Where(x => defaultTypes.Select(x => x.Item1).Contains(x.Name));
                    foreach (var doc in docs)
                    {
                        defaultclient?.DocumentTypes.Add(doc);
                    }
                }
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded default DocumentTypes");
        }

        private async Task SeedDefaultClientUserApplicationReports()
        {
            var userClient = _dbContext.UserClients.Where(x => x.UserId == _adminUserId && x.ClientId == 1).FirstOrDefault();
            if (userClient == null)
            {
                return;
            }
            List<ClientUserApplicationReport> clientUserApplicationReports = new List<ClientUserApplicationReport>()
            {
                new ClientUserApplicationReport()
                {
                    UserClientId = userClient.Id,
                    ApplicationReportId = ApplicationReportEnum.DailyClaimStatusReport,
                    CreatedBy = "Database Seed",
                    CreatedOn = DateTime.UtcNow
                },
            };

            await Task.Run(async () =>
            {
                foreach (var rpt in clientUserApplicationReports)
                {
                    if (!_dbContext.ClientUserApplicationReports.Any(t => t.ApplicationReportId == rpt.ApplicationReportId))
                    {
                        _dbContext.ClientUserApplicationReports.Add(rpt);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });

            _logger.LogInformation("Seeded ClientUserApplicationReports");
        }

        private async Task SeedDefaultClientApiIntegrationKeys()
        {
            List<ClientApiIntegrationKey> clientApiIntegrationKeys = new List<ClientApiIntegrationKey>()
            {
                new ClientApiIntegrationKey()
                {
                    ClientId = 1,
                    ApiIntegrationId = MedHelpAuthorizations.Shared.Enums.ApiIntegrationEnum.SelfPayEligibility,
                    ApiKey = "tE564fdMEu8hjXjZeLX4xM7mkUHhtWPlIk2XNWKdYsBTGXIKIHJAS4FvMzlPY1KGCQt7J5edfXlib3wkQvI3osULuj8T4rwFj0XZI5czRUrq6UFpxVCAl21fKXZM2B2F",
                    ApiSecret = string.Empty,
                    ApiUrl = "https://sp-eligibility.azurewebsites.net",
                    ApiVersion = "1",
                    ApiUsername = string.Empty,
                    ApiPassword = string.Empty,
                    CreatedBy = "Database Seed",
                    CreatedOn = DateTime.UtcNow
                },
            };

            await Task.Run(async () =>
            {
                foreach (var key in clientApiIntegrationKeys)
                {
                    if (!_dbContext.ClientApiIntegrationKeys.Any(t => t.ApiIntegrationId == key.ApiIntegrationId))
                    {
                        _dbContext.ClientApiIntegrationKeys.Add(key);
                    }
                }

                await _dbContext.SaveChangesAsync();
            });

            _logger.LogInformation("Seeded ClientApiIntegrationKeys");
        }

        #region custom dashboard seeding
        public static List<DashboardItem> DashboardItems = new()
        {
            new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Date Lag Cards", Selector = "available", Icon = Icons.Material.Filled.SpaceDashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ClaimsDateLagInfoCardComponent", NeedsLayoutFilter = true },
	        //new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Info Cards", Selector = "available", Icon = Icons.Material.Filled.Dashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout },
	        new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Revenue Analysis Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "RevenueAnalysisChartComponent"},
            new DashboardItem() { Order = 3, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "AR Aging Amount Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ARAgingAmountDistributionChartComponent" },
            new DashboardItem() { Order = 4, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Reverse Analysis Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ARAgingReverseAnalysisChartComponent" },
            new DashboardItem() { Order = 5, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Claim Status Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ClaimStatusDashboardComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 6, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Denial Reason Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsDashboardComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 7, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Denial Reasons By Insurance Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsByInsuranceDashboardComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 8, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Claims In Process By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ClaimsInProcessByPayerComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 9, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Average Allowed Amount By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "AverageAllowedAmountByPayerComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "AR Aging Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ARAgingChartComponent", NeedsLayoutFilter = true },
	        //new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Info Cards", Selector = "available", Icon = Icons.Material.Filled.Dashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout },
	        new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Claim Status Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "ClaimStatusDashboardComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Denial Reason Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsDashboardComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 3, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Denial Reasons By Insurance Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsByInsuranceDashboardComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 4, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Initially Reviewed By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "InitiallyReviewedByPayerComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.ComparisonDashboard, Name = "Provider Comparison Grid", Selector = "available", Icon = Icons.Material.Filled.TableChart, Category = ItemCategoryEnum.Grid, Layout = LayoutCategoryEnum.ComparisonDashboardLayout, ComponentTitle = "ProviderComparisonTableComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.ComparisonDashboard, Name = "Provider Visits Stacked Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ComparisonDashboardLayout, ComponentTitle = "ProviderComparisonByVisitComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.ComparisonDashboard, Name = "Provider Visits Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ComparisonDashboardLayout, ComponentTitle = "ProviderComparisonByVisitTotalsComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Insurance Self Pay Review Card", Selector = "available", Icon = Icons.Material.Filled.SpaceDashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "InsurancesSelfPayReviewedComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Eligibilities By Status Insurance Stacked Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "EligibilitiesByStatusInsuranceStackedComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Discovered Eligibilities Bar Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "DiscoveredEligibilitiesBarChartComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Eligibility Monthly Totals Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "EligibilityMonthlyTotalsComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Eligibility Value By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "EligibilityValueByPayerComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Percentage Returned By Month Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "PercentageReturnedByMonthComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.InsurancesDashbaord, Name = "Charges By Insurance Bar Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ChargesByInsuranceBarChartComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.InsurancesDashbaord, Name = "Payments By Insurance Bar Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "PaymentsByInsuranceBarChartComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.InsurancesDashbaord, Name = "Denials By Insurance Bar Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialsByInsuranceBarChartComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.ProcedureLevelsDashboard, Name = "Procedures By Denial Reason Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ProceduresByDenialReasonComponent", NeedsLayoutFilter = true },
            new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.ProcedureLevelsDashboard, Name = "Denials By Procedure Bar Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialsByProcedureBarChartComponent", NeedsLayoutFilter = true },
        };
        private async Task SeedDashboardItems()
        {
            Task.Run(async () =>
            {
                foreach (var item in DashboardItems)
                {
                    var DashboardItemExists = _dbContext.DashboardItems.FirstOrDefault(z => z.Name == item.Name && z.Dashboard == item.Dashboard) ?? null;
                    if (DashboardItemExists is null)
                    {
                        _dbContext.DashboardItems.Add(item);
                    }
                }
            }).GetAwaiter().GetResult();
            _logger.LogInformation("Seeded default DashboardItems");
        }
        #endregion

        public static List<RpaInsuranceGroup> RpaInsuranceGroups = new()
        {
            new RpaInsuranceGroup() { Name = "Availity", DefaultTargetUrl = "https://apps.availity.com/availity/web/public.elegant.login" },
            new RpaInsuranceGroup() { Name = "Carefirst", DefaultTargetUrl = "https://provider.carefirst.com/prv/#/login" },
            new RpaInsuranceGroup() { Name = "Dentaquest", DefaultTargetUrl = "https://connectsso.dentaquest.com/" },
            new RpaInsuranceGroup() { Name = "DentaquestGov", DefaultTargetUrl = "https://govservices.dentaquest.com/" },
            new RpaInsuranceGroup() { Name = "Optum", DefaultTargetUrl = "https://omd.infomc.biz/IPC" },
            new RpaInsuranceGroup() { Name = "Valence", DefaultTargetUrl = "https://mpmdportal.valence.care/" },
            new RpaInsuranceGroup() { Name = "UHC", DefaultTargetUrl = "https://www.uhcprovider.com/" },
            new RpaInsuranceGroup() { Name = "Novitas", DefaultTargetUrl = "https://www.novitasphere.com/hpp/login" },
            new RpaInsuranceGroup() { Name = "EmdHealthChoice", DefaultTargetUrl = "https://encrypt.emdhealthchoice.org/emedicaid/" },
            new RpaInsuranceGroup() { Name = "Cigna", DefaultTargetUrl = "https://cignaforhcp.cigna.com/app/login" },
            new RpaInsuranceGroup() { Name = "MedStar", DefaultTargetUrl = "https://medstarfamilychoiceprofessionalpwp.wonderboxsystem.com/Account/Login" },
            new RpaInsuranceGroup() { Name = "NGSMedicare", DefaultTargetUrl = "https://www.ngsmedicare.com/NGS_LandingPage/Home" },
            new RpaInsuranceGroup() { Name = "CareFirstChpmd", DefaultTargetUrl = "https://providers.carefirstchpmd.com/" },
            new RpaInsuranceGroup() { Name = "HealthTrio", DefaultTargetUrl = "https://ehp.healthtrioconnect.com/app/index.page" },
            new RpaInsuranceGroup() { Name = "LibertyDental", DefaultTargetUrl = "https://libertydentaloffice.b2clogin.com/libertydentaloffice.onmicrosoft.com/b2c_1a_signup_signin/oauth2/v2.0/authorize?client_id=a6fca9b8-8b7e-470b-b6e6-40af4dbf3486&redirect_uri=https%3A%2F%2Fproviderportal.libertydentalplan.com&response_mode=form_post&response_type=code id_token&scope=openid profile offline_access https%3A%2F%2Flibertydentaloffice.onmicrosoft.com%2Fproviderportalapi%2Fproviderapi.read https%3A%2F%2Flibertydentaloffice.onmicrosoft.com%2Fproviderportalapi%2Fproviderapi.write&state=OpenIdConnect.AuthenticationProperties%3DKkPlMj0US36Q-iDAkQMwhilD8ELaSGtu8n_Z178ODXlNvnhQhsflLS4HkWNtlLkRcrfE3bSKNeLNncdnWvJz6R8wIQ2ELdFOYH6kBG9xbbMNgeENg5JVPg9B8Sx87axhjXQCUi_M1Wd-nUkQUEJVqaQBITAfWY55zm5q9yvII1rsYzL606KandX1rhnmj7B75LoV2ZaPSsm_ToJtx0viiM8OjIduWkCTh-pAE_GshWWyOIcecJgcFuMxwo7W5uOT&nonce=638175093581397608.YzJkNmFjMmEtYWZlMC00ZTEyLTgzNjQtMjExNGQ3MjYzZjUzMWU0MzRjYTItZGRjNS00ZDhlLWE5NDUtODk3YWE0MWU1ZDBh&x-client-SKU=ID_NET451&x-client-ver=5.2.4.0#id_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IkNzWWMyTlo1cXBBakZDYnpKR2lqbll3YTloNUl6RHkwLWF2WDJ1dk5hNUkifQ.eyJleHAiOjE2ODE5MTYxNTgsIm5iZiI6MTY4MTkxMjU1OCwidmVyIjoiMS4wIiwiaXNzIjoiaHR0cHM6Ly9saWJlcnR5ZGVudGFsb2ZmaWNlLmIyY2xvZ2luLmNvbS9iYzkwNmVmMC0" },
            new RpaInsuranceGroup() { Name = "Oscar", DefaultTargetUrl = "https://accounts.hioscar.com/account/login/?client_context=provider" },
            new RpaInsuranceGroup() { Name = "CareFirstAdmin", DefaultTargetUrl = "https://ebixhub.ebix.com/sso/client/ClientLogin.aspx?client=cfablue" },
            new RpaInsuranceGroup() { Name = "Tricare", DefaultTargetUrl = "https://www.tricare4u.com/wps/portal/tdp/login/" },
            new RpaInsuranceGroup() { Name = "Meridian", DefaultTargetUrl = "https://www.mimeridian.com/providers.html" },
            new RpaInsuranceGroup() { Name = "Navinet", DefaultTargetUrl = "https://identity.navinet.net/" },
            new RpaInsuranceGroup() { Name = "BlueCrossNc", DefaultTargetUrl = "https://bluee.bcbsnc.com/providers/web/login" },
            new RpaInsuranceGroup() { Name = "NCMMIS", DefaultTargetUrl = "https://www.nctracks.nc.gov/ncmmisPortal/loginAction?flow=default&deepLink=/ncmmisPortal/login&version=NO-COOKIE-VERSION" },
            new RpaInsuranceGroup() { Name = "WellCare", DefaultTargetUrl = "https://provider.wellcare.com/" },
            new RpaInsuranceGroup() { Name = "CollabMD", DefaultTargetUrl = "" },
            new RpaInsuranceGroup() { Name = "PaySpan", DefaultTargetUrl = "" },
            new RpaInsuranceGroup() { Name = "CFBCBS", DefaultTargetUrl = "" },
            new RpaInsuranceGroup() { Name = "HealthyBlue", DefaultTargetUrl = "" },
            new RpaInsuranceGroup() { Name = "Friday", DefaultTargetUrl = "https://providers.fridayhealthplans.com/authentication" },
            new RpaInsuranceGroup() { Name = "WVMedicaid", DefaultTargetUrl = "https://www.wvmmis.com/default.aspx" },
            new RpaInsuranceGroup() { Name = "HumanaMilitary", DefaultTargetUrl = "https://infocenter.humana-military.com/provider/service/Account/Login" },
            new RpaInsuranceGroup() { Name = "UMR", DefaultTargetUrl = "https://provider.umr.com/tpa-ap-web/navigateTo?forcemainsite=true&userType=Provider" },
            new RpaInsuranceGroup() { Name = "SunshineHealth", DefaultTargetUrl = "https://provider.sunshinehealth.com/" },
            new RpaInsuranceGroup() { Name = "SkyGen", DefaultTargetUrl = "https://app.dentalhub.com/app/login" },
            new RpaInsuranceGroup() { Name = "CTMedicaid", DefaultTargetUrl = "https://www.ctdssmap.com/CTPortal/Provider" },
            new RpaInsuranceGroup() { Name = "NYMedicaid", DefaultTargetUrl = "https://visionacrd.netsmartcloud.com" }, //THis is a client specific. NEed to change the address to actual clearinghouse. https://portal.focusedi.com/system/login? Need paertner ID added to config. 
            new RpaInsuranceGroup() { Name = "GAMedicaid", DefaultTargetUrl = "https://evv-dashboard.4tellus.net" }, //"https://public.gammis.com/home" },
			new RpaInsuranceGroup() { Name = "NYePacesMA", DefaultTargetUrl = "https://epaces.emedny.org/Login.aspx" },
            new RpaInsuranceGroup() { Name = "VA Community Care Network", DefaultTargetUrl = "https://www.myvaccn.com" },
            new RpaInsuranceGroup() { Name = "Anthem New York", DefaultTargetUrl = "https://mltcprovider.anthem.com" },
            new RpaInsuranceGroup() { Name = "Fidelis Care New York", DefaultTargetUrl = "https://providers.fideliscare.org/Login?returnurl=%2f" },
            new RpaInsuranceGroup() { Name = "AlterwoodAdv", DefaultTargetUrl = "https://my.alterwoodadvantage.com/providers/" },
            new RpaInsuranceGroup() { Name = "VNSHealth", DefaultTargetUrl = "https://providerportal.vnshealthplans.org/login-redirect" },
            new RpaInsuranceGroup() {Name = "Unknown", DefaultTargetUrl = ""}
        };

        private async Task SeedDefaultRpaInsuranceGroup()
        {
            try
            {
                foreach (var item in RpaInsuranceGroups)
                {
                    var rpaInsuranceGroup = _dbContext.RpaInsuranceGroups.FirstOrDefault(z => z.Name == item.Name);
                    if (rpaInsuranceGroup == null)
                    {
                        _dbContext.RpaInsuranceGroups.Add(item);
                    }
                    else
                    {
                        rpaInsuranceGroup.DefaultTargetUrl = item.DefaultTargetUrl;
                        _dbContext.RpaInsuranceGroups.Update(rpaInsuranceGroup);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding default RpaInsuranceGroups.");
                throw;
            }
            _logger.LogInformation("Seeded Default RpaInsuranceGroups");
        }

        //private async Task SeedDefaultRpaCredentialConfiguration() //AA-23
        //{
        //    Task.Run(async () =>
        //    {
        //        //Check if config Exists
        //        if (!_dbContext.ClientRpaCredentialConfigurations.Any())
        //        {
        //            var defaultClientRpaCredentialConfiguration = new ClientRpaCredentialConfiguration()
        //            {
        //                RpaInsuranceGroupId = 1,
        //                Username = "defaultConfigUsername",
        //                Password = "defaultConfigPassword",
        //                ReportFailureToEmail = "defaultReportFailureToEmail",
        //                IsCredentialInUse = false,
        //                UseOffHoursOnly = false,
        //            };

        //            _dbContext.ClientRpaCredentialConfigurations.Add(defaultClientRpaCredentialConfiguration);
        //            _dbContext.SaveChanges();
        //        }
        //    }).GetAwaiter().GetResult();

        //    _logger.LogInformation("Seeded Default ClientRpaCredentialConfigurations");

        //}
        private async Task SeedSystemDefaultCustomReportFilters()//EN-110
        {
            ///Seed a report definition in SystemDefaultReportFilter.
            await Task.Run(async () =>
            {
                var savedUserReportArray = await _dbContext.ClientUserReportFilters
                                                    .Include(z => z.SystemDefaultReportFilter)
                                                    .Where(z => z.ReportId == ReportsEnum.Custom_Reports)
                                                    .ToListAsync() ?? new List<ClientUserReportFilter>();
                ///Get Employee Roles.
                var employeeAssignedRoles = Enum.GetValues(typeof(EmployeeRoleEnum)).Cast<EmployeeRoleEnum>().ToList() ?? new List<EmployeeRoleEnum>();//.Take(5)

                ///As per conversation with kevin, Jim will create  List of custom reports and he'll tell us the list of reports [report name] which he wants to set for all employee/clients as default.
                ///Then we will grab those reports configurations and create new system reports based on ReportId.
                ///List of roles and report configuration, reportType Id.

                var systemDefaultReports = await _dbContext.SystemDefaultReportFilters
                                                                .Include(z => z.SystemDefaultReportFilterEmployeeRoles)
                                                                .Where(z => z.IsActive && z.ReportId == ReportsEnum.Custom_Reports)
                                                                .ToListAsync() ?? new List<SystemDefaultReportFilter>();

                if (savedUserReportArray.Any())
                {
                    ///Iterate over savedUserReportArray
                    foreach (var userReport in savedUserReportArray)
                    {
                        if (!userReport.SystemDefaultReportFilterId.HasValue)
                        {
                            try
                            {

                                ///Create new entry.
                                SystemDefaultReportFilter systemDefaultReport = new()
                                {
                                    IsActive = true,
                                    ReportId = userReport.ReportId,
                                    FilterConfiguration = userReport.FilterConfiguration,
                                    FilterName = userReport.FilterName,
                                };
                                _dbContext.SystemDefaultReportFilters.Add(systemDefaultReport);
                                _dbContext.SaveChanges();

                                //Update filter configurations.
                                if (!systemDefaultReport.SystemDefaultReportFilterEmployeeRoles?.Any() ?? false)
                                {
                                    var systemDefaultReportFilterEmployeeRoles = employeeAssignedRoles.Select(role => new SystemDefaultReportFilterEmployeeRole()
                                    {
                                        SystemDefaultReportFilterId = systemDefaultReport.Id,
                                        EmployeeRoleId = role
                                    }).ToList();

                                    systemDefaultReport.SystemDefaultReportFilterEmployeeRoles = systemDefaultReportFilterEmployeeRoles;
                                }

                                ///Update SystemDefaultReportFilterId in Client User reports.
                                if (!userReport.SystemDefaultReportFilterId.HasValue)
                                {
                                    userReport.SystemDefaultReportFilterId = systemDefaultReport.Id;
                                    _dbContext.ClientUserReportFilters.Update(userReport);
                                }

                                //Update filter configurations
                                systemDefaultReport.FilterConfiguration = userReport.FilterConfiguration;
                                _dbContext.SystemDefaultReportFilters.Update(systemDefaultReport);
                                _dbContext.SaveChanges();

                            }
                            catch (Exception e)
                            {

                                throw;
                            }
                        }
                        else
                        {
                            var systemDefaultReport = systemDefaultReports.FirstOrDefault(z => z.Id == userReport.SystemDefaultReportFilterId);

                            //Update filter configurations.
                            if (!systemDefaultReport.SystemDefaultReportFilterEmployeeRoles?.Any() ?? false)
                            {
                                var systemDefaultReportFilterEmployeeRoles = employeeAssignedRoles.Select(role => new SystemDefaultReportFilterEmployeeRole()
                                {
                                    SystemDefaultReportFilterId = systemDefaultReport.Id,
                                    EmployeeRoleId = role
                                }).ToList();

                                systemDefaultReport.SystemDefaultReportFilterEmployeeRoles = systemDefaultReportFilterEmployeeRoles;
                            }

                            ///Update SystemDefaultReportFilterId in Client User reports.
                            if (!userReport.SystemDefaultReportFilterId.HasValue)
                            {
                                userReport.SystemDefaultReportFilterId = systemDefaultReport.Id;
                                _dbContext.ClientUserReportFilters.Update(userReport);
                            }

                            //Update filter configurations
                            systemDefaultReport.FilterConfiguration = userReport.FilterConfiguration;
                            _dbContext.SystemDefaultReportFilters.Update(systemDefaultReport);
                            _dbContext.SaveChanges();
                        }
                    }
                }

            });
            //await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeded system default CustomReport");
        }

        #region Seed X12 References and related
        public static List<X12ClaimCategoryCodeLineItemStatus> X12ClaimCategoryCodeLineItemStatuses = new()
        {
            // Supplemental 

            //Acknowledgements
			new X12ClaimCategoryCodeLineItemStatus("A0", "Acknowledgement/Forwarded-The claim/encounter has been forwarded to another entity.", X12ClaimCategoryEnum.Acknowledgment, null), //ToDO: What should this be? Typically they would only forward on if they paid it and want to send to secondary? 
			new X12ClaimCategoryCodeLineItemStatus("A1", "Acknowledgement/Receipt-The claim/encounter has been received. This does not mean that the claim has been accepted for adjudication.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.Received),
            new X12ClaimCategoryCodeLineItemStatus("A2", "Acknowledgement/Acceptance into adjudication system-The claim/encounter has been accepted into the adjudication system.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.NotAdjudicated),
            new X12ClaimCategoryCodeLineItemStatus("A3", "Acknowledgement/Returned as unprocessable claim-The claim/encounter has been rejected and has not been entered into the adjudication system.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("A4", "Acknowledgement/Not Found-The claim/encounter can not be found in the adjudication system.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.NotOnFile),
            new X12ClaimCategoryCodeLineItemStatus("A5", "Acknowledgement/Split Claim-The claim/encounter has been split upon acceptance into the adjudication system.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.NotAdjudicated),
            new X12ClaimCategoryCodeLineItemStatus("A6", "Acknowledgement/Rejected for Missing Information - The claim/encounter is missing the information specified in the Status details and has been rejected.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("A7", "Acknowledgement/Rejected for Invalid Information - The claim/encounter has invalid information as specified in the Status details and has been rejected.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("A8", "Acknowledgement/Rejected for relational field in error.", X12ClaimCategoryEnum.Acknowledgment, ClaimLineItemStatusEnum.Rejected),

            //Data Reporting Acknowledgments
			new X12ClaimCategoryCodeLineItemStatus("DR01", "Acknowledgement/Receipt - The claim/encounter has been received. This does not mean the claim has been accepted into the data reporting/processing system. Usage: Can only be used in the Data Reporting Acknowledgement Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.Received),
            new X12ClaimCategoryCodeLineItemStatus("DR02", "Acknowledgement/Acceptance into the data reporting/processing system - The claim/encounter has been accepted into the data reporting/processing system. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.NotAdjudicated),
            new X12ClaimCategoryCodeLineItemStatus("DR03", "Acknowledgement/Returned as unprocessable claim - The claim/encounter has been rejected and has not been entered into the data reporting/processing system. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("DR04", "Acknowledgement/Not Found - The claim/encounter can not be found in the data reporting/processing system. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.NotOnFile),
            new X12ClaimCategoryCodeLineItemStatus("DR05", "Acknowledgement/Rejected for Missing Information - The claim/encounter is missing the information specified in the Status details and has been rejected. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("DR06", "Acknowledgment/Rejected for invalid information - The claim/encounter has invalid information as specified in the Status details and has been rejected. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("DR07", "Acknowledgement/Rejected for relational field in error. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.Rejected),
            new X12ClaimCategoryCodeLineItemStatus("DR08", "Acknowledgement/Warning - The claim/encounter has been accepted into the data reporting/processing system but has received a warning as specified in the Status details. Usage: Can only be used in the Data Reporting Acknowledgment Transaction.", X12ClaimCategoryEnum.DataReportingAcknowledgment, ClaimLineItemStatusEnum.NotAdjudicated),

            //Pending
			new X12ClaimCategoryCodeLineItemStatus("P0", "Pending: Adjudication/Details-This is a generic message about a pended claim. A pended claim is one for which no remittance advice has been issued, or only part of the claim has been paid.", X12ClaimCategoryEnum.Pending, ClaimLineItemStatusEnum.Approved),
            new X12ClaimCategoryCodeLineItemStatus("P1", "Pending/In Process-The claim or encounter is in the adjudication system.", X12ClaimCategoryEnum.Pending, ClaimLineItemStatusEnum.NotAdjudicated),
            new X12ClaimCategoryCodeLineItemStatus("P2", "Pending/Payer Review-The claim/encounter is suspended and is pending review (e.g. medical review, repricing, Third Party Administrator processing).", X12ClaimCategoryEnum.Pending, ClaimLineItemStatusEnum.Pended),
            new X12ClaimCategoryCodeLineItemStatus("P3", "Pending/Provider Requested Information - The claim or encounter is waiting for information that has already been requested from the provider. (Usage: A Claim Status Code identifying the type of information requested, must be reported)", X12ClaimCategoryEnum.Pending, ClaimLineItemStatusEnum.Pended),
            new X12ClaimCategoryCodeLineItemStatus("P4", "Pending/Patient Requested Information - The claim or encounter is waiting for information that has already been requested from the patient. (Usage: A status code identifying the type of information requested must be sent)", X12ClaimCategoryEnum.Pending, ClaimLineItemStatusEnum.Pended),
            new X12ClaimCategoryCodeLineItemStatus("P5", "Pending/Payer Administrative/System hold", X12ClaimCategoryEnum.Pending, ClaimLineItemStatusEnum.NeedsReview),

            //Finalized
			new X12ClaimCategoryCodeLineItemStatus("F0",  "Finalized-The claim/encounter has completed the adjudication cycle and no more action will be taken.", X12ClaimCategoryEnum.Finalized, null),
            new X12ClaimCategoryCodeLineItemStatus("F1",  "Finalized/Payment-The claim/line has been paid.", X12ClaimCategoryEnum.Finalized, ClaimLineItemStatusEnum.Paid),
            new X12ClaimCategoryCodeLineItemStatus("F2",  "Finalized/Denial-The claim/line has been denied.", X12ClaimCategoryEnum.Finalized, ClaimLineItemStatusEnum.Denied),
            new X12ClaimCategoryCodeLineItemStatus("F3",  "Finalized/Revised - Adjudication information has been changed", X12ClaimCategoryEnum.Finalized, null),
            new X12ClaimCategoryCodeLineItemStatus("F3F", "Finalized/Forwarded-The claim/encounter processing has been completed. Any applicable payment has been made and the claim/encounter has been forwarded to a subsequent entity as identified on the original claim or in this payer's records.", X12ClaimCategoryEnum.Finalized, ClaimLineItemStatusEnum.Paid),
            new X12ClaimCategoryCodeLineItemStatus("F3N", "Finalized/Not Forwarded-The claim/encounter processing has been completed. Any applicable payment has been made. The claim/encounter has NOT been forwarded to any subsequent entity identified on the original claim.", X12ClaimCategoryEnum.Finalized, ClaimLineItemStatusEnum.Paid),
            new X12ClaimCategoryCodeLineItemStatus("F4",  "Finalized/Adjudication Complete - No payment forthcoming-The claim/encounter has been adjudicated and no further payment is forthcoming.", X12ClaimCategoryEnum.Finalized, null),

            //Requests for additional information
			new X12ClaimCategoryCodeLineItemStatus("R1", "Requests for additional Information/General Requests-Requests that don't fall into other R-type categories.", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R2", "Requests for additional Information/Entity Requests-Requests for information about specific entities (subscribers, patients, various providers).", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R3", "Requests for additional Information/Claim/Line-Requests for information that could normally be submitted on a claim.", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R4", "Requests for additional Information/Documentation-Requests for additional supporting documentation. Examples: certification, x-ray, notes.", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R5", "Request for additional information/more specific detail-Additional information as a follow up to a previous request is needed. The original information was received but is inadequate. More specific/detailed information is requested.", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R6", "Requests for additional information – Regulatory requirements", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R7", "Requests for additional information – Confirm care is consistent with Health Plan policy coverage", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R8", "Requests for additional information – Confirm care is consistent with health plan coverage exceptions", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R9", "Requests for additional information – Determination of medical necessity", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R10", "Requests for additional information – Support a filed grievance or appeal", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R11", "Requests for additional information – Pre-payment review of claims", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R12", "Requests for additional information – Clarification or justification of use for specified procedure code", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R13", "Requests for additional information – Original documents submitted are not readable. Used only for subsequent request(s).", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R14", "Requests for additional information – Original documents received are not what was requested. Used only for subsequent request(s).", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R15", "Requests for additional information – Workers Compensation coverage determination.", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R16", "Requests for additional information – Eligibility determination", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimCategoryCodeLineItemStatus("R17", "Replacement of a Prior Request. Used to indicate that the current attachment request replaces a prior attachment request.", X12ClaimCategoryEnum.RequestsForAdditionalInfo, ClaimLineItemStatusEnum.NeedsReview),

            //General

            //Error
			new X12ClaimCategoryCodeLineItemStatus("E0", "Response not possible - error on submitted request data", X12ClaimCategoryEnum.Error, ClaimLineItemStatusEnum.TransientError),
            new X12ClaimCategoryCodeLineItemStatus("E1", "Response not possible - System Status", X12ClaimCategoryEnum.Error, ClaimLineItemStatusEnum.TransientError),
            new X12ClaimCategoryCodeLineItemStatus("E2", "Information Holder is not responding; resubmit at a later time.", X12ClaimCategoryEnum.Error, ClaimLineItemStatusEnum.TransientError),
            new X12ClaimCategoryCodeLineItemStatus("E3", "Correction required - relational fields in error.", X12ClaimCategoryEnum.Error, ClaimLineItemStatusEnum.Error),
            new X12ClaimCategoryCodeLineItemStatus("E4", "Trading partner agreement specific requirement not met: Data correction required. (Usage: A status code identifying the type of information requested must be sent)", X12ClaimCategoryEnum.Error, ClaimLineItemStatusEnum.Error),

            //Searches
            new X12ClaimCategoryCodeLineItemStatus("D0", "Data Search Unsuccessful - The payer is unable to return status on the requested claim(s) based on the submitted search criteria.", X12ClaimCategoryEnum.Error, ClaimLineItemStatusEnum.TransientError),
        };



        private async Task SeedX12ClaimCategoryCodeLineItemStatuses()
        {
            await Task.Run(async () =>
            {
                foreach (var item in X12ClaimCategoryCodeLineItemStatuses)
                {
                    var catCodeStatus = await _dbContext.X12ClaimCategoryCodeLineItemStatuses.FirstOrDefaultAsync(x => x.Code == item.Code) ?? null;
                    if (catCodeStatus is null)
                    {
                        _dbContext.X12ClaimCategoryCodeLineItemStatuses.Add(item);
                    }
                    else
                    {
                        catCodeStatus.Code = item.Code;
                        catCodeStatus.Description = item.Description;
                        catCodeStatus.ClaimLineItemStatusId = item.ClaimLineItemStatusId;
                        _dbContext.X12ClaimCategoryCodeLineItemStatuses.Update(catCodeStatus);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            });
            _logger.LogInformation("Seeded X12ClaimCategoryCodeLineItemStatuses");
        }

        private async Task SeedX12ClaimCodesFromDataFiles()
        {
            //Task.Run(async () =>
            //{
            //if (_dbContext.X12ClaimCodeLineItemStatuses.Any())
            //	return;

            //var resourceNames = this.GetType().Assembly.GetManifestResourceNames();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MedHelpAuthorizations.Infrastructure.SeedData.X12ClaimCodes_1.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                CsvConfiguration csvConfig = new(CultureInfo.InvariantCulture) { Delimiter = ",", IgnoreBlankLines = true, HasHeaderRecord = true, MissingFieldFound = null };

                using (var reader = new StreamReader(stream))
                using (var csvReader = new CsvReader(reader, csvConfig))
                {
                    try
                    {
                        csvReader.Context.RegisterClassMap<X12ClaimCodeLineItemStatusCsvModel.X12ClaimCodeLineItemStatusCsvModelMap>();

                        csvReader.Read();
                        csvReader.ReadHeader();

                        var records = csvReader.GetRecords<X12ClaimCodeLineItemStatusCsvModel>().ToList();

                        // Loop through the List and show them in Console
                        foreach (var record in records)
                        {
                            var x12ClaimCodelineItemStatus = await _dbContext.X12ClaimCodeLineItemStatuses.FirstOrDefaultAsync(x => x.Code == record.Code && x.X12ClaimCodeTypeId == record.X12ClaimCodeTypeId);
                            if (x12ClaimCodelineItemStatus == null)
                            {
                                x12ClaimCodelineItemStatus = new X12ClaimCodeLineItemStatus()
                                {
                                    ClaimLineItemStatusId = record.ClaimLineItemStatusId,
                                    ClaimStatusExceptionReasonCategoryId = record.ClaimStatusExceptionReasonCategoryId,
                                    Code = record.Code?.Trim(),
                                    Description = record.Description?.Trim(),
                                    X12ClaimCodeTypeId = record.X12ClaimCodeTypeId
                                };
                                _ = await _dbContext.X12ClaimCodeLineItemStatuses.AddAsync(x12ClaimCodelineItemStatus);
                            }
                            else
                            {
                                x12ClaimCodelineItemStatus.ClaimLineItemStatusId = record.ClaimLineItemStatusId;
                                x12ClaimCodelineItemStatus.ClaimStatusExceptionReasonCategoryId = record.ClaimStatusExceptionReasonCategoryId;
                                x12ClaimCodelineItemStatus.Code = record.Code?.Trim();
                                x12ClaimCodelineItemStatus.Description = record.Description?.Trim();
                                x12ClaimCodelineItemStatus.X12ClaimCodeTypeId = record.X12ClaimCodeTypeId;
                                _ = _dbContext.X12ClaimCodeLineItemStatuses.Update(x12ClaimCodelineItemStatus);
                            }
                            _dbContext.SaveChanges();
                        }
                    }
                    catch (CsvHelper.HeaderValidationException exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }
        }

        private async Task SeedCptCodesFromDataFilesAsync()
        {
            // Uncomment if you want to skip the entire process if any records exist
            // if (_dbContext.CptCodes.Any())
            //     return;

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MedHelpAuthorizations.Infrastructure.SeedData.HCPC2020_ANWEB_w_disclaimer.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                CsvConfiguration csvConfig = new(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    IgnoreBlankLines = true,
                    HasHeaderRecord = true,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(stream))
                using (var csvReader = new CsvReader(reader, csvConfig))
                {
                    try
                    {
                        await csvReader.ReadAsync();
                        csvReader.ReadHeader();

                        var records = csvReader.GetRecords<CptCode>().ToList();

                        // Get the existing codes from the database
                        var existingCodes = new HashSet<string>(_dbContext.CptCodes.Select(c => c.Code));

                        foreach (var record in records)
                        {
                            if (!existingCodes.Contains(record.Code))
                            {
                                await _dbContext.CptCodes.AddAsync(record);
                            }
                        }

                        await _dbContext.SaveChangesAsync();
                    }
                    catch (CsvHelper.HeaderValidationException exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }
            _logger.LogInformation("SeedCptCodesFromDataFilesAsync completed.");
        }

        public static List<X12ClaimStatusCode> X12ClaimStatusCodes = new()
        {
            new X12ClaimStatusCode("0", "Cannot provide further status electronically.", ClaimLineItemStatusEnum.Unknown),
            new X12ClaimStatusCode("1", "For more detailed information, see remittance advice.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("2", "More detailed information in letter.", ClaimLineItemStatusEnum.Approved),
            new X12ClaimStatusCode("3", "Claim has been adjudicated and is awaiting payment cycle.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("6", "Balance due from the subscriber.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("12", "One or more originally submitted procedure codes have been combined.", ClaimLineItemStatusEnum.BundledFqhc),
            new X12ClaimStatusCode("15", "One or more originally submitted procedure codes have been modified.", ClaimLineItemStatusEnum.UnMatchedProcedureCode),
            new X12ClaimStatusCode("16", "Claim/encounter has been forwarded to entity.", ClaimLineItemStatusEnum.Rebilled),
            new X12ClaimStatusCode("17", "Claim/encounter has been forwarded by third party entity to entity.", ClaimLineItemStatusEnum.Rebilled),
            new X12ClaimStatusCode("18", "Entity received claim/encounter, but returned invalid status.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("19", "Entity acknowledges receipt of claim/encounter.", ClaimLineItemStatusEnum.Received),
            new X12ClaimStatusCode("20", "Accepted for processing.", ClaimLineItemStatusEnum.Received),
            new X12ClaimStatusCode("21", "Missing or invalid information.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("23", "Returned to Entity.", ClaimLineItemStatusEnum.Returned),
            new X12ClaimStatusCode("24", "Entity not approved as an electronic submitter.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("25", "Entity not approved.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("26", "Entity not found.", ClaimLineItemStatusEnum.MemberNotFound),
            new X12ClaimStatusCode("27", "Policy canceled.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("29", "Subscriber and policy number/contract number mismatched.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("30", "Subscriber and subscriber id mismatched.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("31", "Subscriber and policyholder name mismatched.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("32", "Subscriber and policy number/contract number not found.", ClaimLineItemStatusEnum.MemberNotFound),
            new X12ClaimStatusCode("33", "Subscriber and subscriber id not found.", ClaimLineItemStatusEnum.MemberNotFound),
            new X12ClaimStatusCode("34", "Subscriber and policyholder name not found.", ClaimLineItemStatusEnum.MemberNotFound),
            new X12ClaimStatusCode("35", "Claim/encounter not found.", ClaimLineItemStatusEnum.NotOnFile),
            new X12ClaimStatusCode("37", "Predetermination is on file, awaiting completion of services.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("38", "Awaiting next periodic adjudication cycle.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("39", "Charges for pregnancy deferred until delivery.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("40", "Waiting for final approval.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("41", "Special handling required at payer site.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("42", "Awaiting related charges.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("44", "Charges pending provider audit.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("45", "Awaiting benefit determination.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("46", "Internal review/audit.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("47", "Internal review/audit - partial payment made.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("49", "Pending provider accreditation review.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("50", "Claim waiting for internal provider verification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("51", "Investigating occupational illness/accident.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("52", "Investigating existence of other insurance coverage.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("53", "Claim being researched for Insured ID/Group Policy Number error.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("54", "Duplicate of a previously processed claim/line.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("55", "Claim assigned to an approver/analyst.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("56", "Awaiting eligibility determination.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("57", "Pending COBRA information requested.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("59", "Information was requested by a non-electronic method.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("60", "Information was requested by an electronic method.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("61", "Eligibility for extended benefits.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("64", "Re-pricing information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("65", "Claim/line has been paid.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("66", "Payment reflects usual and customary charges.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("72", "Claim contains split payment.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("73", "Payment made to entity, assignment of benefits not on file.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("78", "Duplicate of an existing claim/line, awaiting processing.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("81", "Contract/plan does not cover pre-existing conditions.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("83", "No coverage for newborns.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("84", "Service not authorized.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("85", "Entity not primary.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("86", "Diagnosis and patient gender mismatch.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("88", "Entity not eligible for benefits for submitted dates of service.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("89", "Entity not eligible for dental benefits for submitted dates of service.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("90", "Entity not eligible for medical benefits for submitted dates of service.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("91", "Entity not eligible/not approved for dates of service.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("92", "Entity does not meet dependent or student qualification.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("93", "Entity is not selected primary care provider.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("94", "Entity not referred by selected primary care provider.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("95", "Requested additional information not received.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("96", "No agreement with entity.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("97", "Patient eligibility not found with entity.", ClaimLineItemStatusEnum.MemberNotFound),
            new X12ClaimStatusCode("98", "Charges applied to deductible.", ClaimLineItemStatusEnum.ZeroPay),
            new X12ClaimStatusCode("99", "Pre-treatment review.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("100", "Pre-certification penalty taken.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("101", "Claim was processed as adjustment to previous claim.", ClaimLineItemStatusEnum.Rebilled),
            new X12ClaimStatusCode("102", "Newborn's charges processed on mother's claim.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("103", "Claim combined with other claim(s).", ClaimLineItemStatusEnum.BundledFqhc),
            new X12ClaimStatusCode("104", "Processed according to plan provisions.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("105", "Claim/line is capitated.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("106", "This amount is not entity's responsibility.", ClaimLineItemStatusEnum.ZeroPay),
            new X12ClaimStatusCode("107", "Processed according to contract provisions.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("109", "Entity not eligible.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("110", "Claim requires pricing information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("111", "At the policyholder's request these claims cannot be submitted electronically.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("114", "Claim/service should be processed by entity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("116", "Claim submitted to incorrect payer.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("117", "Claim requires signature-on-file indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("121", "Service line number greater than maximum allowable for payer.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("123", "Additional information requested from entity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("124", "Entity's name, address, phone and ID number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("125", "Entity's name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("126", "Entity's address.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("127", "Entity's Communication Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("128", "Entity's tax ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("129", "Entity's Blue Cross provider ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("130", "Entity's Blue Shield provider ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("131", "Entity's Medicare provider ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("132", "Entity's Medicaid provider ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("133", "Entity's UPIN.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("134", "Entity's TRICARE provider ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("135", "Entity's commercial provider ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("136", "Entity's health industry ID number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("137", "Entity's plan network ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("138", "Entity's site ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("139", "Entity's health maintenance provider ID (HMO).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("140", "Entity's preferred provider organization ID (PPO).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("141", "Entity's administrative services organization ID (ASO).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("142", "Entity's license/certification number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("143", "Entity's state license number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("144", "Entity's specialty license number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("145", "Entity's specialty/taxonomy code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("146", "Entity's anesthesia license number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("147", "Entity's qualification degree/designation (e.g., RN, PhD, MD).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("148", "Entity's social security number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("149", "Entity's employer ID.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("150", "Entity's drug enforcement agency (DEA) number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("152", "Processor Control Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("153", "Entity's ID number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("154", "Relationship of surgeon & assistant surgeon.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("155", "Entity's relationship to patient.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("156", "Patient relationship to subscriber.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("157", "Entity's Gender.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("158", "Entity's date of birth.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("159", "Entity's date of death.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("160", "Entity's marital status.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("161", "Entity's employment status.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("162", "Entity's health insurance claim number (HICN).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("163", "Entity's policy/group number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("164", "Entity's contract/member number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("165", "Entity's employer name, address and phone.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("166", "Entity's employer name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("167", "Entity's employer address.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("168", "Entity's employer phone number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("170", "Entity's employee id.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("171", "Other insurance coverage information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("172", "Other employer name, address and telephone number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("173", "Entity's name, address, phone, gender, DOB, marital status, employment status and relation to subscriber.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("174", "Entity's student status.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("175", "Entity's school name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("176", "Entity's school address.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("177", "Transplant recipient's name, date of birth, gender, relationship to insured.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("178", "Total Claim Charge Amount.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("179", "Outside lab charges.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("180", "Hospital's semi-private room rate.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("181", "Hospital's room rate.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("182", "Allowable/paid from other entities coverage.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("183", "Amount entity has paid.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("184", "Purchase price for the rented durable medical equipment.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("185", "Rental price for durable medical equipment.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("186", "Purchase and rental price of durable medical equipment.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("187", "Date(s) of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("188", "Statement from-through dates.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("189", "Facility admission date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("190", "Facility discharge date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("191", "Date of Last Menstrual Period (LMP).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("192", "Date of first service for current series/symptom/illness.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("193", "First consultation/evaluation date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("194", "Confinement dates.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("195", "Unable to work dates/Disability Dates.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("196", "Return to work dates.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("197", "Effective coverage date(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("198", "Medicare effective date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("199", "Date of conception and expected date of delivery.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("200", "Date of equipment return.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("201", "Date of dental appliance prior placement.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("202", "Date of dental prior replacement/reason for replacement.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("203", "Date of dental appliance placed.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("204", "Date dental canal(s) opened and date service completed.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("205", "Date(s) dental root canal therapy previously performed.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("206", "Most recent date of curettage, root planing, or periodontal surgery.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("207", "Dental impression and seating date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("208", "Most recent date pacemaker was implanted.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("209", "Most recent pacemaker battery change date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("210", "Date of the last x-ray.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("211", "Date(s) of dialysis training provided to patient.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("212", "Date of last routine dialysis.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("213", "Date of first routine dialysis.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("214", "Original date of prescription/orders/referral.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("215", "Date of tooth extraction/evolution.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("216", "Drug information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("217", "Drug name, strength and dosage form.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("218", "NDC number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("219", "Prescription number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("222", "Drug dispensing units and average wholesale price (AWP).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("223", "Route of drug/myelogram administration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("224", "Anatomical location for joint injection.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("225", "Anatomical location.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("226", "Joint injection site.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("227", "Hospital information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("228", "Type of bill for UB claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("229", "Hospital admission source.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("230", "Hospital admission hour.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("231", "Hospital admission type.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("232", "Admitting diagnosis.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("233", "Hospital discharge hour.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("234", "Patient discharge status.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("235", "Units of blood furnished.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("236", "Units of blood replaced.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("237", "Units of deductible blood.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("238", "Separate claim for mother/baby charges.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("239", "Dental information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("240", "Tooth surface(s) involved.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("241", "List of all missing teeth (upper and lower).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("242", "Tooth numbers, surfaces, and/or quadrants involved.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("243", "Months of dental treatment remaining.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("244", "Tooth number or letter.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("245", "Dental quadrant/arch.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("246", "Total orthodontic service fee, initial appliance fee, monthly fee, length of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("247", "Line information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("249", "Place of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("250", "Type of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("251", "Total anesthesia minutes.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("252", "Entity's prior authorization/certification number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("254", "Principal diagnosis code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("255", "Diagnosis code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("256", "DRG code(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("257", "ADSM-III-R code for services rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("258", "Days/units for procedure/revenue code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("259", "Frequency of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("260", "Length of medical necessity, including begin date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("261", "Obesity measurements.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("262", "Type of surgery/service for which anesthesia was administered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("263", "Length of time for services rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("264", "Number of liters/minute & total hours/day for respiratory support.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("265", "Number of lesions excised.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("266", "Facility point of origin and destination - ambulance.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("267", "Number of miles patient was transported.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("268", "Location of durable medical equipment use.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("269", "Length/size of laceration/tumor.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("270", "Subluxation location.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("271", "Number of spine segments.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("272", "Oxygen contents for oxygen system rental.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("273", "Weight.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("274", "Height.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("275", "Claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("276", "UB04/HCFA-1450/1500 claim form.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("277", "Paper claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("279", "Claim/service must be itemized.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("281", "Related confinement claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("282", "Copy of prescription.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("283", "Medicare entitlement information is required to determine primary coverage.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("284", "Copy of Medicare ID card.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("286", "Other payer's Explanation of Benefits/payment information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("287", "Medical necessity for service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("288", "Hospital late charges.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("290", "Pre-existing information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("291", "Reason for termination of pregnancy.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("292", "Purpose of family conference/therapy.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("293", "Reason for physical therapy.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("294", "Supporting documentation.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("295", "Attending physician report.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("296", "Nurse's notes.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("297", "Medical notes/report.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("298", "Operative report.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("299", "Emergency room notes/report.", ClaimLineItemStatusEnum.NeedsReview),
             new X12ClaimStatusCode("300", "Lab/test report/notes/results.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("301", "MRI report.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("305", "Radiology/x-ray reports and/or interpretation.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("306", "Detailed description of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("307", "Narrative with pocket depth chart.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("308", "Discharge summary.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("310", "Progress notes for the six months prior to statement date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("311", "Pathology notes/report.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("312", "Dental charting.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("313", "Bridgework information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("314", "Dental records for this service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("315", "Past perio treatment history.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("316", "Complete medical history.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("318", "X-rays/radiology films.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("319", "Pre/post-operative x-rays/photographs.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("320", "Study models.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("322", "Recent Full Mouth X-rays.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("323", "Study models, x-rays, and/or narrative.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("324", "Recent x-ray of treatment area and/or narrative.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("325", "Recent FM x-rays and/or narrative.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("326", "Copy of transplant acquisition invoice.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("327", "Periodontal case type diagnosis and recent pocket depth chart with narrative.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("329", "Exercise notes.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("330", "Occupational notes.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("331", "History and physical.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("333", "Patient release of information authorization.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("334", "Oxygen certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("335", "Durable medical equipment certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("336", "Chiropractic certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("337", "Ambulance certification/documentation.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("339", "Enteral/parenteral certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("340", "Pacemaker certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("341", "Private duty nursing certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("342", "Podiatric certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("343", "Documentation that facility is state licensed and Medicare approved as a surgical facility.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("344", "Documentation that provider of physical therapy is Medicare Part B approved.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("345", "Treatment plan for service/diagnosis.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("346", "Proposed treatment plan for next 6 months.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("352", "Duration of treatment plan.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("353", "Orthodontics treatment plan.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("354", "Treatment plan for replacement of remaining missing teeth.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("360", "Benefits Assignment Certification Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("363", "Possible Workers' Compensation.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("364", "Is accident/illness/condition employment related?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("365", "Is service the result of an accident?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("366", "Is injury due to auto accident?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("374", "Is prescribed lenses a result of cataract surgery?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("375", "Was refraction performed?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("380", "CRNA supervision/medical direction.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("382", "Did provider authorize generic or brand name dispensing?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("383", "Nerve block use (surgery vs. pain management).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("384", "Is prosthesis/crown/inlay placement an initial placement or a replacement?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("385", "Is appliance upper or lower arch & is appliance fixed or removable?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("386", "Orthodontic Treatment/Purpose Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("387", "Date patient last examined by entity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("388", "Date post-operative care assumed.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("389", "Date post-operative care relinquished.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("390", "Date of most recent medical event necessitating service(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("391", "Date(s) dialysis conducted.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("394", "Date(s) of most recent hospitalization related to service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("395", "Date entity signed certification/recertification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("396", "Date home dialysis began.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("397", "Date of onset/exacerbation of illness/condition.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("398", "Visual field test results.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("400", "Claim is out of balance.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("401", "Source of payment is not valid.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("402", "Amount must be greater than zero.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("403", "Entity referral notes/orders/prescription.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("406", "Brief medical history as related to service(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("407", "Complications/mitigating circumstances.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("408", "Initial certification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("409", "Medication logs/records (including medication therapy).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("414", "Necessity for concurrent care (more than one physician treating the patient).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("417", "Prior testing, including result(s) and date(s) as related to service(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("419", "Individual test(s) comprising the panel and the charges for each test.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("420", "Name, dosage and medical justification of contrast material used for radiology procedure.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("428", "Reason for transport by ambulance.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("430", "Nearest appropriate facility.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("431", "Patient's condition/functional status at time of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("432", "Date benefits exhausted.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("433", "Copy of patient revocation of hospice benefits.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("434", "Reasons for more than one transfer per entitlement period.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("435", "Notice of Admission.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("441", "Entity professional qualification for service(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("442", "Modalities of service.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("443", "Initial evaluation report.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("449", "Projected date to discontinue service(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("450", "Awaiting spend down determination.", ClaimLineItemStatusEnum.Pended),
             new X12ClaimStatusCode("451", "Preoperative and post-operative diagnosis.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("452", "Total visits in total number of hours/day and total number of hours/week.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("453", "Procedure Code Modifier(s) for Service(s) Rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("454", "Procedure code for services rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("455", "Revenue code for services rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("456", "Covered Day(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("457", "Non-Covered Day(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("458", "Coinsurance Day(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("459", "Lifetime Reserve Day(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("460", "NUBC Condition Code(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("464", "Payer Assigned Claim Control Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("465", "Principal Procedure Code for Service(s) Rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("466", "Entity's Original Signature.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("467", "Entity Signature Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("468", "Patient Signature Source.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("469", "Purchase Service Charge.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("470", "Was service purchased from another entity?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("471", "Were services related to an emergency?", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("472", "Ambulance Run Sheet.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("473", "Missing or invalid lab indicator.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("474", "Procedure code and patient gender mismatch.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("475", "Procedure code not valid for patient age.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("476", "Missing or invalid units of service.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("477", "Diagnosis code pointer is missing or invalid.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("478", "Claim submitter's identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("479", "Other Carrier payer ID is missing or invalid.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("480", "Entity's claim filing indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("481", "Claim/submission format is invalid.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("483", "Maximum coverage amount met or exceeded for benefit period.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("484", "Business Application Currently Not Available.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("485", "More information available than can be returned in real-time mode. Narrow your search.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("486", "Principal Procedure Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("487", "Claim not found, should have been submitted to/through entity.", ClaimLineItemStatusEnum.NotOnFile),
            new X12ClaimStatusCode("488", "Diagnosis code(s) for the services rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("489", "Attachment Control Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("490", "Other Procedure Code for Service(s) Rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("491", "Entity not eligible for encounter submission.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("492", "Other Procedure Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("493", "Version/Release/Industry ID code not supported by information holder.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("494", "Real-time requests not supported by the information holder, resubmit as batch request.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("495", "Requests for re-adjudication must reference the newly assigned payer claim control number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("496", "Submitter not approved for electronic claim submissions on behalf of this entity.", ClaimLineItemStatusEnum.MemberNotEligible),
            new X12ClaimStatusCode("497", "Sales tax not paid.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("498", "Maximum leave days exhausted.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("499", "No rate on file with the payer for this service for this entity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("500", "Entity's Postal/Zip Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("501", "Entity's State/Province.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("502", "Entity's City.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("503", "Entity's Street Address.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("504", "Entity's Last Name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("505", "Entity's First Name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("506", "Entity is changing processor/clearinghouse.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("507", "HCPCS.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("508", "ICD9 requires a related procedure/diagnosis code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("509", "External Cause of Injury Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("510", "Future date error.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("511", "Invalid character in data.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("512", "Invalid length for receiver's system.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("513", "HIPPS Rate Code for services rendered.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("514", "Entity's Middle Name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("515", "Managed Care review.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("516", "Other Entity's Adjudication or Payment Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("517", "Adjusted Repriced Claim Reference Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("518", "Adjusted Repriced Line item Reference Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("519", "Adjustment Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("520", "Adjustment Quantity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("521", "Adjustment Reason Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("522", "Anesthesia Modifying Units.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("523", "Anesthesia Unit Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("524", "Arterial Blood Gas Quantity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("525", "Begin Therapy Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("526", "Bundled or Unbundled Line Number.", ClaimLineItemStatusEnum.BundledFqhc),
            new X12ClaimStatusCode("527", "Certification Condition Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("528", "Certification Period Projected Visit Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("529", "Certification Revision Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("530", "Claim Adjustment Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("531", "Claim Disproportionate Share Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("532", "Claim DRG Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("533", "Claim DRG Outlier Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("534", "Claim ESRD Payment Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("535", "Claim Frequency Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("536", "Claim Indirect Teaching Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("537", "Claim MSP Pass-through Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("538", "Claim or Encounter Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("539", "Claim PPS Capital Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("540", "Claim PPS Capital Outlier Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("541", "Claim Submission Reason Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("542", "Claim Total Denied Charge Amount.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("543", "Clearinghouse or Value Added Network Trace.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("544", "Clinical Laboratory Improvement Amendment (CLIA) Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("545", "Contract Amount.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("546", "Contract Code.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("547", "Contract Percentage.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("548", "Contract Type Code.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("549", "Contract Version Identifier.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("550", "Coordination of Benefits Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("551", "Coordination of Benefits Total Submitted Charge.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("552", "Cost Report Day Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("553", "Covered Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("554", "Date Claim Paid.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("555", "Delay Reason Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("556", "Demonstration Project Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("557", "Diagnosis Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("558", "Discount Amount.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("559", "Document Control Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("560", "Entity's Additional/Secondary Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("561", "Entity's Contact Name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("562", "Entity's National Provider Identifier (NPI).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("563", "Entity's Tax Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("564", "EPSDT Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("565", "Estimated Claim Due Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("566", "Exception Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("567", "Facility Code Qualifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("568", "Family Planning Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("569", "Fixed Format Information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("571", "Frequency Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("572", "Frequency Period.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("573", "Functional Limitation Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("574", "HCPCS Payable Amount Home Health.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("575", "Homebound Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("576", "Immunization Batch Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("577", "Industry Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("578", "Insurance Type Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("579", "Investigational Device Exemption Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("580", "Last Certification Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("581", "Last Worked Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("582", "Lifetime Psychiatric Days Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("583", "Line Item Charge Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("584", "Line Item Control Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("585", "Denied Charge or Non-covered Charge.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("586", "Line Note Text.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("587", "Measurement Reference Identification Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("588", "Medical Record Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("589", "Provider Accept Assignment Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("590", "Medicare Coverage Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("591", "Medicare Paid at 100% Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("592", "Medicare Paid at 80% Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("593", "Medicare Section 4081 Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("594", "Mental Status Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("595", "Monthly Treatment Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("596", "Non-covered Charge Amount.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("597", "Non-payable Professional Component Amount.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("598", "Non-payable Professional Component Billed Amount.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("599", "Note Reference Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("600", "Oxygen Saturation Qty.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("601", "Oxygen Test Condition Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("602", "Oxygen Test Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("603", "Old Capital Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("604", "Originator Application Transaction Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("605", "Orthodontic Treatment Months Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("606", "Paid From Part A Medicare Trust Fund Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("607", "Paid From Part B Medicare Trust Fund Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("608", "Paid Service Unit Count.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("609", "Participation Agreement.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("610", "Patient Discharge Facility Type Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("611", "Peer Review Authorization Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("612", "Per Day Limit Amount.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("613", "Physician Contact Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("614", "Physician Order Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("615", "Policy Compliance Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("616", "Policy Name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("617", "Postage Claimed Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("618", "PPS-Capital DSH DRG Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("619", "PPS-Capital Exception Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("620", "PPS-Capital FSP DRG Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("621", "PPS-Capital HSP DRG Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("622", "PPS-Capital IME Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("623", "PPS-Operating Federal Specific DRG Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("624", "PPS-Operating Hospital Specific DRG Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("625", "Predetermination of Benefits Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("626", "Pregnancy Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("627", "Pre-Tax Claim Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("628", "Pricing Methodology.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("629", "Property Casualty Claim Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("630", "Referring CLIA Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("631", "Reimbursement Rate.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("632", "Reject Reason Code.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("633", "Related Causes Code (Accident, auto accident, employment).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("634", "Remark Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("635", "Repriced Ambulatory Patient Group Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("636", "Repriced Line Item Reference Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("637", "Repriced Saving Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("638", "Repricing Per Diem or Flat Rate Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("639", "Responsibility Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("640", "Sales Tax Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("642", "Service Authorization Exception Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("643", "Service Line Paid Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("644", "Service Line Rate.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("645", "Service Tax Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("646", "Ship, Delivery or Calendar Pattern Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("647", "Shipped Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("648", "Similar Illness or Symptom Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("649", "Skilled Nursing Facility Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("650", "Special Program Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("651", "State Industrial Accident Provider Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("652", "Terms Discount Percentage.", ClaimLineItemStatusEnum.Contractual),
            new X12ClaimStatusCode("653", "Test Performed Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("654", "Total Denied Charge Amount.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("655", "Total Medicare Paid Amount.", ClaimLineItemStatusEnum.Paid),
            new X12ClaimStatusCode("656", "Total Visits Projected This Certification Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("657", "Total Visits Rendered Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("658", "Treatment Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("659", "Unit or Basis for Measurement Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("660", "Universal Product Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("661", "Visits Prior to Recertification Date Count CR702.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("662", "X-ray Availability Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("663", "Entity's Group Name.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("664", "Orthodontic Banding Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("665", "Surgery Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("666", "Surgical Procedure Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("667", "Real-time requests not supported by the information holder, do not resubmit.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("668", "Missing Endodontics treatment history and prognosis.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("669", "Dental service narrative needed.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("670", "Funds applied from a consumer spending account.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("671", "Funds may be available from a consumer spending account.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("672", "Other Payer's payment information is out of balance.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("673", "Patient Reason for Visit.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("674", "Authorization exceeded.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("675", "Facility admission through discharge dates.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("676", "Entity possibly compensated by facility.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("677", "Entity not affiliated.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("678", "Revenue code and patient gender mismatch.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("679", "Submit newborn services on mother's claim.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("680", "Entity's Country.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("681", "Claim currency not supported.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("682", "Cosmetic procedure.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("683", "Awaiting Associated Hospital Claims.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("684", "Rejected due to syntax error.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("685", "Claim will continue processing in batch mode. Do not resubmit.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("686", "Claim has been voided.", ClaimLineItemStatusEnum.Voided),
            new X12ClaimStatusCode("687", "Claim predetermination/estimation could not be completed in real-time.", ClaimLineItemStatusEnum.NeedsReview),
             new X12ClaimStatusCode("688", "Present on Admission Indicator for reported diagnosis code(s).", ClaimLineItemStatusEnum.NeedsReview),
             new X12ClaimStatusCode("689", "Entity was unable to respond within the expected time frame.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("690", "Multiple claims or estimate requests cannot be processed in real-time.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("691", "Multiple claim status requests cannot be processed in real-time.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("692", "Contracted funding agreement, subscriber is employed by the provider of services.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("693", "Amount must be greater than or equal to zero.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("694", "Amount must not be equal to zero.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("695", "Entity's Country Subdivision Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("696", "Claim Adjustment Group Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("697", "Invalid Decimal Precision.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("698", "Form Type Identification.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("699", "Question/Response from Supporting Documentation Form.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("700", "ICD10 requires additional related procedure/diagnosis code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("701", "Initial Treatment Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("702", "Repriced Claim Reference Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("703", "Advanced Billing Concepts (ABC) code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("704", "Claim Note Text.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("705", "Repriced Allowed Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("706", "Repriced Approved Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("707", "Repriced Approved Ambulatory Patient Group Amount.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("708", "Repriced Approved Revenue Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("709", "Repriced Approved Service Unit Count.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("710", "Line Adjudication Information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("711", "Stretcher purpose.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("712", "Obstetric Additional Units.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("713", "Patient Condition Description.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("714", "Care Plan Oversight Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("715", "Acute Manifestation Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("716", "Repriced Approved DRG Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("717", "This claim has been split for processing.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("718", "Claim/service not submitted within the required timeframe (timely filing).", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("719", "NUBC Occurrence Code(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("720", "NUBC Occurrence Code Date(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("721", "NUBC Occurrence Span Code(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("722", "NUBC Occurrence Span Code Date(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("723", "Drug days supply.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("724", "Drug Quantity.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("725", "NUBC Value Code(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("726", "NUBC Value Code Amount(s).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("727", "Accident date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("728", "Accident state.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("729", "Accident description.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("730", "Accident cause.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("731", "Measurement value/test result.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("732", "Information submitted inconsistent with billing guidelines.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("733", "Prefix for entity's contract/member number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("734", "Verifying premium payment.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("735", "This service/claim is included in the allowance for another service or claim.", ClaimLineItemStatusEnum.BundledFqhc),
            new X12ClaimStatusCode("736", "A related or qualifying service/claim has not been received/adjudicated.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("737", "Current Dental Terminology (CDT) Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("738", "Home Infusion EDI Coalition (HEIC) Product/Service Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("739", "Jurisdiction Specific Procedure or Supply Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("740", "Drop-Off Location.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("741", "Entity must be a person.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("742", "Payer Responsibility Sequence Number Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("743", "Entity's credential/enrollment information.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("744", "Services/charges related to the treatment of a hospital-acquired condition or preventable medical error.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("745", "Identifier Qualifier error.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("746", "Duplicate Submission.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("747", "Hospice Employee Indicator.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("748", "Corrected Data required.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("749", "Date of Injury/Illness.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("750", "Auto Accident State or Province Code.", ClaimLineItemStatusEnum.NeedsReview),
             new X12ClaimStatusCode("751", "Ambulance Pick-up State or Province Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("752", "Ambulance Drop-off State or Province Code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("753", "Co-pay status code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("754", "Entity Name Suffix.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("755", "Entity's primary identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("756", "Entity's Received Date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("757", "Last seen date.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("758", "Repriced approved HCPCS code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("759", "Round trip purpose description.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("760", "Tooth status code.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("761", "Entity's referral number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("762", "Locum Tenens Provider Identifier.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("763", "Ambulance Pickup ZipCode.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("764", "Professional charges are non-covered.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("765", "Institutional charges are non-covered.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("766", "Services performed during a Health Insurance Exchange (HIX) premium payment grace period.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("767", "Qualifications for emergent/urgent care.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("768", "Service date outside the accidental injury coverage period.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("769", "DME Repair or Maintenance.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("770", "Duplicate of a claim processed or in process as a crossover/coordination of benefits claim.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("771", "Claim submitted prematurely. Please resubmit after crossover/payer to payer COB allotted waiting period.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("772", "The greatest level of diagnosis code specificity is required.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("773", "One calendar year per claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("774", "Experimental/Investigational.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("775", "Entity Type Qualifier (Person/Non-Person Entity).", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("776", "Pre/Post-operative care.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("777", "Processed based on multiple or concurrent procedure rules.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("778", "Non-Compensable incident/event.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("779", "Service submitted for the same/similar service within a set timeframe.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("780", "Lifetime benefit maximum.", ClaimLineItemStatusEnum.Denied),
            new X12ClaimStatusCode("781", "Claim has been identified as a readmission.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("782", "Second surgical opinion.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("783", "Federal sequestration adjustment.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("784", "Electronic Visit Verification criteria do not match.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("785", "Missing/Invalid Sterilization/Abortion/Hospital Consent Form.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("786", "Submit claim to the third party property and casualty automobile insurer.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("787", "Resubmit a new claim, not a replacement claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("788", "Submit these services to the Pharmacy plan/processor for further consideration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("789", "Submit these services to the patient's Medical Plan for further consideration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("790", "Submit these services to the patient's Dental Plan for further consideration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("791", "Submit these services to the patient's Vision Plan for further consideration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("792", "Submit these services to the patient's Behavioral Health Plan for further consideration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("793", "Submit these services to the patient's Property and Casualty Plan for further consideration.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("794", "Claim could not complete adjudication in real time. Resubmit as a batch request.", ClaimLineItemStatusEnum.TransientError),
            new X12ClaimStatusCode("795", "Claim submitted prematurely. Please provide the prior payer's final adjudication.", ClaimLineItemStatusEnum.Pended),
            new X12ClaimStatusCode("796", "Procedure code not valid for date of service.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("798", "Claim predetermination/estimation requires manual review. Do not resubmit.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("799", "Resubmit a replacement claim, not a new claim.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("800", "Entity's required reporting has been forwarded to the jurisdiction.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("801", "Entity's required reporting was accepted by the jurisdiction.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("802", "Entity's required reporting was rejected by the jurisdiction.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("803", "Provider reporting rejected due to non-compliance with jurisdiction's registration.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("804", "Exceeds inquiry limit for batch.", ClaimLineItemStatusEnum.Error),
            new X12ClaimStatusCode("805", "Mammography Certification Number.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("806", "Residential county does not match the county of the service location.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("807", "Health Risk Assessment.", ClaimLineItemStatusEnum.NeedsReview),
            new X12ClaimStatusCode("808", "Manifestation diagnosis code cannot be billed as a Principal Diagnosis.", ClaimLineItemStatusEnum.Error),
        };


        private async Task SeedX12ClaimStatusCode()
        {
            await Task.Run(async () =>
            {
                foreach (var item in X12ClaimStatusCodes)
                {
                    var catCodeStatus = await _dbContext.X12ClaimStatusCodes.FirstOrDefaultAsync(x => x.Code == item.Code) ?? null;
                    if (catCodeStatus is null)
                    {
                        _dbContext.X12ClaimStatusCodes.Add(item);
                    }
                    else
                    {
                        catCodeStatus.Code = item.Code;
                        catCodeStatus.Description = item.Description;
                        catCodeStatus.ClaimLineItemStatusId = item.ClaimLineItemStatusId;
                        _dbContext.X12ClaimStatusCodes.Update(catCodeStatus);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            });
            _logger.LogInformation("Seeded X12ClaimStatusCodes");
        }
        #endregion
    }
}
