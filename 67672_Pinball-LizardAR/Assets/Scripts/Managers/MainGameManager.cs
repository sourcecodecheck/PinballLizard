using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public int ScoreUnit;
    public int AppetiteMax;
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
    
    private int appetiteCurrent;
    private bool gameStarted;
    private bool gameEnded;
    private bool daBombDetonated;

    void Awake()
    {
        gameStarted = false;
        gameEnded = false;
        daBombDetonated = false;
        appetiteCurrent = AppetiteMax;
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
        GamePlayEvents.SendUpdateAppetite(appetiteCurrent, AppetiteMax);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if (gameStarted == true && gameEnded == false && daBombDetonated == false)
        {
            if (buildingCount <= 0)
            {
                VictoryScreen.SetActive(true);
                Invoke("Victory", 0.2f);
                gameEnded = true;
            }
            else if (appetiteCurrent <= 0)
            {
                gameEnded = true;
                appetiteCurrent = 0;
                DefeatScreen.SetActive(true);
                Invoke("Defeat", 0.2f);
            }
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
        GamePlayEvents.SendUpdateAppetite(appetiteCurrent, AppetiteMax);
    }

    public void DefeatCheck()
    {
        if (appetiteCurrent <= 0 && buildingCount > 0 && gameEnded == false && daBombDetonated == false)
        {
            gameEnded = true;
            appetiteCurrent = 0;
            DefeatScreen.SetActive(true);
            Invoke("Defeat", 0.2f);
        }
    }

    public void BuildingDestroyed()
    {
        --buildingCount;
        if (buildingCount <= 0 && gameEnded == false && daBombDetonated == false)
        {
            gameEnded = true;
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

    public void CityGenerated(int numBuildings)
    {
        gameStarted = true;
        buildingCount = numBuildings;
        StoreEvents.SendLoadCurrencies();
        TrackingEvents.SendLoadPlayerInfo();
    }
    private void BombBonus()
    {
        if (daBombDetonated == false)
        {
            daBombDetonated = true;
            gameScore += 20000;
            Invoke("VictoryBomb", 2.0f);
        }
    }

    private void VictoryBomb()
    {
        gameEnded = true;
        VictoryScreen.SetActive(true);
        Invoke("Victory", 0.2f);
    }

    private void Victory()
    {
        gameScore += 1000 * appetiteCurrent;
        PlayerPrefs.SetInt(PlayerPrefsKeys.ConsecutiveLosses, 0);
        TrackingEvents.SendGameVictory(gameScore, bugsEatenThisGame, highestMultiplier);
    }

    private void Defeat()
    {
        int losses = 1;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.ConsecutiveLosses))
        {
            losses = PlayerPrefs.GetInt(PlayerPrefsKeys.ConsecutiveLosses) + 1;
        }
        if (losses >= 3)
        {
            //StoreEvents.SendAwardItemOnLoss();
            losses = 0;
        }
        PlayerPrefs.SetInt(PlayerPrefsKeys.ConsecutiveLosses, losses);

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
        GamePlayEvents.OnBombDetonated -= BombBonus;
    }
}
