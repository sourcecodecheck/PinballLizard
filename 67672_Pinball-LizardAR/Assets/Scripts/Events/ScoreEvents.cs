using System.Collections.Generic;
using PlayFab.ClientModels;
public static class ScoreEvents
{
    //Subscribers:
    //MainGameManager
    public delegate void AddScore(int scoreToAdd);
    public static event AddScore OnAddScore;
    public static void SendAddScore(int scoreToAdd)
    {
        OnAddScore?.Invoke(scoreToAdd);
    }

    //Subscribers:
    //ScoreDisplay
    public delegate void ScoreUpdated(int currentScore);
    public static event AddScore OnScoreUpdated;
    public static void SendScoreUpdated(int currentScore)
    {
        OnScoreUpdated?.Invoke(currentScore);
    }

    //Subscribers:
    //MainGameManager
    public delegate void SetMultiplier(float multiplierToSet);
    public static event SetMultiplier OnSetMultiplier;
    public static void SendSetMultiplier(float multiplierToSet)
    {
        OnSetMultiplier?.Invoke(multiplierToSet);
    }

    //Subscribers:
    //MainGameManager
    public delegate void AddMultiplier(float multiplierToAdd);
    public static event AddMultiplier OnAddMultiplier;
    public static void SendAddMultiplier(float multiplierToAdd)
    {
        OnAddMultiplier?.Invoke(multiplierToAdd);
    }

    //Subscribers:
    //ChallengeMode
    public delegate void LoadLeaderBoard();
    public static event LoadLeaderBoard OnLoadLeaderBoard;
    public static void SendLoadLeaderBoard()
    {
        OnLoadLeaderBoard?.Invoke();
    }

    //Subscribers:
    //LeaderBoard
    public delegate void LeaderBoardRetrieved(List<PlayerLeaderboardEntry> leaderboardEntries);
    public static event LeaderBoardRetrieved OnLeaderBoardRetrieved;
    public static void SendLeaderBoardRetrieved(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        OnLeaderBoardRetrieved?.Invoke(leaderboardEntries);
    }

    //Subscribers:
    //UserInfo
    public delegate void GetMayhemMultiplier();
    public static event GetMayhemMultiplier OnGetMayhemMultiplier;
    public static void SendGetMayhemMultiplier()
    {
        OnGetMayhemMultiplier?.Invoke();
    }
}

