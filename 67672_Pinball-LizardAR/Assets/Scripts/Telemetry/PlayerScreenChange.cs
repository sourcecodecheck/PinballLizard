
public class PlayerScreenChange :IPlayerEvent
{
    public PlayerBase PlayerInfo { get; set; }
    public string PreviousScreenID { get; set; }
    public int PrevScreenDuration { get; set; }
    public string CurrentStringID { get; set; }
}

