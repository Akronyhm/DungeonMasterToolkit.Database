ALTER TABLE [dbo].[X_Organization] ADD
	[OrganizationId] uniqueidentifier not null constraint [DF_X_Organization_OrganizationId] DEFAULT(newsequentialid())
GO
CREATE UNIQUE INDEX [IX_X_Organization_OrganizationId] on [dbo].[X_Organization] ([OrganizationId])

GO

ALTER TABLE [dbo].[X_Character] ADD
	[CharacterId] uniqueidentifier not null constraint [DF_X_Character_CharacterId] DEFAULT(newsequentialid())
GO
CREATE UNIQUE INDEX [IX_X_Character_CharacterId] on [dbo].[X_Character] ([CharacterId])

GO

ALTER TABLE [dbo].[X_Location] ADD
	[LocationId] uniqueidentifier not null constraint [DF_X_Location_LocationId] DEFAULT(newsequentialid())
GO
CREATE UNIQUE INDEX [IX_X_Location_LocationId] on [dbo].[X_Location] ([LocationId])