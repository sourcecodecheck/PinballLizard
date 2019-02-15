using System.Collections.Generic;
public static class TrackingEvents
{
    public delegate void BugEaten();
    public static event BugEaten OnBugEaten;
    public static void SendBugEaten()
    {
        OnBugEaten?.Invoke();
    }

    public delegate void BuildingDestroyed();
    public static event BuildingDestroyed OnBuildingDestroyed;
    public static void SendBuildingDestroyed()
    {
        OnBuildingDestroyed?.Invoke();
    }

    public delegate void CityGenerated(int numBuildings);
    public static event CityGenerated OnCityGenerated;
    public static void SendCityGenerated(int numBuildings)
    {
        OnCityGenerated?.Invoke(numBuildings);
    }

    public delegate void GameVictory(int score, int numbugsEaten, float maxMultiplier);
    public static event GameVictory OnGameVictory;
    public static void SendGameVictory(int score, int numbugsEaten, float maxMultiplier)
    {
        OnGameVictory?.Invoke(score, numbugsEaten, maxMultiplier);
    }

    public delegate void GameDefeat(int score, int numbugsEaten, float maxMultiplier);
    public static event GameDefeat OnGameDefeat;
    public static void SendGameDefeat(int score, int numbugsEaten, float maxMultiplier)
    {
        OnGameDefeat?.Invoke(score, numbugsEaten, maxMultiplier);
    }

    public delegate void LoadPlayerInfo();
    public static event LoadPlayerInfo OnLoadPlayerInfo;
    public static void SendLoadPlayerInfo()
    {
        OnLoadPlayerInfo?.Invoke();
    }

    public delegate void PlayFabPlayerEvent(Dictionary<string, object> eventBody, string eventTitle);
    public static event PlayFabPlayerEvent OnPlayFabPlayerEvent;
    public static void SendPlayFabPlayerEvent(Dictionary<string, object> eventBody, string eventTitle)
    {
        OnPlayFabPlayerEvent?.Invoke(eventBody, eventTitle);
    }

    public delegate void PlayFabTitleEvent(Dictionary<string, object> eventBody, string eventTitle);
    public static event PlayFabTitleEvent OnPlayFabTitleEvent;
    public static void SendPlayFabTitleEvent(Dictionary<string, object> eventBody, string eventTitle)
    {
        OnPlayFabTitleEvent?.Invoke(eventBody, eventTitle);
    }
}
