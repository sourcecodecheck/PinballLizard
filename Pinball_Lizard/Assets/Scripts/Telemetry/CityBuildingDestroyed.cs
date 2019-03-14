
public class CityBuildingDestroyed : ICityEvent
{
    public PlayerBase PlayerInfo { get; set; }
    public CityBase CityInfo { get; set; }
    public string DamageType { get; set; }
    public float DamageLoactionX { get; set; }
    public float DamageLoactionY { get; set; }
    public float DamageLoactionZ { get; set; }
    public int ScoreBaseValue { get; set; }
    public float ScoreModifier { get; set; }
    public int ScoreActiveBug { get; set; }
}
