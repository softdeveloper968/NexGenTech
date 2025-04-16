-- Recreate foreign key constraints for the new table
-- Add the foreign key constraint for RpaInsuranceGroupId in RpaInsurances
ALTER TABLE [IntegratedServices].[RpaInsurances]
ADD CONSTRAINT FK_RpaInsurances_RpaInsuranceGroups_RpaInsuranceGroupId
FOREIGN KEY (RpaInsuranceGroupId) REFERENCES [dbo].[RpaInsuranceGroups](Id);
GO

-- Add the foreign key constraint for RpaInsuranceGroupId in ClientRpaCredentialConfigurations
ALTER TABLE [dbo].[ClientRpaCredentialConfigurations]
ADD CONSTRAINT FK_ClientRpaCredentialConfigurations_RpaInsuranceGroups_RpaInsuranceGroupId
FOREIGN KEY (RpaInsuranceGroupId) REFERENCES [dbo].[RpaInsuranceGroups](Id);
GO