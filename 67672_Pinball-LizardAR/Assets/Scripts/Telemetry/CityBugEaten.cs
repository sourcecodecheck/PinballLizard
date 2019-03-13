
public class CityBugEaten : ICityEvent
{
    public PlayerBase PlayerInfo { get; set; }
    public CityBase CityInfo { get; set; }
    public float BugLoactionX { get; set; }
    public float BugLoactionY { get; set; }
    public float BugLoactionZ { get; set; }
    public int ActiveBug { get; set; }
}

