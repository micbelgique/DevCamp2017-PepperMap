CREATE TABLE [dbo].[Locations]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RouteId] INT NOT NULL, 
    [Name] NCHAR(100) NULL

    CONSTRAINT [FK_Locations_To_Routes] FOREIGN KEY ([RouteID]) REFERENCES [Routes]([ID])
	
)
