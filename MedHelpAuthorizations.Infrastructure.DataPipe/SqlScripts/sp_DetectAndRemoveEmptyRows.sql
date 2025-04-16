/****** Object:  StoredProcedure [dbo].[sp_DetectandRemoveEmptyRows]    Script Date: 07/15/2024 9:36:54 AM ******/
DROP PROCEDURE [dbo].[sp_DetectandRemoveEmptyRows]
GO

/****** Object:  StoredProcedure [dbo].[sp_DetectandRemoveEmptyRows]    Script Date: 07/15/2024 9:36:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE     PROCEDURE [dbo].[sp_DetectandRemoveEmptyRows]
@FileName nvarchar(max)
AS
BEGIN
    -- The filename also matched with the Sourcename similar to mapping table
      if( LOWER(@FileName) = LOWER('PlacesOfServices') OR LOWER(@FileName) = LOWER('PlacesOfService'))
	   Begin
      -- Delete empty rows from tbl_PlaceOfServices
        DELETE FROM [dbo].[tbl_PlaceOfServices]
        WHERE TenantClientString IS NOT NULL
          AND ([PlaceOfServiceId] IS NULL AND [Name] IS NULL AND [Address] IS NULL AND [City] IS NULL 
          AND [State] IS NULL AND [PostalCode] IS NULL AND [PlaceOfServiceCode] IS NULL 
          AND [OfficePhone] IS NULL AND [FaxPhone] IS NULL AND [NPI] IS NULL 
          AND [ExternalId] IS NULL AND [LocationId] IS NULL);
		  End
	  else if( LOWER(@FileName) = LOWER('AdjustmentCodes') OR LOWER(@FileName) = LOWER('AdjustmentCode'))
	     Begin
        -- Delete rows from tbl_AdjustmentCodes
        DELETE FROM [dbo].[tbl_AdjustmentCodes]
        WHERE TenantClientString IS NOT NULL
          AND ([Id] IS NULL AND [Name] IS NULL AND [Type] IS NULL);
        End
	  else  if( LOWER(@FileName) = LOWER('CardHolders') OR LOWER(@FileName) = LOWER('CardHolder'))
	     Begin
        -- Delete rows from tbl_CardHolders
        DELETE FROM [dbo].[tbl_CardHolders]
        WHERE TenantClientString IS NOT NULL
          AND ([CardHolderId] IS NULL AND [FirstName] IS NULL AND [LastName] IS NULL 
          AND [Address] IS NULL AND [City] IS NULL AND [State] IS NULL AND [PostalCode] IS NULL 
          AND [Employer] IS NULL AND [Gender] IS NULL AND [DateOfBirth] IS NULL);
       End
	  else  if( LOWER(@FileName) = LOWER('Charges') OR LOWER(@FileName) = LOWER('Charge'))
	     Begin
        -- Delete rows from tbl_Charges
        DELETE FROM [dbo].[tbl_Charges]
        WHERE TenantClientString IS NOT NULL
          AND ([ChargeId] IS NULL AND [ClaimNumber] IS NULL AND [PatientId] IS NULL 
          AND [ResponsiblePartyId] IS NULL AND [ProcedureCode] IS NULL AND [PlaceOfServiceCode] IS NULL 
          AND [Quantity] IS NULL AND [ChargeAmount] IS NULL AND [Description] IS NULL 
          AND [PatientInsuranceCard1Id] IS NULL AND [Insurance1] IS NULL AND [PatientInsuranceCard2Id] IS NULL 
          AND [Insurance2] IS NULL AND [PatientInsuranceCard3Id] IS NULL AND [Insurance3] IS NULL 
          AND [PatientFirstBillDate] IS NULL AND [PatientLastBillDate] IS NULL AND [BilledToInsuranceId] IS NULL 
          AND [RenderingProviderId] IS NULL AND [PlaceOfServiceId] IS NULL AND [Modifier1] IS NULL 
          AND [Modifier2] IS NULL AND [Modifier3] IS NULL AND [Modifier4] IS NULL 
          AND [IcdCode1] IS NULL AND [IcdCode2] IS NULL AND [IcdCode3] IS NULL 
          AND [IcdCode4] IS NULL AND [DateOfServiceFrom] IS NULL AND [DateOfServiceTo] IS NULL 
          AND [LocationId] IS NULL AND [EntryDate] IS NULL AND [ModifiedDate] IS NULL);
       End
	  else  if( LOWER(@FileName) = LOWER('ClaimAdjustments') OR LOWER(@FileName) = LOWER('ClaimAdjustment'))
	     Begin
        -- Delete rows from tbl_ClaimAdjustments
        DELETE FROM [dbo].[tbl_ClaimAdjustments]
        WHERE TenantClientString IS NOT NULL
          AND ([ClaimAdjustmentsId] IS NULL AND [ClaimChargeId] IS NULL AND [ClaimNumber] IS NULL 
          AND [AdjustmentCodeId] IS NULL AND [RemittenceId] IS NULL AND [Description] IS NULL 
          AND [Amount] IS NULL AND [EntryDate] IS NULL AND [ModifiedDate] IS NULL);
       End
	  else  if( LOWER(@FileName) = LOWER('ClaimPayments') OR LOWER(@FileName) = LOWER('ClaimPayment'))
	     Begin
        -- Delete rows from tbl_ClaimPayments
        DELETE FROM [dbo].[tbl_ClaimPayments]
        WHERE TenantClientString IS NOT NULL
          AND ([ClaimPaymentId] IS NULL AND [ClaimChargeId] IS NULL AND [ClaimNumber] IS NULL 
          AND [RemittanceId] IS NULL AND [Description] IS NULL AND [Amount] IS NULL 
          AND [EntryDate] IS NULL AND [ModifiedDate] IS NULL);
       End
	  else  if( LOWER(@FileName) = LOWER('Insurances') OR LOWER(@FileName) = LOWER('Insurance'))
	     Begin
        -- Delete rows from tbl_Insurances
        DELETE FROM [dbo].[tbl_Insurances]
        WHERE TenantClientString IS NOT NULL
          AND ([InsuranceId] IS NULL AND [Name] IS NULL AND [AddressStreetLine1] IS NULL 
          AND [AddressStreetLine2] IS NULL AND [City] IS NULL AND [State] IS NULL 
          AND [PostalCode] IS NULL AND [Phone] IS NULL AND [Active] IS NULL);
        End
	  else  if( LOWER(@FileName) = LOWER('Locations') OR LOWER(@FileName) = LOWER('Location'))
	     Begin
        -- Delete rows from tbl_Locations
        DELETE FROM [dbo].[tbl_Locations]
        WHERE TenantClientString IS NOT NULL
          AND ([LocationId] IS NULL AND [LocationName] IS NULL AND [AddressStreetLine1] IS NULL 
          AND [AddressStreetLine2] IS NULL AND [City] IS NULL AND [State] IS NULL 
          AND [PostalCode] IS NULL);
		  End
	  else  if( LOWER(@FileName) = LOWER('ProviderLocations') OR LOWER(@FileName) = LOWER('ProviderLocation'))
	     Begin
        -- Delete rows from tbl_ProviderLocations
        DELETE FROM [dbo].[tbl_ProviderLocations]
        WHERE TenantClientString IS NOT NULL
          AND ([ProviderId] IS NULL AND [LocationId] IS NULL);
		  End
	  else  if( LOWER(@FileName) = LOWER('Providers') OR LOWER(@FileName) = LOWER('Provider'))
	     Begin
        -- Delete rows from tbl_Providers
        DELETE FROM [dbo].[tbl_Providers]
        WHERE TenantClientString IS NOT NULL
          AND ([ProviderId] IS NULL AND [FullName] IS NULL AND [Address] IS NULL 
          AND [City] IS NULL AND [State] IS NULL AND [PostalCode] IS NULL 
          AND [OfficePhone] IS NULL AND [LicenseNumber] IS NULL AND [SocialSecurityNumber] IS NULL 
          AND [TaxId] IS NULL AND [SpecialtyId] IS NULL AND [FirstName] IS NULL 
          AND [LastName] IS NULL AND [IsPhysiciansAssistant] IS NULL 
          AND [Npi] IS NULL AND [FaxNumber] IS NULL AND [ExternalId] IS NULL 
          AND [IsActive] IS NULL);
		  End
	  else  if( LOWER(@FileName) = LOWER('ResponsibleParties') OR LOWER(@FileName) = LOWER('ResponsiblePartie'))
	     Begin
        -- Delete rows from tbl_ResponsibleParties
        DELETE FROM [dbo].[tbl_ResponsibleParties]
        WHERE TenantClientString IS NOT NULL
          AND ([ResponsiblePartiesId] IS NULL AND [LastName] IS NULL AND [FirstName] IS NULL 
          AND [AddressStreetLine1] IS NULL AND [AddressStreetLine2] IS NULL AND [City] IS NULL 
          AND [State] IS NULL AND [PostalCode] IS NULL AND [Email] IS NULL 
          AND [MobilePhone] IS NULL);
		   End
	  else  if( LOWER(@FileName) = LOWER('Remittances') OR LOWER(@FileName) = LOWER('Remittance'))
	     Begin
        -- Delete rows from tbl_Remittances
        DELETE FROM [dbo].[tbl_Remittances]
        WHERE TenantClientString IS NOT NULL
          AND ([RemittanceId] IS NULL AND [InsuranceId] IS NULL AND [UndistributedAmount] IS NULL 
          AND [PaymentAmount] IS NULL AND [CheckNumber] IS NULL AND [PatientId] IS NULL 
          AND [RemittanceFormType] IS NULL AND [RemittanceSource] IS NULL AND [LocationId] IS NULL 
          AND [CheckDate] IS NULL);
		   End
	  else  if( LOWER(@FileName) = LOWER('Patients') OR LOWER(@FileName) = LOWER('Patient'))
	     Begin
        -- Delete rows from tbl_Patients
        DELETE FROM [dbo].[tbl_Patients]
        WHERE TenantClientString IS NOT NULL
          AND ([PatientId] IS NULL AND [ResponsiblePartyId] IS NULL AND [LastName] IS NULL 
          AND [FirstName] IS NULL AND [MiddleName] IS NULL AND [Gender] IS NULL 
          AND [AddressStreetLine1] IS NULL AND [City] IS NULL AND [State] IS NULL 
          AND [PostalCode] IS NULL AND [HomePhoneNumber] IS NULL AND [SocialSecurityNumber] IS NULL 
          AND [DateOfBirth] IS NULL AND [PrimaryInsuranceId] IS NULL AND [RenderingProviderId] IS NULL 
          AND [SecondaryInsuranceId] IS NULL AND [TertiaryInsuranceId] IS NULL 
          AND [CreatedOn] IS NULL AND [LastModifiedOn] IS NULL AND [ExternalId] IS NULL);
		  End
	  else  if( LOWER(@FileName) = LOWER('PatientInsuranceCards') OR LOWER(@FileName) = LOWER('PatientInsuranceCard'))
	     Begin
		  -- Delete rows from tbl_PatientInsuranceCards
		DELETE FROM [dbo].[tbl_PatientInsuranceCards]
		WHERE TenantClientString IS NOT NULL
		  AND ([PatientId] IS NULL AND [InsuranceId] IS NULL AND [CardHolderId] IS NULL 
		  AND [InsuranceCardOrder] IS NULL AND [GroupId] IS NULL AND [MemberNumber] IS NULL 
		  AND [EffectiveStartDate] IS NULL AND [EffectiveEndDate] IS NULL AND [CoPay] IS NULL 
		  AND [CoInsurance] IS NULL AND [ActiveDate] IS NULL AND [InactiveDate] IS NULL 
		  AND [InactivePosition] IS NULL AND [CardHolderRelationship] IS NULL AND [PlanType] IS NULL
		  AND [Id] IS NULL);
		END
END;
GO


