CREATE TABLE [dbo].[X_Organization_X_Location](
	[X_Organization_Id] bigint not null,
	[X_Location_Id] bigint not null,
	[Created] [datetimeoffset](7) not null CONSTRAINT [DF_X_Organization_X_Location_Created] DEFAULT(sysdatetimeoffset()),
	CONSTRAINT [PK_X_Organization_X_Location] PRIMARY KEY CLUSTERED ([X_Organization_Id] asc, [X_Location_Id] asc),
	CONSTRAINT [FK_X_Organization_X_Location_X_Organization_Id] FOREIGN KEY ([X_Organization_Id]) REFERENCES [dbo].[X_Organization] ([Id]) ON DELETE NO ACTION,
	CONSTRAINT [FK_X_Organization_X_Location_Id] FOREIGN KEY ([X_Location_Id]) REFERENCES [dbo].[X_Location] ([Id]) ON DELETE CASCADE
)

GO

CREATE TABLE [dbo].[X_Organization_X_Character](
	[X_Organization_Id] bigint not null,
	[X_Character_Id] bigint not null,
	[Created] [datetimeoffset](7) not null CONSTRAINT [DF_X_Organization_X_Character_Created] DEFAULT(sysdatetimeoffset()),
	CONSTRAINT [PK_X_Organization_X_Character] PRIMARY KEY CLUSTERED ([X_Organization_Id] asc, [X_Character_Id] asc),
	CONSTRAINT [FK_X_Organization_X_Character_X_Organization_Id] FOREIGN KEY ([X_Organization_Id]) REFERENCES [dbo].[X_Organization] ([Id]) ON DELETE NO ACTION,
	CONSTRAINT [FK_X_Organization_X_Character_Id] FOREIGN KEY ([X_Character_Id]) REFERENCES [dbo].[X_Character] ([Id]) ON DELETE CASCADE
)

GO

CREATE TABLE [dbo].[X_PersistentUserState](
	[UserId] nvarchar(450) NOT NULL,
	[ActiveCampaignId] bigint NULL,
	CONSTRAINT [PK_X_PersistentUserState] PRIMARY KEY ([UserId]),
	CONSTRAINT [FK_X_PersistentUserState_AspNetUsers_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)