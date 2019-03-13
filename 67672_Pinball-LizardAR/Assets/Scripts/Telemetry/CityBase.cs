

public class CityBase
{
    public string ClientSessionID { get; set; }
    public string CitySessionID { get; set; }
    public float PlayerLocationX { get; set; }
    public float PlayerLoactionY { get; set; }
    public float PlayerLoactionZ { get; set; }
    public bool ARMode { get; set; }
    public bool IsChallengeMode { get; set; }
    public int TotalBuildingCount { get; set; }
    public int CurrentBuildingCount { get; set; }
    public int RemainingBugs { get; set; }

}
