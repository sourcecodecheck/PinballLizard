using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class UserInfo : MonoBehaviour
{
    public Inventory PlayerInventory;
    public string LevelKey;
    public string ExperienceKey;
    public string BugsEatenKey;
    public string BestScoreKey;

    private ChallengeMode challengeMode;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateUserDataFromPlayFab", 0.5f, 5.0f);
        TrackingEvents.OnGameVictory += GameVictory;
        challengeMode = new ChallengeMode();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateUserDataFromPlayFab()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "getPlayerStatistics"
               },
               (result) =>
               {
                   UpdateInventory((JsonObject)result.FunctionResult);
               },
               (error) =>
               {
               });
        }
    }

    public void SubmitScoreToPlayFab()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "submitScore",
                    FunctionParameter = new { score = PlayerInventory.LastGameScore,
                        bugsEaten = PlayerInventory.BugsEatenCount } 
                }, 
                (result) => 
                {
                    UpdateInventory(result.FunctionResult as JsonObject);
                },
                (error) => 
                {
                });
        }
    }
    public void GameVictory(int score, int bugsEaten)
    {
        PlayerInventory.LastGameScore = score;
        PlayerInventory.BugsEatenCount += bugsEaten;
        if (PlayerPrefs.HasKey("ischallenge") && PlayerPrefs.GetInt("ischallenge") == 1)
        {
            challengeMode.SendScore(PlayerInventory.LastGameScore, PlayerInventory.BugsEatenCount);
        }
        else
        {
            SubmitScoreToPlayFab();
        }
    }

    private void SetUpNewPlayer()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "initializePlayer"
                },
                (result) =>
                {
                    UpdateInventory(result.FunctionResult as JsonObject);
                },
                (error) =>
                {
                });
        }
    }

    private void UpdateInventory(JsonObject inventoryResult)
    {
        if(inventoryResult!=null)
        {
            JsonObject statistics = inventoryResult["Statistics"] as JsonObject;
            foreach(JsonObject stat in statistics.Values)
            {
                string statName = stat[0] as string;
                int value = (int)stat[1];
                switch( statName)
                {
                    case "Best Score":
                        PlayerInventory.BestScore = value;
                        break;
                    case "Bugs Eaten":
                        PlayerInventory.BugsEatenCount = value;
                        break;
                    case "Level":
                        PlayerInventory.PlayerLevel = value;
                        break;
                }
            }
        }
        else
        {
            SetUpNewPlayer();
        }
    }
    private void OnDestroy()
    {
        TrackingEvents.OnGameVictory -= GameVictory;
    }
}
