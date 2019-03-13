using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public int ScoreUnit;
    public int AppetiteMax;
    public float DefaultMultiplier;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public Canvas GameplayCanvas;
    public Canvas GameEndCanvas;
    public bool IsAR;

    private int gameScore;
    private float gameMultiplier;
    private float highestMultiplier;
    private bool resetMultiplier;
    private bool isFeastActive;
    private int bugsEatenThisGame;
    private int buildingCount;
    private int totalBuildingCount;
    private int appetiteCurrent;
    private int powerUpCount;
    private bool gameStarted;
    private bool gameEnded;
    private bool daBombDetonated;

    private System.Guid cityID;
    private System.DateTime startTime;

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
        powerUpCount = 0;
        ScoreEvents.OnAddScore += AddScore;
        ScoreEvents.OnSetMultiplier += ChangeMultiplier;
        ScoreEvents.OnAddMultiplier += AddMultiplier;
        TrackingEvents.OnBugEaten += AddBug;
        TrackingEvents.OnCityGenerated += CityGenerated;
        TrackingEvents.OnBuildingDestroyed += BuildingDestroyed;
        GamePlayEvents.OnFeastStart += FeastStart;
        GamePlayEvents.OnFeastEnd += FeastEnd;
        GamePlayEvents.OnBombDetonated += BombBonus;
        GamePlayEvents.OnUsePowerUp += IncrementPowerup;
        AnimationEvents.OnHandsExited += DefeatCheck;
        TrackingEvents.OnBuildCityEvent += BuildCityEvent;
        TrackingEvents.OnBuildBuildingDestroyedStep2 += BuildBuildingDestroyedStep2;
        TrackingEvents.OnBuildBugEatenStep2 += BuildBugEatenStep2;
        TrackingEvents.OnBuildVolleyActionStep2 += BuildVolleyActionStep2;
        TrackingEvents.OnBuildSessionEndStep2 += BuildSessionEndStep2;
    }

    private void IncrementPowerup(string keyTerm)
    {
        ++powerUpCount;
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
        StoreEvents.SendLoadCurrencies();
        TrackingEvents.SendLoadPlayerInfo();
        totalBuildingCount = numBuildings;
        buildingCount = numBuildings;
        GameEndCanvas.gameObject.SetActive(false);
        GameplayCanvas.gameObject.SetActive(true);
        cityID = System.Guid.NewGuid();
        startTime = System.DateTime.Now;
        BuildCityEvent(new CitySessionStart() { }, EventNames.SessionStart);
        MenuEvents.SendSwitchCanvas(GameplayCanvas);
    }
    private void BombBonus(string damageType)
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
        gameScore += 4000 * appetiteCurrent;
        PlayerPrefs.SetInt(PlayerPrefsKeys.ConsecutiveLosses, 0);
        TrackingEvents.SendGameVictory(gameScore, appetiteCurrent, highestMultiplier);
        BuildCityEvent(new CitySessionEnd()
        {
            ExitType = "victory",
            FinalScore = gameScore,
            HighestCombo = highestMultiplier,
            PowerUpsUsed = powerUpCount,
            CitySessionDuration = (int)(DateTime.Now - startTime).TotalSeconds
        }, EventNames.SessionEnd);
        GameplayCanvas.gameObject.SetActive(false);
        MenuEvents.SendSwitchCanvas(GameEndCanvas);
    }

    private void Defeat()
    {
        TrackingEvents.SendGameDefeat(gameScore, appetiteCurrent, highestMultiplier);
        BuildCityEvent(new CitySessionEnd()
        {
            ExitType = "defeat",
            FinalScore = gameScore,
            HighestCombo = highestMultiplier,
            PowerUpsUsed = powerUpCount,
            CitySessionDuration = (int)(DateTime.Now - startTime).TotalSeconds
        }, EventNames.SessionEnd);
        GameplayCanvas.gameObject.SetActive(false);
        MenuEvents.SendSwitchCanvas(GameEndCanvas);
    }
    private void BuildCityEvent(ICityEvent cityEvent, string name)
    {
        cityEvent.CityInfo = new CityBase()
        {
            ARMode = IsAR,
            CitySessionID = cityID.ToString(),
            ClientSessionID = cityID.ToString(),
            IsChallengeMode = PlayerPrefs.GetInt(PlayerPrefsKeys.ChallengeModeSet) == 1,
            CurrentBuildingCount = buildingCount,
            PlayerLocationX = Camera.main.transform.position.x,
            PlayerLoactionY = Camera.main.transform.position.y,
            PlayerLoactionZ = Camera.main.transform.position.z,
            RemainingBugs = appetiteCurrent,
            TotalBuildingCount = totalBuildingCount
        };
        TrackingEvents.SendBuildPlayerEvent(cityEvent, name);
    }
    private void BuildBuildingDestroyedStep2(CityBuildingDestroyed buildingDestroyed, string name)
    {
        buildingDestroyed.ScoreBaseValue *= ScoreUnit;
        buildingDestroyed.ScoreModifier = gameMultiplier;
        buildingDestroyed.ScoreActiveBug = appetiteCurrent;
        BuildCityEvent(buildingDestroyed, name);
    }

    private void BuildBugEatenStep2(CityBugEaten bugEaten, string name)
    {
        bugEaten.ActiveBug = appetiteCurrent;
        BuildCityEvent(bugEaten, name);
    }

    private void BuildVolleyActionStep2(CityVolleyAction volleyAction, string name)
    {
        volleyAction.ActiveBug = appetiteCurrent;
        BuildCityEvent(volleyAction, name);
    }

    private void BuildSessionEndStep2(CitySessionEnd sessionEnd, string name)
    {
        if (startTime != default(DateTime))
        {
            sessionEnd.FinalScore = gameScore;
            sessionEnd.HighestCombo = highestMultiplier;
            sessionEnd.PowerUpsUsed = powerUpCount;
            sessionEnd.CitySessionDuration = (int)(DateTime.Now - startTime).TotalSeconds;
            BuildCityEvent(sessionEnd, name);
        }
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
        GamePlayEvents.OnUsePowerUp -= IncrementPowerup;
        TrackingEvents.OnBuildCityEvent -= BuildCityEvent;
        TrackingEvents.OnBuildBuildingDestroyedStep2 -= BuildBuildingDestroyedStep2;
        TrackingEvents.OnBuildBugEatenStep2 -= BuildBugEatenStep2;
        TrackingEvents.OnBuildVolleyActionStep2 -= BuildVolleyActionStep2;
        TrackingEvents.OnBuildSessionEndStep2 -= BuildSessionEndStep2;
    }
}
