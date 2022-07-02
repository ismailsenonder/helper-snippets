/* MSSQL: Copy contents of one table into another (Column names and types must be the same) */

-- Copy all records
SELECT * INTO new_table FROM old_table; 

-- Copy selected columns only
SELECT column_name1, column_name2 INTO new_table FROM old_table;

-- Copy specified number of records
SELECT TOP 100 * INTO new_table FROM old_table;



