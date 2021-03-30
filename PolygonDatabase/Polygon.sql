CREATE TABLE [dbo].[Polygon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Data] [sys].[geometry] NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
