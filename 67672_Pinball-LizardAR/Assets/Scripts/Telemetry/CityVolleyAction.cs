
public class CityVolleyAction : ICityEvent
{
    public CityBase CityInfo { get; set; }
    public PlayerBase PlayerInfo { get; set; }
    public int ActiveBug { get; set; }
    public string VolleyAction { get; set; }
    public int VolleySequence { get; set; }
    public string VolleySource { get; set; }
}

