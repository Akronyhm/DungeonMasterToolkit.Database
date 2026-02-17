IF (NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Core_Version'))
	BEGIN
		CREATE TABLE [dbo].[Core_Version](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[DatabaseFeature] [nvarchar](100) NOT NULL,
		[Date] [nvarchar](10) NOT NULL,
		[Version] [nvarchar](2) NOT NULL,
		[UninstallScript] [nvarchar](MAX) NOT NULL,
		[Created] [datetimeoffset](7) NULL CONSTRAINT [DF_Core_Version_Created] DEFAULT(sysdatetimeoffset()),
		CONSTRAINT [PK_Core_Version] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [UQ_Core_Version] UNIQUE ([DatabaseFeature],[Date],[Version]))
		END;