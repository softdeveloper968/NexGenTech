/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM[IntegratedServices].[RpaInsurances]

  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM[IntegratedServices].[TransactionTypes]

  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM[dbo].[AuthTypes]

INSERT INTO [IntegratedServices].[ClientInsuranceRpaConfigurations]
           ([ClientId]
           ,[RpaInsuranceId]
           ,[TransactionTypeId]
           ,[AuthTypeId]
           ,[Username]
           ,[Password]
           ,[TargetUrl]
           ,[FailureReported]
           ,[IsDeleted]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (1
		   ,4
		   ,1
           ,2
           ,'' -- UN
           ,'' -- PW
           ,'' -- URL
           ,0
           ,0
           ,null
           ,GETDATE())
GO

 