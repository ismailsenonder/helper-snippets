/* MSSQL: Search text in procedures, triggers and functions. */
-- METHOD 1
SELECT DISTINCT o.name AS Object_Name, o.type_desc FROM sys.sql_modules m 
INNER JOIN sys.objects o ON m.object_id = o.object_id 
WHERE m.definition Like '%search_text%';  
-- METHOD 2
SELECT name FROM sys.procedures 
WHERE Object_definition(object_id) LIKE '%search_text%'

/* MSSQL: Get all tables containing columns with a specified name */
SELECT c.name AS ColName, t.name AS TableName
FROM sys.columns c
JOIN sys.tables t ON c.object_id = t.object_id
WHERE c.name LIKE '%colName%';

/* MSSQL: Get all table names in a database */
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='dbName'

/****************************************************/

/* MySQL: Get all table names in a database */
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='dbName' 

/****************************************************/

/* MySQL: Find all procedures and functions that uses, references or depends on a table */
SELECT * FROM Mysql.proc where body LIKE '%table_name%';