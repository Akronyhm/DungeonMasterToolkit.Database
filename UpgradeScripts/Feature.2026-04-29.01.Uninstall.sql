ALTER TABLE [dbo].[X_Organization] DROP CONSTRAINT IF EXISTS [DF_X_Organization_VisionLevel];
ALTER TABLE [dbo].[X_Character] DROP CONSTRAINT IF EXISTS [DF_X_Character_VisionLevel];
ALTER TABLE [dbo].[X_Location] DROP CONSTRAINT IF EXISTS [DF_X_Location_VisionLevel];
ALTER TABLE [dbo].[X_Campaign_AspNetUser] DROP CONSTRAINT IF EXISTS [DF_X_CAMPAIGN_ASPNETUSER_USERVISIONLEVEL];
ALTER TABLE [dbo].[X_Note] DROP CONSTRAINT IF EXISTS [DF_X_Note_VisionLevel]
ALTER TABLE [dbo].[X_Note] DROP CONSTRAINT IF EXISTS [DF_X_Note_IsGlobalNote];

GO

ALTER TABLE [dbo].[X_Character] DROP COLUMN IF EXISTS [VisionLevel];
ALTER TABLE [dbo].[X_Organization] DROP COLUMN IF EXISTS [VisionLevel];
ALTER TABLE [dbo].[X_Location] DROP COLUMN IF EXISTS [VisionLevel];
ALTER TABLE [dbo].[X_Campaign_AspNetUser] DROP COLUMN IF EXISTS [UserVisionLevel];
ALTER TABLE [dbo].[X_Note] DROP COLUMN IF EXISTS [VisionLevel];
ALTER TABLE [dbo].[X_Note] DROP COLUMN IF EXISTS [IsGlobalNote];