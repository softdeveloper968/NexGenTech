CREATE OR ALTER PROCEDURE [IntegratedServices].[spUpdateClaimStatusExceptionReasonCategory] 
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tx
    SET tx.ClaimStatusExceptionReasonCategoryId = (
        SELECT TOP 1 ClaimStatusExceptionReasonCategoryId 
        FROM [IntegratedServices].ClaimStatusExceptionReasonCategoryMaps maps
        WHERE 1 = 1
        AND st.ClaimStatusTypeId = 2
        AND UPPER(LTRIM(RTRIM(tx.ExceptionReason))) LIKE '%' + UPPER(LTRIM(RTRIM(maps.ClaimStatusExceptionReasonText))) + '%'
    )
    FROM [IntegratedServices].[ClaimStatusTransactions] tx
    JOIN [IntegratedServices].ClaimLineItemStatuses st ON st.Id = tx.ClaimLineItemStatusId;
END
GO
