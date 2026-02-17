CREATE TABLE [dbo].[X_Campaign](
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(128) NOT NULL,
	[Notes] nvarchar(500) NULL,
	[CampaignId] uniqueidentifier NOT NULL CONSTRAINT [DF_X_Campaign_CampaignId] DEFAULT(NEWSEQUENTIALID()),
	[Created] datetimeoffset(7) NOT NULL CONSTRAINT [DF_X_Campaign_Created] DEFAULT(sysdatetimeoffset()),
	CONSTRAINT [PK_X_Campaign] PRIMARY KEY CLUSTERED ([Id] asc),
	CONSTRAINT [UQ_X_Campaign] UNIQUE([CampaignId])
	)

GO

CREATE TABLE [dbo].[X_Campaign_AspNetUser] (
	[AspNetUsers_Id] NVARCHAR(450) NOT NULL,
	[X_Campaign_Id] bigint NOT NULL,
	[AccessLevel] int NOT NULL CONSTRAINT [DF_X_Campaign_AspNetUser_AccessLevel] DEFAULT(20),
	[Created] datetimeoffset(7) NOT NULL CONSTRAINT [DF_X_Campaign_AspNetUser_Created] DEFAULT(sysdatetimeoffset()),
	CONSTRAINT [PK_X_Campaign_AspNetUser] PRIMARY KEY([AspNetUsers_Id] asc, [X_Campaign_Id] asc),
	CONSTRAINT [FK_X_Campaign_AspNetUser_X_Campaign] FOREIGN KEY ([X_Campaign_Id]) REFERENCES [dbo].[X_Campaign] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_X_Campaign_AspNetUser_X_AspNetUsers] FOREIGN KEY ([AspNetUsers_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)