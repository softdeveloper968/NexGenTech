SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetExecutiveCurrentMonthEmployeeWork]
@ClientId NVARCHAR(MAX)

WITH RECOMPILE
AS
BEGIN

DECLARE @ClaimBilledFrom DATETIME = DATEADD(month, DATEDIFF(month, 0, GETDATE()) -2, 0)
DECLARE @ClaimBilledTo DATETIME = EOMONTH(GETDATE())
DECLARE @employeeId INT;
DECLARE @UserId NVARCHAR(max)

DECLARE @FirstDayOfLastMonth DateTime = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
DECLARE @LastDayOfLastMonth DateTime = EOMONTH(@FirstDayOfLastMonth);

DECLARE @tempBatchClaims TABLE (
	Id int NOT NULL PRIMARY KEY,
	ClientId int, 
	ClaimLevelMd5Hash char(34),
	PatientId int,
	PatientLastName nvarchar(36),
	ClientInsuranceId int,
	ClientLocationId int,
	ClaimBilledOn DateTime, 
	BilledAmount decimal(18,2),
	DateOfServiceFrom DateTime,
	DateOfServiceTo DateTime,
	CreatedOn DateTime, 
	ClaimStatusTransactionId int, 
	Quantity int, 
	ClientProviderId int, 
	NormalizedClaimNumber nvarchar(max), 
	ClientCptCodeId int
-- INDEXES
	INDEX idx_tbl_ClientId (ClientId),
	INDEX idx_tbl_ClaimLevelMd5Hash (ClaimLevelMd5Hash),
	INDEX idx_tbl_PatientId(PatientId),
	INDEX idx_tbl_ClientInsuranceId (ClientInsuranceId),
	INDEX idx_tbl_ClaimBilledOn (ClaimBilledOn),
	INDEX idx_tbl_ClaimStatusTransactionId (ClaimStatusTransactionId),
	INDEX idx_tbl_ClientProviderId (ClientProviderId)
)

DECLARE @EmployeeClientInsurances TABLE (
		ClientId int,
		EmployeeId int, 
		EmployeeClientId int,
		ClientInsuranceId INT
	)

INSERT INTO @EmployeeClientInsurances (ClientID, EmployeeId, EmployeeClientId, ClientInsuranceId)

SELECT EC.ClientId, EC.EmployeeId, EC.Id, ECI.ClientInsuranceId 
FROM [dbo].[EmployeeClientInsurances] AS ECI
JOIN [dbo].EmployeeClients AS EC ON EC.Id = ECI.EmployeeClientId
WHERE 1=1
AND EC.ClientId = @ClientId

DECLARE @EmployeeClientLocations TABLE (
		ClientId int,
		EmployeeId int, 
		EmployeeClientId int,
		ClientLocationId INT
	)

INSERT INTO @EmployeeClientLocations (ClientID, EmployeeId, EmployeeClientId, ClientLocationId)
SELECT  EC.ClientId, EC.EmployeeId, EC.Id, ECL.ClientLocationId
FROM [dbo].[EmployeeClientLocations] AS ECL
JOIN [dbo].EmployeeClients AS EC ON EC.Id = ECL.EmployeeClientId
WHERE 1=1
AND EC.ClientId = @ClientId

DECLARE @CurrentMonthEmployeeWork TABLE (
	UserId NVARCHAR(MAX),
	ClientId int,
	EmployeeId int, 
	EmployeeClientId int,
	EmployeeLastCommaFirst NVARCHAR,
	Visits INT,
	Charges DECIMAL,
	AvgCharges DECIMAL,
	LastMonthTotals DECIMAL
)

DECLARE employeeCursor CURSOR FOR
SELECT E.Id, E.UserId
FROM [dbo].[Employees] AS E
RIGHT JOIN dbo.EmployeeClients as ec on ec.EmployeeId = E.Id
WHERE IsDeleted = 0
AND ec.ClientId = @ClientId
GROUP BY E.Id, E.UserId

