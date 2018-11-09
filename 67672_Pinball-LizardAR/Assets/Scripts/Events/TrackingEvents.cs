using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TrackingEvents
{
    public delegate void BugEaten();
    public static event BugEaten OnBugEaten;
    public static void SendBugEaten()
    {
        OnBugEaten();
    }

    public delegate void BuildingDestroyed();
    public static event BuildingDestroyed OnBuildingDestroyed;
    public static void SendBuildingDestroyed()
    {
        OnBuildingDestroyed();
    }

    public delegate void CityGenerated(int numBuildings);
    public static event CityGenerated OnCityGenerated;
    public static void SendCityGenerated(int numBuildings)
    {
        OnCityGenerated(numBuildings);
    }

    public delegate void GameVictory(int score, int numbugsEaten);
    public static event GameVictory OnGameVictory;
    public static void SendGameVictory(int score, int numbugsEaten)
    {
        OnGameVictory(score, numbugsEaten);
    }
}
