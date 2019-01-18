using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using UnityEngine;

public class ChallengeMode : MonoBehaviour
{
    private void Start()
    {
        ScoreEvents.OnLoadLeaderBoard += GetLeaderBoard;
        StoreEvents.OnSubtractAnimosity += SubtractAnimosity;
    }

    public void GetChallengeSeed()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            string todayString = DateTime.Today.ToShortDateString();
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.DailyChallengeTimeStamp) == false || PlayerPrefs.GetString(PlayerPrefsKeys.DailyChallengeTimeStamp) != todayString)
            {
                PlayFabClientAPI.ExecuteCloudScript(
                   new ExecuteCloudScriptRequest()
                   {
                       FunctionName = "getChallengeSeed"
                   },
                   (result) =>
                   {
                       int randomResult =
                       PlayFabSimpleJson.DeserializeObject<int>(
                           PlayFabSimpleJson.SerializeObject(((JsonObject)result.FunctionResult)[0]));
                       PlayerPrefs.SetInt(PlayerPrefsKeys.DailyChallengeSeed, randomResult);
                       PlayerPrefs.SetString(PlayerPrefsKeys.DailyChallengeTimeStamp, DateTime.Today.ToShortDateString());
                       PlayerPrefs.Save();
                   },
                   (error) =>
                   {
                   });
            }
        }
    }

    public void GetLeaderBoard()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            PlayFabClientAPI.GetLeaderboard(
                new GetLeaderboardRequest()
                {
                    StartPosition = 0,
                    MaxResultsCount = 10,
                    StatisticName = "Daily Challenge"
                },
                (result) =>
                {
                    ScoreEvents.SendLeaderBoardRetrieved(result.Leaderboard);
                },
                (error) =>
                {
                    Debug.Log(error);
                });
        }
    }

    public void SubtractAnimosity(int animosityToSubtract)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
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
               });
        }
    }

    private void OnDestroy()
    {
        ScoreEvents.OnLoadLeaderBoard -= GetLeaderBoard;
        StoreEvents.OnSubtractAnimosity -= SubtractAnimosity;
    }
}
