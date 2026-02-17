DROP INDEX [IX_X_Organization_OrganizationId] on [dbo].[X_Organization] 
DROP INDEX [IX_X_Character_CharacterId] on [dbo].[X_Character] 
DROP INDEX [IX_X_Location_LocationId] on [dbo].[X_Location] 

GO

ALTER TABLE [dbo].[X_Organization] DROP CONSTRAINT [DF_X_Organization_OrganizationId]
ALTER TABLE [dbo].[X_Character] DROP CONSTRAINT [DF_X_Character_CharacterId]
ALTER TABLE [dbo].[X_Location] DROP CONSTRAINT [DF_X_Location_LocationId]

GO

ALTER TABLE [dbo].[X_Organization] DROP COLUMN [OrganizationId]
ALTER TABLE [dbo].[X_Character] DROP COLUMN [CharacterId]
ALTER TABLE [dbo].[X_Location] DROP COLUMN [LocationId]