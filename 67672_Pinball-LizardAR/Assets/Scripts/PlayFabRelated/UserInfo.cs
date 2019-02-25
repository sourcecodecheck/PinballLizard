using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using Microsoft.AppCenter.Unity.Crashes;


public class UserInfo : MonoBehaviour
{
    public Inventory PlayerInventory;
    public string LevelKey;
    public string ExperienceKey;
    public string BugsEatenKey;
    public string BestScoreKey;

    void Start()
    {
        TrackingEvents.OnGameVictory += GameEnd;
        TrackingEvents.OnGameDefeat += GameEnd;
        TrackingEvents.OnLoadPlayerInfo += UpdateUserDataFromPlayFab;
    }

    void Update()
    {

    }

    void UpdateUserDataFromPlayFab()
    {
        //if we've logged in
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //get stats from custom cloud script
            PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "getPlayerStatistics"
               },
               (result) =>
               {
                   UpdateInventory((JsonObject)result.FunctionResult);
                   GetNextFiveLevels();
                   GetFirstLoginTime();
               },
               (error) =>
               {
                   Debug.Log(error);
               });
        }
    }

    void GetFirstLoginTime()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //get first login time from cloud script
            PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "getFirstLogin"
               },
               (result) =>
               {
                   string creationTime = result.FunctionResult as string;
                   PlayerInventory.DateJoined = DateTime.Parse(creationTime);
               },
               (error) =>
               {
                   Debug.Log(error);
               });
        }
    }

    public void SubmitScoreToPlayFab(bool isChallenge)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "submitScore",
                    FunctionParameter = new
                    {
                        score = PlayerInventory.LastGameScore,
                        bugsEaten = PlayerInventory.BugsEatenCount,
                        bestCombo = PlayerInventory.BestCombo * 10f,
                        isEvent = isChallenge
                    }
                },
                (result) =>
                {
                    GetNextFiveLevels();
                    UpdateInventory((JsonObject)result.FunctionResult);
                    GetFirstLoginTime();
                },
                (error) =>
                {
                    Debug.Log(error);
                });
        }
    }

    public void GetNextFiveLevels()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "getUpcomingExperienceRequirements",
                    FunctionParameter = new
                    {
                        level = PlayerInventory.PlayerLevel,
                        count = 5
                    }
                },
                (result) =>
                {
                    PlayerInventory.ExperienceToNextLevel.Clear();
                    JsonObject response = result.FunctionResult as JsonObject;
                    JsonObject requirements = response[0] as JsonObject;
                    for (int i = 0; i < 5; ++i)
                    {
                        PlayerInventory.ExperienceToNextLevel.Add(
                            PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(requirements[i])));
                    }
                },
                (error) =>
                {
                    Debug.Log(error);
                });
        }
    }

    public void GameEnd(int score, int bugsEaten, float maxMultiplier)
    {
        PlayerInventory.LastGameScore = score;
        PlayerInventory.BugsEatenCount += bugsEaten;
        PlayerInventory.BestCombo = maxMultiplier;
        bool isChallenge = PlayerPrefs.GetInt("ischallenge") == 1;
        SubmitScoreToPlayFab(isChallenge);

    }

    private void SetUpNewPlayer()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //initialize statistics to default values
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "initializePlayer"
                },
                (result) =>
                {
                    UpdateInventory((JsonObject)result.FunctionResult);
                    GetNextFiveLevels();
                    GetFirstLoginTime();
                },
                (error) =>
                {
                    Debug.Log(error);
                });
        }
    }


    private void UpdateInventory(JsonObject inventoryResult)
    {
        //update internal inventory object with the inventory results
        if (inventoryResult != null)
        {
            JsonObject statistics = inventoryResult["Statistics"] as JsonObject;
            foreach (JsonObject stat in statistics.Values)
            {
                string statName = stat[0] as string;
                switch (statName)
                {
                    case "Best Score":
                        PlayerInventory.BestScore = PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(stat[1]));
                        break;
                    case "Bugs Eaten":
                        PlayerInventory.BugsEatenCount = PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(stat[1]));
                        break;
                    case "Level":
                        PlayerInventory.PlayerLevel = PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(stat[1]));
                        break;
                    case "Experience":
                        PlayerInventory.PreviousExperienceCount = PlayerInventory.ExperienceCount;
                        PlayerInventory.ExperienceCount = PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(stat[1]));
                        break;
                    case "Daily Challenge":
                        break;
                    case "Best Combo":
                        PlayerInventory.BestCombo = PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(stat[1]));
                        break;
                }
            }
            MenuEvents.SendUpdateLevelDisplay();
        }
        else
        {
            SetUpNewPlayer();
        }
    }
    private void OnDestroy()
    {
        TrackingEvents.OnGameVictory -= GameEnd;
        TrackingEvents.OnGameDefeat -= GameEnd;
        TrackingEvents.OnLoadPlayerInfo -= UpdateUserDataFromPlayFab;
    }
}
