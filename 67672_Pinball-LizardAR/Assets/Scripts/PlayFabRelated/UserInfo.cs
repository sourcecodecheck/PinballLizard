using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using Microsoft.AppCenter.Unity.Crashes;
using System.Collections.Generic;

public class UserInfo : MonoBehaviour
{
    public Inventory PlayerInventory;
    public string LevelKey;
    public string ExperienceKey;
    public string BugsEatenKey;
    public string BestScoreKey;

    void Awake()
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
                   if (result.Error != null)
                   {
                       try
                       {
                           throw new Exception(result.Error.Message);
                       }
                       catch (Exception exception)
                       {
                           Crashes.TrackError(exception);
                       }
                   }
               },
               (error) =>
               {
                   Debug.Log(error);
                   try
                   {
                       throw new Exception(error.ErrorMessage);
                   }
                   catch (Exception exception)
                   {
                       Crashes.TrackError(exception);
                   }
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
                   StoreEvents.SendUpdateInventoryDisplay();
                   if (result.Error != null)
                   {
                       try
                       {
                           throw new Exception(result.Error.Message);
                       }
                       catch (Exception exception)
                       {
                           Crashes.TrackError(exception);
                       }
                   }
               },
               (error) =>
               {
                   Debug.Log(error);
                   try
                   {
                       throw new Exception(error.ErrorMessage);
                   }
                   catch (Exception exception)
                   {
                       Crashes.TrackError(exception);
                   }
               });
        }
    }

    public void SubmitScoreToPlayFab(bool isChallenge, bool isWin)
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
                        bugsEaten = 2,
                        bestCombo = Math.Round(PlayerInventory.BestCombo * 10f),
                        isDailyChallenge = isChallenge,
                        isVictory = isWin
                    }
                },
                (result) =>
                {
                    GetNextFiveLevels();
                    UpdateInventory((JsonObject)result.FunctionResult);
                    GetFirstLoginTime();
                    if(result.Error!=null)
                    {
                        try
                        {
                            throw new Exception(result.Error.Message);
                        }
                        catch (Exception exception)
                        {
                            Crashes.TrackError(exception);
                        }
                    }
                },
                (error) =>
                {
                    Debug.Log(error);
                    try
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                    catch (Exception exception)
                    {
                        Crashes.TrackError(exception);
                    }
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
                        level = PlayerInventory.PlayerLevel > 1 ? PlayerInventory.PlayerLevel - 1 : PlayerInventory.PlayerLevel,
                        count = 5
                    }
                },
                (result) =>
                {
                    if (result.FunctionResult != null)
                    {
                        PlayerInventory.ExperienceToNextLevel.Clear();
                        JsonObject response = result.FunctionResult as JsonObject;
                        JsonObject requirements = response[0] as JsonObject;
                        for (int i = 0; i < 5; ++i)
                        {
                            PlayerInventory.ExperienceToNextLevel.Add(
                                PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(requirements[i])));
                        }
                        StoreEvents.SendUpdateInventoryDisplay();
                    }
                    if (result.Error != null)
                    {
                        try
                        {
                            throw new Exception(result.Error.Message);
                        }
                        catch (Exception exception)
                        {
                            Crashes.TrackError(exception);
                        }
                    }
                },
                (error) =>
                {
                    Debug.Log(error);
                    try
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                    catch (Exception exception)
                    {
                        Crashes.TrackError(exception);
                    }
                });
        }
    }

    public void GameEnd(int score, int bugsEaten, float maxMultiplier, bool isWin)
    {
        PlayerInventory.LastGameScore = score;
        PlayerInventory.BugsEatenCount += bugsEaten;
        PlayerInventory.BestCombo = maxMultiplier;
        bool isChallenge = PlayerPrefs.GetInt(PlayerPrefsKeys.ChallengeModeSet) == 1;
        SubmitScoreToPlayFab(isChallenge, isWin);

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
                    if (result.Error != null)
                    {
                        try
                        {
                            throw new Exception(result.Error.Message);
                        }
                        catch (Exception exception)
                        {
                            Crashes.TrackError(exception);
                        }
                    }
                    UpdateInventory((JsonObject)result.FunctionResult);
                    GetNextFiveLevels();
                    GetFirstLoginTime();
                },
                (error) =>
                {
                    Debug.Log(error);
                    try
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                    catch (Exception exception)
                    {
                        Crashes.TrackError(exception);
                    }
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
                    case "BugsEaten":
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
            if (inventoryResult.ContainsKey("Rewards"))
            {
                JsonArray rewards = inventoryResult["Rewards"] as JsonArray;
                List<ItemInstance> items = new List<ItemInstance>();
                Dictionary<string, uint> currencies = new Dictionary<string, uint>();
                if (rewards != null)
                {
                    foreach (JsonObject reward in rewards)
                    {
                        switch (reward["Type"] as string)
                        {
                            case "chest":
                                StoreEvents.SendOpenContainer(new ItemInstance() { ItemInstanceId = reward["InstanceId"] as string }, PlayerInventory.CatalogVersion);
                                break;
                            case "token":
                            case "powerup":
                                int itemAmount = PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(reward["Amount"]));
                                for (int i = 0; i < itemAmount; ++i)
                                {
                                    ItemInstance item = new ItemInstance()
                                    {
                                        ItemId = reward["Id"] as string,

                                    };
                                    items.Add(item);
                                }
                                break;
                            case "currency":
                                currencies.Add(reward["Id"] as string,
                                    PlayFabSimpleJson.DeserializeObject<uint>(PlayFabSimpleJson.SerializeObject(reward["Amount"])));
                                break;
                        }
                    }
                }
                if (items.Count > 0 || currencies.Count > 0)
                {
                    MenuEvents.SendShowContainerPopUp(items, currencies);
                }
            }
            MenuEvents.SendUpdateLevelDisplay();
        }
        else
        {
            SetUpNewPlayer();
        }

    }
    private IEnumerator<WaitForSeconds> Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void OnDestroy()
    {
        TrackingEvents.OnGameVictory -= GameEnd;
        TrackingEvents.OnGameDefeat -= GameEnd;
        TrackingEvents.OnLoadPlayerInfo -= UpdateUserDataFromPlayFab;
    }
}
