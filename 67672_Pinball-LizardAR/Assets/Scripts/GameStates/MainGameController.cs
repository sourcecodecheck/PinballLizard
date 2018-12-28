using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    public int ScoreUnit;
    public float DefaultMultiplier;
    public GameObject VictoryScreen;
    public Canvas UIParent;

    private int gameScore;
    private float gameMultiplier;
    private bool resetMultiplier;
    private int bugsEatenThisGame;
    private int buildingCount;
    void Start()
    {
        gameScore = 0;
        gameMultiplier = 1.0f;
        bugsEatenThisGame = 0;
        ScoreEvents.OnAddScore += AddScore;
        ScoreEvents.OnSetMultiplier += ChangeMultiplier;
        ScoreEvents.OnAddMultiplier += AddMultiplier;
        TrackingEvents.OnBugEaten += AddBug;
        TrackingEvents.OnCityGenerated += CityGenerated;
        TrackingEvents.OnBuildingDestroyed += BuildingDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void AddScore(int scoreUnits)
    {
        gameScore += (int)(scoreUnits * ScoreUnit * gameMultiplier);
        ScoreEvents.SendScoreUpdated(gameScore);
    }

    public void ChangeMultiplier(float multiplier)
    {
        gameMultiplier = multiplier;
    }
    public void AddMultiplier(float multiplier)
    {
        gameMultiplier = multiplier;
    }

    public void AddBug()
    {
        ++bugsEatenThisGame;
    }
    public void BuildingDestroyed()
    {
        --buildingCount;
        if(buildingCount <=0)
        {
            Victory();
        }
    }

    public void CityGenerated(int numBuildings)
    {
        buildingCount = numBuildings;
    }

    private void Victory()
    {
        Instantiate(VictoryScreen, UIParent.transform);
        TrackingEvents.SendGameVictory(gameScore, bugsEatenThisGame);
    }

    private void OnDestroy()
    {
        ScoreEvents.OnAddScore -= AddScore;
        ScoreEvents.OnSetMultiplier -= ChangeMultiplier;
        ScoreEvents.OnAddMultiplier -= AddMultiplier;
        TrackingEvents.OnBugEaten -= AddBug;
        TrackingEvents.OnCityGenerated -= CityGenerated;
        TrackingEvents.OnBuildingDestroyed -= BuildingDestroyed;
    }
}
