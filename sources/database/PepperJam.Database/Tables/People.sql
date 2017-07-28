CREATE TABLE [dbo].[People]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Type] INT NOT NULL,
	[Lastname] NVARCHAR(255) NULL,
	[Firstname] NVARCHAR(255) NULL,
	[LocationId] INT NOT NULL,
	[Title] NVARCHAR(255) NULL,
	[Service] NVARCHAR(255) NULL,
	[Flag] INT NOT NULL DEFAULT 0

    CONSTRAINT [FK_People_To_Locations] FOREIGN KEY ([LocationID]) REFERENCES [Locations]([ID])	
)
