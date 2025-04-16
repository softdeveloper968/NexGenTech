using MedHelpAuthorizations.Domain.CustomAttributes;
using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
	// Added/Updated SpecialtyEnum as per Records present in https://www.cms.gov/Medicare/Provider-Enrollment-and-Certification/MedicareProviderSupEnroll/downloads/taxonomy.pdf
	public enum SpecialtyEnum
	{
		/// <summary>
		/// General Practice
		/// </summary>
		[Description("General Practice")]
		GeneralPractice = 1,


		/// <summary>
		/// General Surgery
		/// </summary>
		[Description("General Surgery")]
		GeneralSurgery = 2,


		/// <summary>
		/// Allergy / Immunology
		/// </summary>
		[Description("Allergy / Immunology")]
		AllergyImmunology = 3,


		/// <summary>
		/// Otolaryngology
		/// </summary>
		[Description("Otolaryngology")]
		Otolaryngology = 4,


		/// <summary>
		/// Anesthesiology
		/// </summary>
		[Description("Anesthesiology")]
		Anesthesiology = 5,


		/// <summary>
		/// Cardiology
		/// </summary>
		[Description("Cardiology")]
		Cardiology = 6,


		/// <summary>
		/// Dermatology
		/// </summary>
		[Description("Dermatology")]
		Dermatology = 7,


		/// <summary>
		/// Family Practice
		/// </summary>
		[Description("Family Practice")]
		FamilyPractice = 8,

		/// <summary>
		/// Interventional Pain Management
		/// </summary>
		[Description("Interventional Pain Management")]
		InterventionalPainManagement = 9,


		/// <summary>
		/// Gastroenterology
		/// </summary>
		[Description("Gastroenterology")]
		Gastroenterology = 10,


		/// <summary>
		/// Internal Medicine
		/// </summary>
		[Description("Internal Medicine")]
		InternalMedicine = 11,


		/// <summary>
		/// Osteopathic Manipulative Therapy
		/// </summary>
		[Description("Osteopathic Manipulative Therapy")]
		OsteopathicManipulativeTherapy = 12,


		/// <summary>
		/// Neurology
		/// </summary>
		[Description("Neurology")]
		Neurology = 13,


		/// <summary>
		/// Neurosurgery
		/// </summary>
		[Description("Neurosurgery")]
		Neurosurgery = 14,


		/// <summary>
		///  Obstetrics / Gynecology
		/// </summary>
		[Description(" Obstetrics / Gynecology")]
		ObstetricsGynecology = 16,


		/// <summary>
		///  Ophthalmology
		/// </summary>
		[Description("Ophthalmology")]
		Ophthalmology = 18,


		/// <summary>
		/// OralSurgery
		/// </summary>
		[Description("OralSurgery")]
		OralSurgery = 19,


		/// <summary>
		/// Orthopedic Surgery
		/// </summary>
		[Description("Orthopedic Surgery")]
		OrthopedicSurgery = 20,


		/// <summary>
		///  Pathology
		/// </summary>
		[Description("Pathology")]
		Pathology = 22,


		/// <summary>
		/// Plastic and Reconstructive Surgery
		/// </summary>
		[Description("Plastic and Reconstructive Surgery")]
		PlasticSurgery = 24,


		/// <summary>
		/// Physical Medicine and Rehabilitation
		/// </summary>
		[Description("Physical Medicine and Rehabilitation")]
		PhysicalMedicineRehabilitation = 25,


		/// <summary>
		/// Psychiatry
		/// </summary>
		[Description("Psychiatry")]
		Psychiatry = 26,


		/// <summary>
		/// Colorectal Surgery (formerly Proctology)
		/// </summary>
		[Description("Colorectal Surgery (formerly Proctology)")]
		ColorectalSurgery = 28,


		/// <summary>
		/// Pulmonary Disease
		/// </summary>
		[Description("Pulmonary Disease")]
		PulmonaryDisease = 29,


		/// <summary>
		/// Diagnostic Radiology
		/// </summary>
		[Description("Diagnostic Radiology")]
		DiagnosticRadiology = 30,


		/// <summary>
		/// Anesthesiologist Assistant
		/// </summary>
		[Description("Anesthesiologist Assistant")]
		AnesthesiologistAssistant = 32,


		/// <summary>
		/// Thoracic Surgery
		/// </summary>
		[Description("Thoracic Surgery")]
		ThoracicSurgery = 33,


		/// <summary>
		/// Urology
		/// </summary>
		[Description("Urology")]
		Urology = 34,


		/// <summary>
		/// Chiropractic
		/// </summary>
		[Description("Chiropractic")]
		Chiropractic = 35,


		/// <summary>
		/// Nuclear Medicine
		/// </summary>
		[Description("Nuclear Medicine")]
		NuclearMedicine = 36,


		/// <summary>
		/// Pediatric Medicine
		/// </summary>
		[Description("Pediatric Medicine")]
		PediatricMedicine = 37,


		/// <summary>
		/// Geriatric Medicine
		/// </summary>
		[Description("Geriatric Medicine")]
		GeriatricMedicine = 38,


		/// <summary>
		/// Nephrology
		/// </summary>
		[Description("Nephrology")]
		Nephrology = 39,


		/// <summary>
		/// Hand Surgery
		/// </summary>
		[Description("Hand Surgery")]
		HandSurgery = 40,


		/// <summary>
		/// Optometry
		/// </summary>
		[Description("Optometry")]
		Optometry = 41,


		/// <summary>
		/// Certified Nurse Midwife
		/// </summary>
		[Description("Certified Nurse Midwife")]
		CertifiedNurseMidwife = 42,


		/// <summary>
		/// Certified Registered Nurse Assistant (CRNA)
		/// </summary>
		[Description("Certified Registered Nurse Assistant (CRNA)")]
		CertifiedRegisteredNurseAssistant = 43,


		/// <summary>
		/// Infectious Disease
		/// </summary>
		[Description("Infectious Disease")]
		InfectiousDisease = 44,


		/// <summary>
		/// Mammography Screening Center
		/// </summary>
		[Description("Mammography Screening Center")]
		MammographyScreeningCenter = 45,


		/// <summary>
		/// Endocrinology
		/// </summary>
		[Description("Endocrinology")]
		Endocrinology = 46,


		/// <summary>
		/// Independent Diagnostic Testing Facility
		/// </summary>
		[Description("Independent Diagnostic Testing Facility")]
		IndependentDiagnosticTestingFacility = 47,

		/// <summary>
		/// Podiatry
		/// </summary>
		[Description("Podiatry")]
		Podiatry = 48,


		/// <summary>
		/// Ambulatory Surgical Center
		/// </summary>
		[Description("Ambulatory Surgical Center")]
		AmbulatorySurgicalCenter = 49,


		/// <summary>
		/// Nurse Practitioner
		/// </summary>
		[Description("Nurse Practitioner")]
		NursePractitioner = 50,

		/// <summary>
		/// Medical supply company specializing with orthotist.
		/// </summary>
		[Description("Medical Supply Company with Orthotist")]
		MedicalSupplyCompanyWithOrthotist = 51,

		/// <summary>
		/// Medical supply company specializing with Prosthetist.
		/// </summary>
		[Description("Medical Supply Company with Prosthetist")]
		MedicalSupplyCompanyWithProsthetist = 52,

		/// <summary>
		/// Medical supply company with orthotics-prosthetics.
		/// </summary>
		[Description("Medical Supply Company with Orthotist-Prosthetist")]
		MedicalSupplyCompanyWithOrthotistProsthetist = 53,

		/// <summary>
		/// Other Medical Supply Company
		/// </summary>
		[Description("Other Medical Supply Company")]
		OtherMedicalSupplyCompany = 54,

		/// <summary>
		/// Individual certified Orthotist.
		/// </summary>
		[Description("Individual Certified Orthotist")]
		IndividualCertifiedOrthotist = 55,

		/// <summary>
		/// Individual certified Prosthetist.
		/// </summary>
		[Description("Individual Certified Prosthetist")]
		IndividualCertifiedProsthetist = 56,

		/// <summary>
		/// Individual certified Prosthetist-Orthotist.
		/// </summary>
		[Description("Individual Certified Prosthetist-Orthotist")]
		IndividualCertifiedProsthetistOrthotist = 57,

		/// <summary>
		/// Medical Supply Company with Pharmacist
		/// </summary>
		[Description("Medical Supply Company with Pharmacist")]
		MedicalSupplyCompanyWithPharmacist = 58,

		/// <summary>
		/// Ambulance Service Provider
		/// </summary>
		[Description("Ambulance Service Provider")]
		AmbulanceServiceProvider = 59,

		/// <summary>
		/// Public health or welfare agency.
		/// </summary>
		[Description("Public Health or Welfare Agency")]
		PublicHealthOrWelfareAgency = 60,

		/// <summary>
		/// Voluntary health or charitable agency.
		/// </summary>
		[Description("Voluntary Health or Charitable Agency")]
		VoluntaryHealthOrCharitableAgency = 61,

		/// <summary>
		/// Psychologist
		/// </summary>
		[Description("Psychologist")]
		Psychologist = 62,

		/// <summary>
		/// Portable X-Ray Supplier
		/// </summary>
		[Description("Portable X-Ray Supplier")]
		PortableXRaySupplier = 63,

		/// <summary>
		/// Audiologist
		/// </summary>
		[Description("Audiologist")]
		Audiologist = 64,

		/// <summary>
		/// Physical Therapist
		/// </summary>
		[Description("Physical Therapist")]
		PhysicalTherapist = 65,

		/// <summary>
		/// Rheumatology
		/// </summary>
		[Description("Rheumatology")]
		Rheumatology = 66,

		/// <summary>
		/// Occupational Therapist 
		/// </summary>
		[Description("Occupational Therapist ")]
		OccupationalTherapist = 67,

		/// <summary>
		/// Clinical Psychologist 
		/// </summary>
		[Description("Clinical Psychologist")]
		ClinicalPsychologist = 68,

		/// <summary>
		/// Clinical Laboratory
		/// </summary>
		[Description("Clinical Laboratory")]
		ClinicalLaboratory = 69,


		/// <summary>
		///  Multi-specialty Clinic or Group Practice
		/// </summary>
		[Description("Multi-specialty Clinic or Group Practice")]
		MultispecialtyClinicOrGroupPractice = 70,


		/// <summary>
		/// Registered Dietitian/Nutrition Professional
		/// </summary>
		[Description("Registered Dietitian/Nutrition Professional ")]
		RegisteredDietitianNutritionProfessional = 71,

		/// <summary>
		///  Pain Management
		/// </summary>
		[Description("Pain Management")]
		PainManagement = 72,

		/// <summary>
		///  Mass Immunization Roster Billers
		/// </summary>
		[Description("Mass Immunization Roster Billers")]
		MassImmunizationRosterBillers = 73,

		/// <summary>
		/// Radiation Therapy Center
		/// </summary>
		[Description("Radiation Therapy Center")]
		RadiationTherapyCenter = 74,

		/// <summary>
		/// Slide Preparation Facilities
		/// </summary>
		[Description("Slide Preparation Facilities")]
		SlidePreparationFacilities = 75,


		/// <summary>
		///  Peripheral Vascular Disease
		/// </summary>
		[Description("Peripheral Vascular Disease")]
		PeripheralVascularDisease = 76,

		/// <summary>
		/// Vascular Surgery
		/// </summary>
		[Description("Vascular Surgery")]
		VascularSurgery = 77,

		/// <summary>
		///  Cardiac Surgery
		/// </summary>
		[Description("Cardiac Surgery")]
		CardiacSurgery = 78,


		/// <summary>
		///  Addiction Medicine
		/// </summary>
		[Description("Addiction Medicine")]
		AddictionMedicine = 79,


		/// <summary>
		/// Licensed Clinical Social Worker
		/// </summary>
		[Description("Licensed Clinical Social Worker")]
		LicensedClinicalSocialWorker = 80,


		/// <summary>
		///  Critical Care (Intensivists)
		/// </summary>
		[Description("Critical Care (Intensivists)")]
		CriticalCare = 81,


		/// <summary>
		/// Hematology
		/// </summary>
		[Description("Hematology")]
		Hematology = 82,


		/// <summary>
		/// Hematology/Oncology
		/// </summary>
		[Description("Hematology/Oncology")]
		HematologyOncology = 83,


		/// <summary>
		/// Preventative Medicine
		/// </summary>
		[Description("Preventative Medicine")]
		PreventativeMedicine = 84,


		/// <summary>
		/// Maxillofacial Surgery
		/// </summary>
		[Description("Maxillofacial Surgery")]
		MaxillofacialSurgery = 85,


		/// <summary>
		/// Neuro Psychiatry
		/// </summary>
		[Description("Neuro Psychiatry")]
		NeuroPsychiatry = 86,

		/// <summary>
		/// All Other Suppliers
		/// </summary>
		[Description("All Other Suppliers")]
		AllOtherSuppliers = 87,

		/// <summary>
		/// Unknown Supplier/Provider Specialty
		/// </summary>
		[Description("Unknown Supplier/Provider Specialty")]
		UnknownSupplierProviderSpecialty = 88,


		/// <summary>
		/// Certified Clinical Nurse Specialist
		/// </summary>
		[Description("Certified Clinical Nurse Specialist")]
		CertifiedClinicalNurseSpecialist = 89,

		/// <summary>
		/// Medical Oncology
		/// </summary>
		[Description("Medical Oncology")]
		MedicalOncology = 90,


		/// <summary>
		/// Surgical Oncology
		/// </summary>
		[Description("Surgical Oncology")]
		SurgicalOncology = 91,


		/// <summary>
		/// Radiation Oncology
		/// </summary>
		[Description("Radiation Oncology")]
		RadiationOncology = 92,


		/// <summary>
		/// Emergency Medicine
		/// </summary>
		[Description("Emergency Medicine")]
		EmergencyMedicine = 93,


		/// <summary>
		/// Interventional Radiology
		/// </summary>
		[Description("Interventional Radiology")]
		InterventionalRadiology = 94,

		/// <summary>
		/// Optician 
		/// </summary>
		[Description("Optician")]
		Optician = 96,

		/// <summary>
		/// Physician Assistant 
		/// </summary>
		[Description("Physician Assistant")]
		PhysicianAssistant = 97,

		/// <summary>
		/// Gynecological/Oncology
		/// </summary>
		[Description("Gynecological/Oncology")]
		GynecologicalOncology = 98,

		/// <summary>
		/// Unknown Physician Specialty
		/// </summary>
		[Description("Unknown Physician Specialty")]
		UnknownPhysicianSpecialty = 99,

		[ServiceType(name: "Hospital", code: "A0", startDate: "", description: "Hospital")]
		Hospital = 100,

		[ServiceType(name: "Skilled Nursing Facility", code: "A1", startDate: "", description: "Skilled Nursing Facility")]
		SkilledNursingFacility = 101,
		
		[ServiceType(name: "Intermediate Care Nursing Facility", code: "A2", startDate: "", description: "Intermediate Care Nursing Facility")]
		IntermediateCareNursingFacility = 102,
		
		[ServiceType(name: "Other Nursing Facility", code: "A3", startDate: "", description: "Other Nursing Facility")]
		OtherNursingFacility = 103,
		
		[ServiceType(name: "Home Health Agency", code: "A4", startDate: "", description: "Home Health Agency")]
		HomeHealthAgency = 104,
		
		[ServiceType(name: "Pharmacy", code: "A5", startDate: "", description: "Pharmacy")]
		Pharmacy = 105,
		
		[ServiceType(name: "Medical Supply Company with Respiratory Therapist", code: "A6", startDate: "", description: "Medical Supply Company with Respiratory Therapist")]
		MedicalSupplyCompanyWithRespiratoryTherapist = 106,
		
		[ServiceType(name: "Department Store", code: "A7", startDate: "", description: "Department Store")]
		DepartmentStore = 107,
		
		[ServiceType(name: "Grocery Store", code: "A8", startDate: "", description: "Grocery Store ")]
		GroceryStore = 108,


	}
}
