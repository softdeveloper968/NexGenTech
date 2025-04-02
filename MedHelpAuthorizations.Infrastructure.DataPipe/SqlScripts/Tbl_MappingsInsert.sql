/****** Object:  Table [dbo].[tbl_Mappings]    Script Date: 07/12/2024 5:11:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CREATE TABLE [dbo].[tbl_Mappings](
--	[TableSchema] [nvarchar](50) NULL,
--	[TableName] [nvarchar](50) NULL,
--	[Mapping] [nvarchar](max) NULL,
--	[TableId] [nvarchar](255) NULL,
--	[Source] [varchar](50) NULL,
--	[SourceFileName] [varchar](255) NULL,
--	[TenantClientString] [varchar](100) NULL
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
--GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_Charges', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ChargeId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "claimnumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimNumber",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "patientid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "responsiblepartyid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ResponsiblePartyId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "procedurecode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ProcedureCode",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "placeofservicecode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PlaceOfServiceCode",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "quantity",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Quantity",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "chargeamount",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ChargeAmount",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 10
            }
        },
        {
            "source": {
                "name": "description",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Description",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "patientinsurancecard1id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientInsuranceCard1Id",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "insurance1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Insurance1",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "patientinsurancecard2id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientInsuranceCard2Id",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "insurance2",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Insurance2",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "patientinsurancecard3id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientInsuranceCard3Id",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "insurance3",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Insurance3",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "patientfistbilldate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientFirstBillDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "patientlastbilldate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientLastBillDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "billedtoinsuranceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "BilledToInsuranceId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "renderingproviderid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RenderingProviderId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "placeofserviceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PlaceOfServiceId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "modifier1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Modifier1",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "modifier2",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Modifier2",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "modifier3",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Modifier3",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "modifier4",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Modifier4",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "icdcode1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "IcdCode1",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "icdcode2",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "IcdCode2",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "icdcode3",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "IcdCode3",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "icdcode4",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "IcdCode4",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "dateofservicefrom",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "DateOfServiceFrom",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "dateofserviceto",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "DateOfServiceTo",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "locationid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LocationId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "entrydate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "EntryDate",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "modifieddate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ModifiedDate",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'ChargeId', N'CyFluent', N'Charges', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_Remittances', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RemittanceId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "insuranceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "InsuranceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "undistributedamount",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "UndistributedAmount",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 10
            }
        },
        {
            "source": {
                "name": "paymentamount",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PaymentAmount",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 10
            }
        },
        {
            "source": {
                "name": "checknumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CheckNumber",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "patientid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "remittanceformtype",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RemittanceFormType",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "remittancesource",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RemittanceSource",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "locationid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LocationId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "checkdate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CheckDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'RemittanceId', N'CyFluent', N'Remittances', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_Patients', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "responsiblepartyid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ResponsiblePartyId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "lastname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LastName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "firstname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FirstName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "middlename",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "MiddleName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "gender",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Gender",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine1",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "homephonenumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "HomePhoneNumber",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "socialsecuritynumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "SocialSecurityNumber",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "dateofbirth",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "DateOfBirth",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "primaryinsuranceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PrimaryInsuranceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "renderingproviderid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RenderingProviderId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "secondaryinsuranceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "SecondaryInsuranceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "tertiaryinsuranceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TertiaryInsuranceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "createdon",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CreatedOn",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "lastmodifiedon",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LastModifiedOn",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "externalid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ExternalId",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'PatientId', N'CyFluent', N'Patients', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_PlaceOfServices', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "placeofserviceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PlaceOfServiceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "name",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Name",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "address",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Address",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "placeofservicecode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PlaceOfServiceCode",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "officephone",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "OfficePhone",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "faxphone",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FaxPhone",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "npi",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "NPI",
                "type": "Int64",
                "physicalType": "bigint"
            }
        },
        {
            "source": {
                "name": "externalid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ExternalId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "locationid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LocationId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
	     {
			"source": {
				"name": "TenantClientString",
				"type": "String",
				"physicalType": "String"
			},
			"sink": {
				"name": "TenantClientString",
				"type": "String",
				"physicalType": "varchar"
			}
		}
    ]
}', N'PlaceOfServiceId', N'CyFluent', N'PlacesOfService', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_Providers', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ProviderId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "fullname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FullName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "address",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Address",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "officephone",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "OfficePhone",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "licensenumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LicenseNumber",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "socialsecuritynumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "SocialSecurityNumber",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "taxid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TaxId",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "specialtyid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "SpecialtyId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "firstname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FirstName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "lastname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LastName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "isphysciansassistant",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "IsPhysiciansAssistant",
                "type": "String",
                "physicalType": "char"
            }
        },
        {
            "source": {
                "name": "npi",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Npi",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "faxnumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FaxNumber",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "externalid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ExternalId",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "isactive",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "IsActive",
                "type": "String",
                "physicalType": "char"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'ProviderId', N'CyFluent', N'Providers', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_PatientInsuranceCards', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "patientid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PatientId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "insuranceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "InsuranceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "cardholderid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CardHolderId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "insurancecardorder",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "InsuranceCardOrder",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "groupid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "GroupId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "membernumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "MemberNumber",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "effectivestartdate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "EffectiveStartDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "effectiveenddate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "EffectiveEndDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "copay",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CoPay",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 10
            }
        },
        {
            "source": {
                "name": "coinsurance",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CoInsurance",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 10
            }
        },
        {
            "source": {
                "name": "activedate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ActiveDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "inactivedate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "InactiveDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "inactiveposition",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "InactivePosition",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "cardholderrelationship",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CardHolderRelationship",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "plantype",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PlanType",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        },
		{
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Id",
                "type": "Int32",
                "physicalType": "int"
            }
        }
    ]
}', N'Id', N'CyFluent', N'PatientInsuranceCards', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_AdjustmentCodes', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Id",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "name",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Name",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "type",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Type",
                "type": "String",
                "physicalType": "char"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'Id', N'CyFluent', N'AdjustmentCodes', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_ProviderLocations', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "ProviderId",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ProviderId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "LocationId",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LocationId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'ProviderId', N'CyFluent', N'ProviderLocations', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_Insurances', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "InsuranceId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "name",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Name",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine1",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline2",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine2",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "phone",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Phone",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "active",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Active",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'InsuranceId', N'CyFluent', N'Insurances', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_Locations', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LocationId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "name",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LocationName",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine1",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline2",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine2",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'LocationId', N'CyFluent', N'Locations', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_CardHolders', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "CardHolderId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "firstname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FirstName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "lastname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LastName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "address",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Address",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "employer",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Employer",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "gender",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Gender",
                "type": "String",
                "physicalType": "char"
            }
        },
        {
            "source": {
                "name": "dateofbirth",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "DateOfBirth",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'CardHolderId', N'CyFluent', N'Cardholders', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_ClaimAdjustments', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimAdjustmentsId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "claimchargeid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimChargeId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "claimnumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimNumber",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "adjustmentcodeid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AdjustmentCodeId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "remittenceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RemittenceId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "description",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Description",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "amount",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Amount",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 10
            }
        },
        {
            "source": {
                "name": "entrydate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "EntryDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "modifieddate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ModifiedDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
		  {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'ClaimAdjustmentsId', N'CyFluent', N'ClaimAdjustments', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_ClaimPayments', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimPaymentId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "claimchargeid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimChargeId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "claimnumber",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ClaimNumber",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "remittenceid",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "RemittanceId",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "description",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Description",
                "type": "String",
                "physicalType": "nvarchar"
            }
        },
        {
            "source": {
                "name": "amount",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Amount",
                "type": "Decimal",
                "physicalType": "decimal",
                "scale": 2,
                "precision": 18
            }
        },
        {
            "source": {
                "name": "entrydate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "EntryDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "modifieddate",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ModifiedDate",
                "type": "String",
                "physicalType": "varchar"
            }
        },
		  {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'ClaimPaymentId', N'CyFluent', N'ClaimPayments', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
INSERT [dbo].[tbl_Mappings] ([TableSchema], [TableName], [Mapping], [TableId], [Source], [SourceFileName], [TenantClientString]) VALUES (N'dbo', N'tbl_ResponsibleParties', N'{
    "type": "TabularTranslator",
    "mappings": [
        {
            "source": {
                "name": "id",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "ResponsiblePartiesId",
                "type": "Int32",
                "physicalType": "int"
            }
        },
        {
            "source": {
                "name": "lastname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "LastName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "firstname",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "FirstName",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline1",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine1",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "addressstreetline2",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "AddressStreetLine2",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "city",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "City",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "state",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "State",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "postalcode",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "PostalCode",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "email",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "Email",
                "type": "String",
                "physicalType": "varchar"
            }
        },
		 {
            "source": {
                "name": "mobilephone",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "MobilePhone",
                "type": "String",
                "physicalType": "varchar"
            }
        },
        {
            "source": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "String"
            },
            "sink": {
                "name": "TenantClientString",
                "type": "String",
                "physicalType": "varchar"
            }
        }
    ]
}', N'ResponsiblePartiesId', N'CyFluent', N'ResponsibleParties', N'96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934')
GO
