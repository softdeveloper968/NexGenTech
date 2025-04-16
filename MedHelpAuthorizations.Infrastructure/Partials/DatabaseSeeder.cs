namespace MedHelpAuthorizations.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using MedHelpAuthorizations.Domain.Entities.Enums;
    using MedHelpAuthorizations.Domain.IntegratedServices;

    using Microsoft.Extensions.Logging;

    public partial class DatabaseSeeder
    {
        private void AddClaimStatusExceptionReasonCategoryMaps()
        {
            List<ClaimStatusExceptionReasonCategoryMap> categoryMaps = new List<ClaimStatusExceptionReasonCategoryMap>()
            {
                //Authorization Denials
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.AuthorizationDenial, "AUTHORIZATION REQUIRED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.AuthorizationDenial, "NO AUTHORIZATION ON FILE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.AuthorizationDenial, "NOT AUTHORIZED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.AuthorizationDenial, "AUTH ON FILE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.AuthorizationDenial, "EXCEED AUTHORIZED"),

                //COB
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "COB DISALLOW"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "OTHER CARRIER PAYMENT INFORMATION NEEDED BEFORE CLAIM CAN BE PROCESSED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "PAYMENT FOR THESE SERVICES HAVE BEEN DENIED UNTIL REPLY TO COB QUESTIONNAIRE IS RECEIVED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "THIS CARE MAY BE COVERED BY ANOTHER PAYER PER COORDINATION OF BENEFITS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "MISSING PLAN INFORMATION FOR OTHER INSURANCE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "YOU MUST FIRST SUBMIT A CLAIM FOR THIS CHARGE TO MEDICARE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "AFTER MEDICARE HAS PAID, PLEASE SEND YOUR CLAIM FOR BENEFITS TO YOUR LOCAL BLUE CROSS AND BLUE SHIELD PLAN OR THE PLAN SERVING THE AREA WHERE THE SERVICES WERE RENDERED. Ã‚Â BE SURE TO INCLUDE A COMPLETED FEDERAL EMPLOYEE PROGRAM HEALTH BENEFITS CLAIM FORM, THE MEDICARE SUMMARY NOTICE AND THE ITEMIZED BILLS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "THIS CARE MAY BE COVERED BY ANOTHER PAYER PER COORDINATION OF BENEFITS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.COB, "MISSING PLAN INFORMATION FOR OTHER INSURANCE"),

                 //Claim Issue
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.ClaimIssue, "MISSING/INCOMPLETE/INVALID SERVICE FACILITY PRIMARY ADDRESS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.ClaimIssue, "N294 MISSING/INCOMPLETE/INVALID SERVICE FACILITY PRIMARY ADDRESS"),

                 //Demographics Issue
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue, "ERROR ON SUBMITTED REQUEST DATA. ENTITY'S DATE OF BIRTH"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue, "DATE OF BIRTH"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue, "INVALID/MISSING PATIENT NAME"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue, "MISSING/INCOMPLETE/INVALID PATIENT'S NAME"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue, "MEMBER DATE OF BIRTH DOES NOT MATCH DATE ON FILE"),                

                //Coding Issue
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THE PROCEDURE IS NOT COVERED BY THIS CONTRACT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THIS SERVICE IS NOT PAID. ACCORDING TO CPT CODING GUIDELINES, THE MODIFIER IS NOT VALID IN COMBINATION WITH THE PROCEDURE CODE SUBMITTED. AN INTERNAL RULE, GUIDELINE OR PROTOCOL WAS RELIED UPON IN MAKING THIS ADVERSE BENEFIT DETERMINATION AND A COPY OF SUCH WILL BE PROVIDED FREE OF CHARGE UPON YOUR REQUEST"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THIS PROCEDURE IS NOT CONTRACTUALLY COVERED UNDER YOUR CONTRACT BASED ON THE DIAGNOSIS REPORTED. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THE CLAIM CANNOT BE PROCESSED BECAUSE THE MEDICAL DOCUMENTATION THAT THE PROVIDER OF SERVICE SUPPLIED TO US IS INCOMPLETE. THE INFORMATION STILL NEEDED IS A STANDARD CPT OR NON-MEDICAID/NON-MEDICARE HCPCS CODE FOR THIS SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THE PROCEDURE CODE IS INCONSISTENT WITH THE PROVIDER TYPE/SPECIALTY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THE DIAGNOSIS IS INCONSISTENT WITH THE PATIENT'S GENDER. USAGE: REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110 SERVICE PAYMENT INFORMATAION REF), IF PRESENT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "CLAIM/SERVICE DENIED/REDUCED BECAUSE \"NEW PATIENT\" QUALIFICATIONS WERE NOT MET.M13ONLY ONE INITIAL VISIT IS COVERED PER SPECIALTY PER MEDICAL GROUP"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "BENEFITS WERE NOT PAID FOR THIS CLAIM BECAUSE MEDICARE DID NOT RECEIVE THE CORRECT INFORMATION FOR THIS SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THE DATE OF DEATH PRECEDES THE DATE OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "DENY - PLEASE SUBMIT TO FEE FOR SERVICE MEDICAID FOR PROCESSING"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "DIAGNOSIS DOES NOT CORRESPOND TO PROCEDURE CODE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "DIAGNOSIS NOT ALLOWED FOR BH CLAIMS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "DID NOT MEET MINIMUM CASE RATE UNIT REQUIREMENT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "INAPPROPRIATE USE OF UA MODIFIER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "INVALID COMBINATION OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "INVALID LOC/MODIFIER/PLACE OF SERVICE COMBINATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "INVALID OR MISSING CPT4 OR HCPCS PROCEDURE CODE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "MISSING ITEMIZED SERVICES FOR DATE OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PRIMARY DIAGNOSIS IS NOT TYPICAL FOR PATIENT'S AGE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PRIMARY DX LIMIT REACHED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PROC CODE NOT FOUND ON ANY FEE SCHEDULE FOR MARYLAND MEDICAID PROGRAM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PROCEDURE CODE FOR SERVICES RENDERED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PROCEDURE CODE MUST BE BILLED WITH A PRIMARY CODE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "STANDALONE CODE CANNOT BE BILLED IN AN FQHC WITHOUT A TRIGGER CODE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "SUBMITTED COST IS REQUIRED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "THE TYPE OF BILL OR FREQUENCY CODE IS INVALID. PLEASE RESUBMIT WITH A CORRECTED CODE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PROCEDURE CODE NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "MISSING PROCEDURE MODIFIER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.CodingIssue, "PROCEDURE CODE MODIFIER(S) FOR SERVICE(S) RENDERED"),

                //Contractual
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Contractual, "CHARGE EXCEEDS ALLOWED AMOUNT FOR THIS SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Contractual, "BILLED AMOUNT IS HIGHER THAN THE MAXIMUM ALLOWED PAYMENT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Contractual, "WE HAVE IDENTIFIED THAT THE PROCEDURE ON THIS CLAIM LINE WAS MUTUALLY EXCLUSIVE TO ANOTHER PROCEDURE PERFORMED BY THE SAME PROVIDER FOR THE SAME DATES OF SERVIVCE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Contractual, "THE MEDICAL VISIT IS CUSTOMARILY PROVIDED WITH THE RELATED PROCEDURE. PAYMENT FOR THIS SERVICE IS INCLUDED IN THE PAYMENT MADE FOR THAT PROCEDURE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Contractual, "PROCESSED PER PARTICIPATING CONTRACT OR FEE SCHEDULE"),

                //Coverage
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "UNDER THIS MEMBER'S COVERAGE, BENEFITS ARE NOT AVAILABLE FOR THIS SERVICE. THEREFORE, WE ARE UNABLE TO PROVIDE BENEFITS FOR THIS CHARGE. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "MEMBER'S COVERAGE NOT IN EFFECT ON DATE OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THESE CHARGES ARE NOT COVERED. THE SERVICES EXCEED THE MAXIMUM NUMBER OF UNITS ALLOWED PER THE MEMBER'S BENEFIT PLAN OR POLICY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THIS SERVICE IS NOT A COVERED BENEFIT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "PATIENT NOT ENROLLED AT TIME OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THE DATE OF SERVICE WAS PRIOR TO THE SUBSCRIBER'S EFFECTIVE DATE OF COVERAGE OR AFTER SUBSCRIBER'S CANCEL DATE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "SERVICES IDENTIFIED BY THIS DIAGNOSIS, PROCEDURE, OR SURGICAL CODENOT COVERED UNDER THE MEMBER'S COVERAGE. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATON"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THIS PROCEDURE CODE IS NOT A COVERED BENEFIT UNDER THE MEMBER'S COVERAGE. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "SERVICES FOR DIAGNOSIS REPORTED ARE NOT A BENEFIT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "BENEFITS ARE NOT PROVIDED FOR SERVICES OBTAINED FROM NON-PARTICIPATING PROVIDERS PER THE MEMBER'S BENEFIT PLAN OR POLICY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THIS SERVICE WAS PERFORMED PRIOR TO THE EFFECTIVE DATE OR AFTER THE CANCELLATION DATE OF YOUR PARTICIPATING STATUS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "ACCORDING TO OUR RECORDS, THIS PATIENT'S COVERAGE WAS NOT IN EFFECT ON THE DATE THESE SERVICES WERE RENDERED. THEREFORE, WE ARE UNABLE TO PROVIDE BENEFITS FOR THESE CHARGES. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "SERVICES IDENTIFIED BY THIS DIAGNOSIS, PROCEDURE, OR SURGICAL CODEARE NOT COVERED UNDER THE MEMBER'S COVERAGE. PLEASE REFERTO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THIS SERVICE IS NOT COVERED UNDER THE MEMBER'S PLAN"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THESE SERVICES WERE PROVIDED AFTER YOU CHANGED YOUR COVERAGE TO ANOTHER CARRIER UNDER THE FEDERAL EMPLOYEES HEALTH BENEFITS PROGRAM. Ã‚Â YOU SHOULD SUBMIT THESE CHARGES TO YOUR NEW CARRIER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "PLAN NOT EFFECTIVE ON DATE REQUESTED "),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "UNDER THIS MEMBER'S COVERAGE, BENEFITS ARE NOT AVAILABLE FOR THIS SERVICE. THEREFORE, WE ARE UNABLE TO PROVIDE BENEFITS FOR THIS CHARGE. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "THE MEMBERS CONTRACT WAS NOT IN EFFECT ON THE DATE OF SERVICE. IF YOU DISAGREE WITH THIS DETERMINATION, PLEASE CONTACT YOUR EMPLOYER. PLEASE REFER TO THE EMPLOYEE BENEFIT BOOKLET OR CONTRACT FOR ADDITIONAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "BENEFITS ARE NOT PROVIDED FOR SERVICES/SUPPLIES LISTED IN THE GENERAL EXCLUSIONS SECTION OF THE BLUE CROSS BLUE SHIELD SERVICE BENEFIT PLAN BROCHURE. Ã‚Â YOU ARE RESPONSIBLE FOR THESE CHARGES EVEN IF THE SERVICES/SUPPLIES WERE ORDERED BY A PROVIDER, EXCEPT WHEN ANOTHER CARRIER HAS PAID FOR THE SERVICE(S) IN FULL"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "EXPENSES INCURRED PRIOR TO COVERAGE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "SERVICES NOT COVERED BECAUSE THE PATIENT IS ENROLLED IN A HOSPICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "DATE OF SERVICE NOT COVERED/AUTHORIZED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "DENIED CHARGE OR NON-COVERED CHARGE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "EXPENSES INCURRED PRIOR TO COVERAGE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "ONLY ONE INITIAL VISIT IS COVERED PER SPECIALTY PER MEDICAL GROUP"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "NOT COVERED WHEN PERFORMED DURING THE SAME SESSION/DATE AS A PREVIOUSLY PROCESSED SERVICE FOR THE PATIENT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Coverage, "INVESTIGATING EXISTENCE OF OTHER INSURANCE COVERAGE"),


                //Credentialing
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "THIS CLAIM WAS RECEIVED BY A PROVIDER IN YOUR GROUP PRACTICE WHO, ACCORDING TO OUR RECORDS, DID NOT PARTICIPATE WITH CAREFIRST BLUE CROSS BLUE SHIELD AT THE TIME THE SERVICES WERE RENDERED. IT IS OUR POLICY THAT ALL PROVIDERS WITHIN A GROUP PRACTICE MUST APPLY AND BE ACCEPTED FOR PARTICIPATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "BILLING PHYSICIAN IS NOT PAR WITH GROUP"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "THIS PROVIDER WAS NOT CERTIFIED/ELIGIBLE TO BE PAID FOR THIS PROCEDURE/SERVICE ON THIS DATE OF SERVICE. USAGE: REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110 SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "THE REFERRING PROVIDER IS NOT ELIGIBLE TO REFER THE SERVICE BILLED. USAGE: REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110 SERVICE PAYMENT INFORMATION REF), IF PRESENT.MA13ALERT: YOU MAY BE SUBJECT TO PENALTIES IF YOU BILL THE BENEFICIARY FOR AMOUNTS NOT REPORTED WITH THE PR (PATIENT RESPONSIBILITY) GROUP CODE.N574OUR RECORDS INDICATE THE ORDERING/REFERRING PROVIDER IS OF A TYPE/SPECIALTY THAT CANNOT ORDER OR REFER. PLEASE VERIFY THAT THE CLAIM ORDERING/REFERRING PROVIDER INFORMATION IS ACCURATE OR CONTACT THE ORDERING/REFERRING PROVIDER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "BILLING PROVIDER ENROLLMENT IS TERMINATED WITH MD MEDICAID"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "ENTITY IS NOT SELECTED PRIMARY CARE PROVIDER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "ENTITY NOT ELIGIBLE FOR BENEFITS FOR SUBMITTED DATES OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "ENTITY'S MEDICAID PROVIDER ID"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "NO AGREEMENT WITH ENTITY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "RENDERING PROV MUST BE AN ACTIVE MEDICAID PROVIDER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "RENDERING PROVIDER ENROLLMENT IS SUSPENDED WITH MD MEDICAID"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing, "RENDERING PROVIDER REQUIRED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing,"PAYMENT IS DENIED WHEN BILLED BY THIS PROV TYPE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Credentialing,"RENDERING PROV TYPE NOT ELIGIBLE FOR PROV GROUP"),
                //Duplicate
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "THIS CLAIM LINE IS AN EXACT DUPLICATE OF ANOTHER CLAIM LINE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "ALL SUBMISSIONS DENIED AS DUPLICATE OF THE OTHER, NO ORIGINAL LINE ITEM WITH A DIFFERENT STATUS FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "THIS CLAIM LINE IS AN EXACT DUPLICATE OF ANOTHER CLAIM LINE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "ANOTHER PROVIDER HAS BILLED FOR THE SERVICES DESCRIBED IN THIS CLAIM. PAYMENT OF THIS SERVICE, THEREFORE, WOULD RESULT IN A DUPLICATE PAYMENT ACCORDING TO THE ADMINISTRATION PROVISIONS OF THE SUBSCRIBERS CONTRACT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "THIS CLAIM IS A DUPLICATE OF A PREVIOUSLY SUBMITTED CLAIM FOR THIS MEMBER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "THIS CHARGE/BILL IS A DUPLICATE OF ANOTHER YOU INCLUDED WITH THIS CLAIM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "THIS CLAIM IS A DUPLICATE OF A CLAIM CURRENTLY IN PROCESS. YOU WILL RECEIVE AN EXPLANATION WHEN WE HAVE COMPLETED PROCESSING OF THE ORIGINAL CLAIM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "EXACT DUPLICATE CLAIM/SERVICE (USE ONLY WITH GROUP CODE OA EXCEPT WHERE STATE WORKERS' COMPENSATION REGULATIONS REQUIRES CO)"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "YOU HAVE ALREADY RECEIVED CREDIT TOWARD YOUR DEDUCTIBLE OR RECEIVED BENEFITS FOR THIS CHARGE ON A PREVIOUS CLAIM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "BILLED MORE THAN ONCE/DAY WITH HISTORY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "WE HAVE IDENTIFIED THAT THE PROCEDURE CODE ON THIS CLAIM LINE IS A DUPLICATE PROCEDURE AND SHOULD NOT BE REIMBURSED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "DUPLICATE CLAIM/SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "DUPLICATE OF A CLAIM PROCESSED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "DUPLICATE CLAIM PREVIOUSLY PROCESSED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "THIS CLAIM IS A DUPLICATE OF A PREVIOUSLY SUBMITTED CLAIM FOR THIS MEMBER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "DUPLICATE OF A CLAIM PROCESSED, OR TO BE PROCESSED, AS A CROSSOVER CLAIM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "DUPLICATE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Duplicate, "DUPLICATE OF A PREVIOUSLY PROCESSED CLAIM/LINE"),

                //Insurance Termed
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "TERMINATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "TERMINATION - NON PAYMENT OF PREMIUM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "HAS BEEN TERMINATED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "DATE REQUESTED AFTER GROUP TERM DATE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "THE MEMBERS CONTRACT WAS NOT IN EFFECT ON THE DATE OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "AFTER COVERAGE TERMINATED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InsuranceTermed, "POLICY CANCELED"),

                //Invalid Insurance                
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InvalidInsurance, "WE HAVE NO RECORD OF THE IDENTIFICATION NUMBER SUBMITTED FOR THIS SUBSCRIBER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InvalidInsurance, "THE MEMBERS CONTRACT WAS NOT IN EFFECT ON THE DATE OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InvalidInsurance, "YOU MUST FIRST SUBMIT A CLAIM FOR THIS CHARGE TO MEDICARE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InvalidInsurance, "THE DATE OF SERVICE WAS PRIOR TO THE SUBSCRIBER'S EFFECTIVE DATE OF COVERAGE OR AFTER SUBSCRIBER'S CANCEL DATE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.InvalidInsurance, "EXPENSES INCURRED PRIOR TO COVERAGE"),

                //Medical Necessity
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.MedicalNecessity, "THESE ARE NON-COVERED SERVICES BECAUSE THIS IS NOT DEEMED A 'MEDICAL NECESSITY' BY THE PAYER"),                
               
                //MR Needed
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.MRNeeded, "FINAL BENEFIT DETERMINATION CANNOT BE MADE UNTIL WE RECEIVE SPECIFIC REQUESTED MEDICAL INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.MRNeeded, "BEFORE WE CAN REVIEW YOUR CLAIM, WE NEED TO KNOW THE SPECIFIC ILLNESS, INJURY, CONDITION OR DIAGNOSIS THAT REQUIRED TREATMENT. WE WILL RECONSIDER YOUR CLAIM ONCE WE RECEIVE THIS INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.MRNeeded, "YOUR CLAIM IS CURRENTLY BEING REVIEWED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.MRNeeded, "CLAIM CANNOT BE PROCESSED BECAUSE THE MEDICAL DOCUMENTATION"),

                //NO Claim / Missing Procedure
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "CLAIM NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "UNMATCHED-PROCEDURECODE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "CLAIMS WERE NOT FOUND THAT MATCHED THE CRITERIA GIVEN"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "PROCEDURE CODE NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "PROCEDURE CODE NOT FOUND IN CLAIM LINES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "0 CLAIMS FOUND WITH THE PROVIDED CRITERIA IN THE INCEDO APPLICATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "ACKNOWLEDGEMENT NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "NO CLAIMS/ENCOUNTERS FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "ACKNOWLEDGEMENT/NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "CLAIM/ENCOUNTER NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "ENTITY NOT ELIGIBLE FOR BENEFITS FOR SUBMITTED DATES OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "CLAIM UNAVAILABLE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "THE CLAIM/ENCOUNTER CAN NOT BE FOUND IN THE ADJUDICATION SYSTEM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "DATA SEARCH UNSUCCESSFUL"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.NoClaimMissingProcedure, "CLAIM/ENCOUNTER NOT FOUND"),

                //Other
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Other, "AIT IS CURRENTLY UNABLE TO REVIEW THIS CLAIM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Other, "ALERT: YOU MAY NOT APPEAL THIS DECISION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Other, "STATUTORILY EXCLUDED SERVICES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Other, "PROCESSED ACCORDING TO CONTRACT PROVISIONS"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Other, "REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110  SERVICE PAYMENT INFORMATION REF)"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Other, "ACCEPTED FOR PROCESSING"),

                //Policy Number
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.PolicyNumber, "THE MEMBER'S HOME PLAN HAS ADVISED US THAT THE PREFIX FOR THIS ID NUMBER IS INCORRECT.PLEASE DO NOT RESUBMIT THE CLAIM AS WE WILL HAVE IT REPROCESSED CORRECTLY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.PolicyNumber, "SUBSCRIBER AND SUBSCRIBER ID NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.PolicyNumber, "MEDICARE BENEFICIARY ID IS INVALID"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.PolicyNumber, "MISSING/INCOMPLETE/INVALID PATIENT/IDENTIFIER"), 

                //Provider Type
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.ProviderType, "PAYMENT IS DENIED WHEN BILLED BY THIS PROV TYPE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.ProviderType, "THIS CLAIM WAS RECEIVED BY A PROVIDER IN YOUR GROUP PRACTICE WHO, ACCORDING TO OUR RECORDS, DID NOT PARTICIPATE WITH CAREFIRST BLUE CROSS BLUE SHIELD AT THE TIME THE SERVICES WERE RENDERED. IT IS OUR POLICY THAT ALL PROVIDERS WITHIN A GROUP PRACTICE MUST APPLY AND BE ACCEPTED FOR PARTICIPATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.ProviderType, "RENDERING PROV TYPE NOT ELIGIBLE FOR PROV GROUP"),
                
                //Internal Review
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "MEMBER ID IGNORED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THIS CLAIM CANNOT BE PROCESSED WITHOUT ADDITIONAL INFORMATION. Â HOWEVER, SINCE YOUR PROVIDER IS A PARTICIPATING OR PREFERRED PROVIDER, YOU ARE NOT RESPONSIBLE FOR THESE CHARGES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THIS CLAIM CANNOT BE PROCESSED WITHOUT ADDITIONAL INFORMATION.  HOWEVER, SINCE YOUR PROVIDER IS A PARTICIPATING OR PREFERRED PROVIDER, YOU ARE NOT RESPONSIBLE FOR THESE CHARGES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THESE SERVICES ARE INELIGIBLE AS THEY ARE NOT CONSIDERED MEDICALLY NECESSARY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "BENEFITS FOR THIS SERVICE HAVE BEEN PAID TO ANOTHER PROVIDER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "N/A"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "YOUR ADJUSTMENT REQUEST WAS RECEIVED AND THE ORIGINAL CLAIM WAS ADJUSTED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "N/A"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "N/A"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "N/A"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "PENDING/PAYER ADMINISTRATIVE/SYSTEM HOLD,46 - INTERNAL REVIEW/AUDIT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THE SERVICES RENDERED WERE NOT BILLED USING THE AGREED UPON FORMAT. THE PROVIDER IS PARTICIPATING, THEREFORE, THE PATIENT IS NOT RESPONSIBLE FOR PAYMENT OF THIS SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "BENEFITS FOR THESE SERVICES ARE INCLUDED IN OUR ALLOWABLE CHARGES FOR ANOTHER COVERED SERVICE PROVIDED ON THE SAME DATE OF SERVICE.  ADDITIONAL BENEFITS ARE NOT AVAILABLE FOR THIS CHARGE.  BECAUSE THIS PROVIDER IS A PREFERRED OR PARTICIPATING NETWORK PROVIDER, YOU ARE NOT RESPONSIBLE FOR THESE CHARGES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "MEMBER NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "MEMBER WAS NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "MEMBER WAS NOT SUCESSFULLY FOUND OR MORE THAN ONE MEMBER FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "N/A"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "BENEFITS ARE NOT PROVIDED FOR SERVICES/SUPPLIES LISTED IN THE GENERAL EXCLUSIONS SECTION OF THE BLUE CROSS BLUE SHIELD SERVICE BENEFIT PLAN BROCHURE.Â YOU ARE RESPONSIBLE FOR THESE CHARGES EVEN IF THE SERVICES/SUPPLIES WERE ORDERED BY A PROVIDER, EXCEPT WHEN ANOTHER CARRIER HAS PAID FOR THE SERVICE(S) IN FULL"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "BENEFITS WERE NOT PAID FOR THIS CLAIM BECAUSE MEDICARE DID NOT RECEIVE THE CORRECT INFORMATION FOR THIS SERVICE. YOUR CLAIM WILL BE PROCESSED AS SOON AS YOUR PHYSICIAN SENDS THE CORRECT INFORMATION TO MEDICARE. THANK YOU FOR YOUR PATIENCE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "REIMBURSEMENT FOR THIS SERVICE IS CONSIDERED TO BE A PORTION OF ANOTHER SERVICE WHICH HAS BEEN ALLOWED. THEREFORE, NO PAYMENT CAN BE MADE FOR THIS SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THIS LINE OF BUSINESS IS PROCESSED BY A VENDOR. HANDLE DIRECT WITH VENDOR"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "OUR CONTRACT WITH THIS PROVIDER INCLUDES THIS SERVICE WITHIN THE REIMBURSEMENT OF ANOTHER SERVICE AND, THEREFORE, DOES NOT ALLOW SEPARATE PAYMENT OF BENEFITS. IF THE PROVIDER IS PARTICIPATING WITH OUR PLAN. THE MEMBER IS NOT RESPONSIBLE FOR THE PAYMENT OF THIS SERVICE. AN INTERNAL RULE, GUIDELINE, OR PROTOCOL WAS RELIED UPON IN MAKING THIS ADVERSE BENEFIT DETERMINATION AND A COPY OF SUCH WILL BE PROVIDED FREE OF CHARGE UPON YOUR REQUEST"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "BENEFITS FOR THESE SERVICES ARE INCLUDED IN OUR ALLOWABLE CHARGES FOR ANOTHER COVERED SERVICE PROVIDED ON THE SAME DATE OF SERVICE. ADDITIONAL BENEFITS ARE NOT AVAILABLE FOR THIS CHARGE. BECAUSE THIS PROVIDER IS A PREFERRED OR PARTICIPATING NETWORK PROVIDER, YOU ARE NOT RESPONSIBLE FOR THESE CHARGES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "OUR CONTRACT WITH THIS PROVIDER INCLUDES THIS SERVICE WITHIN THE REIMBURSEMENT OF ANOTHER SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "REJECT EOB"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THE DATE OF DEATH PRECEDES THE DATE OF SERVICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "PAYMENT ADJUSTED BECAUSE THE PAYER DEEMS THE INFORMATION SUBMITTED DOES NOT SUPPORT THIS MANY/FREQUENCY OF SERVICES.MA01ALERT: IF YOU DO NOT AGREE WITH WHAT WE APPROVED FOR THESE SERVICES, YOU MAY APPEAL OUR DECISION. TO MAKE SURE THAT WE ARE FAIR TO YOU, WE REQUIRE ANOTHER INDIVIDUAL THAT DID NOT PROCESS YOUR INITIAL CLAIM TO CONDUCT THE APPEAL. HOWEVER, IN ORDER TO BE ELIGIBLE FOR AN APPEAL, YOU MUST WRITE TO US WITHIN 120 DAYS OF THE DATE YOU RECEIVED THIS NOTICE, UNLESS YOU HAVE A GOOD REASON FOR BEING LATE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "CLAIM/SERVICE LACKS INFORMATION OR HAS SUBMISSION/BILLING ERROR"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "NON-COVERED CHARGE(S). AT LEAST ONE REMARK CODE MUST BE PROVIDED (MAY BE COMPRISED OF EITHER THE NCPDP REJECT REASON CODE, OR REMITTANCE ADVICE REMARK CODE THAT IS NOT AN ALERT.) USAGE: REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110 SERVICE PAYMENT INFORMATION REF), IF PRESENT.N425STATUTORILY EXCLUDED SERVICES(S)"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THE BENEFIT FOR THIS SERVICE IS INCLUDED IN THE PAYMENT/ALLOWANCE FOR ANOTHER SERVICE/PROCEDURE THAT HAS ALREADY BEEN ADJUDICATED. USAGE: REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110 SERVICE PAYMENT INFORMATION REF), IF PRESENT.N211ALERT: YOU MAY NOT APPEAL THIS DECISION.N666ONLY ONE EVALUATION AND MANAGEMENT CODE AT THIS SERVICE LEVEL IS COVERED DURING THE COURSE OF CARE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "SERVICES NOT COVERED BECAUSE THE PATIENT IS ENROLLED IN A HOSPICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THIS SERVICE IS NOT COVERED BECAUSE IT IS CONSIDERED EXPERIMENTAL"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "CLAIM WAS PROCESSED AS ADJUSTMENT TO PREVIOUS CLAIM"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "HOWEVER, IN ORDER TO BE ELIGIBLE FOR AN APPEAL, YOU MUST WRITE TO US WITHIN 120  DAYS OF THE DATE YOU RECEIVED THIS NOTICE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "PLEASE SUBMIT COPY OF PRIMARY INSURANCE EXPLANATION OF PAYMENT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "RECOUPMENT HAS BEEN MADE DUE TO AN UPDATE TO THE PROVIDER'S FEE SCHEDULE WHICH NOW RESULTED IN AN OVERPAYMENT"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "RECOUPMENT MADE DUE TO AN INCORRECT BENEFIT PREVIOUSLY PAID"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "UNIT OF SERVICE EXCEEDS MAXIMUM ALLOWED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "POLICY NUMBER NOT REVIEWABLE"),
                 new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "CORRECTED CLAIM CANNOT BE MATCHED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "CLAIM HAS BEEN PROCESSED"),// So far .. these are found to be paid with no check - Approved
                //This is for a "PAID" claim, but it indicates a "Takeback" from another claim and needs to be reviewed
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "ORIGINAL CLAIM HAS BEEN ADJUSTED BY FULL CLAIM ADJ"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "BECAUSE THIS PROVIDER IS A PREFERRED OR PARTICIPATING NETWORK PROVIDER, YOU ARE NOT RESPONSIBLE FOR THE DIFFERENCE BETWEEN THE SUBMITTED CHARGES AND OUR ALLOWABLE CHARGES"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "CANNOT PROVIDE FURTHER STATUS ELECTRONICALLY"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "REFER TO THE 835 HEALTHCARE POLICY IDENTIFICATION SEGMENT (LOOP 2110  SERVICE PAYMENT INFORMATION REF)"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "PROCESSED ACCORDING TO PLAN PROVISIONS (PLAN REFERS TO PROVISIONS THAT EXIST BETWEEN THE HEALTH PLAN AND THE CONSUMER OR PATIENT)"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "THE HEALTH PLAN IS DOWN FOR MAINTENANCE"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "ERROR ON SUBMITTED REQUEST DATA. SUBMITTER NOT FOUND"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "MISSING OR INVALID INFORMATION"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.Review, "INVALID/MISSING PATIENT ID"),

                //Secondary missing EOB
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.SecondaryMissingEob, "MEDICARE PAYMENT INFORMATION WAS NOT RECEIVED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.SecondaryMissingEob, "THE INFORMATION STILL NEEDED IS THE COPY OF THE EXPLANATION OF PART B MEDICARE BENEFITS (EOMB)"),

                //Timely Filing
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.TimelyFiling, "THE TIME LIMIT FOR FILING HAS EXPIRED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.TimelyFiling, "CLAIM/SERVICE NOT SUBMITTED WITHIN THE REQUIRED TIMEFRAME"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.TimelyFiling, "TIMELY FILING"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.TimelyFiling, "UNTIMELY FILED CLAIM"),

                //Wrong Payer 
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "CLAIM NOT COVERED BY THIS PAYER/CONTRACTOR"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "SERVICE PAYABLE BY OTHER PRIMARY CARRIER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "OTHER CARRIER PAYMENT INFORMATION NEEDED BEFORE CLAIM CAN BE PROCESSED"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "CLAIM SUBMITTED TO INCORRECT PAYER"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "YOU MUST SEND THE CLAIM TO THE CORRECT PAYER/CONTRACTOR"),
                new ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum.WrongPayer, "MISSING PLAN INFORMATION FOR OTHER INSURANCE"),
            };
            Task.Run(async () =>
            {
                foreach (var map in categoryMaps)
                {
                    if (!_db.ClaimStatusExceptionReasonCategoryMaps.Any(cm => cm.ClaimStatusExceptionReasonText == map.ClaimStatusExceptionReasonText))
                    {
                        map.CreatedOn = DateTime.UtcNow;
                        _db.ClaimStatusExceptionReasonCategoryMaps.Add(map);
                    }
                }
            }).GetAwaiter().GetResult();

            _logger.LogInformation("Seeded ClaimStatusExceptionReasonCategoryMaps");
        }
    }
}