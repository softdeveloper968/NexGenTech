using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    /// <summary>
    /// The PlaceOfServiceCOdeEnum Enumeration
    /// Defines a set of codes that can be used to indicate the Place a service was rendered.
    /// </summary>
    public enum PlaceOfServiceCodeEnum
    {
        //TODO: Add in all missing summaries. (See Below Comments)
        /// <summary>
        /// Pharmacy is a place where medicinal drugs and other medically related things are sold or dispensed.Place of Service 01 is indicated, when the services rendered by pharmacy setting to the patient.
        /// </summary>
        Pharmacy = 1,
        TeleHealth = 2,
        School = 3,
        HomelessShelter = 4,
        IhsFreeStandingFacility = 5,
        IhsProviderBasedFacility = 6,
        Tribal638FreeStandingFacility = 7,
        Tribal638ProviderBasedFacility = 8,
        CorrectionalFacility = 9,
        Office = 11,
        Home = 12,
        AssistedLivingFacility = 13,
        GroupHome = 14,
        MobileUnit = 15,
        TemporaryLodging = 16,
        WalkInRetailHealthClinic = 17,
        EmploymentWorkSite = 18,
        OffCampusOutpatientHospital = 19,
        UrgentCareFacility = 20,
        InpatientHospital = 21,
        OnCampusOutpatientHospital = 22,
        EmergencyRoomHospital = 23,
        AmbulatorySurgicalCenter = 24,
        BirthingCenter = 25,
        MilitaryTreatmentFacility = 26,
        SkilledNursingFacility = 31,
        NursingFacility = 32,
        CustodialCareFacility = 33,
        Hospice = 34,
        AmbulanceLand = 41,
        AmbulanceAirOrWater = 42,
        IndependentClinic = 49,
        FederallyQualifiedHealthCenter = 50,
        InpatientPsychiatricFacility = 51,
        PsychFacilityPartialHospitalization = 52,
        CommunityMentalHealthCenter = 53,
        IntermediateCareIntellectualDisabilities = 54,
        ResidentialSubstanceAbuse = 55,
        PsychiatricResidentialTreatment = 56,
        NonResidentialSubstanceAbuse = 57,
        MassImmunizationCenter = 60,
        ComprehensiveInpatientRehabilitation = 61,
        ComprehensiveOutpatientRehabilitation = 62,
        EndStageRenalDiseaseFacility = 65,
        PublicHealthClinic = 71,
        RuralHealthClinic = 72,
        IndependentLaboratory = 81,
        OtherPlaceOfService = 99
    }

    //	01	Pharmacy is a place where medicinal drugs and other medically related things are sold or dispensed.Place of Service 01 is indicated, when the services rendered by pharmacy setting to the patient.
    //	02	Place of Service 02 – POS 02 Description in Medical Billing
    //	03	 It is a place or location whose main motive is education.
    //	04	Homeless shelter is a place whose primary purpose is to provide a temporary shelter to homeless individual and families.Place of service 04 is designated, when the homeless service agency provides the temporary housing services to the homeless individual and families. (Example: Temporary emergency Shelter’s provided during natural disaster).
    //	05	It’s a place operated and owned by IHS, which provides medical services (such as diagnostic, therapeutic, and rehabilitation services) to American Indian and Alaska Natives who do not require hospitalization.Place of service 05 is indicated, when the health care services provided by IHS to American Indian and Alaska natives on free standing facility.
    //	06	It’s a place operated and owned by IHS, which provides medical services (such as diagnostic, therapeutic, and rehabilitation services) rendered by, or under the supervision of physicians to American Indian and Alaska Natives admitted as inpatient or outpatients.Place of service 06 is indicated, when the medical services provided by IHS under the direction of medical doctors to American Indian and Alaska natives admitted as inpatient or outpatients.
    //	07	It’s a place operated by tribes under 638 agreement, which provides medical services (such as diagnostic, therapeutic, and rehabilitation services) to tribal members who do not require hospitalization. Place of service 07 is denoted, when the medical services provided to tribal members on free standing facility.
    //	08	It’s a place operated by tribes under 638 agreement, which provides medical services (such as diagnostic, therapeutic, and rehabilitation services) to tribal members admitted as inpatients or outpatients. Place of service 08 is denoted, when the medical services provided to tribal members admitted as inpatients or outpatients.
    //	09	Prison relate as correctional facility, jail, custodial, lock up, detention center or any other alike facility maintained by either Federal, State or local authorities for the purpose of confinement or treatment of mature or immature unlawful criminals.
    //	10	N/A
    //	11	Place of Service 11 – POS 11 Description with example in Medical billing
    //	12	Place of Service 12 – POS 12 description with example in Medical Billing
    //	13	Place of service 13 – Assisted Living Facility Description: Place of service 13 is indicated when mass residential facility with independent living units providing assessment of each resident’s requirements and on-site support 24/7, with the ability to provide or arrange for services (including well-being precaution services and other services). Place of Service 13 also known as POS 13 in Medical billing. When the services performed in Assisted Living Place, then it is indicated with place of service 13.
    //	14	Place of service 14 is stated on claim form when a residence, with common living areas, where customers get direction and other facilities like behavioral and/or social facilities, custodial facility, and slight facilities (Example: Prescription management) performed.
    //	15	Place of service 15 also called as POS 15 and it is indicated when a facility/unit that moves from place-to-place equipped to provide preventive, screening, diagnostic, and/or treatment services.
    //	16	Place of Service 16 is indicated, when a place or location where the patient receives health care services, and which is not identified by any other POS code.
    //	17	POS 17 is reported when a walk-in health clinic, other than an office, urgent care facility, pharmacy or independent clinic and not described by any other POS code, that is placed inside a retail process and provides, on an ambulatory source, preventive and primary care services.
    //	18	
    //	19	Place of Service 19 – POS 19 Description in Medical Billing
    //	20	POS 20 – Urgent Care Facility Description: POS 20 is indicated when a location, distinct from a hospital emergency room, an office, or a clinic, whose purpose is to diagnose and treat illness or injury for unscheduled, ambulatory patients seeking immediate medical attention. Place of Service 20 is also known as POS 20 and when the services performed in an “Urgent Care Facility”, it is indicated by place of service 20 in medical billing
    //	21	Place of Service 21 – POS 21 description with example in Medical Billing
    //	22	Place of Service 22 – POS 22 vs 11 with Description in Medical billing
    //	23	Place of Service 23 – POS 23 description with rules in Medical Billing
    //	24	Place of Service 24 – POS 24 Description with example in Medical Billing
    //	25	POS 25 is reported when a facility, other than a hospital’s maternity facilities or a physician’s office, which provides a setting for labor, delivery, and immediate post-partum repair as well as instant care of new born infants.
    //	26	POS 26 is indicated when a medical facility operated by one or more of the Uniformed Services. MTF also denotes to certain former USPHS facilities now chosen as USTF.
    //	31	Place of Service 31- POS 31 vs 32 Description in Medical Billing
    //	32	Place of Service 32 – POS 32 vs 31 Description in Medical Billing
    //	33	Place of Service 33 – Custodial Care Facility Description: Place of service 33 is reported when a facility which provides room, board and other personal assistance services, generally on a long-term basis, and which does not include a medical component. Place of service 33 is also known as POS 33 in Medical Billing and when the health care services rendered in Custodial Care facility it is reported with POS 33.
    //	34	Place of Service 34 is reported when a facility, other than a patient’s home, in which palliative and supportive care for terminally ill patients and their families are provided. Place of service 34 reported on the claim form, when the procedure performed in Hospice. It is also known as POS 34.
    //	41	POS 41 is stated when a land vehicle specially designed, equipped and operated for lifesaving and transferring the sick or injured.
    //	42	POS 42 is stated on a claim form when water or an air vehicle specially designed, furnished and operated for lifesaving and transferring the sick or injured.
    //	49	Place of Service 49 – Independent Clinic Description: Place of service 49 is indicated when a location, not part of a hospital and not described by any other Place of Service code, that is organized and operated to deliver precautionary, diagnostic, therapeutic, rehabilitative, or palliative facilities to outpatients only. Place of Service 49 is also known as POS 49. When the procedure performed in Independent Clinic, then the claims reported with place of service 49.
    //	50	Place of service 50 – Federally Qualified Health Center Description: Place of service 50 is used when a facility placed in a medically underserved space that delivers Medicare recipients precautionary primary medical services under the broad direction of a practitioner. Place of Service 50 in Medical billing also called as POS 50. When the procedure performed in Federally Qualified Health Center, it is indicated with place of service 50.
    //	51	Place of Service 51 – Inpatient Psychiatric Facility Description: Place of Service 51 is indicated when a facility that provides inpatient psychiatric services for the diagnosis and treatment of mental illness on a 24-hour basis, by or under the supervision of a physician. Place of service 51 is also known as POS 51. When the procedure performed in “Inpatient Psychiatric Facility”, then it is reported with Place of service 51.
    //	52	POS 52 is reported on a claim form when a facility for the diagnosis and treatment of mental illness that delivers a scheduled therapeutic package for patients who do not necessitate full time hospitalization, but who need wider packages than are probable from outpatient visits to a hospital-based/affiliated facility.
    //	53	POS 53 is indicated on a claim form when a facility that delivers the subsequent facilities such as ·         Outpatient services, with specified outpatient services for kids, the aged, individuals who are persistently long time ill, and residents of the CMHC’s mental health services area who have been discharged from inpatient services at a mental health facility. ·         Emergency care facilities (24 hours a day). ·         Day treatment, other partial hospitalization facilities, or psycho-social rehabilitation facilities. ·         Showing for patients being considered for admission to State mental health facilities to conclude the suitability of such admission and ·         Consultation and education services.
    //	54	Place of service 54 Description: A facility which primarily delivers health-related services and care beyond the level of custodial care to individuals but does not deliver the level of care or services offered in a hospital or Skilled Nursing Facility.
    //	55	Place of service 55 Description: A facility which delivers services for substance abuse (Drug and alcohol) to live-in people who do not need critical medical services. Services consists individual and group remedy and counseling, family counseling, laboratory tests, drugs and supplies, psychological testing, and room and board.
    //	56	Place of service 56 indicated when a facility or separate portion of a facility for psychiatric services which delivers a full 24-hour therapeutically scheduled and professionally operated group living and learning setting.
    //	57	Place of service 57 is reported when a facility which delivers services for substance abuse (Drug and alcohol) on an ambulatory basis.  Services consists individual and group remedy and counseling, family counseling, laboratory tests, drugs and supplies, and psychological testing.
    //	60	Place of service 60 is indicated when a setting where practitioners manage influenza virus and pneumococcal pneumonia vaccinations and bill these treatments as electronic claims, paper claims, or using the roster billing mode. This normally takes place in a mass immunization location (such as: PHC – Public Health Center, drugstore, or mall but may contain a doctor office setting.
    //	61	POS 61 Description: A facility that delivers comprehensive rehabilitation facilities under the direction of a doctor to inpatients with physical disabilities. Services contain physical, occupational therapy, speech pathology, social or psychological facilities, and orthotics and prosthetics facilities.
    //	62	POS 62 is reported when a facility that delivers comprehensive rehabilitation facilities under the direction of a doctor to outpatients with physical disabilities. Services contain physical, occupational therapy, and speech pathology facilities.
    //	65	POS 65 reported when a facility other than a hospital, which delivers dialysis services, maintenance, and/or training to patients or caregivers on an ambulatory or home-care basis.
    //	71	POS 71 is stated on the claim form when a certified facility which is situated in a rural medically underserved region that delivers ambulatory primary health care under the general instructions of a doctor.
    //	72	Place of Service 72 – Rural Health Clinic Description: Place of Service 72 is reported on the claim form when a certified facility which is located in a rural medically underserved area that provides ambulatory primary health care under the general instructions of a physician. Place of Service 72 is indicated when the procedure performed in Rural health Clinic. Place of Service 72 is also called as POS 72 in Medical Billing.
    //	81	Place of Service 81 – POS 81 description with guidelines in Medical Billing
    //	99	Other Place of Service which is not identified above should be reported with place of service 99
}
