
DECLARE @DatabaseName sysname = 'Authorizations_change_db_name', @SQL NVARCHAR(MAX);

SET @SQL = N'USE ' + QUOTENAME(@DatabaseName);

EXECUTE(@SQL);
GO

------------- AuthorizationStatuses -------------
EXECUTE(@SQL);
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (0
           ,'Unknown'
           ,'Unknown')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (1
           ,'ClientRequestAdded'
           ,'Auth requested')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (2
           ,'InformationNeeded'
           ,'Questionnaire not complete or other info required')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (3
           ,'NurseReview'
           ,'Nurse to review questionnaire for completeness')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (4
           ,'RFR'
           ,'Request ready for robot')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (5
           ,'AuthApproved'
           ,'Auth approved on insurance website')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (6
           ,'AuthPended'
           ,'Auth pending by insurance company')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (7
           ,'AuthDischarged'
           ,'Auth has been discharged in Insurance website')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (8
           ,'AuthDenied'
           ,'Auth denied by insurance company')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (9
           ,'AuthExpiring'
           ,'Auth end date in the next 30 days or less')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (10
           ,'AuthConcurrent'
           ,'New auth request for this service type')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (11
           ,'AuthExpired'
           ,'Auth end date is past the current date')
GO

INSERT INTO [dbo].[AuthorizationStatuses]
           ([AuthorizationStatusId]
           ,[Name]
           ,[Description])
     VALUES
           (12
           ,'AuthInProcess'
           ,'In-Process')
GO

-- SEED ADMINISTRATIVE GENDER ENUM DATA

INSERT INTO [dbo].[AdministrativeGenders]
           ([AdministrativeGenderId]
           ,[Name])
     VALUES
           (
		   0
           ,'Unknown'
		   )
GO

INSERT INTO [dbo].[AdministrativeGenders]
           ([AdministrativeGenderId]
           ,[Name])
     VALUES
           (
		   1
           ,'Male'
		   )
GO

INSERT INTO [dbo].[AdministrativeGenders]
           ([AdministrativeGenderId]
           ,[Name])
     VALUES
           (
		   2
           ,'Female'
		   )
GO

INSERT INTO [dbo].[AdministrativeGenders]
           ([AdministrativeGenderId]
           ,[Name])
     VALUES
           (
		   3
           ,'Undifferentiated'
		   )
GO

EXECUTE(@SQL);
GO

UPDATE [dbo].[Patients] SET AdministrativeGender = 0 Where AdministrativeGenderId is null
GO

--- States - United States of America 

EXECUTE(@SQL);
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (1
           ,'Unknown'
           ,'UNK')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (2
           ,'Alabama'
           ,'AL')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (3
           ,'Alaska'
           ,'AK')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (4
           ,'Arizona'
           ,'AZ')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (5
           ,'Arkansas'
           ,'AR')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (6
           ,'California'
           ,'CA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (7
           ,'Colorado'
           ,'CO')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (8
           ,'Connecticut'
           ,'CT')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (9
           ,'Delaware'
           ,'DE')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (10
           ,'Florida'
           ,'FL')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (11
           ,'Georgia'
           ,'GA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (12
           ,'Hawaii'
           ,'HI')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (13
           ,'Iowa'
           ,'IA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (14
           ,'Idaho'
           ,'ID')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (15
           ,'Illinois'
           ,'IL')
GO


INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (16
           ,'Indiana'
           ,'IN')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (17
           ,'Kansas'
           ,'KS')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (18
           ,'Kentucky'
           ,'KY')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (19
           ,'Louisiana'
           ,'LA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (20
           ,'Massachusetts'
           ,'MA')
GO


INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (21
           ,'Maryland'
           ,'MD')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (22
           ,'Maine'
           ,'ME')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (23
           ,'Michigan'
           ,'MI')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (24
           ,'Minnesota'
           ,'MN')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (25
           ,'Missouri'
           ,'MO')
GO


INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (26
           ,'Mississippi'
           ,'MS')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (27
           ,'Montana'
           ,'MT')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (28
           ,'North Carolina'
           ,'NC')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (29
           ,'North Dakota'
           ,'ND')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (30
           ,'Nebraska'
           ,'NE')
GO


INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (31
           ,'New Hampshire'
           ,'NH')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (32
           ,'New Jersey'
           ,'NJ')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (33
           ,'New Mexico'
           ,'NM')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (34
           ,'Nevada'
           ,'NV')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (35
           ,'New York'
           ,'NY')
GO


INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (36
           ,'Oklahoma'
           ,'OK')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (37
           ,'Ohio'
           ,'OH')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (38
           ,'Oregon'
           ,'OR')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (39
           ,'Pennsylvania'
           ,'PA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (40
           ,'Rhode Island'
           ,'RI')
GO


INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (41
           ,'South Carolina'
           ,'SC')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (42
           ,'South Dakota'
           ,'SD')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (43
           ,'Tennessee'
           ,'TE')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (44
           ,'Texas'
           ,'TX')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (45
           ,'Utah'
           ,'UT')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (46
           ,'Virginia'
           ,'VA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (47
           ,'Vermont'
           ,'VT')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (48
           ,'Washington'
           ,'WA')
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (49
           ,'Wisconsin'
           ,'WI')
		   

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (50
           ,'West Virginia'
           ,'WV')

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (51
           ,'Wyoming'
           ,'WY')		   		   		   
GO

INSERT INTO [dbo].[States]
           ([StateId]
           ,[Name]
           ,[Code])
     VALUES
           (52
           ,'D.C.'
           ,'DC')
GO

--- TypesOfService -  

EXECUTE(@SQL);
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (1
 ,'Whole Blood'
 ,'WholeBlood'
 ,'0')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (2
 ,'Medical Care'
 ,'MedicalCare'
 ,'1')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (3
 ,'Surgery'
 ,'Surgery'
 ,'2')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (4
 ,'Consultation'
 ,'Consultation'
 ,'3')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (5
 ,'Diagnostic Radiology'
 ,'DiagnosticRadiology'
 ,'4')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (6
 ,'Diagnostic Laboratory'
 ,'DiagnosticLaboratory'
 ,'5')
GO 
INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (7
 ,'Therapeutic Radiology'
 ,'TherapeuticRadiology'
 ,'6')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (8
 ,'Anesthesia'
 ,'Anesthesia'
 ,'7')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (9
 ,'Assistant at Surgery'
 ,'AssistantatSurgery'
 ,'8')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (10
 ,'Other Medical Items or Services'
 ,'OtherMedicalItemsOrServices'
 ,'9')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (11
 ,'Used DME'
 ,'UsedDme'
 ,'A')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (12
 ,'High Risk Screening Mammography'
 ,'HighRiskScreeningMammography'
 ,'B')
GO 
INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (13
 ,'Low Risk Screening Mammography'
 ,'LowRiskScreeningMammography'
 ,'C')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (14
 ,'Ambulance'
 ,'Ambulance'
 ,'D')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (15
 ,'Enteral/Parenteral Nutrients/Supplies'
 ,'EnteralParenteralNutrientsSupplies'
 ,'E')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (16
 ,'Ambulatory Surgical Center (Facility
Usage for Surgical Services)'
 ,'AmbulatorySurgicalCenter'
 ,'F')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (17
 ,'Immunosuppressive Drugs'
 ,'ImmunosuppressiveDrugs'
 ,'G')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (18
 ,'Hospice'
 ,'Hospice'
 ,'H')
GO 
INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (19
 ,'Diabetic Shoes'
 ,'DiabeticShoes'
 ,'J')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (20
 ,'Hearing Items and Services'
 ,'HearingItemsAndServices'
 ,'K')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (21
 ,'ESRD Supplies'
 ,'EsrdSupplies'
 ,'L')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (22
 ,'Monthly Capitation Payment for Dialysis'
 ,'MonthlyCapitationPaymentForDialysis'
 ,'M')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (23
 ,'Kidney Donor'
 ,'KidneyDonor'
 ,'N')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (24
 ,'Lump Sum Purchase of DME, Prosthetics,
Orthotics'
 ,'LumpSumPurchaseOfDmeProstheticsOrthotics'
 ,'P')
