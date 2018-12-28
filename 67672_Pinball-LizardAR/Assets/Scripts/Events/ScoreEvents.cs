using System.Collections.Generic;
using PlayFab.ClientModels;
public static class ScoreEvents
{
    public delegate void AddScore(int scoreToAdd);
    public static event AddScore OnAddScore;
    public static void SendAddScore(int scoreToAdd)
    {
        OnAddScore(scoreToAdd);
    }

    public delegate void ScoreUpdated(int currentScore);
    public static event AddScore OnScoreUpdated;
    public static void SendScoreUpdated(int currentScore)
    {
        OnScoreUpdated(currentScore);
    }

    public delegate void SetMultiplier(float multiplierToSet);
    public static event SetMultiplier OnSetMultiplier;
    public static void SendSetMultiplier(float multiplierToSet)
    {
        OnSetMultiplier(multiplierToSet);
    }

    public delegate void AddMultiplier(float multiplierToAdd);
    public static event AddMultiplier OnAddMultiplier;
    public static void SendAddMultiplier(float multiplierToAdd)
    {
        OnAddMultiplier(multiplierToAdd);
    }

    public delegate void LoadLeaderBoard();
    public static event LoadLeaderBoard OnLoadLeaderBoard;
    public static void SendLoadLeaderBoard()
    {
        OnLoadLeaderBoard();
    }

    public delegate void LeaderBoardRetrieved(List<PlayerLeaderboardEntry> leaderboardEntries);
    public static event LeaderBoardRetrieved OnLeaderBoardRetrieved;
    public static void SendLeaderBoardRetrieved(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        OnLeaderBoardRetrieved(leaderboardEntries);
    }
}

