IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ClientServicePortal')
BEGIN
  CREATE DATABASE ClientServicePortal;
END