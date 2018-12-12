using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChallengeMode
{
    public void GetChallengeSeed()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            string todayString = DateTime.Today.ToShortDateString();
            if (PlayerPrefs.HasKey("dailychallengeretrieval") == false || PlayerPrefs.GetString("dailychallengeretrieval") != todayString)
            {
                PlayFabClientAPI.ExecuteCloudScript(
                   new ExecuteCloudScriptRequest()
                   {
                       FunctionName = "getRandom"
                   },
                   (result) =>
                   {
                       int randomResult = (int)((JsonObject)result.FunctionResult)[0];
                       PlayerPrefs.SetInt("dailychallenge", randomResult);
                       PlayerPrefs.SetString("dailychallengeretrieval", DateTime.Today.ToShortDateString());
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
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                new GetLeaderboardAroundPlayerRequest()
                {
                    MaxResultsCount = 100,
                    StatisticName = "Daily Challenge",
                    Version = 1
                },
                (result) =>
                {
                    //ScoreEvents.SendLeaderBoardRetrieved(result.Leaderboard);
                },
                (error) =>
                {
                });
        }
    }

    public void SendScore(int score, int bugsEaten)
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "submitDailyChallengeScore",
                    FunctionParameter = new { score,  bugsEaten }
                },
                (result) =>
                {
                    //UpdateInventory(result.FunctionResult as JsonObject);
                },
                (error) =>
                {
                });
        }
    }
}
