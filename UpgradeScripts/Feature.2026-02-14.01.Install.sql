CREATE TABLE [dbo].[X_Organization](
	[Id] bigint identity(1,1) not null,
	[Name] nvarchar(max) not null,
	[Description] nvarchar(max) null,
	[Disposition] int not null constraint [DF_X_Organization_Disposition] default(20),
	[X_Campaign_Id] bigint not null,
	[Created] [datetimeoffset](7) not null CONSTRAINT [DF_X_Organization_Created] DEFAULT(sysdatetimeoffset()),
	constraint [PK_X_Organization] PRIMARY KEY clustered ([Id] asc),
	constraint [FK_X_Organization_X_Campaign] foreign key ([X_Campaign_Id]) references [dbo].[X_Campaign] ([Id]) on delete cascade
	)

GO

CREATE TABLE [dbo].[X_Character](
	[Id] bigint identity(1,1) not null,
	[Name] nvarchar(max) not null,
	[Description] nvarchar(max) null,
	[IsAlive] bit not null constraint [DF_X_Character_IsAlive] default(1),
	[Disposition] int not null constraint [DF_X_Character_Disposition] default(20),
	[X_Campaign_Id] bigint not null,
	[Created] [datetimeoffset](7) not null constraint [DF_X_Character_Created] DEFAULT(sysdatetimeoffset()),
	constraint [PK_X_Character] primary key clustered ([Id] asc),
	constraint [FK_X_Character_X_Campaign] foreign key ([X_Campaign_Id]) references [dbo].[X_Campaign] ([Id]) on delete cascade
	)

GO 

CREATE TABLE [dbo].[X_Location](
	[Id] bigint identity(1,1) not null,
	[Name] nvarchar(max) not null,
	[Description] nvarchar(max) null,
	[X_Campaign_Id] bigint not null,
	[Created] [datetimeoffset](7) not null constraint [DF_X_Location_Created] default(sysdatetimeoffset()),
	constraint [PK_X_Location] primary key clustered ([Id] asc),
	constraint [FK_X_Location_X_Campaign] foreign key ([X_Campaign_Id]) references [dbo].[X_Campaign] ([Id]) on delete cascade
	)

GO

CREATE TABLE [dbo].[X_Note](
	[Id] bigint identity(1,1) not null,
	[NoteId] uniqueidentifier not null constraint [DF_X_Note_NoteId] default(NEWSEQUENTIALID()),
	[Text] nvarchar(max) null,
	[X_Campaign_Id] bigint not null,
	[Created] datetimeoffset(7) NOT NULL CONSTRAINT [DF_X_Note_Created] DEFAULT(sysdatetimeoffset())
	constraint [PK_X_Note] primary key clustered ([Id] asc),
	constraint [FK_X_Note_X_Campaign] FOREIGN KEY ([X_Campaign_Id]) REFERENCES [dbo].[X_Campaign] ([Id]) ON DELETE CASCADE,
	constraint [UQ_X_Note] UNIQUE([NoteId])
	)

GO

CREATE TABLE [dbo].[X_Note_AspNetUser](
	[X_Note_Id] bigint not null,
	[AspNetUser_Id] nvarchar(450) not null,
	[CanEdit] bit not null constraint [DF_X_Note_AspNetUser_CanEdit] DEFAULT(0),
	constraint [PK_X_Note_AspNetUser] primary key ([X_Note_Id] asc, [AspNetUser_Id] asc),
	constraint [FK_X_Note_Note_Id] foreign key ([X_Note_Id]) references [dbo].[X_Note] ([Id]) on delete cascade,
	constraint [FK_X_Note_AspNetUser_Id] foreign key ([AspNetUser_Id]) references [dbo].[AspNetUsers] ([Id]) on delete cascade

	)

GO

CREATE TABLE [dbo].[X_Note_X_Character](
	[X_Note_Id] bigint not null,
	[X_Character_Id] bigint not null,
	constraint [PK_X_Note_X_Character] primary key ([X_Note_Id] asc, [X_Character_Id] asc),
	constraint [FK_X_Note_X_Character_Note_Id] foreign key ([X_Note_Id]) REFERENCES [dbo].[X_Note] ([Id]) ON DELETE cascade,
	constraint [FK_X_Note_X_Character_Character_Id] foreign key ([X_Character_Id]) REFERENCES [dbo].[X_Character] ([Id]) ON DELETE no action
)

GO

CREATE TABLE [dbo].[X_Note_X_Location](
	[X_Note_Id] bigint not null,
	[X_Location_Id] bigint not null,
	constraint [PK_X_Note_X_Location] primary key ([X_Note_Id] asc, [X_Location_Id] asc),
	constraint [FK_X_Note_X_Location_Note_Id] foreign key ([X_Note_Id]) references [dbo].[X_Note] ([Id]) on delete cascade,
	constraint [FK_X_Note_X_Location_Location_Id] foreign key ([X_Location_Id]) references [dbo].[X_Location] ([Id]) on delete no action
)

GO

CREATE TABLE [dbo].[X_Note_X_Organization](
	[X_Note_Id] bigint not null,
	[X_Organization_Id] bigint not null,
	constraint [PK_X_Note_X_Organization] primary key ([X_Note_Id] asc, [X_Organization_Id] asc),
	constraint [FK_X_Note_X_Organization_Note_Id] foreign key ([X_Note_Id]) references [dbo].[X_Note] ([Id]) on delete cascade,
	constraint [FK_X_Note_X_Organization_Organization_Id] foreign key ([X_Organization_Id]) references [dbo].[X_Organization] ([Id]) on delete no action
)