GO 
INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (25
 ,'Vision Items or Services'
 ,'VisionItemsOrServices'
 ,'Q')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (26
 ,'Rental of DME'
 ,'RentalofDme'
 ,'R')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (27
 ,'Surgical Dressings or Other Medical
Supplies'
 ,'SurgicalDressingsOrOtherMedicalSupplies'
 ,'S')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (28
 ,'Outpatient Mental Health Treatment
Limitation'

,'OutpatientMentalHealthTreatmentLimitation'
 ,'T')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (29
 ,'Occupational Therapy'
 ,'OccupationalTherapy'
 ,'U')
GO

INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (30
 ,'Pneumococcal/Flu Vaccine'
 ,'PneumococcalFluVaccine'
 ,'V')
GO 
INSERT INTO [dbo].[TypesOfService]
 ([TypeOfServiceId]
 ,[Name]
 ,[LookupName]
 ,[Code])
 VALUES
 (31
 ,'Physical Therapy '
 ,'PhysicalTherapy'
 ,'W')
GO


-- Seed AddressTypes
INSERT INTO [dbo].[AddressTypes]
           ([AddressTypeId]
           ,[Name])
     VALUES
           (0
           ,'Residential')
GO

INSERT INTO [dbo].[AddressTypes]
           ([AddressTypeId]
           ,[Name])
     VALUES
           (1
           ,'Commercial')
GO

--PlaceOfServiceCodeEnum

EXECUTE(@SQL);
GO

INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (1
           ,'Pharmacy'
           ,'Pharmacy')
GO

INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (2
           ,'Tele-Health'
           ,'TeleHealth')
GO

INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (3
           ,'School'
           ,'School')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (4
           ,'Homeless Shelter'
           ,'HomelessShelter')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (5
           ,'IHS Free Standing Facility'
           ,'IhsFreeStandingFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (6
           ,'IHS Provider Based Facility'
           ,'IhsProviderBasedFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (7
           ,'Tribal 638 Free Standing Facility'
           ,'Tribal638FreeStandingFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (8
           ,'Tribal 638 Provider Based Facility'
           ,'Tribal638ProviderBasedFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (9
           ,'Correctional Facility'
           ,'CorrectionalFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (11
           ,'Office'
           ,'Office')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (12
           ,'Home'
           ,'Home')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (13
           ,'Assisted Living Facility'
           ,'AssistedLivingFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (14
           ,'NAME'
           ,'LUNAME')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (15
           ,'Mobile Unit'
           ,'MobileUnit')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (16
           ,'Temporary Lodging'
           ,'TemporaryLodging')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (17
           ,'Walk In Retail Health Clinic'
           ,'WalkInRetailHealthClinic')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (18
           ,'Employment Work Site'
           ,'EmploymentWorkSite')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (19
           ,'Off Campus Outpatient Hospital'
           ,'OffCampusOutpatientHospital')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (20
           ,'Urgent Care Facility'
           ,'UrgentCareFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (21
           ,'Inpatient Hospital'
           ,'InpatientHospital')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (22
           ,'On Campus Outpatient Hospital'
           ,'OnCampusOutpatientHospital')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (23
           ,'Emergency Room Hospital'
           ,'EmergencyRoomHospital')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (24
           ,'Ambulatory Surgical Center'
           ,'AmbulatorySurgicalCenter')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (25
           ,'Birthing Center'
           ,'BirthingCenter')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (26
           ,'Military Treatment Facility'
           ,'MilitaryTreatmentFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (31
           ,'Skilled Nursing Facility'
           ,'SkilledNursingFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (32
           ,'Nursing Facility'
           ,'NursingFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (33
           ,'Custodial Care Facility'
           ,'CustodialCareFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (34
           ,'Hospice'
           ,'Hospice')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (41
           ,'Ambulance Land'
           ,'AmbulanceLand')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (42
           ,'Ambulance Air Or Water'
           ,'AmbulanceAirOrWater')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (49
           ,'Independent Clinic'
           ,'IndependentClinic')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (50
           ,'Federally Qualified Health Center'
           ,'FederallyQualifiedHealthCenter')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (51
           ,'Inpatient Psychiatric Facility'
           ,'InpatientPsychiatricFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (52
           ,'Psychiatric Facility Partial Hospitalization'
           ,'PsychFacilityPartialHospitalization')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (53
           ,'Community Mental Health Center'
           ,'CommunityMentalHealthCenter')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (54
           ,'Intermediate Care Intellectual Disabilities'
           ,'IntermediateCareIntellectualDisabilities')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (55
           ,'ResidentiaSubstancAbuse'
           ,'ResidentialSubstanceAbuse')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (56
           ,'Psychiatric Residential Treatment'
           ,'PsychiatricResidentialTreatment')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (57
           ,'Non Residential SubstanceAbuse'
           ,'NonResidentialSubstanceAbuse')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (60
           ,'Mass Immunization Center'
           ,'MassImmunizationCenter')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (61
           ,'Comprehensive Inpatient Rehabilitation'
           ,'ComprehensiveInpatientRehabilitation')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (62
           ,'Comprehensive Outpatient Rehabilitation'
           ,'ComprehensiveOutpatientRehabilitation')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (65
           ,'EndStageRenalDiseaseFacility'
           ,'EndStageRenalDiseaseFacility')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (71
           ,'Public Health Clinic'
           ,'PublicHealthClinic')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (72
           ,'Rural Health Clinic'
           ,'RuralHealthClinic')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (81
           ,'Independent Laboratory'
           ,'IndependentLaboratory')
