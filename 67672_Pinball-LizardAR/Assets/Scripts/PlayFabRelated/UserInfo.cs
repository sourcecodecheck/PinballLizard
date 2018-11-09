using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class UserInfo : MonoBehaviour
{
    public Inventory PlayerInventory;
    public string LevelKey;
    public string ExperienceKey;
    public string BugsEatenKey;
    public string BestScoreKey;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateUserDataFromPlayFab", 0.5f, 5.0f);
        TrackingEvents.OnGameVictory += GameVictory;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateUserDataFromPlayFab()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest()
            {
                StatisticNames = new List<string>
                {
                    LevelKey,
                    ExperienceKey,
                    BugsEatenKey,
                    BestScoreKey
                }
            },
                (result) =>
                {
                    if (result.Statistics.Count == 0)
                    {
                        SetUpNewPlayer();
                    }
                    else
                    {
                        foreach (var statistic in result.Statistics)
                        {
                            if (statistic.StatisticName == LevelKey)
                            {
                                PlayerInventory.PlayerLevel = statistic.Value;
                            }
                            if (statistic.StatisticName == ExperienceKey)
                            {
                                PlayerInventory.ExperienceCount = statistic.Value;
                            }
                            if (statistic.StatisticName == BugsEatenKey)
                            {
                                PlayerInventory.BugsEatenCount = statistic.Value;
                            }
                            if (statistic.StatisticName == BestScoreKey)
                            {
                                PlayerInventory.BestScore = statistic.Value;
                            }
                        }
                    }
                },
                (error) =>
                {
                });
        }
    }

    public void UpdateUserDataToPlayFab()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest()
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = ExperienceKey, Value = PlayerInventory.ExperienceCount },
                        new StatisticUpdate { StatisticName = BugsEatenKey, Value = PlayerInventory.BugsEatenCount },
                        new StatisticUpdate { StatisticName = BestScoreKey, Value = PlayerInventory.BestScore }
                    }
                },
                (result) =>
                {
                },
                (error) =>
                {
                });
        }
    }
    public void GameVictory(int score, int bugsEaten)
    {
        if (PlayerInventory.BestScore < score)
        {
            PlayerInventory.BestScore = score;
        }
        PlayerInventory.ExperienceCount += score / 10;
        PlayerInventory.BugsEatenCount += bugsEaten;
        UpdateUserDataToPlayFab();
    }

    private void SetUpNewPlayer()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest()
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = LevelKey, Value = 1 },
                        new StatisticUpdate { StatisticName = ExperienceKey, Value = 0 },
                        new StatisticUpdate { StatisticName = BugsEatenKey, Value = 0 },
                        new StatisticUpdate { StatisticName = BestScoreKey, Value = 0 }
                    }
                },
                (result) =>
                {
                    PlayerInventory.PlayerLevel = 1;
                    PlayerInventory.ExperienceCount = 0;
                    PlayerInventory.BugsEatenCount = 0;
                    PlayerInventory.BestScore = 0;
                },
                (error) =>
                {
                });
        }
    }
    private void OnDestroy()
    {
        TrackingEvents.OnGameVictory -= GameVictory;
    }
}
