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

    public delegate void GameVictory(int score, int numbugsEaten, float maxMultiplier, bool isWin = true);
    public static event GameVictory OnGameVictory;
    public static void SendGameVictory(int score, int numbugsEaten, float maxMultiplier)
    {
        OnGameVictory?.Invoke(score, numbugsEaten, maxMultiplier);
    }

    public delegate void GameDefeat(int score, int numbugsEaten, float maxMultiplier, bool isWin = false);
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

    public delegate void AddExperience(int numBuildings);
    public static event AddExperience OnAddExperience;
    public static void SendAddExperience(int experienceToAdd)
    {
        OnAddExperience?.Invoke(experienceToAdd);
    }

    public delegate void BuildCityEvent(ICityEvent cityEvent, string name);
    public static event BuildCityEvent OnBuildCityEvent;
    public static void SendBuildCityEvent(ICityEvent cityEvent, string name)
    {
        OnBuildCityEvent?.Invoke(cityEvent, name);
    }

    public delegate void BuildBuildingDestroyedStep2(CityBuildingDestroyed buildingDestroyed, string name);
    public static event BuildBuildingDestroyedStep2 OnBuildBuildingDestroyedStep2;
    public static void SendBuildingDestroyedStep2(CityBuildingDestroyed buildingDestroyed, string name)
    {
        OnBuildBuildingDestroyedStep2?.Invoke(buildingDestroyed, name);
    }

    public delegate void BuildBugEatenStep2(CityBugEaten bugEaten, string name);
    public static event BuildBugEatenStep2 OnBuildBugEatenStep2;
    public static void SendBuildBugEatenStep2(CityBugEaten bugEaten, string name)
    {
        OnBuildBugEatenStep2?.Invoke(bugEaten, name);
    }

    public delegate void BuildVolleyActionStep2(CityVolleyAction volleyAction, string name);
    public static event BuildVolleyActionStep2 OnBuildVolleyActionStep2;
    public static void SendBuildVolleyActionStep2(CityVolleyAction volleyAction, string name)
    {
        OnBuildVolleyActionStep2?.Invoke(volleyAction, name);
    }

    public delegate void BuildSessionEndStep2(CitySessionEnd sessionEnd, string name);
    public static event BuildSessionEndStep2 OnBuildSessionEndStep2;
    public static void SendBuildSessionEndStep2(CitySessionEnd sessionEnd, string name)
    {
        OnBuildSessionEndStep2?.Invoke(sessionEnd, name);
    }

    public delegate void BuildPlayerEvent(IPlayerEvent playerEvent, string name);
    public static event BuildPlayerEvent OnBuildPlayerEvent;
    public static void SendBuildPlayerEvent(IPlayerEvent playerEvent, string name)
    {
        OnBuildPlayerEvent?.Invoke(playerEvent, name);
    }

    public delegate void QueueEvent(IPlayerEvent playerEvent, string name);
    public static event QueueEvent OnQueueEvent;
    public static void SendQueueEvent(IPlayerEvent playerEvent, string name)
    {
        OnQueueEvent?.Invoke(playerEvent, name);
    }
}
