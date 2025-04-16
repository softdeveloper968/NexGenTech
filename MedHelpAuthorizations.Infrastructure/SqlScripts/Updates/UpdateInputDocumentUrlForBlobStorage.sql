DECLARE @DatabaseName nvarchar(128)
DECLARE @SQL nvarchar(max)
DECLARE @RowsAffected int

-- Temporary table to store the results
CREATE TABLE #Results (DatabaseName nvarchar(128), RowsAffected int)

-- Cursor to iterate through each database
DECLARE db_cursor CURSOR FOR
SELECT name
FROM sys.databases
WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @DatabaseName

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Dynamic SQL to update the InputDocuments table in each database
    SET @SQL = 'USE [' + @DatabaseName + ']; ' +
               'IF EXISTS (SELECT 1 FROM sys.tables WHERE name = ''InputDocuments'' AND schema_id = SCHEMA_ID(''IntegratedServices'')) ' +
               'BEGIN ' +
               'UPDATE [IntegratedServices].[InputDocuments] ' +
               'SET [URL] = REPLACE(REPLACE([URL], ''Files\IntegratedServices'', ''/IntegratedServices''), ''\'', ''/''); ' +
               'SELECT @RowsAffected = @@ROWCOUNT; ' +
               'INSERT INTO #Results (DatabaseName, RowsAffected) VALUES (''' + @DatabaseName + ''', @RowsAffected); ' +
               'END'

    -- Execute the dynamic SQL
    EXEC sp_executesql @SQL, N'@RowsAffected int OUTPUT', @RowsAffected OUTPUT

    FETCH NEXT FROM db_cursor INTO @DatabaseName
END

CLOSE db_cursor
DEALLOCATE db_cursor

-- Select the results
SELECT * FROM #Results

-- Drop the temporary table
DROP TABLE #Results
 