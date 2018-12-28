using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public int ScoreUnit;
    public float DefaultMultiplier;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public Canvas UIParent;

    private int gameScore;
    private float gameMultiplier;
    private float highestMultiplier;
    private bool resetMultiplier;
    private bool isFeastActive;
    private int bugsEatenThisGame;
    private int buildingCount;
    private int appetiteMax;
    private int appetiteCurrent;
    void Awake()
    {
        appetiteMax = 10;
        appetiteCurrent = appetiteMax;
        gameScore = 0;
        gameMultiplier = 1.0f;
        bugsEatenThisGame = 0;
        highestMultiplier = 1.0f;
        ScoreEvents.OnAddScore += AddScore;
        ScoreEvents.OnSetMultiplier += ChangeMultiplier;
        ScoreEvents.OnAddMultiplier += AddMultiplier;
        TrackingEvents.OnBugEaten += AddBug;
        TrackingEvents.OnCityGenerated += CityGenerated;
        TrackingEvents.OnBuildingDestroyed += BuildingDestroyed;
        GamePlayEvents.OnFeastStart += FeastStart;
        GamePlayEvents.OnFeastEnd += FeastEnd;
        GamePlayEvents.OnBombDetonated += BombBonus;
        AnimationEvents.OnHandsExited += DefeatCheck;
    }



    private void Start()
    {
        GamePlayEvents.SendUpdateAppetite(appetiteCurrent, appetiteMax);
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
        highestMultiplier = Mathf.Max(highestMultiplier, gameMultiplier);
    }
    public void AddMultiplier(float multiplier)
    {
        if (isFeastActive)
        {
            gameMultiplier += multiplier * 2;
        }
        else
        {
            gameMultiplier += multiplier;
        }
        highestMultiplier = Mathf.Max(highestMultiplier, gameMultiplier);
    }

    public void AddBug()
    {
        ++bugsEatenThisGame;
        --appetiteCurrent;
        if (appetiteCurrent <= 0)
        {
            appetiteCurrent = 0;
        }
        GamePlayEvents.SendUpdateAppetite(appetiteCurrent, appetiteMax);
    }

    public void DefeatCheck()
    {
        if (appetiteCurrent <= 0 && buildingCount > 0)
        {
            appetiteCurrent = 0;
            DefeatScreen.SetActive(true);
            Invoke("Defeat", 0.2f);
        }
    }

    public void BuildingDestroyed()
    {
        --buildingCount;
        if (buildingCount <= 0)
        {
            VictoryScreen.SetActive(true);
            Invoke("Victory", 0.2f);
        }
    }
    public void FeastStart()
    {
        isFeastActive = true;
    }
    public void FeastEnd()
    {
        isFeastActive = false;
    }
    private void BombBonus()
    {
        gameScore += 20000;
    }

    public void CityGenerated(int numBuildings)
    {
        buildingCount = numBuildings;
    }

    private void Victory()
    {
        TrackingEvents.SendGameVictory(gameScore, bugsEatenThisGame, highestMultiplier);
    }

    private void Defeat()
    {
        TrackingEvents.SendGameDefeat(gameScore, bugsEatenThisGame, highestMultiplier);
    }

    private void OnDestroy()
    {
        ScoreEvents.OnAddScore -= AddScore;
        ScoreEvents.OnSetMultiplier -= ChangeMultiplier;
        ScoreEvents.OnAddMultiplier -= AddMultiplier;
        TrackingEvents.OnBugEaten -= AddBug;
        TrackingEvents.OnCityGenerated -= CityGenerated;
        TrackingEvents.OnBuildingDestroyed -= BuildingDestroyed;
        GamePlayEvents.OnFeastStart -= FeastStart;
        GamePlayEvents.OnFeastEnd -= FeastEnd;
        AnimationEvents.OnHandsExited -= DefeatCheck;
    }
}
