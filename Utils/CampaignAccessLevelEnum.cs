namespace DungeonMasterToolkit.Database.Utils;
public enum CampaignAccessLevelEnum : int
{
    Unspecified = -1,
    Owner = 0,
    Moderator = 10,
    Member = 20,
    Spectator = 30
}
