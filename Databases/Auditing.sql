/* MSSQL: Get running queries on specified db excluding system queries */

SELECT      r.start_time [Start Time],session_ID [SPID],
            DB_NAME(database_id) [DatabaseName],
            SUBSTRING(t.text,(r.statement_start_offset/2)+1,
            CASE WHEN statement_end_offset=-1 OR statement_end_offset=0
            THEN (DATALENGTH(t.Text)-r.statement_start_offset/2)+1
            ELSE (r.statement_end_offset-r.statement_start_offset)/2+1
            END) [Executing SQL],
            Status,command,wait_type,wait_time,wait_resource,
            last_wait_type
FROM        sys.dm_exec_requests r
OUTER APPLY sys.dm_exec_sql_text(sql_handle) t
WHERE session_id > 50 -- don't show system queries
AND DB_NAME(database_id) = 'DATABASE_NAME'
ORDER BY r.start_time

/****************************************************/

/* MSSQL: Get information about a running query on the database by specifying SPID */

CREATE TABLE #sp_who2 (SPID INT,Status VARCHAR(255),
      Login  VARCHAR(255),HostName  VARCHAR(255),
      BlkBy  VARCHAR(255),DBName  VARCHAR(255),
      Command VARCHAR(255),CPUTime INT,
      DiskIO INT,LastBatch VARCHAR(255),
      ProgramName VARCHAR(255),SPID2 INT,
      REQUESTID INT)
INSERT INTO #sp_who2 EXEC sp_who2
SELECT      *
FROM        #sp_who2
-- Add any filtering of the results here :
WHERE      SPID = 610
-- Add any sorting of the results here :
ORDER BY    DBName ASC
 
DROP TABLE #sp_who2