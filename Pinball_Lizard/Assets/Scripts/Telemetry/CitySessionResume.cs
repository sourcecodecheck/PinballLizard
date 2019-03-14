
public class CitySessionResume : ICityEvent
{
    public PlayerBase PlayerInfo { get; set; }
    public CityBase CityInfo { get; set; }
    public int CityPauseDuration { get; set; }
}

