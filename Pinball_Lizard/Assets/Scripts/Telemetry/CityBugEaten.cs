
public class CityBugEaten : ICityEvent
{
    public PlayerBase PlayerInfo { get; set; }
    public CityBase CityInfo { get; set; }
    public float BugLocationX { get; set; }
    public float BugLocationY { get; set; }
    public float BugLocationZ { get; set; }
    public int ActiveBug { get; set; }
}

