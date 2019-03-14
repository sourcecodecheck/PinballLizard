using System.Collections.Generic;
public static class TrackingEvents
{
    //Subscribers:
    //MainGameManager
    public delegate void BugEaten();
    public static event BugEaten OnBugEaten;
    public static void SendBugEaten()
    {
        OnBugEaten?.Invoke();
    }

    //Subscribers:
    //MainGameManager
    public delegate void BuildingDestroyed();
    public static event BuildingDestroyed OnBuildingDestroyed;
    public static void SendBuildingDestroyed()
    {
        OnBuildingDestroyed?.Invoke();
    }

    //Subscribers:
    //MainGameManager
    public delegate void CityGenerated(int numBuildings);
    public static event CityGenerated OnCityGenerated;
    public static void SendCityGenerated(int numBuildings)
    {
        OnCityGenerated?.Invoke(numBuildings);
    }

    //Subscribers:
    //GameEndDisplay
    //UserInfo
    public delegate void GameVictory(int score, int numbugsEaten, float maxMultiplier, bool isWin = true);
    public static event GameVictory OnGameVictory;
    public static void SendGameVictory(int score, int numbugsEaten, float maxMultiplier)
    {
        OnGameVictory?.Invoke(score, numbugsEaten, maxMultiplier);
    }

    //Subscribers:
    //GameEndDisplay
    //UserInfo
    public delegate void GameDefeat(int score, int numbugsEaten, float maxMultiplier, bool isWin = false);
    public static event GameDefeat OnGameDefeat;
    public static void SendGameDefeat(int score, int numbugsEaten, float maxMultiplier)
    {
        OnGameDefeat?.Invoke(score, numbugsEaten, maxMultiplier);
    }

    //Subscribers:
    //UserInfo
    public delegate void LoadPlayerInfo();
    public static event LoadPlayerInfo OnLoadPlayerInfo;
    public static void SendLoadPlayerInfo()
    {
        OnLoadPlayerInfo?.Invoke();
    }

    //Subscribers:
    public delegate void PlayFabPlayerEvent(Dictionary<string, object> eventBody, string eventTitle);
    public static event PlayFabPlayerEvent OnPlayFabPlayerEvent;
    public static void SendPlayFabPlayerEvent(Dictionary<string, object> eventBody, string eventTitle)
    {
        OnPlayFabPlayerEvent?.Invoke(eventBody, eventTitle);
    }

    //Subscribers:
    public delegate void PlayFabTitleEvent(Dictionary<string, object> eventBody, string eventTitle);
    public static event PlayFabTitleEvent OnPlayFabTitleEvent;
    public static void SendPlayFabTitleEvent(Dictionary<string, object> eventBody, string eventTitle)
    {
        OnPlayFabTitleEvent?.Invoke(eventBody, eventTitle);
    }

    //Subscribers:
    public delegate void AddExperience(int numBuildings);
    public static event AddExperience OnAddExperience;
    public static void SendAddExperience(int experienceToAdd)
    {
        OnAddExperience?.Invoke(experienceToAdd);
    }

    //Subscribers:
    //MainGameManager
    public delegate void BuildCityEvent(ICityEvent cityEvent, string name);
    public static event BuildCityEvent OnBuildCityEvent;
    public static void SendBuildCityEvent(ICityEvent cityEvent, string name)
    {
        OnBuildCityEvent?.Invoke(cityEvent, name);
    }

    //Subscribers:
    //MainGameManager
    public delegate void BuildBuildingDestroyedStep2(CityBuildingDestroyed buildingDestroyed, string name);
    public static event BuildBuildingDestroyedStep2 OnBuildBuildingDestroyedStep2;
    public static void SendBuildingDestroyedStep2(CityBuildingDestroyed buildingDestroyed, string name)
    {
        OnBuildBuildingDestroyedStep2?.Invoke(buildingDestroyed, name);
    }

    //Subscribers:
    //MainGameManager
    public delegate void BuildBugEatenStep2(CityBugEaten bugEaten, string name);
    public static event BuildBugEatenStep2 OnBuildBugEatenStep2;
    public static void SendBuildBugEatenStep2(CityBugEaten bugEaten, string name)
    {
        OnBuildBugEatenStep2?.Invoke(bugEaten, name);
    }

    //Subscribers:
    //MainGameManager
    public delegate void BuildVolleyActionStep2(CityVolleyAction volleyAction, string name);
    public static event BuildVolleyActionStep2 OnBuildVolleyActionStep2;
    public static void SendBuildVolleyActionStep2(CityVolleyAction volleyAction, string name)
    {
        OnBuildVolleyActionStep2?.Invoke(volleyAction, name);
    }

    //Subscribers:
    //MainGameManager
    public delegate void BuildSessionEndStep2(CitySessionEnd sessionEnd, string name);
    public static event BuildSessionEndStep2 OnBuildSessionEndStep2;
    public static void SendBuildSessionEndStep2(CitySessionEnd sessionEnd, string name)
    {
        OnBuildSessionEndStep2?.Invoke(sessionEnd, name);
    }

    //Subscribers:
    //Inventory
    public delegate void BuildPlayerEvent(IPlayerEvent playerEvent, string name);
    public static event BuildPlayerEvent OnBuildPlayerEvent;
    public static void SendBuildPlayerEvent(IPlayerEvent playerEvent, string name)
    {
        OnBuildPlayerEvent?.Invoke(playerEvent, name);
    }

    //Subscribers:
    //Events
    public delegate void QueueEvent(IPlayerEvent playerEvent, string name);
    public static event QueueEvent OnQueueEvent;
    public static void SendQueueEvent(IPlayerEvent playerEvent, string name)
    {
        OnQueueEvent?.Invoke(playerEvent, name);
    }
}
