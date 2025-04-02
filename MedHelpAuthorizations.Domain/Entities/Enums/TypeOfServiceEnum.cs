using MedHelpAuthorizations.Domain.CustomAttributes;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    /// <summary>
    ///  Tyoes of Service Enumeration 
    /// </summary>
    public enum TypeOfServiceEnum
    {
        ///// <summary>
        ///// None
        ///// </summary>
        //[ServiceType("None", "No Service", "", "None")]
        //None = 0,

        /// <summary>
        /// Medical services to diagnose and/or treat a medical condition, illness, or injury.
        /// </summary>
        [ServiceType("Medical Care", "Medical services to diagnose and/or treat a medical condition, illness, or injury.", "09/20/2009", "1")]
        MedicalCare = 1,

        /// <summary>
        /// Surgical services provided by a healthcare provider.
        /// </summary>
        [ServiceType("Surgical", "Surgical services provided by a healthcare provider.", "09/20/2009", "2")]
        Surgical = 2,

        /// <summary>
        /// Counseling and/or coordination of care with other Physicians, other qualified Healthcare Providers or agencies.
        /// </summary>
        [ServiceType("Consultation", "Counseling and/or coordination of care with other Physicians, other qualified Healthcare Providers or agencies.", "09/20/2009", "3")]
        Consultation = 3,

        /// <summary>
        /// Diagnostic x-ray provided by a healthcare provider.
        /// </summary>
        [ServiceType("Diagnostic X-Ray", "Diagnostic x-ray provided by a healthcare provider.", "09/20/2009", "4")]
        DiagnosticXRay = 4,

        /// <summary>
        /// Diagnostic lab provided by a healthcare provider.
        /// </summary>
        [ServiceType("Diagnostic Lab", "Diagnostic lab provided by a healthcare provider.", "09/20/2009", "5")]
        DiagnosticLab = 5,

        /// <summary>
        /// Radiation therapy provided by a healthcare provider.
        /// </summary>
        [ServiceType("Radiation Therapy", "Radiation therapy provided by a healthcare provider.", "09/20/2009", "6")]
        RadiationTherapy = 6,

        /// <summary>
        /// Anesthesia services provided by a healthcare provider.
        /// </summary>
        [ServiceType("Anesthesia", "Anesthesia services provided by a healthcare provider.", "09/20/2009", "7")]
        Anesthesia = 7,

        /// <summary>
        /// Assistant surgeon/surgical assistance provided by a healthcare provider if required because of the complexity of the surgical procedures.
        /// </summary>
        [ServiceType("Surgical Assistance", "Assistant surgeon/surgical assistance provided by a healthcare provider if required because of the complexity of the surgical procedures.", "09/20/2009", "8")]
        SurgicalAssistance = 8,

        /// <summary>
        /// Small electronic device that is worn in or above the ear.
        /// </summary>
        [ServiceType("Hearing Aid", "Small electronic device that is worn in or above the ear.", "09/20/2009", "9")]
        HearingAid = 9,

        /// <summary>
        /// The allotment of whole blood, blood plasma, or blood derivatives.
        /// </summary>
        [ServiceType("Blood", "The allotment of whole blood, blood plasma, or blood derivatives.", "09/20/2009", "10")]
        Blood = 10,

        /// <summary>
        /// Used equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a health care provider for use in the home.
        /// </summary>
        [ServiceType("Durable Medical Equipment Used", "Used equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a health care provider for use in the home.", "09/20/2009", "11")]
        DurableMedicalEquipmentUsed = 11,

        /// <summary>
        /// Purchased equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a healthcare provider for use in the home.
        /// </summary>
        [ServiceType("Durable Medical Equipment Purchased", "Purchased equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a healthcare provider for use in the home.", "09/20/2009", "12")]
        DurableMedicalEquipmentPurchased = 12,

        /// <summary>
        /// Batteries or other type of device used to charge a hearing aid.
        /// </summary>
        [ServiceType("Hearing Aid Power Source", "Batteries or other type of device used to charge a hearing aid.", "09/20/2009", "13")]
        HearingAidPowerSource = 13,

        /// <summary>
        /// Supplies to support treatment of kidneys, or bladder functions. (Example: Dialysis Supplies and/or catheters)
        /// </summary>
        [ServiceType("Renal Supplies", "Supplies to support treatment of kidneys, or bladder functions. (Example: Dialysis Supplies and/or catheters)", "09/20/2009", "14")]
        RenalSupplies = 14,

        /// <summary>
        /// Services related to the preparation for admission to establish the patient's current health status.
        /// </summary>
        [ServiceType("Pre-Admission Testing", "Services related to the preparation for admission to establish the patient's current health status.", "09/20/2009", "17")]
        PreAdmissionTesting = 17,

        /// <summary>
        /// Rental equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a healthcare provider for use in the home.
        /// </summary>
        [ServiceType("Durable Medical Equipment Rental", "Rental equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a healthcare provider for use in the home.", "09/20/2009", "18")]
        DurableMedicalEquipmentRental = 18,

        /// <summary>
        /// Services provided by a physician or other healthcare provider related to administration of Pneumococcal Pneumonia vaccination.
        /// </summary>
        [ServiceType("Pneumonia Vaccine", "Services provided by a physician or other healthcare provider related to administration of Pneumococcal Pneumonia vaccination.", "09/20/2009", "19")]
        PneumoniaVaccine = 19,

        /// <summary>
        /// Second professional opinion sought to verify or confirm the necessity for surgical procedures.
        /// </summary>
        [ServiceType("Second Surgical Opinion", "Second professional opinion sought to verify or confirm the necessity for surgical procedures.", "09/20/2009", "20")]
        SecondSurgicalOpinion = 20,

        /// <summary>
        /// Third professional opinion sought to verify or confirm the necessity for surgical procedures.
        /// </summary>
        [ServiceType("Third Surgical Opinion", "Third professional opinion sought to verify or confirm the necessity for surgical procedures.", "09/20/2009", "21")]
        ThirdSurgicalOpinion = 21,

        /// <summary>
        /// Services related to a systematic way of helping individuals and groups towards better adaptation to society.
        /// </summary>
        [ServiceType("Social Work", "Services related to a systematic way of helping individuals and groups towards better adaptation to society.", "09/20/2009", "22")]
        SocialWork = 22,

        /// <summary>
        /// The translation of data gathered by clinical and radiographic examination into an organized, classified definition of conditions present.
        /// </summary>
        [ServiceType("Diagnostic Dental", "The translation of data gathered by clinical and radiographic examination into an organized, classified definition of conditions present.", "09/20/2009", "23")]
        DiagnosticDental = 23,

        /// <summary>
        /// The art and science of examination, diagnosis, and treatment of diseases affecting the periodontium; a study of the supporting structures of the teeth, normal anatomy and physiology and the deviations.
        /// </summary>
        [ServiceType("Periodontics", "The art and science of examination, diagnosis, and treatment of diseases affecting the periodontium; a study of the supporting structures of the teeth, normal anatomy and physiology and the deviations.", "09/20/2009", "24")]
        Periodontics = 24,

        /// <summary>
        /// Broad term applied to any restorations to the tooth/teeth structure(s). Anterior teeth include up to five surface classifications - Mesial, Distal, Incisal, Lingual and Labial. Posterior teeth include up to five surface classifications: Mesial, Distal, Occlusal, Lingual and Buccal.
        /// </summary>
        [ServiceType("Restorative", "Broad term applied to any restorations to the tooth/teeth structure(s). Anterior teeth include up to five surface classifications - Mesial, Distal, Incisal, Lingual and Labial. Posterior teeth include up to five surface classifications: Mesial, Distal, Occlusal, Lingual and Buccal.", "09/20/2009", "25")]
        Restorative = 25,

        /// <summary>
        /// The branch of dentistry that is concerned with the morphology, physiology and pathology of the dental pulp and periradicular (gum) tissues.
        /// </summary>
        [ServiceType("Endodontics", "The branch of dentistry that is concerned with the morphology, physiology and pathology of the dental pulp and periradicular (gum) tissues.", "09/20/2009", "26")]
        Endodontics = 26,

        /// <summary>
        /// The branch of prosthetics is concerned with the restoration of stomatognathic and associated facial structure that have been affected by disease, injury, surgery, or congenital defect.
        /// </summary>
        [ServiceType("Maxillofacial Prosthetics", "The branch of prosthetics is concerned with the restoration of stomatognathic and associated facial structure that have been affected by disease, injury, surgery, or congenital defect.", "09/20/2009", "27")]
        MaxillofacialProsthetics = 27,

        /// <summary>
        /// Typically these services involve a drug such as anesthesia or other substances that serve as a supplemental purpose in dental therapy.
        /// </summary>
        [ServiceType("Adjunctive Dental Services", "Typically these services involve a drug such as anesthesia or other substances that serve as a supplemental purpose in dental therapy.", "09/20/2009", "28")]
        AdjunctiveDentalServices = 28,

        /// <summary>
        /// Plan coverage and general benefits for the member's policy or contract.
        /// </summary>
        [ServiceType("Plan Coverage and General Benefits", "Plan coverage and general benefits for the member's policy or contract.", "09/20/2009", "30")]
        PlanCoverageAndGeneralBenefits = 30,

        /// <summary>
        /// Plan waiting period.
        /// </summary>
        [ServiceType("Plan Waiting Period", "Plan waiting period.", "09/20/2009", "32")]
        PlanWaitingPeriod = 32, 

        /// <summary>
        /// Manipulations and modalities provided by a healthcare provider.
        /// </summary>
        [ServiceType("Chiropractic", "Manipulations and modalities provided by a healthcare provider.", "09/20/2009", "33")]
        Chiropractic = 33,

        /// <summary>
        /// The treatment of the teeth and their supporting structures.
        /// </summary>
        [ServiceType("Dental Care", "The treatment of the teeth and their supporting structures.", "09/20/2009", "35")]
        DentalCare = 35,

        /// <summary>
        /// An artificial replacement for the natural crown of the tooth covering all five surfaces (Anterior teeth surface classifications - Mesial, Distal, Incisal, Lingual and Labial. Posterior teeth surface classifications: Mesial, Distal, Occlusal, Lingual and Buccal.
        /// </summary>
        [ServiceType("Dental Crowns", "An artificial replacement for the natural crown of the tooth covering all five surfaces (Anterior teeth surface classifications - Mesial, Distal, Incisal, Lingual and Labial. Posterior teeth surface classifications: Mesial, Distal, Occlusal, Lingual and Buccal.", "09/20/2009", "36")]
        DentalCrowns = 36,

        /// <summary>
        /// Supplies or appliances for care of teeth due to accidental injury provided by healthcare provider.
        /// </summary>
        [ServiceType("Dental Accident", "Supplies or appliances for care of teeth due to accidental injury provided by healthcare provider.", "09/20/2009", "37")]
        DentalAccident = 37,

        /// <summary>
        /// The area of dentistry concerned with the supervision, guidance, and correction of the growing and mature orofacial structures. This includes conditions that require movement of the teeth or correction of the malrelationships and malformations of related structures by the adjustment of relationships between and among teeth and facial bones by the application of forces or the stimulation and redirection of functional forces within the craniofacial complex.
        /// </summary>
        [ServiceType("Orthodontics", "The area of dentistry concerned with the supervision, guidance, and correction of the growing and mature orofacial structures. This includes conditions that require movement of the teeth or correction of the malrelationships and malformations of related structures by the adjustment of relationships between and among teeth and facial bones by the application of forces or the stimulation and redirection of functional forces within the craniofacial complex.", "09/20/2009", "38")]
        Orthodontics = 38,

        /// <summary>
        /// The part of dentistry pertaining to the restoration and maintenance of oral function, comfort, appearance and health of the patient by replacement of missing teeth and contiguous tissues with artificial substitutes. It has three main branches: removable prosthodontics, fixed prosthodontics and maxillofacial prosthetics.
        /// </summary>
        [ServiceType("Prosthodontics", "The part of dentistry pertaining to the restoration and maintenance of oral function, comfort, appearance and health of the patient by replacement of missing teeth and contiguous tissues with artificial substitutes. It has three main branches: removable prosthodontics, fixed prosthodontics and maxillofacial prosthetics.", "09/20/2009", "39")]
        Prosthodontics = 39,

        /// <summary>
        /// Diagnosis and treatment of disorders of the mouth, teeth, jaws and facial structure provided by a healthcare provider.
        /// </summary>
        [ServiceType("Oral Surgery", "Diagnosis and treatment of disorders of the mouth, teeth, jaws and facial structure provided by a healthcare provider.", "09/20/2009", "40")]
        OralSurgery = 40,

        /// <summary>
        /// Preventive Dental.
        /// </summary>
        [ServiceType("Preventive Dental", "The dental procedures in dental practice and health programs that prevent the occurrence of oral diseases.", "09/20/2009", "41")]
        PreventiveDental = 41,

        /// <summary>
        /// Home Health Care.
        /// </summary>
        [ServiceType("Home Health Care", "Healthcare services rendered in the home by a healthcare provider.", "09/20/2009", "42")]
        HomeHealthCare = 42,

        /// <summary>
        /// Home Health Prescriptions.
        /// </summary>
        [ServiceType("Home Health Prescriptions", "Home Health Prescriptions.", "09/20/2009", "43")]
        HomeHealthPrescriptions = 43,

        /// <summary>
        /// Hospice.
        /// </summary>
        [ServiceType("Hospice", "An integrated set of services and supplies to provide palliative and supportive care to terminally ill patients.", "09/20/2009", "45")]
        Hospice = 45,

        /// <summary>
        /// Respite Care.
        /// </summary>
        [ServiceType("Respite Care", "Services related to temporary care of a dependent elderly, ill, or handicapped person, providing relief for their usual caregivers.", "09/20/2009", "46")]
        RespiteCare = 46,

        /// <summary>
        /// Hospitalization.
        /// </summary>
        [ServiceType("Hospitalization", "Hospital Inpatient and Outpatient services and supplies for a patient who may or may not have been admitted to a hospital, for the purpose of receiving medical care or other health services.", "09/20/2009", "47")]
        Hospitalization = 47,

        /// <summary>
        /// Hospital - Room and Board.
        /// </summary>
        [ServiceType("Hospital - Room and Board", "Hospital - Room and Board.", "09/20/2009", "49")]
        HospitalRoomAndBoard = 49,

        /// <summary>
        /// Care provided for an individual when they cannot care for themselves within the home or in a facility.
        /// </summary>
        [ServiceType("Long Term Care", "Care provided for an individual when they cannot care for themselves within the home or in a facility.", "09/20/2009", "54")]
        LongTermCare = 54,

        /// <summary>
        /// Major Medical.
        /// </summary>
        [ServiceType("Major Medical", "Major Medical.", "09/20/2009", "55")]
        MajorMedical = 55,

        /// <summary>
        /// Ambulance, Ambulate or other Medical transport services.
        /// </summary>
        [ServiceType("Medically Related Transportation", "Ambulance, Ambulate or other Medical transport services.", "09/20/2009", "56")]
        MedicallyRelatedTransportation = 56,

        /// <summary>
        /// Indicates whether a patient has active or inactive coverage for the service date requested.
        /// </summary>
        [ServiceType("Plan Coverage", "Indicates whether a patient has active or inactive coverage for the service date requested.", "09/20/2009", "60")]
        PlanCoverage = 60,

        /// <summary>
        /// Services to treat infertility.
        /// </summary>
        [ServiceType("In-vitro Fertilization", "Services to treat infertility.", "09/20/2009", "61")]
        InVitroFertilization = 61,

        /// <summary>
        /// Diagnostic MRI (Magnetic Resonance Imaging) services.
        /// </summary>
        [ServiceType("MRI Scan", "Diagnostic MRI (Magnetic Resonance Imaging) services.", "09/20/2009", "62")]
        MRIScan = 62,

        /// <summary>
        /// Services related to the collection of tissues, organs, or fluids for use in the treatment for another person.
        /// </summary>
        [ServiceType("Donor Procedures", "Services related to the collection of tissues, organs, or fluids for use in the treatment for another person.", "09/20/2009", "63")]
        DonorProcedures = 63,

        /// <summary>
        /// A system of alternative treatment that involves pricking the skin or tissues with needles.
        /// </summary>
        [ServiceType("Acupuncture", "A system of alternative treatment that involves pricking the skin or tissues with needles.", "09/20/2009", "64")]
        Acupuncture = 64,

        /// <summary>
        /// Management of the infant during the transition to extra uterine life and subsequent period of stabilization.
        /// </summary>
        [ServiceType("Newborn Care", "Management of the infant during the transition to extra uterine life and subsequent period of stabilization.", "09/20/2009", "65")]
        NewbornCare = 65,

        /// <summary>
        /// Creation of slides from tissues and its interpretation provided by a healthcare provider.
        /// </summary>
        [ServiceType("Pathology", "Creation of slides from tissues and its interpretation provided by a healthcare provider.", "09/20/2009", "66")]
        Pathology = 66,

        /// <summary>
        /// Treatment to assist in the discontinuation of the use of nicotine.
        /// </summary>
        [ServiceType("Smoking Cessation", "Treatment to assist in the discontinuation of the use of nicotine.", "09/20/2009", "67")]
        SmokingCessation = 67,

        /// <summary>
        /// Medical services and physician visits which are recommended by the American Pediatric Association as appropriate and routine care for a child to a specific age limit.
        /// </summary>
        [ServiceType("Well Baby Care", "Medical services and physician visits which are recommended by the American Pediatric Association as appropriate and routine care for a child to a specific age limit.", "09/20/2009", "68")]
        WellBabyCare = 68,

        /// <summary>
        /// Services related to maternity care including related conditions resulting in childbirth when provided, or ordered and billed by a physician or nurse midwife.
        /// </summary>
        [ServiceType("Maternity", "Services related to maternity care including related conditions resulting in childbirth when provided, or ordered and billed by a physician or nurse midwife.", "09/20/2009", "69")]
        Maternity = 69,

        /// <summary>
        /// Services related to the transfer of living organs or tissue from one body to another.
        /// </summary>
        [ServiceType("Transplants", "Services related to the transfer of living organs or tissue from one body to another.", "09/20/2009", "70")]
        Transplants = 70,

        /// <summary>
        /// Services related to hearing disorders, including evaluation of hearing function and rehabilitation of patients with hearing impairment.
        /// </summary>
        [ServiceType("Audiology", "Services related to hearing disorders, including evaluation of hearing function and rehabilitation of patients with hearing impairment.", "09/20/2009", "71")]
        Audiology = 71,

        /// <summary>
        /// Services related to the use of inhaled agents to treat respiratory diseases and conditions.
        /// </summary>
        [ServiceType("Inhalation Therapy", "Services related to the use of inhaled agents to treat respiratory diseases and conditions.", "09/20/2009", "72")]
        InhalationTherapy = 72,

        /// <summary>
        /// Services required to determine the diagnose to treat a medical condition, illness, or injury.
        /// </summary>
        [ServiceType("Diagnostic Medical", "Services required to determine the diagnose to treat a medical condition, illness, or injury.", "09/20/2009", "73")]
        DiagnosticMedical = 73,

        /// <summary>
        /// A nurse who is hired to provide focused care to an individual patient in a hospital, clinic, nursing home or patient's home.
        /// </summary>
        [ServiceType("Private Duty Nursing", "A nurse who is hired to provide focused care to an individual patient in a hospital, clinic, nursing home or patient's home.", "09/20/2009", "74")]
        PrivateDutyNursing = 74,

        /// <summary>
        /// A device that is used to replace a part of the body that is missing.
        /// </summary>
        [ServiceType("Prosthetics", "A device that is used to replace a part of the body that is missing.", "09/20/2009", "75")]
        Prosthetics = 75,

        /// <summary>
        /// The process by which uric acid and urea are removed from circulating blood by means of a dialyzer.
        /// </summary>
        [ServiceType("Dialysis", "The process by which uric acid and urea are removed from circulating blood by means of a dialyzer.", "09/20/2009", "76")]
        Dialysis = 76,

        /// <summary>
        /// Services related to diagnosis and treatment of the ear and related structures.
        /// </summary>
        [ServiceType("Otology", "Services related to diagnosis and treatment of the ear and related structures.", "09/20/2009", "77")]
        Otology = 77,

        /// <summary>
        /// The treatment of disease by means of chemicals that have a specific toxic effect upon the disease-producing microorganisms or that selectively destroy cancerous tissue.
        /// </summary>
        [ServiceType("Chemotherapy", "The treatment of disease by means of chemicals that have a specific toxic effect upon the disease-producing microorganisms or that selectively destroy cancerous tissue.", "09/20/2009", "78")]
        Chemotherapy = 78,

        /// <summary>
        /// A skin or blood test to determine what substance, or allergen, may trigger an allergic response in a person.
        /// </summary>
        [ServiceType("Allergy Testing", "A skin or blood test to determine what substance, or allergen, may trigger an allergic response in a person.", "09/20/2009", "79")]
        AllergyTesting = 79,

        /// <summary>
        /// The introduction of a vaccine with the goal of producing immunity.
        /// </summary>
        [ServiceType("Immunizations", "The introduction of a vaccine with the goal of producing immunity.", "09/20/2009", "80")]
        Immunizations = 80,

        /// <summary>
        /// A physical examination performed on asymptomatic patients for medical screening purposes.
        /// </summary>
        [ServiceType("Routine Physical", "A physical examination performed on asymptomatic patients for medical screening purposes.", "09/20/2009", "81")]
        RoutinePhysical = 81,

        /// <summary>
        /// Educational services that assist individuals and couples to anticipate and attain their desired number of children and the spacing and timing of their births. It is achieved through use of contraceptive methods and the treatment of involuntary infertility.
        /// </summary>
        [ServiceType("Family Planning", "Educational services that assist individuals and couples to anticipate and attain their desired number of children and the spacing and timing of their births. It is achieved through use of contraceptive methods and the treatment of involuntary infertility.", "09/20/2009", "82")]
        FamilyPlanning = 82,

        /// <summary>
        /// Services to diagnose and/or treat infertility. Covered services may include assisted reproductive technology procedures.
        /// </summary>
        [ServiceType("Infertility", "Services to diagnose and/or treat infertility. Covered services may include assisted reproductive technology procedures.", "09/20/2009", "83")]
        Infertility = 83,

        /// <summary>
        /// Services related to the elective termination of a pregnancy.
        /// </summary>
        [ServiceType("Abortion", "Services related to the elective termination of a pregnancy.", "09/20/2009", "84")]
        Abortion = 84,

        /// <summary>
        /// Services related to diagnosis and treatment of HIV - AIDS.
        /// </summary>
        [ServiceType("HIV - AIDS Treatment", "Services related to diagnosis and treatment of HIV - AIDS.", "09/20/2009", "85")]
        HIVAIDSTreatment = 85,

        /// <summary>
        /// Services provided by healthcare providers for the treatment of a sudden and unexpected medical condition or injury which requires immediate medical attention.
        /// </summary>
        [ServiceType("Emergency Services", "Services provided by healthcare providers for the treatment of a sudden and unexpected medical condition or injury which requires immediate medical attention.", "09/20/2009", "86")]
        EmergencyServices = 86,

        /// <summary>
        /// Services related to diagnosis and treatment of cancer not performed by an Oncologist.
        /// </summary>
        [ServiceType("Cancer Treatment", "Services related to diagnosis and treatment of cancer not performed by an Oncologist.", "09/20/2009", "87")]
        CancerTreatment = 87,

        /// <summary>
        /// A licensed entity that dispenses prescription drugs and provides professional pharmacy services, such as clinical pharmacy consulting respective to the dispensing function. The entity may be a retail/chain or independent pharmacy or any other entity which dispenses prescription drugs.
        /// </summary>
        [ServiceType("Retail/Independent Pharmacy", "A licensed entity that dispenses prescription drugs and provides professional pharmacy services, such as clinical pharmacy consulting respective to the dispensing function. The entity may be a retail/chain or independent pharmacy or any other entity which dispenses prescription drugs.", "09/20/2009", "88")]
        RetailIndependentPharmacy = 88,

        /// <summary>
        /// Members have separate cost sharing for prescription drugs and medical coverage.
        /// </summary>
        [ServiceType("Free Standing Prescription Drug", "Members have separate cost sharing for prescription drugs and medical coverage.", "09/20/2009", "89")]
        FreeStandingPrescriptionDrug = 89,

        /// <summary>
        /// A mail order pharmacy delivers medications directly to patients through the mail.
        /// </summary>
        [ServiceType("Mail Order Pharmacy", "A mail order pharmacy delivers medications directly to patients through the mail.", "09/20/2009", "90")]
        MailOrderPharmacy = 90,

        /// <summary>
        /// The original formulation of a prescription drug, approved by the FDA for distribution.
        /// </summary>
        [ServiceType("Brand Name Prescription Drug", "The original formulation of a prescription drug, approved by the FDA for distribution.", "09/20/2009", "91")]
        BrandNamePrescriptionDrug = 91,

        /// <summary>
        /// Generic drugs are copies of brand-name drugs that have exactly the same dosage, intended use, effects, side effects, route of administration, risks, safety, and strength as the original drug. In other words, their pharmacological effects are exactly the same as those of their brand-name counterparts.
        /// </summary>
        [ServiceType("Generic Prescription Drug", "Generic drugs are copies of brand-name drugs that have exactly the same dosage, intended use, effects, side effects, route of administration, risks, safety, and strength as the original drug. In other words, their pharmacological effects are exactly the same as those of their brand-name counterparts.", "09/20/2009", "92")]
        GenericPrescriptionDrug = 92,

        /// <summary>
        /// Professional services of a physician or other healthcare provider for the care or treatment of conditions of the foot.
        /// </summary>
        [ServiceType("Podiatry", "Professional services of a physician or other healthcare provider for the care or treatment of conditions of the foot.", "09/20/2009", "93")]
        Podiatry = 93,

        /// <summary>
        /// Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).
        /// </summary>
        [ServiceType("Dental and prediagnostic tests and examinations", "Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).", "03/01/2019", "94")]
        DentalAndPrediagnosticTestsAndExaminations = 94,

        /// <summary>
        /// Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).
        /// </summary>
        [ServiceType("Periodontal Surgical Services", "Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).", "03/01/2019", "95")]
        PeriodontalSurgicalServices = 95,

        /// <summary>
        /// Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).
        /// </summary>
        [ServiceType("Adjustment to dentures", "Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).", "03/01/2019", "96")]
        AdjustmentToDenturesRepairsToCompleteDentures = 96,

        /// <summary>
        /// Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).
        /// </summary>
        [ServiceType("Dental, non-surgical extractions", "Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).", "03/01/2019", "97")]
        DentalNonSurgicalExtractions = 97,

        /// <summary>
        /// Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).
        /// </summary>
        [ServiceType("Prescription Drug", "Refer to the American Dental Association Code on Dental Procedures and Nomenclature (CDT Code).", "03/01/2019", "98")]
        PrescriptionDrug = 98,

        /// <summary>
        /// Bariatric services - Services that deal with the causes, education, prevention and treatment of obesity.
        /// Start: 07/01/2019
        /// </summary>
        [ServiceType("Bariatric services", "Bariatric services - Services that deal with the causes, education, prevention and treatment of obesity.", "07/01/2019", "99")]
        BariatricServices = 99,

        /// <summary>
        /// Psychiatric - Services related to the diagnosis or treatment of mental health.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Psychiatric", "Psychiatric - Services related to the diagnosis or treatment of mental health.", "09/20/2009", "A4")]
        Psychiatric = 100,

        /// <summary>
        /// Psychotherapy - Professional services, including individual or group therapy by providers such as psychiatrists, psychologists, clinical social workers, or psychiatric nurses.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Psychotherapy", "Psychotherapy - Professional services, including individual or group therapy by providers such as psychiatrists, psychologists, clinical social workers, or psychiatric nurses.", "09/20/2009", "A6")]
        Psychotherapy = 101,

        /// <summary>
        /// Psychiatric - Inpatient
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Psychiatric - Inpatient", "Psychiatric - Inpatient", "09/20/2009", "A7")]
        PsychiatricInpatient = 102,

        /// <summary>
        /// Psychiatric - Outpatient
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Psychiatric - Outpatient", "Psychiatric - Outpatient", "09/20/2009", "A8")]
        PsychiatricOutpatient = 103,

        /// <summary>
        /// Rehabilitation - Services related to facilitate the process of recovery from injury, illness, or disease to as normal a condition as possible
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Rehabilitation", "Rehabilitation - Services related to facilitate the process of recovery from injury, illness, or disease to as normal a condition as possible", "09/20/2009", "A9")]
        Rehabilitation = 104,

        /// <summary>
        /// Rehabilitation - Inpatient
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Rehabilitation - Inpatient", "Rehabilitation - Inpatient", "09/20/2009", "AB")]
        RehabilitationInpatient = 105,

        /// <summary>
        /// Rehabilitation - Outpatient
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Rehabilitation - Outpatient", "Rehabilitation - Outpatient", "09/20/2009", "AC")]
        RehabilitationOutpatient = 106,

        /// <summary>
        /// Occupational Therapy - Professional and facility occupational therapy services performed by an occupational therapist, physician or other healthcare provider at a hospital, office or other covered facility.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Occupational Therapy", "Occupational Therapy - Professional and facility occupational therapy services performed by an occupational therapist, physician or other healthcare provider at a hospital, office or other covered facility.", "09/20/2009", "AD")]
        OccupationalTherapy = 107,

        /// <summary>
        /// Physical Medicine - Services related to the diagnosis, evaluation, and management of persons of all ages with physical and/or cognitive impairment and disability.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Physical Medicine", "Physical Medicine - Services related to the diagnosis, evaluation, and management of persons of all ages with physical and/or cognitive impairment and disability.", "09/20/2009", "AE")]
        PhysicalMedicine = 108,

        /// <summary>
        /// Speech Therapy - Professional and facility speech therapy services performed by a speech therapist, physician or other healthcare provider at a hospital, office or other covered facility.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Speech Therapy", "Speech Therapy - Professional and facility speech therapy services performed by a speech therapist, physician or other healthcare provider at a hospital, office or other covered facility.", "09/20/2009", "AF")]
        SpeechTherapy = 109,

        /// <summary>
        /// Skilled Nursing Care - Services for a patient in a skilled nursing facility for the purpose of receiving medical care or other health services.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Skilled Nursing Care", "Skilled Nursing Care - Services for a patient in a skilled nursing facility for the purpose of receiving medical care or other health services.", "09/20/2009", "AG")]
        SkilledNursingCare = 110,

        /// <summary>
        /// Substance Abuse - Services provided at a hospital, office, or other covered facility as they are related to the diagnosis and treatment of Substance Abuse.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Substance Abuse", "Services provided at a hospital, office, or other covered facility as they are related to the diagnosis and treatment of Substance Abuse.", "09/20/2009", "AI")]
        SubstanceAbuse = 111,

        /// <summary>
        /// Alcoholism Treatment - Services related to the management of Alcohol dependencies or addiction.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Alcoholism Treatment", "Services related to the management of Alcohol dependencies or addiction.", "09/20/2009", "AJ")]
        AlcoholismTreatment = 112,

        /// <summary>
        /// Drug Addiction - Services related to the management of Drug dependencies or addiction, excluding Alcohol.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Drug Addiction", "Services related to the management of Drug dependencies or addiction, excluding Alcohol.", "09/20/2009", "AK")]
        DrugAddiction = 113,

        /// <summary>
        /// Optometry - Routine vision services furnished by an optometrist. May include coverage for eyeglasses, contact lenses, routine eye exams, and/or vision testing for the prescribing or fitting of eyeglasses or contact lenses.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Optometry", "Routine vision services furnished by an optometrist. May include coverage for eyeglasses, contact lenses, routine eye exams, and/or vision testing for the prescribing or fitting of eyeglasses or contact lenses.", "09/20/2009", "AL")]
        Optometry = 114,

        /// <summary>
        /// Frames - The framework for a pair of eyeglasses.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Frames", "The framework for a pair of eyeglasses.", "09/20/2009", "AM")]
        Frames = 115,

        /// <summary>
        /// Lenses - A piece of transparent substance having two opposite surfaces either both curved or one curved and one plane, used in an optical device in correcting defects of vision.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Lenses", "A piece of transparent substance having two opposite surfaces either both curved or one curved and one plane, used in an optical device in correcting defects of vision.", "09/20/2009", "AO")]
        Lenses = 116,

        /// <summary>
        /// Routine Eye Exam - A series of tests to evaluate an individual's vision and check for eye diseases.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Routine Eye Exam", "A series of tests to evaluate an individual's vision and check for eye diseases.", "09/20/2009", "AP")]
        RoutineEyeExam = 117,

        /// <summary>
        /// Nonmedically Necessary Physical (These physicals are required by other entities e.g., insurance application, pilot license, employment or school) - A physical examination performed on asymptomatic patients for medical screening purposes.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Nonmedically Necessary Physical", "A physical examination performed on asymptomatic patients for medical screening purposes.", "09/20/2009", "AQ")]
        NonmedicallyNecessaryPhysical = 118,

        /// <summary>
        /// Experimental Drug Therapy - Treatment of a physical or mental condition using non-generally accepted drugs, such as not FDA approved, Clinical Trial.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Experimental Drug Therapy", "Treatment of a physical or mental condition using non-generally accepted drugs, such as not FDA approved, Clinical Trial.", "09/20/2009", "AR")]
        ExperimentalDrugTherapy = 119,

        /// <summary>
        /// Burn Care - Services related to the treatment of Burns.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Burn Care", "Services related to the treatment of Burns.", "09/20/2009", "B1")]
        BurnCare = 120,

        /// <summary>
        /// Brand Name Prescription Drug - Formulary - Lists of brand name drugs covered and published by the health plan/payer/processor/PBM to help physicians reach clinically and economically appropriate prescribing decisions for patients.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Brand Name Prescription Drug", "Formulary - Lists of brand name drugs covered and published by the health plan/payer/processor/PBM to help physicians reach clinically and economically appropriate prescribing decisions for patients.", "09/20/2009", "B2")]
        BrandNamePrescriptionDrugFormulary = 121,

        /// <summary>
        /// Brand Name Prescription Drug - Non-Formulary - A brand name drug that is not listed on the covered and published list of the health plan/payer/processor/PBM.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Brand Name Prescription Drug", "Non-Formulary - A brand name drug that is not listed on the covered and published list of the health plan/payer/processor/PBM.", "09/20/2009", "B3")]
        BrandNamePrescriptionDrugNonFormulary = 122,

        /// <summary>
        /// Independent Medical Evaluation - Services when a doctor/physical therapist/chiropractor/psychologist/neuropsychologist who has not previously been involved in a person's care examines an individual. There is no doctor/therapist-patient relationship.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Independent Medical Evaluation", "Services when a doctor/physical therapist/chiropractor/psychologist/neuropsychologist who has not previously been involved in a person's care examines an individual. There is no doctor/therapist-patient relationship.", "09/20/2009", "BA")]
        IndependentMedicalEvaluation = 123,

        /// <summary>
        /// Psychiatric Treatment Partial Hospitalization
        /// Start: 09/20/2009 | Last Modified: 03/01/2023
        /// </summary>
        [ServiceType("Psychiatric Treatment Partial Hospitalization", "Psychiatric Treatment Partial Hospitalization.", "09/20/2009", "BB")]
        PsychiatricTreatmentPartialHospitalization = 124,

        /// <summary>
        /// Day Care (Psychiatric)
        /// Start: 09/20/2009 | Last Modified: 03/01/2023
        /// </summary>
        [ServiceType("Day Care (Psychiatric)", "Day Care (Psychiatric).", "09/20/2009", "BC")]
        DayCarePsychiatric = 125,

        /// <summary>
        /// Cognitive Therapy - A type of psychotherapy in which negative patterns of thought are challenged to alter unwanted behavior patterns or treat mood disorders.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Cognitive Therapy", "A type of psychotherapy in which negative patterns of thought are challenged to alter unwanted behavior patterns or treat mood disorders.", "09/20/2009", "BD")]
        CognitiveTherapy = 126,

        /// <summary>
        /// Massage Therapy - The manipulation of muscles and other soft tissues of the body by a therapist for the treatment of health conditions.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Massage Therapy", "The manipulation of muscles and other soft tissues of the body by a therapist for the treatment of health conditions.", "09/20/2009", "BE")]
        MassageTherapy = 127,

        /// <summary>
        /// Pulmonary Rehabilitation - Services and instructional guidance administered to an individual suffering from respiratory disease in an attempt to improve the quality of life for the patient.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Pulmonary Rehabilitation", "Services and instructional guidance administered to an individual suffering from respiratory disease in an attempt to improve the quality of life for the patient.", "09/20/2009", "BF")]
        PulmonaryRehabilitation = 128,

        /// <summary>
        /// Cardiac Rehabilitation - Services and instructional guidance rendered by a physician or other healthcare provider in a hospital or covered facility to help an individual recover from a cardiovascular event.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Cardiac Rehabilitation", "Services and instructional guidance rendered by a physician or other healthcare provider in a hospital or covered facility to help an individual recover from a cardiovascular event.", "09/20/2009", "BG")]
        CardiacRehabilitation = 129,

        /// <summary>
        /// Pediatric - Treatment or care related to infants, children, and adolescents.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Pediatric", "Treatment or care related to infants, children, and adolescents.", "09/20/2009", "BH")]
        Pediatric = 130,

        /// <summary>
        /// Nursery Room and Board - Treatment or care related to newborns.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Nursery Room and Board", "Treatment or care related to newborns.", "09/20/2009", "BI")]
        NurseryRoomAndBoard = 131,

        /// <summary>
        /// Orthopedic - Services related to the correction or prevention of deformities, disorders, or injuries of the skeleton and associated structures.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Orthopedic", "Services related to the correction or prevention of deformities, disorders, or injuries of the skeleton and associated structures.", "09/20/2009", "BK")]
        Orthopedic = 132,

        /// <summary>
        /// Cardiac - Services of or relating to the heart.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Cardiac", "Services of or relating to the heart.", "09/20/2009", "BL")]
        Cardiac = 133,

        /// <summary>
        /// Lymphatic - Services related to a lymph, lymph node, or a lymphatic vessel.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Lymphatic", "Services related to a lymph, lymph node, or a lymphatic vessel.", "09/20/2009", "BM")]
        Lymphatic = 134,

        /// <summary>
        /// Gastrointestinal - Services to treat disorders of the stomach and intestines, and related systems.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Gastrointestinal", "Services to treat disorders of the stomach and intestines, and related systems.", "09/20/2009", "BN")]
        Gastrointestinal = 135,

        /// <summary>
        /// Endocrine - Services related to the systems that secrete hormones.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Endocrine", "Services related to the systems that secrete hormones.", "09/20/2009", "BP")]
        Endocrine = 136,

        /// <summary>
        /// Neurology - Services related to the treatment of the nerves or nervous system.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Neurology", "Services related to the treatment of the nerves or nervous system.", "09/20/2009", "BQ")]
        Neurology = 137,

        /// <summary>
        /// Gynecological - Medical care and management of the female reproductive system and associated disorders provided by a physician or other healthcare provider.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Gynecological", "Medical care and management of the female reproductive system and associated disorders provided by a physician or other healthcare provider.", "09/20/2009", "BT")]
        Gynecological = 138,

        /// <summary>
        /// Obstetrical - Medical care and management related to the care of a woman prior, during, and after pregnancy, provided by a physician or other healthcare provider.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Obstetrical", "Medical care and management related to the care of a woman prior, during, and after pregnancy, provided by a physician or other healthcare provider.", "09/20/2009", "BU")]
        Obstetrical = 139,

        /// <summary>
        /// Obstetrical/Gynecological
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Obstetrical/Gynecological", "Obstetrical/Gynecological.", "09/20/2009", "BV")]
        ObstetricalGynecological = 140,

        /// <summary>
        /// Physician Visit - Sick - Professional services rendered by a physician or other healthcare provider during a non-routine visit related to an illness.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Physician Visit - Sick", "Professional services rendered by a physician or other healthcare provider during a non-routine visit related to an illness.", "09/20/2009", "BY")]
        PhysicianVisitSick = 141,

        /// <summary>
        /// Physician Visit - Well - Professional services rendered by a physician or other healthcare provider during a routine or preventative care visit.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Physician Visit - Well", "Professional services rendered by a physician or other healthcare provider during a routine or preventative care visit.", "09/20/2009", "BZ")]
        PhysicianVisitWell = 142,

        /// <summary>
        /// Coronary Care - Treatment of diseases of the arteries of the heart.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Coronary Care", "Treatment of diseases of the arteries of the heart.", "09/20/2009", "C1")]
        CoronaryCare = 143,

        /// <summary>
        /// Screening X-ray - X-ray services provided by a physician or other healthcare provider for the purpose of preventative care.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Screening X-ray", "X-ray services provided by a physician or other healthcare provider for the purpose of preventative care.", "09/20/2009", "Ck")]
        ScreeningXRay = 144,

        /// <summary>
        /// Screening Laboratory - Laboratory services provided by a physician or other healthcare provider for the purpose of preventative care.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Screening Laboratory", "Laboratory services provided by a physician or other healthcare provider for the purpose of preventative care.", "09/20/2009", "CL")]
        ScreeningLaboratory = 145,

        /// <summary>
        /// Mammogram, High Risk Patient - Mammography services for patients that have been identified with a greater than normal risk for breast cancers and related diseases.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Mammogram, High Risk Patient", "Mammography services for patients that have been identified with a greater than normal risk for breast cancers and related diseases.", "09/20/2009", "CM")]
        MammogramHighRiskPatient = 146,

        /// <summary>
        /// Mammogram, Low Risk Patient - Mammography services for patients that have been identified with a normal risk for breast cancers and related diseases.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Mammogram, Low Risk Patient", "Mammography services for patients that have been identified with a normal risk for breast cancers and related diseases.", "09/20/2009", "CN")]
        MammogramLowRiskPatient = 147,

        /// <summary>
        /// Flu Vaccination - Services provided by a physician or other healthcare provider related to the administration of influenza virus vaccination.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Flu Vaccination", "Services provided by a physician or other healthcare provider related to the administration of influenza virus vaccination.", "09/20/2009", "CO")]
        FluVaccination = 148,

        /// <summary>
        /// Eyewear Accessories - Services related to Eyewear and Eyewear Accessories.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Eyewear Accessories", "Services related to Eyewear and Eyewear Accessories.", "09/20/2009", "CP")]
        EyewearAccessories = 149,

        /// <summary>
        /// Case Management - Services that assess, plan, implement, coordinate, monitor, and evaluate the options and services required to meet the client's health and human service needs.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Case Management", "Services that assess, plan, implement, coordinate, monitor, and evaluate the options and services required to meet the client's health and human service needs.", "09/20/2009", "Cq")]
        CaseManagement = 150,

        /// <summary>
        /// Dermatology - Services provided by a physician or other healthcare provider involving the skin and its diseases.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Dermatology", "Services provided by a physician or other healthcare provider involving the skin and its diseases.", "09/20/2009", "DG")]
        Dermatology = 151,

        /// <summary>
        /// Durable Medical Equipment - Durable medical equipment that can withstand repeated use and is primarily and customarily used to serve a medical purpose and generally is not useful to a person in the absence of an illness or injury.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Durable Medical Equipment", "Durable medical equipment that can withstand repeated use and is primarily and customarily used to serve a medical purpose and generally is not useful to a person in the absence of an illness or injury.", "09/20/2009", "DM")]
        DurableMedicalEquipment = 152,

        /// <summary>
        /// Diabetic Supplies - Blood sugar (glucose) test strips, monitors, insulin, lancet devices and lancets, glucose control solutions used to monitor and assist in the treatment of diabetes.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Diabetic Supplies", "Blood sugar (glucose) test strips, monitors, insulin, lancet devices and lancets, glucose control solutions used to monitor and assist in the treatment of diabetes.", "09/20/2009", "DS")]
        DiabeticSupplies = 153,

        /// <summary>
        /// Applied Behavioral Analysis Therapy - Services related to the assessment and treatment of learning and/or developmental disabilities to include techniques and principles to bring about meaningful and positive changes in behavior, improve attention, focus, memory, academics and/or increase language, communication, and social skills.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Applied Behavioral Analysis Therapy", "Services related to the assessment and treatment of learning and/or developmental disabilities to include techniques and principles to bring about meaningful and positive changes in behavior, improve attention, focus, memory, academics and/or increase language, communication, and social skills.", "09/20/2009", "E0")]
        AppliedBehavioralAnalysisTherapy = 154,

        /// <summary>
        /// Non-Medical Equipment (non DME) - Durable equipment that can withstand repeated use and serves to augment or replace impaired functionality, environmental control, and facilitate a patient's independent living.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Non-Medical Equipment (non DME)", "Durable equipment that can withstand repeated use and serves to augment or replace impaired functionality, environmental control, and facilitate a patient's independent living.", "09/20/2009", "E1")]
        NonMedicalEquipment = 155,

        /// <summary>
        /// Psychiatric Emergency - Emergency services related to the diagnosis or treatment of mental disease.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Psychiatric Emergency", "Emergency services related to the diagnosis or treatment of mental disease.", "09/20/2009", "E2")]
        PsychiatricEmergency = 156,

        /// <summary>
        /// Step Down Unit - A hospital unit providing a level of care between intensive and routine.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Step Down Unit", "A hospital unit providing a level of care between intensive and routine.", "09/20/2009", "E3")]
        StepDownUnit = 157,

        /// <summary>
        /// Skilled Nursing Facility Head Level of Care - Services directly related to care associated with severe brain injuries requiring a skilled level of care.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Skilled Nursing Facility Head Level of Care", "Services directly related to care associated with severe brain injuries requiring a skilled level of care.", "09/20/2009", "E4")]
        SkilledNursingFacilityHead = 158,

        /// <summary>
        /// Skilled Nursing Facility Ventilator Level of Care - Services directly related to care associated with ventilator-dependent respiratory conditions requiring a skilled level of care.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Skilled Nursing Facility Ventilator Level of Care", "Services directly related to care associated with ventilator-dependent respiratory conditions requiring a skilled level of care.", "09/20/2009", "E5")]
        SkilledNursingFacilityVentilator = 159,

        /// <summary>
        /// Level of Care 1 - Skilled Care - Skilled Nursing Care in a regular hospital bed.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Level of Care 1 - Skilled Care", "Skilled Nursing Care in a regular hospital bed.", "09/20/2009", "E6")]
        LevelOfCare1 = 160,

        /// <summary>
        /// Level of Care 2 - Comprehensive Care - Skilled Nursing Care Level II includes attributes of prior level plus services such as Wound Care (Stage 3), Tracheotomy Care, etc.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Level of Care 2 - Comprehensive Care", "Skilled Nursing Care Level II includes attributes of prior level plus services such as Wound Care (Stage 3), Tracheotomy Care, etc.", "09/20/2009", "E7")]
        LevelOfCare2 = 161,

        /// <summary>
        /// Level of Care 3 - Complex Care - Skilled Nursing Care Level III includes attributes of prior levels plus services such as Ventilator Care, Specialty Beds, Peritoneal Dialysis, etc.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Level of Care 3 - Complex Care", "Skilled Nursing Care Level III includes attributes of prior levels plus services such as Ventilator Care, Specialty Beds, Peritoneal Dialysis, etc.", "09/20/2009", "E8")]
        LevelOfCare3 = 162,

        /// <summary>
        /// Level of Care 4
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Level of Care 4", "Level of Care 4", "09/20/2009", "E9")]
        LevelOfCare4 = 163,

        /// <summary>
        /// Radiographs - An image or picture produced on a radiation-sensitive film emulsion by exposure to ionizing radiation direct through an area, region, or substance of interest, followed by chemical processing of the film.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Radiographs", "An image or picture produced on a radiation-sensitive film emulsion by exposure to ionizing radiation direct through an area, region, or substance of interest, followed by chemical processing of the film.", "01/24/2010", "E10")]
        Radiographs = 164,

        /// <summary>
        /// Diagnostic Imaging - The use of radiographic, sonographic, and other technologies to create a graphic depiction of the body parts in question.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Diagnostic Imaging", "The use of radiographic, sonographic, and other technologies to create a graphic depiction of the body parts in question.", "01/24/2010", "E11")]
        DiagnosticImaging = 165,

        /// <summary>
        /// Fixed Prosthodontics - The branch of prosthodontics concerned with the replacement or restoration of teeth by artificial substitutes that are not readily removable, such as fixed partial dentures, pontics, and abutments.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Fixed Prosthodontics", "The branch of prosthodontics concerned with the replacement or restoration of teeth by artificial substitutes that are not readily removable, such as fixed partial dentures, pontics, and abutments.", "01/24/2010", "E14")]
        FixedProsthodontics = 166,

        /// <summary>
        /// Removable Prosthodontics - The branch of prosthodontics concerned with the replacement or restoration of teeth by artificial substitutes that are readily removable, such as a denture, partial denture, and interim prosthesis.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Removable Prosthodontics", "The branch of prosthodontics concerned with the replacement or restoration of teeth by artificial substitutes that are readily removable, such as a denture, partial denture, and interim prosthesis.", "01/24/2010", "E15")]
        RemovableProsthodontics = 167,

        /// <summary>
        /// Intraoral Images - Complete Series - Complete set of images using radiographic, sonographic, and other technologies representing an image or set of images within the oral cavity.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Intraoral Images - Complete Series", "Complete set of images using radiographic, sonographic, and other technologies representing an image or set of images within the oral cavity.", "01/24/2010", "E16")]
        IntraoralImages = 168,

        /// <summary>
        /// Oral Evaluation - The art and science of evaluation to make a clinical judgment or appraisal of a patient's dental health or condition.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Oral Evaluation", "The art and science of evaluation to make a clinical judgment or appraisal of a patient's dental health or condition.", "01/24/2010", "E17")]
        OralEvaluation = 169,

        /// <summary>
        /// Dental Prophylaxis - A series of procedures where plaque, calculus, and stain are removed from the teeth often referred to as \"prophy\" or teeth cleaning.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Dental Prophylaxis", "A series of procedures where plaque, calculus, and stain are removed from the teeth often referred to as \"prophy\" or teeth cleaning.", "01/24/2010", "E18")]
        DentalProphylaxis = 170,

        /// <summary>
        /// Panoramic Images - A tomogram of the jaws, taken with a specialized machine designed to present a panoramic view of the full circumferential length of the jaws on a single film.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Panoramic Images", "A tomogram of the jaws, taken with a specialized machine designed to present a panoramic view of the full circumferential length of the jaws on a single film.", "01/24/2010", "E19")]
        PanoramicImages = 171,

        /// <summary>
        /// Sealants - A resinous material designed for application to the occlusal surfaces of posterior teeth to seal the surface irregularities and prevent the carious process.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Sealants", "A resinous material designed for application to the occlusal surfaces of posterior teeth to seal the surface irregularities and prevent the carious process.", "01/24/2010", "E20")]
        Sealants = 172,

        /// <summary>
        /// Fluoride Treatments - A separate process from dental prophylaxis of applying prescription strength fluoride product designed to prevent caries.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Fluoride Treatments", "A separate process from dental prophylaxis of applying prescription strength fluoride product designed to prevent caries.", "01/24/2010", "E21")]
        FluorideTreatments = 173,

        /// <summary>
        /// Dental Implants - A device, usually alloplastic, that is surgically inserted into or onto the oral tissue. To be used as a prosthodontic abutment, it should remain quiescent and purely secondary to local tissue physiology.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Dental Implants", "A device, usually alloplastic, that is surgically inserted into or onto the oral tissue. To be used as a prosthodontic abutment, it should remain quiescent and purely secondary to local tissue physiology.", "01/24/2010", "E22")]
        DentalImplants = 174,

        /// <summary>
        /// Temporomandibular Joint Dysfunction - Services related to the Impaired function of the temporomandibular articulation of the jaw.
        /// Start: 01/24/2010
        /// </summary>
        [ServiceType("Temporomandibular Joint Dysfunction", "Services related to the Impaired function of the temporomandibular articulation of the jaw.", "01/24/2010", "E23")]
        TemporomandibularJointDysfunction = 175,

        /// <summary>
        /// Long Term Care Pharmacy - Long term care pharmacy serves the residents of nursing homes, assisted care facilities, extended care facilities, retirement homes, or post-acute care. These are considered \"closed door pharmacies\".
        /// Start: 06/06/2010
        /// </summary>
        [ServiceType("Long Term Care Pharmacy", "Long term care pharmacy serves the residents of nursing homes, assisted care facilities, extended care facilities, retirement homes, or post-acute care. These are considered \"closed door pharmacies\".", "06/06/2010", "E25")]
        LongTermCarePharmacy = 176,

        /// <summary>
        /// Comprehensive Medication Therapy Management Review - A holistic review of medical care provided by pharmacists whose aim is to optimize drug therapy and improve therapeutic outcomes for patients
        /// Start: 06/05/2011
        /// </summary>
        [ServiceType("Comprehensive Medication Therapy Management Review", "A holistic review of medical care provided by pharmacists whose aim is to optimize drug therapy and improve therapeutic outcomes for patients.", "06/05/2011", "E26")]
        ComprehensiveMedicationTherapyManagementReview = 177,

        /// <summary>
        /// Targeted Medication Therapy Management Review - A targeted medication therapy management (MTM) review is consultation with a patient about their medication therapy related to a specific diagnosis, disease state, or medication.
        /// Start: 06/05/2011
        /// </summary>
        [ServiceType("Targeted Medication Therapy Management Review", "A targeted medication therapy management (MTM) review is consultation with a patient about their medication therapy related to a specific diagnosis, disease state, or medication.", "06/05/2011", "E27")]
        TargetedMedicationTherapyManagementReview = 178,

        /// <summary>
        /// Dietary/Nutritional Services - Nutrition and diet counseling such as: weight management, eating disorders, pregnancy, pediatric, food allergy, diabetes, celiac disease
        /// Start: 01/29/2012
        /// </summary>
        [ServiceType("Dietary/Nutritional Services", "Nutrition and diet counseling such as: weight management, eating disorders, pregnancy, pediatric, food allergy, diabetes, celiac disease.", "01/29/2012", "E28")]
        DietaryNutritionalServices = 179,

        /// <summary>
        /// Intensive Cardiac Rehabilitation - A group of physical activities designed to help a patient recover from a cardiovascular event
        /// Start: 06/02/2013
        /// </summary>
        [ServiceType("Intensive Cardiac Rehabilitation", "A group of physical activities designed to help a patient recover from a cardiovascular event.", "06/02/2013", "E33")]
        IntensiveCardiacRehabilitation = 180,

        /// <summary>
        /// Convenience Care - A category of walk-in clinic located in retail stores, supermarkets, and pharmacies that treat uncomplicated minor illnesses
        /// Start: 06/02/2013
        /// </summary>
        [ServiceType("Convenience Care", "A category of walk-in clinic located in retail stores, supermarkets, and pharmacies that treat uncomplicated minor illnesses.", "06/02/2013", "E36")]
        ConvenienceCare = 181,

        /// <summary>
        /// Telemedicine - Services provided via telecommunication and/or information technology venues to provide clinical health services.
        /// Start: 07/01/2015
        /// </summary>
        [ServiceType("Telemedicine", "Services provided via telecommunication and/or information technology venues to provide clinical health services.", "07/01/2015", "E37")]
        Telemedicine = 182,

        /// <summary>
        /// Pharmacist Services - Clinical services provided by a pharmacist
        /// Start: 07/01/2015
        /// </summary>
        [ServiceType("Pharmacist Services", "Clinical services provided by a pharmacist.", "07/01/2015", "E38")]
        PharmacistServices = 183,

        /// <summary>
        /// Diabetic Education - Patient educational program designed to bring awareness of diabetes, what it takes to treat it, and the necessary changes that should be made to improve their lifestyle.
        /// Start: 03/01/2016
        /// </summary>
        [ServiceType("Diabetic Education", "Patient educational program designed to bring awareness of diabetes, what it takes to treat it, and the necessary changes that should be made to improve their lifestyle.", "03/01/2016", "E39")]
        DiabeticEducation = 184,

        /// <summary>
        /// Early Intervention - Services related to treatment for babies or toddlers with developmental delays or disabilities.
        /// Start: 11/01/2016
        /// </summary>
        [ServiceType("Early Intervention", "Services related to treatment for babies or toddlers with developmental delays or disabilities.", "11/01/2016", "E40")]
        EarlyIntervention = 185,

        /// <summary>
        /// Preventive Services - Preventive services such as check-ups, patient counseling, and screenings to prevent illness, disease, and other health-related problems.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Preventive Services", "Preventive services such as check-ups, patient counseling, and screenings to prevent illness, disease, and other health-related problems.", "09/20/2009", "EA")]
        PreventiveServices = 186,

        /// <summary>
        /// Specialty Pharmacy - Specialty pharmacies are designed to efficiently deliver medications with specialized handling, storage, and distribution requirements. Specialty pharmacies are also designed to improve clinical and economic outcomes for patients with complex, often chronic and rare conditions, with close contact and management by clinicians.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Specialty Pharmacy", "Specialty pharmacies are designed to efficiently deliver medications with specialized handling, storage, and distribution requirements. Specialty pharmacies are also designed to improve clinical and economic outcomes for patients with complex, often chronic and rare conditions, with close contact and management by clinicians.", "09/20/2009", "EB")]
        SpecialtyPharmacy = 188,

        /// <summary>
        /// Durable Medical Equipment New - New equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a health care provider for use in the home.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Durable Medical Equipment New", "New equipment needed for medical reasons to be used by a person that is ill or injured and is ordered by a health care provider for use in the home.", "09/20/2009", "EC")]
        DurableMedicalEquipmentNew = 189,

        /// <summary>
        /// CAT Scan - A multi-dimensional diagnostic image of a cross section of the body that is useful in diagnosing disease
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("CAT Scan", "A multi-dimensional diagnostic image of a cross section of the body that is useful in diagnosing disease.", "09/20/2009", "ED")]
        CATScan = 190,

        /// <summary>
        /// Ophthalmology - Services related to diagnosis and treatment of the eye and related structures including surgical services.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Ophthalmology", "Services related to diagnosis and treatment of the eye and related structures including surgical services.", "09/20/2009", "EE")]
        Ophthalmology = 191,

        /// <summary>
        /// Contact Lenses - A thin lens placed directly on the surface of the eye. Contact Lenses are considered medical devices
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Contact Lenses", "A thin lens placed directly on the surface of the eye. Contact Lenses are considered medical devices.", "09/20/2009", "EF")]
        ContactLenses = 192,

        /// <summary>
        /// Fertility Preservation
        /// Start: 11/01/2022
        /// </summary>
        [ServiceType("Fertility Preservation", "Fertility preservation services.", "11/01/2022", "EG")]
        FertilityPreservation = 193,

        /// <summary>
        /// Medically Tailored Meals (MTM) - Meals approved by a medical professional or healthcare plan that reflect appropriate dietary therapy for the individual
        /// Start: 03/01/2023
        /// </summary>
        [ServiceType("Medically Tailored Meals (MTM)", "Meals approved by a medical professional or healthcare plan that reflect appropriate dietary therapy for the individual.", "03/01/2023", "EH")]
        MedicallyTailoredMealsMTM = 194,

        /// <summary>
        /// IV Therapy - Intravenous (IV) therapy is a medical technique of administering fluids directly into a vein.
        /// Start: 03/01/2023
        /// </summary>
        [ServiceType("IV Therapy", "Intravenous (IV) therapy is a medical technique of administering fluids directly into a vein.", "03/01/2023", "EJ")]
        IVTherapy = 195,

        /// <summary>
        /// Medical Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Medical Coverage", "Medical coverage information.", "11/01/2015", "F1")]
        MedicalCoverage = 196,

        /// <summary>
        /// Social Work Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Social Work Coverage", "Social work coverage information.", "11/01/2015", "F2")]
        SocialWorkCoverage = 197,

        /// <summary>
        /// Dental Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Dental Coverage", "Dental coverage information.", "11/01/2015", "F3")]
        DentalCoverage = 198,

        /// <summary>
        /// Hearing Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Hearing Coverage", "Hearing coverage information.", "11/01/2015", "F4")]
        HearingCoverage = 199,

        /// <summary>
        /// Prescription Drug Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Prescription Drug Coverage", "Prescription drug coverage information.", "11/01/2015", "F5")]
        PrescriptionDrugCoverage = 200,

        /// <summary>
        /// Vision Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Vision Coverage", "Vision coverage information.", "11/01/2015", "F6")]
        VisionCoverage = 201,

        /// <summary>
        /// Orthodontia Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Orthodontia Coverage", "Orthodontia coverage information.", "11/01/2015", "F7")]
        OrthodontiaCoverage = 202,

        /// <summary>
        /// Mental Health Coverage - This code cannot be submitted with the 270 Inquiry.
        /// Start: 11/01/2015
        /// </summary>
        [ServiceType("Mental Health Coverage", "Mental health coverage information.", "11/01/2015", "F8")]
        MentalHealthCoverage = 203,

        /// <summary>
        /// Generic Prescription Drug - Formulary - Lists of generic drugs covered and published by the health plan/payer/processor/PBM to help physicians reach clinically and economically appropriate prescribing decisions for patients.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Generic Prescription Drug - Formulary", "Lists of generic drugs covered and published by the health plan/payer/processor/PBM to help physicians reach clinically and economically appropriate prescribing decisions for patients.", "09/20/2009", "GF")]
        GenericPrescriptionDrugFormulary = 204,

        /// <summary>
        /// Generic Prescription Drug - Non-Formulary - A generic drug that is not listed on the covered and published list of the health plan/payer/processor/PBM.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Generic Prescription Drug - Non-Formulary", "A generic drug that is not listed on the covered and published list of the health plan/payer/processor/PBM.", "09/20/2009", "GN")]
        GenericPrescriptionDrugNonFormulary = 205,

        /// <summary>
        /// Allergy - Services for conditions caused by abnormal hypersensitivity of the immune system to medications, chemical or food substances, and/or environmental factors.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Allergy", "Services for conditions caused by abnormal hypersensitivity of the immune system to medications, chemical or food substances, and/or environmental factors.", "09/20/2009", "GY")]
        Allergy = 206,

        /// <summary>
        /// Intensive Care - Continuous and closely monitored health care services provided in a hospital to critically ill patients.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Intensive Care", "Continuous and closely monitored health care services provided in a hospital to critically ill patients.", "09/20/2009", "IC")]
        IntensiveCare = 207,

        /// <summary>
        /// Mental Health - Mental Health services provided by a physician or other healthcare providers who are trained and educated to perform services related to mental health and may be licensed or practice within the scope of licensure or training.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Mental Health", "Mental Health services provided by a physician or other healthcare providers who are trained and educated to perform services related to mental health and may be licensed or practice within the scope of licensure or training.", "09/20/2009", "MH")]
        MentalHealth = 208,

        /// <summary>
        /// Neonatal Intensive Care - Continuous and closely monitored health care services provided in a hospital to critically ill newborn/neonatal patients.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Neonatal Intensive Care", "Continuous and closely monitored health care services provided in a hospital to critically ill newborn/neonatal patients.", "09/20/2009", "NI")]
        NeonatalIntensiveCare = 209,

        /// <summary>
        /// Oncology - Services related to diagnosis and treatment of cancer provided by an Oncology provider
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Oncology", "Services related to diagnosis and treatment of cancer provided by an Oncology provider.", "09/20/2009", "ON")]
        Oncology = 210,


        /// <summary>
        /// Positron Emission Tomography (PET) Scan - A nuclear imaging examination which reveals molecular function and activity.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Positron Emission Tomography (PET) Scan", "A nuclear imaging examination which reveals molecular function and activity.", "09/20/2009", "PE")]
        PositronEmissionTomographyPETScan = 211,

        /// <summary>
        /// Physical Therapy - Services and care related to evaluation and treatment of injury or disorders
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Physical Therapy", "Services and care related to evaluation and treatment of injury or disorders.", "09/20/2009", "PT")]
        PhysicalTherapy = 212,

        /// <summary>
        /// Pulmonary - Services related to the diagnosis and treatment of respiratory conditions.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Pulmonary", "Services related to the diagnosis and treatment of respiratory conditions.", "09/20/2009", "PU")]
        Pulmonary = 213,

        /// <summary>
        /// Renal - Services related to the diagnosis and treatment of kidney conditions.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Renal", "Services related to the diagnosis and treatment of kidney conditions.", "09/20/2009", "RN")]
        Renal = 214,

        /// <summary>
        /// Residential Psychiatric Treatment - Psychiatry services provided at a live-in facility to a person with emotional disorders who requires continuous medication and/or supervision or relief from environmental stresses
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Residential Psychiatric Treatment", "Psychiatry services provided at a live-in facility to a person with emotional disorders who requires continuous medication and/or supervision or relief from environmental stresses.", "09/20/2009", "RT")]
        ResidentialPsychiatricTreatment = 215,

        /// <summary>
        /// Serious Mental Health - Services for disorders characterized by severe deficits and pervasive impairment in multiple areas of development.
        /// Start: 01/30/2011
        /// </summary>
        [ServiceType("Serious Mental Health", "Services for disorders characterized by severe deficits and pervasive impairment in multiple areas of development.", "01/30/2011", "SMH")]
        SeriousMentalHealth = 216,

        /// <summary>
        /// Transitional Care - Services related to the coordination and continuity of health care during a movement from one health care setting to another or to home.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Transitional Care", "Services related to the coordination and continuity of health care during a movement from one health care setting to another or to home.", "09/20/2009", "TC")]
        TransitionalCare = 217,

        /// <summary>
        /// Transitional Nursery Care - Services related to the coordination and continuity of health care for a newborn during a movement from one health care setting to another or to home.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Transitional Nursery Care", "Services related to the coordination and continuity of health care for a newborn during a movement from one health care setting to another or to home.", "09/20/2009", "TN")]
        TransitionalNurseryCare = 218,

        /// <summary>
        /// Urgent Care - Medical services and supplies provided by physicians or other healthcare providers for the treatment of an urgent medical condition or injury which requires medical attention.
        /// Start: 09/20/2009
        /// </summary>
        [ServiceType("Urgent Care", "Medical services and supplies provided by physicians or other healthcare providers for the treatment of an urgent medical condition or injury which requires medical attention.", "09/20/2009", "UC")]
        UrgentCare = 219,

    }
}
