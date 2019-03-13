
public class CitySessionEnd : ICityEvent
{
    public PlayerBase PlayerInfo { get; set; }
    public CityBase CityInfo { get; set; }
    public string ExitType { get; set; }
    public int CitySessionDuration { get; set; }
    public int FinalScore { get; set; }
    public float HighestCombo { get; set; }
    public int PowerUpsUsed { get; set; }
}