OPEN employeeCursor;

	FETCH NEXT FROM employeeCursor INTO @employeeId, @userId;

	WHILE @@FETCH_STATUS = 0
    BEGIN

	DECLARE @EmployeeClientAlphaSplit AS [EmployeeClientAlphaSplitTable]

	INSERT INTO @EmployeeClientAlphaSplit (ClientID, EmployeeId, EmployeeClientId, BeginingAlpha, EndingAlpha)
	SELECT EC.ClientId, EC.EmployeeId, EC.Id
	,CASE WHEN  ECAS.AlphaSplitId = 1 THEN CustomBeginAlpha ELSE APS.BeginAlpha END
	,CASE WHEN  ECAS.AlphaSplitId = 1 THEN CustomEndAlpha ELSE APS.EndAlpha END
	FROM [dbo].[EmployeeClientAlphaSplits] AS ECAS
	JOIN [dbo].EmployeeClients AS EC ON EC.Id = ECAS.EmployeeClientId
	INNER JOIN [dbo].[AlphaSplits] AS APS ON APS.ID = ECAS.Id
	WHERE 1=1
	AND EC.[EmployeeId] = @EmployeeId

	DECLARE @Patients TABLE (
	PatientId INT
	)

	INSERT INTO @Patients (PatientId)
	SELECT P1.ID 
	FROM [dbo].[Patients] AS P1
	LEFT JOIN [dbo].[Persons] AS P2 ON P1.PersonId = P2.Id
	WHERE 
		EXISTS (
			SELECT 1 
			FROM @EmployeeClientAlphaSplit AS ECAS 
			WHERE 
				ECAS.BeginingAlpha IS NOT NULL 
				AND ECAS.EndingAlpha IS NOT NULL 
				AND LEFT(P2.LastName, 1) BETWEEN ECAS.BeginingAlpha AND ECAS.EndingAlpha
		)

	INSERT INTO @CurrentMonthEmployeeWork (UserId, ClientId, EmployeeId, Visits, Charges, AvgCharges, LastMonthTotals)
	SELECT @userId
		,@ClientId
		,@employeeId
		--,CONCAT(U.LastName, ',', U.FirstName) -- NEED TO GET THIS USER INFO FROM THE faIdentityDB DATABASE. THE Local User Table in the Tenant is no longer in service
		,COUNT(DISTINCT(CASE WHEN tmpc1.ClaimBilledOn >= @ClaimBilledFrom AND tmpc1.ClaimBilledOn <= @ClaimBilledTo THEN tmpc1.ClaimLevelMd5Hash ELSE NULL END))
		,SUM(CASE WHEN tmpc1.ClaimBilledOn >= @ClaimBilledFrom AND tmpc1.ClaimBilledOn <= @ClaimBilledTo THEN tmpc1.BilledAmount ELSE 0 END)
		,AVG(CASE WHEN tmpc1.ClaimBilledOn >= @ClaimBilledFrom AND tmpc1.ClaimBilledOn <= @ClaimBilledTo THEN tmpc1.BilledAmount ELSE 0 END)
		,SUM(CASE WHEN tmpc1.ClaimBilledOn >= @FirstDayOfLastMonth AND tmpc1.ClaimBilledOn <= @LastDayOfLastMonth THEN tmpc1.BilledAmount ELSE 0 END) -- TODO: Filter on last Month
		FROM 
		IntegratedServices.ClaimStatusBatchClaims as tmpc1
		--LEFT JOIN [dbo].[Employees] AS E ON E.Id = @employeeId

		--LEFT JOIN [Identity].[Users] AS U ON U.ID = E.UserId -- NEED TO GET THIS USER INFO FROM THE faIdentityDB DATABASE. THE Local User Table in the Tenant is no longer in service
		WHERE tmpc1.ClientId = @ClientId
		AND (tmpc1.ClientInsuranceId IN (SELECT ClientInsuranceId FROM @EmployeeClientInsurances WHERE EmployeeId = @employeeId)
		OR tmpc1.ClientLocationId IN (SELECT ClientLocationId FROM @EmployeeClientLocations WHERE EmployeeId = @employeeId))
		AND (NOT EXISTS (select 1 from @EmployeeClientAlphaSplit WHERE ClientId = tmpc1.ClientId) OR (IntegratedServices.fnHasAlphaSplitMatch(tmpc1.ClientId, tmpc1.PatientLastName, @EmployeeClientAlphaSplit) = 1))

		GROUP BY tmpc1.ClientId

		-- IF EmployeeID is not in CurrentMonthEmployeeWork table; INSERT a Blank Record with that Employee ID. 
		IF NOT EXISTS(SELECT 1 FROM @CurrentMonthEmployeeWork WHERE EmployeeId = @employeeId)
			INSERT INTO @CurrentMonthEmployeeWork(UserId, ClientId, EmployeeId, Visits, Charges, AvgCharges, LastMonthTotals) 
				VALUES (@userId, @ClientId, @employeeId, 0, 0, 0, 0)

	DELETE FROM @EmployeeClientAlphaSplit
	DELETE FROM @EmployeeClientLocations
	DELETE FROM @EmployeeClientInsurances

	FETCH NEXT FROM employeeCursor INTO @employeeId, @userId
    END
	CLOSE employeeCursor
	DEALLOCATE employeeCursor
	SELECT * FROM @CurrentMonthEmployeeWork
END
