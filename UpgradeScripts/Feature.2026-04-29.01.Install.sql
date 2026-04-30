ALTER TABLE [dbo].[X_Character] ADD [VisionLevel] int NULL;
ALTER TABLE [dbo].[X_Organization] ADD [VisionLevel] int NULL;
ALTER TABLE [dbo].[X_Location] ADD [VisionLevel] int NULL;
ALTER TABLE [dbo].[X_Campaign_AspNetUser] ADD [UserVisionLevel] int NULL;
ALTER TABLE [dbo].[X_Note] ADD
	[VisionLevel] int NULL,
	[IsGlobalNote] bit NULL;


GO

UPDATE [dbo].[X_Character] SET [VisionLevel] = 1 WHERE [VisionLevel] IS NULL; 
UPDATE [dbo].[X_Location] SET [VisionLevel] = 1 WHERE [VisionLevel] IS NULL;
UPDATE [dbo].[X_Organization] SET [VisionLevel] = 1 WHERE [VisionLevel] IS NULL;
UPDATE [dbo].[X_Note] SET [VisionLevel] = 1 WHERE [VisionLevel] IS NULL;
UPDATE [dbo].[X_Note] SET [IsGlobalNote] = 0 WHERE [IsGlobalNote] IS NULL;
UPDATE [dbo].[X_Campaign_AspNetUser]
SET [UserVisionLevel] =
	CASE
		WHEN [AccessLevel] = 0
			THEN 
				0
			ELSE
				10
	END
WHERE [UserVisionLevel] IS NULL;

GO

ALTER TABLE [dbo].[X_Character] ALTER COLUMN [VisionLevel] int NOT NULL;
ALTER TABLE [dbo].[X_Organization] ALTER COLUMN [VisionLevel] int NOT NULL;
ALTER TABLE [dbo].[X_Location] ALTER COLUMN [VisionLevel] int NOT NULL;
ALTER TABLE [dbo].[X_Note] ALTER COLUMN [VisionLevel] int NOT NULL;
ALTER TABLE [dbo].[X_Note] ALTER COLUMN [IsGlobalNote] bit NOT NULL;
ALTER TABLE [dbo].[X_Campaign_AspNetUser] ALTER COLUMN [UserVisionLevel] int NOT NULL;

GO

ALTER TABLE [dbo].[X_Character] ADD CONSTRAINT [DF_X_CHARACTER_VISIONLEVEL] DEFAULT(1) FOR [VisionLevel];
ALTER TABLE [dbo].[X_Organization] ADD CONSTRAINT [DF_X_ORGANIZATION_VISIONLEVEL] DEFAULT(1) FOR [VisionLevel];
ALTER TABLE [dbo].[X_Location] ADD CONSTRAINT [DF_X_LOCATION_VISIONLEVEL] DEFAULT(1) FOR [VisionLevel];
ALTER TABLE [dbo].[X_Campaign_AspNetUser] ADD CONSTRAINT [DF_X_CAMPAIGN_ASPNETUSER_USERVISIONLEVEL] DEFAULT(10) FOR [UserVisionLevel];
ALTER TABLE [dbo].[X_Note] ADD
	CONSTRAINT [DF_X_Note_VisionLevel] DEFAULT(1) FOR [VisionLevel],
	CONSTRAINT [DF_X_Note_IsGlobalNote] DEFAULT(0) FOR [IsGlobalNote];