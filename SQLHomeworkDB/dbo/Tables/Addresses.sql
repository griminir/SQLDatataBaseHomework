﻿CREATE TABLE [dbo].[Addresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StreetName] VARCHAR(50) NOT NULL, 
    [City] VARCHAR(50) NOT NULL, 
    [ZipCode] VARCHAR(4) NOT NULL
)
