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
    }

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
                       FunctionName = "getChallengeSeed"
                   },
                   (result) =>
                   {
                       int randomResult = 
                       PlayFabSimpleJson.DeserializeObject<int>(PlayFabSimpleJson.SerializeObject(((JsonObject)result.FunctionResult)[0]));
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

    private void OnDestroy()
    {
        ScoreEvents.OnLoadLeaderBoard -= GetLeaderBoard;
    }
}
