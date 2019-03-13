
public class CityVolleyAction : ICityEvent
{
    public CityVolleyAction()
    {
        VolleyCounter.AddVolley();
        VolleySequence = VolleyCounter.VolleyCount;
    }
    public CityBase CityInfo { get; set; }
    public PlayerBase PlayerInfo { get; set; }
    public int ActiveBug { get; set; }
    public string VolleyAction { get; set; }
    public int VolleySequence { get; private set; }
    public string VolleySource { get; set; }
}

public static class VolleyCounter
{
    public static int VolleyCount { get; private set; }
    public static void AddVolley() { ++VolleyCount; }
    public static void ResetVolleyCount() { VolleyCount = 0; }
}