GO


INSERT INTO [dbo].[PlaceOfServiceCodes]
           ([PlaceOfServiceCodeId]
           ,[Name]
           ,[LookupName])
     VALUES
           (99
           ,'Other Place Of Service'
           ,'OtherPlaceOfService')
GO


------------- ClaimStatuses -------------

UPDATE [IntegratedServices].[ClaimStatuses]
   SET [Code] = 'None'
      ,[Description]= 'None'
 WHERE Code = 'Unknown'

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (0
           ,'None'
           ,'None')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (1
           ,'Completed'
           ,'Completed')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (2
           ,'Rejected'
           ,'Rejected')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (3
           ,'Voided'
           ,'Voided')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (4
           ,'InProcess'
           ,'In-Process')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (5
           ,'Received'
           ,'Received')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (6
           ,'NotAdjudicated'
           ,'Not-Adjudicated')
GO

INSERT INTO [integratedServices].[ClaimStatuses]
           ([ClaimStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (7
           ,'Acknowledged'
           ,'Acknowledged')
GO
------------- ClaimLineItemStatuses -------------

EXECUTE(@SQL);
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (0
           ,'Unknown'
           ,'Unknown')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (1
           ,'Paid'
           ,'Paid')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (2
           ,'Approved'
           ,'Approved')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (3
           ,'Rejected'
           ,'Rejected')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (4
           ,'Voided'
           ,'Voided')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (5
           ,'Received'
           ,'Received')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (6
           ,'NotAdjudicated'
           ,'Not-Adjudicated')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (7
           ,'Denied'
           ,'Denied')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (8
           ,'Pended'
           ,'Pended')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (9
           ,'UnMatchedProcedureCode'
           ,'UnMatched-ProcedureCode')
GO


INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (10
           ,'Error'
           ,'Error / Exception')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (11
           ,'Unavailable'
           ,'Unavailable')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (12
           ,'MemberNotFound'
           ,'Member Not Found')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (13
           ,'Ignored'
           ,'Ignored')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (14
           ,'ZeroPay'
           ,'Zero Pay')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (15
           ,'BundledFqhc'
           ,'Bundled Fqhc')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (16
           ,'NeedsReview'
           ,'Needs Review')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (17
           ,'TransientError'
           ,'Transient Error')
GO


INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (18
           ,'CallPayer'
           ,'Call Payer')
GO

INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (19
           ,'Returned'
           ,'Returned')
GO
INSERT INTO [integratedServices].[ClaimLineItemStatuses]
           ([ClaimLineItemStatusId]
           ,[Code]
           ,[Description])
     VALUES
           (20
           ,'Writeoff'
           ,'Write-off')
GO

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 1
	,[MaximumPipelineDays]= 10
	,[MinimumResolutionAttempts]= 3
	,[MaximumResolutionAttempts]= 10
	,[Rank] = 1
WHERE [Code]  = 'Unknown'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 19
WHERE [Code]  = 'Paid'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 1
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 4
	,[MaximumResolutionAttempts]= 14
	,[Rank] = 16
WHERE [Code]  = 'Approved'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 8
WHERE [Code]  = 'Rejected'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 9
WHERE [Code]  = 'Voided'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 2
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 4
	,[MaximumResolutionAttempts]= 14
	,[Rank] = 14
WHERE [Code]  = 'Received'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 2
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 4
	,[MaximumResolutionAttempts]= 14
	,[Rank] = 10
WHERE [Code]  = 'NotAdjudicated'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 10
	,[MaximumPipelineDays]= 60
	,[MinimumResolutionAttempts]= 3
	,[MaximumResolutionAttempts]= 6
	,[Rank] = 15
WHERE [Code]  = 'Denied'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 2
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 4
	,[MaximumResolutionAttempts]= 10
	,[Rank] = 13
WHERE [Code]  = 'Pended'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 3
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 2
	,[MaximumResolutionAttempts]= 2
	,[Rank] = 7
WHERE [Code]  = 'UnMatchedProcedureCode'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 10
	,[MinimumResolutionAttempts]= 4
	,[MaximumResolutionAttempts]= 10
	,[Rank] = 3
WHERE [Code]  = 'Error'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 2
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 4
	,[MaximumResolutionAttempts]= 10
	,[Rank] = 6
WHERE [Code]  = 'Unavailable'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET 	[DaysWaitBetweenAttempts]= 2
	,[MaximumPipelineDays]= 14
	,[MinimumResolutionAttempts]= 2
	,[MaximumResolutionAttempts]= 2
	,[Rank] = 5
WHERE [Code]  = 'MemberNotFound'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 4
WHERE [Code]  = 'Ignored'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 18
WHERE [Code]  = 'ZeroPay'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 17
WHERE [Code]  = 'BundledFqhc'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 11
WHERE [Code]  = 'NeedsReview'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 99
	,[MinimumResolutionAttempts]= 10
	,[MaximumResolutionAttempts]= 99
	,[Rank] = 2
WHERE [Code]  = 'TransientError'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 12
WHERE [Code]  = 'CallPayer'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 19
WHERE [Code]  = 'Returned'

UPDATE [IntegratedServices].[ClaimLineItemStatuses] 
SET
	[DaysWaitBetweenAttempts]= 0
	,[MaximumPipelineDays]= 0
	,[MinimumResolutionAttempts]= 0
	,[MaximumResolutionAttempts]= 0
	,[Rank] = 20
WHERE [Code]  = 'Writeoff'


------------- ServiceTypes -------------

--EXECUTE(@SQL);
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (0
--           ,'Unknown'
--           ,'Unknown')
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (1
--           ,'General'
--           ,'General')
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (2
--           ,'IOP'
--           ,'Intensive Outpatient')
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (3
--           ,'SUD'
--           ,'Substance Use Disorder')
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (4
--           ,'OMHC'
--           ,'Outpatient Mental Health Clinic')
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (5
--           ,'PRP'
--           ,'Psychiatric Rehabilitation Program')
--GO

--INSERT INTO [integratedServices].[ServiceTypes]
--           ([ServiceTypeId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (6
--           ,'OP'
--           ,'Outpatient')
--GO

------------- TransactionTypes -------------

EXECUTE(@SQL);
GO

INSERT INTO [integratedServices].[TransactionTypes]
           ([TransactionTypeId]
           ,[Code]
           ,[Description])
     VALUES
           (0
           ,'Unknown'
           ,'Unknown')
GO

INSERT INTO [integratedServices].[TransactionTypes]
           ([TransactionTypeId]
           ,[Code]
           ,[Description])
     VALUES
           (1
           ,'ClaimStatus'
           ,'Claim Status')
GO

EXECUTE(@SQL);
GO

INSERT INTO [integratedServices].[TransactionTypes]
           ([TransactionTypeId]
           ,[Code]
           ,[Description])
     VALUES
           (2
           ,'ChargeEntry'
           ,'Charge Entry')
GO

  ------------- DbOperation -------------

EXECUTE(@SQL);
GO

INSERT INTO [integratedServices].[DbOperations]
           ([DbOperationId]
           ,[Name])
     VALUES
           (1
           ,'Insert')
GO

INSERT INTO [integratedServices].[DbOperations]
           ([DbOperationId]
           ,[Name])
     VALUES
           (2
           ,'Update')
GO

  ------------- InputDocumentTypes -------------

INSERT INTO [integratedServices].[InputDocumentTypes]
           ([InputDocumentTypeId]
           ,[Code]
           ,[Description])
     VALUES
           (1
           ,'ClaimStatusInput'
           ,'Claim Status Input File')



------------- ApplicationFeatures -------------

EXECUTE(@SQL);
GO

INSERT INTO [dbo].[ApplicationFeatures]
           ([ApplicationFeatureId]
           ,[Name])
     VALUES
           (1
           ,'Authorizations')
GO

INSERT INTO [dbo].[ApplicationFeatures]
           ([ApplicationFeatureId]
           ,[Name])
     VALUES
           (2
           ,'ClaimStatus')
GO

INSERT INTO [dbo].[ApplicationFeatures]
           ([ApplicationFeatureId]
           ,[Name])
     VALUES
           (3
           ,'ChargeEntry')
GO

---- RPATypes -----

EXECUTE(@SQL);
GO

INSERT INTO [integratedServices].[RpaTypes]
           ([RpaTypeId]
           ,[Code]
           ,[Description]
           ,[ReleaseKey])
     VALUES
           (1
           ,'IcaNotes'
           ,'ICANotes'
           ,'ea3eb734-734a-4d29-88dc-36c7de4f203c')
GO


INSERT INTO [integratedServices].[RpaTypes]
           ([RpaTypeId]
           ,[Code]
           ,[Description]
           ,[ReleaseKey])
     VALUES
           (2
           ,'ECW'
           ,'ECW'
           ,'ea3eb734-734a-4d29-88dc-36c7de4f203c')
GO

---- ClaimStatusExceptionReasonCategory ----
EXECUTE(@SQL);
GO

INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (0
           ,'Other'
           ,'Other')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (1
           ,'COB'
           ,'COB')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (2
           ,'CodingIssue'
           ,'Coding Issue')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (3
           ,'Coverage'
           ,'Coverage')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (4
           ,'Credentialing'
           ,'Credentialing')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (5
           ,'InsuranceTermed'
           ,'Insurance Term')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (6
           ,'InvalidInsurance'
           ,'Invalid Insurance')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (7
           ,'MRNeeded'
           ,'MR Needed')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (8
           ,'NoClaimMissingProcedure'           
           ,'No Claim/CPT')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (9
           ,'PolicyNumber'
           ,'Policy Number')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (10
           ,'ProviderType'
           ,'Provider Type')
GO

--Available To Re-Use
--INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
--           ([ClaimStatusExceptionReasonCategoryId]
--           ,[Code]
--           ,[Description])
--     VALUES
--           (11
--           ,'RenderingProvider'
--           ,'Rendering Prov.')
--GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (12
           ,'Review'
           ,'Internal Review')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (13
           ,'WrongPayer'
           ,'Wrong Payer')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (14
           ,'AuthorizationDenial'
           ,'Authorization')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (15
           ,'Contractual'  
           ,'Contractual')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (16
           ,'Duplicate'
           ,'Duplicate')
GO

INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (17
           ,'SecondaryMissingEob'
           ,'Secondary Missing EOB')
GO

INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (18
           ,'MedicareAdvCoverage'
           ,'Medicare Advantage Coverage')
GO


INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (19
           ,'TimelyFiling'
           ,'Timely Filing')
GO

INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (20
           ,'MedicalNecessity'
           ,'Medical Necessity')
GO

INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (21
           ,'ClaimIssue'
           ,'Claim Issue')
GO

INSERT INTO [integratedServices].[ClaimStatusExceptionReasonCategories]
           ([ClaimStatusExceptionReasonCategoryId]
           ,[Code]
           ,[Description])
     VALUES
           (22
            ,'DemographicsIssue'
           ,'Demographics Issue')
GO
------------- Application Reports -------------

INSERT INTO [dbo].[ApplicationReports]
           ([ApplicationReportId]
           ,[ApplicationFeatureId]
           ,[ReportName])
     VALUES
           (1
           ,2
           ,'Daily Claim Status Report')

GO

------------- Client User Application Reports -------------

INSERT INTO [dbo].[ClientUserApplicationReports]
           ([UserClientId]
           ,[ApplicationReportId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[LastModifiedBy]
           ,[LastModifiedOn])
     VALUES
           (1
           ,1
           ,'DATABASE SEED'
           ,GETDATE()
           ,NULL
           ,NULL)

----------- Specialties -----------------------------------

INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (1
 ,'GeneralPractice'
 ,'General Practice'
 ,'1')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (2
 ,'GeneralSurgery'
 ,'General Surgery'
 ,'2')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (3
 ,'AllergyImmunology'
 ,'Allergy / Immunology'
 ,'3')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (4
 ,'Otolaryngology'
 ,'Otolaryngology'
 ,'4')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (5
 ,'Anesthesiology'
 ,'Anesthesiology'
 ,'5')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (6
 ,'Cardiology'
 ,'Cardiology'
 ,'6')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (7
 ,'Dermatology'
 ,'Dermatology'
 ,'7')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (8
 ,'FamilyPractice'
 ,'Family Practice'
 ,'8')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (9
 ,'Gastroenterology'
 ,'Gastroenterology'
 ,'10')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (10
 ,'InternalMedicine'
 ,'Internal Medicine'
 ,'11')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (11
 ,'OsteopathicManipulativeTherapy'
 ,'Osteopathic Manipulative Therapy'
 ,'12')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (12
 ,'Neurology'
 ,'Neurology'
 ,'13')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (13
 ,'Neurosurgery'
 ,'Neurosurgery'
 ,'14')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (14
 ,'ObstetricsGynecology'
 ,'Obstetrics / Gynecology'
 ,'16')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (15
 ,'Ophthalmology'
 ,'Ophthalmology'
 ,'18')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (16
 ,'OralSurgery'
 ,'Oral Surgery (Dentist Only)'
 ,'19')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (17
 ,'OrthopedicSurgery'
 ,'Orthopedic Surgery'
 ,'20')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (18
 ,'Pathology'
 ,'Pathology'
 ,'22')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (19
 ,'PlasticandReconstructiveSurgery'
 ,'Plastic and Reconstructive Surgery'
 ,'24')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (20
 ,'PhysicalMedicineandRehabilitation'
 ,'Physical Medicine and Rehabilitation'
 ,'25')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (21
 ,'Psychiatry'
 ,'Psychiatry'
 ,'26')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (22
 ,'ColorectalSurgery'
 ,'Colorectal Surgery (formerly Proctology)'
 ,'28')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (23
 ,'PulmonaryDisease'
 ,'Pulmonary Disease'
 ,'29')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (24
 ,'DiagnosticRadiology'
 ,'Diagnostic Radiology'
 ,'30')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (25
 ,'ThoracicSurgery'
 ,'Thoracic Surgery'
 ,'33')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (26
 ,'Urology'
 ,'Urology'
 ,'34')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (27
 ,'Chiropractic'
 ,'Chiropractic'
 ,'35')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (28
 ,'NuclearMedicine'
 ,'Nuclear Medicine'
 ,'36')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (29
 ,'PediatricMedicine'
 ,'Pediatric Medicine'
 ,'37')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (30
 ,'GeriatricMedicine'
 ,'Geriatric Medicine'
 ,'38')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (31
 ,'Nephrology'
 ,'Nephrology'
 ,'39')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (32
 ,'HandSurgery'
 ,'Hand Surgery'
 ,'40')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (33
 ,'Optometry'
 ,'Optometry'
 ,'41')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (34
 ,'InfectiousDisease'
 ,'Infectious Disease'
 ,'44')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (35
 ,'Endocrinology'
 ,'Endocrinology'
 ,'46')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (36
 ,'Podiatry'
 ,'Podiatry'
 ,'48')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (37
 ,'Rheumatology'
 ,'Rheumatology'
 ,'66')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (38
 ,'MultispecialtyClinicorGroupPractice'
 ,'Multispecialty Clinic or Group Practice'
 ,'70')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (39
 ,'PeripheralVascularDisease'
 ,'Peripheral Vascular Disease'
 ,'76')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (40
 ,'CardiacSurgery'
 ,'Cardiac Surgery'
 ,'78')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (41
 ,'AddictionMedicine'
 ,'Addiction Medicine'
 ,'79')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (42
 ,'CriticalCare'
 ,'Critical Care (Intensivists)'
 ,'81')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (43
 ,'Hematology'
 ,'Hematology'
 ,'82')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (44
 ,'HematologyOncology'
 ,'Hematology /Oncology'
 ,'83')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (45
 ,'PreventativeMedicine'
 ,'Preventative Medicine'
 ,'84')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (46
 ,'MaxillofacialSurgery'
 ,'Maxillofacial Surgery'
 ,'85')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (47
 ,'Neuropsychiatry'
 ,'Neuropsychiatry'
 ,'86')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (48
 ,'MedicalOncology'
 ,'Medical Oncology'
 ,'90')
GO
INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (49
 ,'SurgicalOncology'
 ,'Surgical Oncology'
 ,'91')
GO

INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (50
 ,'RadiationOncology'
 ,'Radiation Oncology'
 ,'92')
GO

INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (51
 ,'EmergencyMedicine'
 ,'Emergency Medicine'
 ,'93')
GO

INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (52
 ,'InterventionalRadiology'
 ,'Interventional Radiology'
 ,'94')
GO

INSERT INTO [dbo].[Specialties]
 ([SpecialtyId]
 ,[Name]
 ,[Description]
 ,[Code])
 VALUES
 (53
 ,'VascularSurgery'
 ,'Vascular Surgery'
 ,'77')
GO

----  ApiIntegrations
INSERT INTO [dbo].[ApiIntegrations]
 ([Id]
 ,[Name]
 ,[Description]
 ,[Code]
 ,[CreatedBy]
 ,[CreatedOn])
 VALUES
 (1
 ,'Self Pay Eligibility'
 ,'AIT Self Pay Eligibility'
 ,'SelfPayEligibility'
 ,'Default'
 ,GETDATE())

-- ClientApiIntegrationKeys
INSERT INTO [dbo].[ClientApiIntegrationKeys]
           ([ClientId]
           ,[ApiIntegrationId]
           ,[ApiKey]
           ,[ApiSecret]
           ,[ApiUrl]
           ,[ApiVersion]
           ,[ApiUsername]
           ,[ApiPassword]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (1
           ,1
           ,'tE564fdMEu8hjXjZeLX4xM7mkUHhtWPlIk2XNWKdYsBTGXIKIHJAS4FvMzlPY1KGCQt7J5edfXlib3wkQvI3osULuj8T4rwFj0XZI5czRUrq6UFpxVCAl21fKXZM2B2F'
           ,''
           ,'https://sp-eligibility.azurewebsites.net'
           ,1
           ,''
           ,''           
           ,'Default'
           ,GETDATE())
GO

--- RelativeDateRanges
INSERT INTO [dbo].[RelativeDateRanges]
(
		 [Id]
		,[Code]
		,[Description]
		,[CreatedBy]
		,[CreatedOn]
		,[LastModifiedBy]
		,[LastModifiedOn]
)
VALUES
(
		 1
		,'FirstOfMonthToUtcNow'
		,'1st of month to DateTime.UtcNow'
		,'14bdf90a-4157-4991-b193-5f90945e8afb'
		,GETDATE()
		,null
		,null
),
(
        2
		,'FirstOfYearToUtcNow'
		,'1st of year to DateTime.UtcNow'
		,'14bdf90a-4157-4991-b193-5f90945e8afb'
		,GETDATE()
		,null
		,null
),
(
        3
		,'DateMinToUtcNow'
		,'DateTime.Min to DateTime.UtcNow'
		,'14bdf90a-4157-4991-b193-5f90945e8afb'
		,GETDATE()
		,null
		,null
)

-- UPDATE EmployeeRoles EmployeeLevels

UPDATE  [dbo].[EmployeeRoles] SET EmployeeLevel = 4

UPDATE  [dbo].[EmployeeRoles] SET EmployeeLevel = 1
WHERE Len([Name]) = 3 OR UPPER([Name]) LIKE '%PRESIDENT%'

UPDATE  [dbo].[EmployeeRoles] SET EmployeeLevel = 2
WHERE UPPER([Name]) LIKE '%SUPERVISOR%'


UPDATE  [dbo].[EmployeeRoles] SET EmployeeLevel = 3
WHERE UPPER([Name]) LIKE '%MANAGER%' OR UPPER([Name]) LIKE '%DIRECTOR%'