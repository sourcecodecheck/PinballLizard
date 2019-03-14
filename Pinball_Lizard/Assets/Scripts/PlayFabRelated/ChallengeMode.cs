using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using UnityEngine;
using Microsoft.AppCenter.Unity.Crashes;

public class ChallengeMode : MonoBehaviour
{
    private void Awake()
    {
        ScoreEvents.OnLoadLeaderBoard += GetLeaderBoard;
        StoreEvents.OnSubtractAnimosity += SubtractAnimosity;
    }

    public void GetChallengeSeed()
    {
        //if we have successfully logged in
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //get the date
            string todayString = DateTime.Today.ToShortDateString();
            //if we haven't gotten the new seed today
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.DailyChallengeTimeStamp) == false || PlayerPrefs.GetString(PlayerPrefsKeys.DailyChallengeTimeStamp) != todayString)
            {
                //runa  cloudscript to get the challenge mode seed
                PlayFabClientAPI.ExecuteCloudScript(
                   new ExecuteCloudScriptRequest()
                   {
                       FunctionName = "getChallengeSeed"
                   },
                   (result) =>
                   {
                       if (result.FunctionResult != null)
                       {
                           //retrieve random seed and store it along with the time we retrieved it
                           int randomResult =
                           PlayFabSimpleJson.DeserializeObject<int>(
                               PlayFabSimpleJson.SerializeObject(((JsonObject)result.FunctionResult)[0]));
                           PlayerPrefs.SetInt(PlayerPrefsKeys.DailyChallengeSeed, randomResult);
                           PlayerPrefs.SetString(PlayerPrefsKeys.DailyChallengeTimeStamp, DateTime.Today.ToShortDateString());
                           PlayerPrefs.Save();
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
    }

    public void GetLeaderBoard()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //retrieve top ten in leaderboard
            PlayFabClientAPI.GetLeaderboard(
                new GetLeaderboardRequest()
                {
                    StartPosition = 0,
                    MaxResultsCount = 10,
                    StatisticName = "Daily Challenge"
                },
                (result) =>
                {
                    //notify leaderboard object that it has been retrieved so we can update it
                    ScoreEvents.SendLeaderBoardRetrieved(result.Leaderboard);
                    GetChallengeTimeEnd();
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

    public void SubtractAnimosity(int animosityToSubtract)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //run cloud script to subract one point of animosity
            PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "removeAnimosity",
                   FunctionParameter = new { animosity = animosityToSubtract }
               },
               (result) =>
               {
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

    public void GetChallengeTimeEnd()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //get time challengemode resets
            PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "getChallengeChangeTime"
               },
               (result) =>
               {
                   if (result.FunctionResult != null)
                   {
                       DateTime change = DateTime.Parse(((JsonObject)result.FunctionResult)[0] as string);
                       MenuEvents.SendChallengeModeEndRetrieved(change);
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


        private void OnDestroy()
    {
        ScoreEvents.OnLoadLeaderBoard -= GetLeaderBoard;
        StoreEvents.OnSubtractAnimosity -= SubtractAnimosity;
    }
}
